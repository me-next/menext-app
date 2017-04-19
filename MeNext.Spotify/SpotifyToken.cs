using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class SpotifyToken
    {
        // How much time before we think the token expires when we decide to refresh it just to be safe
        // TODO Switch both debug and release to the half hour one
#if DEBUG
        //private const long TOKEN_EXPIRE_EXTRA_TIME = 60 * 30;
        private const long TOKEN_EXPIRE_EXTRA_TIME = 60 * 60 - 30;  // Refresh every ~30 secs
#else
        private const long TOKEN_EXPIRE_EXTRA_TIME = 60 * 30;  // Refresh every half hour
#endif

        // NO TRAILING SLASH
        public const string TOKEN_SERVER = "https://menext-spotify-token-swap.herokuapp.com";
        public const string TOKEN_REFRESH_URL = TOKEN_SERVER + "/refresh";
        public const string TOKEN_SWAP_URL = TOKEN_SERVER + "/swap";

        private string accessToken;
        private string refreshToken;
        private long expiresAtApprox;

        private HttpClient http;

        public SpotifyToken(SpotifyMusicService sms)
        {
            this.NukeToken();
            this.http = new HttpClient();
        }

        public void NukeToken()
        {
            this.accessToken = null;
            this.refreshToken = null;
            this.expiresAtApprox = 0;
            this.NotifyChanged();
        }

        public void UpdateTokens(string accessToken, string refreshToken, long expiresIn)
        {
            if (this.accessToken == accessToken) {
                return;
            }
            Debug.WriteLine("Updated access token to: " + accessToken);

            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.expiresAtApprox = UnixTimeSecs + expiresIn;

            this.NotifyChanged();
        }

        public void UpdateTokens(string code)
        {
            var swap = this.GetSwap(code);
            this.UpdateTokens(swap.AccessToken, swap.RefreshTokenEncrypted, swap.ExpiresIn);
        }

        public void UpdateAccessToken(string accessToken)
        {
            this.UpdateTokens(accessToken, null, 0);
        }

        private void NotifyChanged()
        {
            // Just a placeholder in case we wanna let people know that the access token changed
            // For now we don't actually need it
        }

        /// <summary>
        /// Refreshes the Spotify authentication tokens if necessary
        /// </summary>
        public void CheckRefresh()
        {
            if (this.refreshToken == null) {
                // We have no refresh token and are thus not responsible for refreshing
                // Somebody must do it externally (ex SpotifyMusicServiceIos handles it)
                Debug.WriteLine("No refresh token.");
                return;
            }

            var timeLeft = this.expiresAtApprox - (UnixTimeSecs + TOKEN_EXPIRE_EXTRA_TIME);
            Debug.WriteLine("Time until next refresh: " + timeLeft);

            if (UnixTimeSecs + TOKEN_EXPIRE_EXTRA_TIME > this.expiresAtApprox) {
                var refresh = GetRefresh();

                var newAccessToken = refresh.AccessToken;
                var newRefreshToken = this.refreshToken;
                if (refresh.RefreshTokenEncrypted != null) {
                    newRefreshToken = refresh.RefreshTokenEncrypted;
                }
                var newExpiresIn = refresh.ExpiresIn;

                this.UpdateTokens(newAccessToken, newRefreshToken, newExpiresIn);
            }
        }

        public string AccessToken
        {
            get
            {
                CheckRefresh();
                return accessToken;
            }
        }

        public RefreshSwapResult GetSwap(string code)
        {
            var task = Task.Run(async () =>
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code)
                });
                return await this.PostJsonFullUri(TOKEN_SWAP_URL, content);
            });
            var json = task.Result;
            var swapResult = JsonConvert.DeserializeObject<RefreshSwapResult>(json);
            return swapResult;
        }

        public RefreshSwapResult GetRefresh()
        {
            var task = Task.Run(async () =>
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("refresh_token", this.refreshToken)
                });
                return await this.PostJsonFullUri(TOKEN_REFRESH_URL, content);
            });
            var json = task.Result;
            var refreshResult = JsonConvert.DeserializeObject<RefreshSwapResult>(json);
            return refreshResult;
        }

        public async Task<string> PostJsonFullUri(string fullUri, HttpContent content)
        {
            Debug.WriteLine("::Getting URI: {0}", fullUri);

            var uri = new Uri(fullUri);
            HttpResponseMessage response;
            try {
                response = await http.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            } catch (WebException we) {
                Debug.WriteLine(we.Message);
                Debug.WriteLine(we.StackTrace);
                return null;
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return null;
            }

            // Get the data
            var jsonResult = await response.Content.ReadAsStringAsync();

            return jsonResult;
        }

        private long UnixTimeSecs
        {
            get
            {
                return (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            }
        }
    }
}
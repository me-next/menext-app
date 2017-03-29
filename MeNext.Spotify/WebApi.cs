using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MeNext.MusicService;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    /// <summary>
    /// Access the spotify web api, which contains metadata and user library information. This class contains code which
    /// would otherwise be identical in MeNext.Spotify.iOS and MeNext.Spotify.Droid.
    /// </summary>
    public class WebApi
    {
        private const string BASE_ADDRESS = "https://api.spotify.com/v1";

        private const int DEFAULT_RETRY_TIME = 10;

        private HttpClient http;

        public MetadataFactory factory { get; set; }

        // Access tokens
        private string accessToken;
        private string tokenType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.WebApi"/> class. This constructor only
        /// provides access to methods which do not require authentication.
        /// </summary>
        public WebApi()
        {
            http = new HttpClient();
            //http.BaseAddress = new Uri(BASE_ADDRESS);
            factory = new MetadataFactory(this);
        }

        /// <summary>
        /// Updates the Spotify Web API access token.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="tokenType">Token type.</param>
        public void updateAccessToken(string accessToken, string tokenType)
        {
            this.accessToken = accessToken;
            this.tokenType = tokenType;
        }

        // TODO: Unit test this
        /// <summary>
        /// Encodes a query string according to the requirements outlines in
        /// https://developer.spotify.com/web-api/search-item/
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="query">Query.</param>
        private static string EncodeQuery(string query)
        {
            string encoded = WebUtility.UrlEncode(query);
            encoded = encoded.Replace("%22", "\"");    // Kludge to maintain
            return encoded;
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            return null;
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            return null;
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            return null;
        }

        public IResultList<ISong> SearchSong(string query)
        {
            return null;
        }

        // TODO: Test rate limiting
        /// <summary>
        /// Processes the web exception.
        /// </summary>
        /// <returns>True if we waited and want to retry now</returns>
        /// <param name="we">We.</param>
        private async Task<bool> ProcessWebException(WebException we)
        {
            var exResponse = ((HttpWebResponse) we.Response);
            var statusCode = exResponse.StatusCode;

            // 429: Too Many Requests
            // We've made too many requests and need to wait a lil bit before trying again.
            if (statusCode == (HttpStatusCode) 429) {
                var headers = exResponse.Headers;
                var retryAfter = DEFAULT_RETRY_TIME;

                // Extract Retry-After header
                if (Array.IndexOf(headers.AllKeys, "Retry-After") >= 0) {
                    try {
                        // We wait an extra second because some guy on stackoverflow said the api chops off
                        // the decimal portion
                        retryAfter = Int32.Parse(headers["Retry-After"]) + 1;
                    } catch (Exception e) when (
                        e is ArgumentNullException
                        || e is FormatException
                        || e is OverflowException) {
                    }
                }

                Debug.WriteLine("RATE LIMITED. Waiting {1} secs", retryAfter);

                // Wait until it should be safe to request again
                await Task.Delay(retryAfter * 1000);

                return true;
            }
            return false;
        }

        public async Task<string> GetJson(string uriEnd)
        {
            Debug.Assert(uriEnd[0] == '/');

            var uri = new Uri(BASE_ADDRESS + uriEnd);
            HttpResponseMessage response;
            try {
                response = await http.GetAsync(uri);
                Debug.WriteLine(response.RequestMessage.RequestUri);
                response.EnsureSuccessStatusCode();

            } catch (WebException we) {
                if (await ProcessWebException(we)) {
                    return await GetJson(uriEnd);
                } else {
                    Debug.WriteLine(uri.AbsolutePath);
                    Debug.WriteLine(we.Message);
                    Debug.WriteLine(we.StackTrace);
                    return null;
                }
            } catch (Exception e) {
                Debug.WriteLine(uri.AbsolutePath);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return null;
            }

            // Get the data
            var jsonResult = await response.Content.ReadAsStringAsync();

            return jsonResult;
        }
    }
}

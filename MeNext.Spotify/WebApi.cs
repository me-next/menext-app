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
        // Don't include trailing /
        private const string BASE_ADDRESS = "https://api.spotify.com/v1";

        private const int DEFAULT_RETRY_TIME = 10;

        private HttpClient http;

        public MetadataFactory metadata { get; set; }

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
            metadata = new MetadataFactory(this);
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
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
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

        public IResultList<ISong> SearchSong(string rawQuery)
        {
            Debug.WriteLine("Searching for: {0}", rawQuery);
            string encodedQuery = EncodeQuery(rawQuery);

            string endUri = string.Format("/search?q={0}&type={1}", encodedQuery, "track");

            return PagingUri<ISong, PartialTrackResult>(BASE_ADDRESS + endUri, true);
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            // TODO
            throw new NotImplementedException();
        }

        public IResultList<ISong> GetUserLibrarySongs()
        {
            return PagingUri<ISong, SavedTrackResult>(BASE_ADDRESS + "/me/tracks", false);
        }

        public IResultList<T> PagingUri<T, Q>(string uri, bool isWrapped) where T : IMetadata where Q : IMetadataResult
        {
            var task = Task.Run(async () =>
            {
                return await GetJsonFullUri(uri);
            });

            var json = task.Result;

            PagingObjectResult<Q> theList = null;

            // TODO: Cache the results from this instead of discarding them in favour of the URI?
            if (isWrapped) {
                var jsonResult = JsonConvert.DeserializeObject<SearchResult<Q>>(json);
                theList = PagingObjectResult<Q>.CastTypeParam<Q>(jsonResult.Items);
            } else {
                theList = JsonConvert.DeserializeObject<PagingObjectResult<Q>>(json);
            }

            var search = new Search<T, Q>(theList, this, isWrapped);

            return search;
        }

        // TODO: Test rate limiting
        // Pretty sure this code is wrong
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

            return await GetJsonFullUri(BASE_ADDRESS + uriEnd);
        }

        public async Task<string> GetJsonFullUri(string fullUri)
        {
            Debug.WriteLine("Getting URI: {0}", fullUri);

            var uri = new Uri(fullUri);
            HttpResponseMessage response;
            try {
                response = await http.GetAsync(uri);
                response.EnsureSuccessStatusCode();

            } catch (WebException we) {
                // TODO: Fix rate limiting code so it works whether WebException thrown or not
                // Or, better, find out if WebException is thrown for 429?
                if (await ProcessWebException(we)) {
                    return await GetJsonFullUri(fullUri);
                } else {
                    Debug.WriteLine(we.Message);
                    Debug.WriteLine(we.StackTrace);
                    return null;
                }
            } catch (Exception e) {
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

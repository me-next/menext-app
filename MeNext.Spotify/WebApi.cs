using System;
using System.Collections.Generic;
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

        public const int MAX_RECOMMENDATION_SEEDS = 5;

        private const int DEFAULT_RETRY_TIME = 10;

        private HttpClient http;

        public MetadataFactory metadata { get; set; }

        // Access tokens
        private string accessToken;

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
        public void updateAccessToken(string accessToken)
        {
            this.accessToken = accessToken;
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
        }

        /// <summary>
        /// Gets recommendations for a set of songs.
        /// </summary>
        /// <returns>The recommendations.</returns>
        /// <param name="seeds">Seeds.</param>
        public IList<ISong> GetRecommendations(IList<ISong> rawSeeds)
        {
            if (rawSeeds.Count == 0) {
                return new List<ISong>();
            }

            // Select up to 5 random seeds from the list of rawSeeds
            var copiedRawSeeds = new List<ISong>(rawSeeds);
            Shuffle(copiedRawSeeds);

            var seeds = copiedRawSeeds.GetRange(0, Math.Min(copiedRawSeeds.Count, MAX_RECOMMENDATION_SEEDS));

            // Format the seed URIs
            var commaSeeds = "";
            foreach (var seed in seeds) {
                commaSeeds += "," + seed.UniqueId.Substring(14);
            }
            commaSeeds = commaSeeds.Substring(1);

            // Hit the Spotify server
            var task = Task.Run(async () =>
            {
                // TODO Unmagick the 30
                return await GetJson("/recommendations?limit=30&seed_tracks=" + commaSeeds);
            });

            var json = task.Result;
            var tracksResult = JsonConvert.DeserializeObject<RecommendedTracksResult>(json);

            // Submit tracks to cache and compile their UIDs
            var trackUids = new List<string>();
            foreach (var track in tracksResult.Items) {
                this.metadata.CacheSubmit(track);
                trackUids.Add(track.uri);
            }

            return this.metadata.GetSongs(trackUids);
        }

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
        /// <summary>
        /// Searches for a specified song. 
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="rawQuery">Raw query to be encoded and sent in a request.</param>
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
        /// <summary>
        /// Gets the users library songs.
        /// </summary>
        /// <returns>The users library songs.</returns>
        public IResultList<ISong> GetUserLibrarySongs()
        {
            return PagingUri<ISong, SavedTrackResult>(BASE_ADDRESS + "/me/tracks", false);
        }
        /// <summary>
        /// Gets the users library playlists.
        /// </summary>
        /// <returns>The users library playlists.</returns>
        public IResultList<IPlaylist> GetUserLibraryPlaylists()
        {
            return PagingUri<IPlaylist, PartialPlaylistResult>(BASE_ADDRESS + "/me/playlists", false);
        }

        /// <summary>
        /// Creates an IResultList for the paging object located in the response to a Spotify URI.
        /// </summary>
        /// <returns>The IResultList.</returns>
        /// <param name="uri">The entire Spotify request URI (including the BASE_ADDRESS).</param>
        /// <param name="isWrapped">
        /// If set to <c>true</c>, the paging object is wrapped in a SearchResult, which actually contains multiple
        /// paging objects. We deduce which one to use based on type parameter Q.
        /// </param>
        /// <typeparam name="T">The internal data type; an instance of IMetadata.</typeparam>
        /// <typeparam name="Q">The json data type; an instance of IMetadataResult.</typeparam>
        public IResultList<T> PagingUri<T, Q>(string uri, bool isWrapped) where T : IMetadata where Q : IMetadataResult
        {
            var task = Task.Run(async () =>
            {
                return await GetJsonFullUri(uri);
            });

            var json = task.Result;

            PagingObjectResult<Q> theList;

            if (isWrapped) {
                var jsonResult = JsonConvert.DeserializeObject<SearchResult<Q>>(json);
                theList = PagingObjectResult<Q>.CastTypeParam<Q>(jsonResult.Items);
            } else {
                theList = JsonConvert.DeserializeObject<PagingObjectResult<Q>>(json);
            }

            // Store metadata in the cache so we can use it, if possible
            foreach (var item in theList.items) {
                metadata.CacheSubmit(item);
            }

            var search = new PagingWrapper<T, Q>(theList, this, isWrapped);

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
        /// <summary>
        /// Gets the json response from a request without the base address.
        /// </summary>
        /// <returns>The json response.</returns>
        /// <param name="uriEnd">URI without base address.</param>
        public async Task<string> GetJson(string uriEnd)
        {
            Debug.Assert(uriEnd[0] == '/');

            return await GetJsonFullUri(BASE_ADDRESS + uriEnd);
        }
        /// <summary>
        /// Gets the json with a full URI.
        /// </summary>
        /// <returns>The json response.</returns>
        /// <param name="fullUri">a full URI with base address.</param>
        public async Task<string> GetJsonFullUri(string fullUri)
        {
            Debug.WriteLine("::Getting URI: {0}", fullUri);

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

            // TODO We should be using this, but Spotify is being stupid
            // See http://stackoverflow.com/questions/39065988/utf8-is-not-a-supported-encoding-name
            //var jsonResult = await response.Content.ReadAsStringAsync();

            // So temporary kludge, just assume UTF-8 and parse the bytes:
            var jsonResultBytes = await response.Content.ReadAsByteArrayAsync();
            var jsonResult = System.Text.Encoding.UTF8.GetString(jsonResultBytes, 0, jsonResultBytes.Length);

            return jsonResult;
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

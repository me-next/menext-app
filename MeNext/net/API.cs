using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using MeNext.MusicService;

namespace MeNext
{
    public class API
    {
        private const string BASE_URI = "https://api.spotify.com/v1";

        private string ServerIP { get; set; }
        private HttpClient Client { get; set; }

        public API(string ip)
        {
            this.ServerIP = ip;
            this.Client = new HttpClient();

            // set the baseURI so that things are cleaner
            var baseURI = new Uri(this.ServerIP);
            this.Client.BaseAddress = baseURI;
        }

        /// <summary>
        ///  SayHello is a testing method that just servers to ping the server. 
        /// </summary>
        /// <returns>The server response string.</returns>
        public async Task<string> SayHello()
        {
            var uri = new Uri("/hello");
            return await FireRequest(uri);
        }

        /// <summary>
        /// Pulls a request from the server.
        /// </summary>
        /// <param name="uid">User id.</param>
        /// <param name="eid">Event id.</param>
        /// <param name="changeID">Change identifier.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> Pull(string uid, string eid, UInt64 changeID)
        {
            var uri = new Uri(string.Format("/pull/{0}/{1}/{2}", uid, eid, changeID));
            return await FireRequest(uri);

        }

        /// <summary>
        /// Try to create the party. 
        /// </summary>
        /// <returns> The party.</returns>
        public async Task<string> CreateParty(string id, string name)
        {
            var uri = new Uri(string.Format("/createParty/{0}/{1}", id, name));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Try to join a party. 
        /// </summary>
        /// <param name="slug">Event ID</param>
        /// <param name="ukey">User Key</param>
        /// <param name="uid">User ID</param>
        /// <returns>The server response string.</returns>
        public async Task<string> JoinParty(string slug, string ukey, string uid)
        {
            var uri = new Uri(string.Format("/joinParty/{0}/{1}/{2}", slug, ukey, uid));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Suggests the song to the suggestion queue.
        /// </summary>
        /// <param name="uid">User id.</param>
        /// <param name="eid">Event id.</param>
        /// <param name="sid">song identifier.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SuggestAddSong(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/suggest/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Called when song finishes.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SongFinished(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/songFinished/{0}/{1}/{2}", eid, uid, sid));
            Debug.WriteLine("Song finished uri:" + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Skips the song.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SkipSong(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/skip/{0}/{1}/{2}", eid, uid, sid));
            Debug.WriteLine("Skip song uri:" + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Suggestion downvote.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SuggestionDownvote(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/suggestDown/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Suggestion upvote.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SuggestionUpvote(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/suggestUp/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Clears the suggestion vote.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> SuggestionClearvote(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/suggestClearvote/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }


        /// <summary>
        /// Sends a request URI to the server. 
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="uri">URI.</param>
        private async Task<string> FireRequest(Uri uri)
        {
            var result = "";

            var response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsStringAsync();
            }


            return result;
        }
    };
}

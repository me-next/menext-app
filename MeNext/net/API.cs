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
        /// Pulls the results down from the server.
        /// </summary>
        /// <param name="uid">User id.</param>
        /// <param name="eid">Event id.</param>
        /// <param name="changeID">Change identifier.</param>
        /// <returns>The server response.</returns>
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
            Debug.WriteLine("create with name uri: " + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Try to create party with given name.
        /// </summary>
        /// <returns>The party.</returns>
        /// <param name="id">User ID.</param>
        /// <param name="userName">User name.</param>
        /// <param name="eventName">Event name.</param>
        public async Task<string> CreateParty(string id, string userName, string eventName)
        {
            var uri = new Uri(string.Format("/createPartyWithName/{0}/{1}/{2}", id, userName, eventName));
            Debug.WriteLine("uri = " + uri);
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
            Debug.WriteLine("uri = " + uri);
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
        /// Called when adding a song to the PlayNext Queue.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> AddPlayNext(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/addPlayNext/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        public async Task<string> SetVolume(string eid, string uid, int vol)
        {
            var uri = new Uri(string.Format("/setVolume/{0}/{1}/{2}", eid, uid, vol));
            return await FireRequest(uri);
        }

        public async Task<string> AddTopPlayNext(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/addTopPlayNext/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        public async Task<string> RemovePlayNext(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/removePlayNext/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        public async Task<string> PlayNow(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/playNow/{0}/{1}/{2}", eid, uid, sid));
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
            sid = "dummy";  // TODO
            var uri = new Uri(string.Format("/skip/{0}/{1}/{2}", eid, uid, sid));
            Debug.WriteLine("Skip song uri:" + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> PrevSong(string eid, string uid, string sid)
        {
            sid = "dummy";  // TODO
            var uri = new Uri(string.Format("/previous/{0}/{1}/{2}", eid, uid, sid));
            Debug.WriteLine("Prev song uri:" + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Play the current song.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> PlaySong(string eid, string uid)
        {
            var uri = new Uri(string.Format("/play/{0}/{1}", eid, uid));
            Debug.WriteLine("Play song uri:" + uri.ToString());
            return await FireRequest(uri);
        }

        /// <summary>
        /// Pause the current song.
        /// </summary>
        /// <param name="eid">Event id.</param>
        /// <param name="uid">User id.</param>
        /// <param name="sid">Song id.</param>
        /// <returns>The server response string.</returns>
        public async Task<string> PauseSong(string eid, string uid, double pos)
        {
            var uri = new Uri(string.Format("/pause/{0}/{1}/{2}", eid, uid, pos));
            Debug.WriteLine("Pause song uri:" + uri.ToString());
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
        /// Gets the permissions with descriptions from the server
        /// </summary>
        /// <returns>Server response string.</returns>
        public async Task<string> GetPermissions()
        {
            var uri = new Uri(string.Format("/permissions"));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Attempts to set a permission
        /// </summary>
        /// <returns>The permission to set.</returns>
        /// <param name="eid">Event ID.</param>
        /// <param name="uid">User ID.</param>
        /// <param name="which">Which permission.</param>
        /// <param name="enabled">If set to <c>true</c> allow users to use that permission.</param>
        public async Task<string> SetPermission(string eid, string uid, string which, bool enabled)
        {
            if (enabled) {
                var innerUri = new Uri(string.Format("/setPermission/{0}/{1}/{2}/true", eid, uid, which));
                Debug.WriteLine(innerUri.ToString());
                return await FireRequest(innerUri);
            }

            var uri = new Uri(string.Format("/setPermission/{0}/{1}/{2}/false", eid, uid, which));
            Debug.WriteLine(uri.ToString());
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
            } else {
                Debug.WriteLine(" *** ERR: BAD SERVER STATUS CODE: " + response.StatusCode);
                Debug.WriteLine(" *** FROM URI: " + uri.AbsoluteUri);
                return null;
            }

            return result;
        }
    };
}

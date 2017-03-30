﻿using System;
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
        /// <returns>The pull.</returns>
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
        /// id = userKey, name = userName, slug = PartyID
        /// </summary>
        /// <returns> The party.</returns>
        public async Task<string> JoinParty(string slug, string id, string name)
        {
            var uri = new Uri(string.Format("/joinParty/{0}/{1}/{2}", slug, id, name));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Suggests the song to the suggestion queue
        /// </summary>
        /// <returns>The add song.</returns>
        /// <param name="uid">User id.</param>
        /// <param name="eid">Event id.</param>
        /// <param name="sid">song identifier.</param>
        public async Task<string> SuggestAddSong(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/suggest/{0}/{1}/{2}", eid, uid, sid));
            return await FireRequest(uri);
        }

        public async Task<string> SongFinished(string eid, string uid, string sid)
        {
            var uri = new Uri(string.Format("/songFinished/{0}/{1}/{2}", eid, uid, sid));
            Debug.WriteLine("Song finished uri:" + uri.ToString());
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

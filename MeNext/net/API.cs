using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace MeNext
{

    public class API
    {
        public API(string ip)
        {
            this.ServerIP = ip;
            this.Client = new HttpClient();

        }

        /// <summary>
        ///  SayHello is a testing method that just servers to ping the server. 
        /// </summary>
        /// <returns>The server response string.</returns>
        public async System.Threading.Tasks.Task<string> SayHello()
        {
            var uri = new Uri(this.ServerIP + "/hello");
            return await FireRequest(uri);
        }

        /// <summary>
        /// Pulls the results down from the server.
        /// </summary>
        /// <returns>The pull.</returns>
        public async System.Threading.Tasks.Task<string> Pull(string uid, string eid, UInt64 changeID)
        {
            var uri = new Uri(string.Format("/{0}/{1}/pull/{2}", uid, eid, changeID));
            return await FireRequest(uri);

        }

        /// <summary>
        /// Try to create the party. 
        /// </summary>
        /// <returns> The party.</returns>
        public async System.Threading.Tasks.Task<string> CreateParty(string id, string name)
        {
            var uri = new Uri(string.Format("/createParty/{0}/{1}", id, name));
            return await FireRequest(uri);
        }

        /// <summary>
        /// Sends a request URI to the server. 
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="uri">URI.</param>
        private async System.Threading.Tasks.Task<string> FireRequest(Uri uri)
        {
            string result = "";
            try {

                var response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode) {
                    result = await response.Content.ReadAsStringAsync();
                }
            } catch (Exception e) {
                Debug.WriteLine("error on net request:" + e.Message);
            }

            return result;
        }


        private string ServerIP
        {
            get;
            set;
        }

        private HttpClient Client;
    };
}

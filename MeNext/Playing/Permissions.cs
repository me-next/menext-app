using System;

using System.Diagnostics;
using System.Collections.Generic;

namespace MeNext
{
    public class Permissions : IPullUpdateObserver
    {
        public static string PlayNext = "PlayNext";
        public static string PlayPause = "PlayPause";
        public static string Seek = "Seek";
        public static string Suggest = "Suggest";
        public static string SuggestVote = "SuggestVote";
        public static string Volume = "Volume";

        private Dictionary<string, bool> permissions;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Permissions"/> class.
        /// This class defaults to having all the permissions set to true, but can be turned off. 
        /// </summary>
        public Permissions()
        {
            permissions = new Dictionary<string, bool>();
            permissions.Add(PlayNext, true);
            permissions.Add(PlayPause, true);
            permissions.Add(Seek, true);
            permissions.Add(Suggest, true);
            permissions.Add(SuggestVote, true);
            permissions.Add(Volume, true);

        }

        public bool GetPermission(string which)
        {
            if (permissions.Count == 0) {
                return true;
            }

            bool val; 
            var has = permissions.TryGetValue(which, out val);
            if (!has) {
                Debug.WriteLine("can't find value: " + which);
                return false;
            }

            return val;
        }

        public void OnNewPullData(PullResponse data)
        {
            this.permissions = data.Permissions;
        }
    }
}

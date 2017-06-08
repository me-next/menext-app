using System;

using System.Diagnostics;
using System.Collections.Generic;

namespace MeNext
{
    /// <summary>
    /// Represents the event permissions
    /// </summary>
    public class Permissions : IPullUpdateObserver
    {
        // TODO This should be an enum
        public const string PlayNext = "PlayNext";
        public const string PlayPause = "PlayPause";
        public const string Skip = "Skip";
        public const string Volume = "Volume";

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
            permissions.Add(Skip, true);
            //permissions.Add(Volume, true);
        }

        /// <summary>
        /// Checks if REGULULAR USERS have a permission. Use Event.ThisHasPermission(..) to check if YOU have permission
        /// to do something.
        /// </summary>
        /// <returns><c>true</c>, if permission is had by regular users, <c>false</c> otherwise.</returns>
        /// <param name="which">The permission (consts in this class)</param>
        public bool GetPermission(string which)
        {
            if (permissions.Count == 0) {
                return false;
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

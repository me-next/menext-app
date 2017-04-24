using System;
using System.Threading;

namespace MeNext
{
    /// <summary>
    /// The message which indicates we should start polling the server
    /// </summary>
    public class StartPollMessage
    {
        public string EventSlug { get; set; }

        public string UserKey { get; set; }

        public StartPollMessage(string EventSlug, string UserKey)
        {
            this.EventSlug = EventSlug;
            this.UserKey = UserKey;
        }
    }
}

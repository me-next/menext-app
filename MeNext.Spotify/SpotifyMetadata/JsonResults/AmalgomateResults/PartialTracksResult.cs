using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class PartialTracksResult : IResultWithList<PartialTrackResult>
    {
        public string href { get; set; }

        public IList<PartialTrackResult> Items
        {
            get
            {
                return items;
            }
        }

        public IList<PartialTrackResult> items { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class TracksResult : IResultWithList<TrackResult>
    {
        public IList<TrackResult> Items
        {
            get
            {
                return tracks;
            }
        }

        public IList<TrackResult> tracks { get; set; }
    }


}

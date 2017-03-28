using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
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

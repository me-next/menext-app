using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
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
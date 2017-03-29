using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class PartialTracksResult
    {
        public string href { get; set; }
        public IList<PartialTrackResult> items { get; set; }
    }
}
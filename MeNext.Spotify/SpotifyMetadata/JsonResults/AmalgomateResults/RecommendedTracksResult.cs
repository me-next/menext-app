using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/get-recommendations/
    /// </summary>
    public class RecommendedTracksResult : IResultWithList<PartialTrackResult>
    {
        public IList<PartialTrackResult> Items
        {
            get
            {
                return tracks;
            }
        }

        public IList<PartialTrackResult> tracks { get; set; }

        public object seeds { get; set; }
    }


}

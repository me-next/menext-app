using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class SearchResult
    {
        //public IList<ArtistResult> artists { get; set; }
        //public IList<PartialAlbumResult> albums { get; set; }
        public SearchChunkResult<PartialTrackResult> tracks { get; set; }
        //public IList<object> playlists { get; set; }
    }
}

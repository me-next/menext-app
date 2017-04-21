using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Represents the result of a search
    /// </summary>
    public class SearchResult<Q> where Q : IMetadataResult
    {
        public PagingObjectResult<PartialTrackResult> tracks { get; set; }
        public PagingObjectResult<PartialAlbumResult> albums { get; set; }
        public PagingObjectResult<ArtistResult> artists { get; set; }

        // The nominal items are based on the type of this search result. We only ever search for one type at a time
        // even though the Spotify API supports more that one type at a time.
        public PagingObjectResult<Q> Items
        {
            get
            {
                if (typeof(Q) == typeof(PartialTrackResult)) {
                    return PagingObjectResult<PartialTrackResult>.CastTypeParam<Q>(this.tracks);
                } else if (typeof(Q) == typeof(PartialAlbumResult)) {
                    return PagingObjectResult<PartialAlbumResult>.CastTypeParam<Q>(this.albums);
                } else if (typeof(Q) == typeof(ArtistResult)) {
                    return PagingObjectResult<ArtistResult>.CastTypeParam<Q>(this.artists);
                } else {
                    throw new Exception("Invalid SearchResult type: " + typeof(Q));
                }
            }
        }
    }
}

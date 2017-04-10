using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class SearchResult<Q> where Q : IMetadataResult
    {
        public PagingObjectResult<PartialTrackResult> tracks { get; set; }
        public PagingObjectResult<PartialAlbumResult> albums { get; set; }
        public PagingObjectResult<ArtistResult> artists { get; set; }
        //public IList<object> playlists { get; set; }

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

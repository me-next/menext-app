using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class AlbumsResult : IResultWithList<AlbumResult>
    {
        public IList<AlbumResult> albums { get; set; }

        public IList<AlbumResult> Items
        {
            get
            {
                return albums;
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
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

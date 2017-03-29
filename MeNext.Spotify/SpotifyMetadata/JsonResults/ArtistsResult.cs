using System;
using MeNext.Spotify;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class ArtistsResult : IResultWithList<ArtistResult>
    {
        public IList<ArtistResult> artists { get; set; }

        public IList<ArtistResult> Items
        {
            get
            {
                return artists;
            }
        }
    }
}

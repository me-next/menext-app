using System;
using MeNext.Spotify;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
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

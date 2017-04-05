using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class AlbumResult : IMetadataResult
    {
        public string album_type { get; set; }
        public IList<PartialArtistResult> artists { get; set; }
        public IList<string> available_markets { get; set; }
        public IList<object> copyrights { get; set; }
        public object external_ids { get; set; }
        public IList<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<PartialImageResult> images { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public PartialTracksResult tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata)
        {
            return new SpotifyAlbum(metadata, this);
        }
    }
}

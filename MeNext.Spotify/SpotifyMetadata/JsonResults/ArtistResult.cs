using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class ArtistResult : IMetadataResult
    {
        public object external_urls { get; set; }
        public object followers { get; set; }
        public IList<string> genres { get; set; }
        public string id { get; set; }
        public IList<PartialImageResult> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata)
        {
            return new SpotifyArtist(metadata, this);
        }
    }
}
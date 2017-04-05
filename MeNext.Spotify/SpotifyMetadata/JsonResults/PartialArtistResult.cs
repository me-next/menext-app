using System;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class PartialArtistResult : IMetadataResult
    {
        public object external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata)
        {
            // TODO Cache?
            return null;
        }
    }
}

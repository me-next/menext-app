using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class PartialAlbumResult : IMetadataResult
    {
        public string album_type { get; set; }
        public IList<PartialArtistResult> artists { get; set; }
        public IList<string> available_markets { get; set; }
        public object external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<PartialImageResult> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}

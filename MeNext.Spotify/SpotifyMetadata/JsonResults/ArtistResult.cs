using System.Collections.Generic;

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
    }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class PartialPlaylistResult : IMetadataResult
    {
        public bool collaborative { get; set; }
        public object external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<PartialImageResult> images { get; set; }
        public string name { get; set; }
        public object owner { get; set; }

        [JsonProperty("public")]
        public object isPublic { get; set; }

        public string snapshot_id { get; set; }
        public object tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}

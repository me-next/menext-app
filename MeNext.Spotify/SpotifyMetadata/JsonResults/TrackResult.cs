using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class TrackResult
    {
        public PartialAlbumResult album { get; set; }
        public IList<PartialArtistResult> artists { get; set; }
        public IList<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }

        [JsonProperty("explicit")]
        public bool isExplicit { get; set; }

        public object external_ids { get; set; }
        public object external_urls { get; set; }

        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}

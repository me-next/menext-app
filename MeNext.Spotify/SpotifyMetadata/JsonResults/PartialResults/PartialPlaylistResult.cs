﻿using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class PartialPlaylistResult : IMetadataResult
    {
        public bool? collaborative { get; set; }
        public object external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public IList<PartialImageResult> images { get; set; }
        public string name { get; set; }
        public object owner { get; set; }

        [JsonProperty("public")]
        public object isPublic { get; set; }

        public string snapshot_id { get; set; }
        public TracksHrefResult tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata)
        {
            return new SpotifyPlaylist(metadata, this, webApi);
        }
    }

    public class TracksHrefResult
    {
        public string href { get; set; }
        public int? total { get; set; }
    }
}

using System;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class SavedTrackResult : IMetadataResult
    {
        public string added_at { get; set; }
        public TrackResult track { get; set; }

        public string uri
        {
            get
            {
                return track.uri;
            }
        }

        public IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata)
        {
            return new SpotifySong(metadata, track);
        }
    }
}

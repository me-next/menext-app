using System;
using MeNext.MusicService;

namespace MeNext.Spotify
{
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
            // TODO Cache?
            return null;
        }
    }
}

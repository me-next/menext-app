using System;
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
    }
}

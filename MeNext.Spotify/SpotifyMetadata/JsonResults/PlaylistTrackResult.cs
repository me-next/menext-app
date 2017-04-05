﻿using System;
namespace MeNext.Spotify
{
    public class PlaylistTrackResult : IMetadataResult
    {
        public string added_at { get; set; }
        public object added_by { get; set; }
        public bool is_local { get; set; }
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

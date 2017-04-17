using System;
using System.Diagnostics;
using AVFoundation;
using MediaPlayer;
using MeNext.Spotify.iOS.Playback;
using UIKit;

namespace MeNext.Spotify.iOS
{
    public class StreamingPlaybackDelegate : SPTAudioStreamingPlaybackDelegate
    {
        private SpotifyMusicServiceIos service;

        public StreamingPlaybackDelegate(SpotifyMusicServiceIos service)
        {
            this.service = service;


        }

        public override void AudioStreamingDidStartPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri)
        {
            if (trackUri == null) {
                // Some wonkiness going on
                return;
            }
            var song = service.GetSong(trackUri);

            // TODO: This is only working w/ old versions of iOS
            // See https://forums.xamarin.com/discussion/93235/mpnowplayinginfocenter-defaultcenter-null-in-simulator/p1
            // Just going to leave this partially implemented until a resolution is found
            if (MPNowPlayingInfoCenter.DefaultCenter != null) {
                Debug.WriteLine("iOS Now Playing Centre Not Null");

                var artists = "";
                foreach (var artist in song.Artists) {
                    artists += ", " + artist.Name;
                }
                if (artists.Length > 0) {
                    artists = artists.Substring(2);
                }

                MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = new MPNowPlayingInfo
                {
                    AlbumTitle = song.Album.Name,
                    AlbumTrackNumber = song.TrackNumber,
                    Artist = artists,
                    //Artwork = ..., TODO
                    ElapsedPlaybackTime = 0,
                    ExternalContentIdentifier = song.UniqueId,
                    MediaType = MPNowPlayingInfoMediaType.Audio,
                    PlaybackDuration = song.Duration,
                    PlaybackRate = 1,
                    Title = song.Name,
                };
            } else {
                Debug.WriteLine("iOS Now Playing Centre Null");
            }
        }

        public override void AudioStreamingDidStopPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri)
        {
            Debug.WriteLine("Track ended: " + trackUri, "playback");
            service.SomethingChanged();
            service.SongEnds(trackUri);
        }

        public override void AudioStreamingDidChangePosition(SPTAudioStreamingController audioStreaming, double position)
        {
            //Debug.WriteLine("Position: " + position);
            service.SomethingChanged();
        }

        public override void AudioStreamingDidChangePlaybackStatus(SPTAudioStreamingController audioStreaming, bool isPlaying)
        {
            Debug.WriteLine("Playback changed: " + isPlaying, "playback");

            service.SomethingChanged();
        }
    }
}
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

        /// <summary>
        /// Music has started playing. Update the service accordingly.
        /// </summary>
        /// <param name="audioStreaming">Audio streaming.</param>
        /// <param name="trackUri">Track URI.</param>
        public override void AudioStreamingDidStartPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri)
        {
            // Get playing song
            var song = service.GetSong(trackUri);

            // Process remote control
            if (MPNowPlayingInfoCenter.DefaultCenter != null) {
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
                    // Artwork = ..., TODO
                    ElapsedPlaybackTime = 0,
                    ExternalContentIdentifier = song.UniqueId,
                    MediaType = MPNowPlayingInfoMediaType.Audio,
                    PlaybackDuration = song.Duration,
                    PlaybackRate = 1,
                    Title = song.Name,
                };
            } else {
                Debug.WriteLine("*** ERROR: iOS Now Playing Centre Null");
            }
        }

        /// <summary>
        /// Handles when a song stops playing.
        /// </summary>
        /// <param name="audioStreaming">Audio streaming.</param>
        /// <param name="trackUri">Track URI.</param>
        public override void AudioStreamingDidStopPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri)
        {
            Debug.WriteLine("Track ended: " + trackUri, "playback");
            service.SomethingChanged();
            service.SongEnds(trackUri);

            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = new MPNowPlayingInfo();
        }

        /// <summary>
        /// Update UI since the audio changed position.
        /// </summary>
        /// <param name="audioStreaming">Audio streaming.</param>
        /// <param name="position">Position.</param>
        public override void AudioStreamingDidChangePosition(SPTAudioStreamingController audioStreaming, double position)
        {
            //Debug.WriteLine("Position: " + position);
            service.SomethingChanged();
        }

        /// <summary>
        /// Update UI since the Playback status has changed
        /// </summary>
        public override void AudioStreamingDidChangePlaybackStatus(SPTAudioStreamingController audioStreaming, bool isPlaying)
        {
            Debug.WriteLine("Playback changed: " + isPlaying, "playback");

            service.SomethingChanged();
        }
    }
}
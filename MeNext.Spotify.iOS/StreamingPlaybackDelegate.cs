using System;
using System.Diagnostics;
using AVFoundation;
using MeNext.Spotify.iOS.Playback;

namespace MeNext.Spotify.iOS
{
    public class StreamingPlaybackDelegate : SPTAudioStreamingPlaybackDelegate
    {
        private SpotifyMusicServiceIos service;

        public StreamingPlaybackDelegate(SpotifyMusicServiceIos service)
        {
            this.service = service;
        }

        public override void AudioStreamingDidStopPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri)
        {
            //Debug.WriteLine("Track ended: " + trackUri);
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
            service.SomethingChanged();

            if (isPlaying) {
                // This makes it so we can actually hear audio
                // I hate how long it took to figure this out
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                AVAudioSession.SharedInstance().SetActive(true);
            } else {
                AVAudioSession.SharedInstance().SetActive(false);
            }
        }
    }
}
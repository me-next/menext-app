using System;
using System.Diagnostics;
using MeNext.Spotify.iOS.Playback;

namespace MeNext.Spotify.iOS
{
    public class StreamingPlaybackDelegate : SPTAudioStreamingPlaybackDelegate
    {
        public StreamingPlaybackDelegate()
        {
        }

        public override void AudioStreamingDidChangePosition(SPTAudioStreamingController audioStreaming, double position)
        {
            Debug.WriteLine("Position: " + position);
        }
    }
}

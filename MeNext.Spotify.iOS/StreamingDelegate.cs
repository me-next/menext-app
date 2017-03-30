using System;
using System.Diagnostics;
using AVFoundation;
using Foundation;
using MeNext.Spotify.iOS.Auth;
using MeNext.Spotify.iOS.Playback;

namespace MeNext.Spotify.iOS
{
    public class StreamingDelegate : SPTAudioStreamingDelegate
    {
        public SPTAudioStreamingController Player { get; set; }

        // TODO: Disabling for now because it breaks some songs
        // ex "G String Tuning Note" often doesn't work w/ cache
        public const bool SPOTIFY_CACHE_ENABLED = false;

        public StreamingDelegate(SpotifyMusicServiceIos service)
        {
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("sessionUpdated"), (NSNotification obj) =>
            {
                var auth = SPTAuth.DefaultInstance;

                if (this.Player == null) {
                    NSError error = null;
                    this.Player = SPTAudioStreamingController.SharedInstance();

                    bool success = this.Player.StartWithClientId(auth.ClientID, null, SPOTIFY_CACHE_ENABLED, out error);
                    if (success) {
                        this.Player.Delegate = this;
                        this.Player.PlaybackDelegate = new StreamingPlaybackDelegate(service);
                        if (SPOTIFY_CACHE_ENABLED) {
                            this.Player.DiskCache = new SPTDiskCache(1024 * 1024 * 64);
                        }
                        this.Player.LoginWithAccessToken(auth.Session.AccessToken);
                        Debug.WriteLine("success in streaming delegate");
                    } else {
                        this.Player = null;
                        // TODO
                        Debug.WriteLine("Error init " + error.Description);
                    }
                }
            });
        }

        public override void AudioStreamingDidLogin(SPTAudioStreamingController audioStreaming)
        {
            Debug.Write("audio stream login");
            // TODO: Stop doing this
            //this.Player.PlaySpotifyURI("spotify:track:0imYRG0WKxUOOcqBu3VX10", 0, 0, (NSError error1) =>
            //           {
            //               if (error1 != null) {
            //                   Debug.WriteLine("Err Playing: " + error1.DebugDescription);
            //               }
            //           });
            this.Player.SetVolume(1, (arg0) => { });
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            AVAudioSession.SharedInstance().SetActive(true);
        }

        public override void AudioStreamingDidReceiveError(SPTAudioStreamingController audioStreaming, NSError error)
        {
            Debug.WriteLine("Error: " + error.Description);
        }

        public override void AudioStreamingDidReceiveMessage(SPTAudioStreamingController audioStreaming, string message)
        {
            Debug.WriteLine("Message: " + message);
        }
    }
}

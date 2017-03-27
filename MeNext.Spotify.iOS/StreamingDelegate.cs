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
        private SPTAudioStreamingController player;

        public StreamingDelegate()
        {
            Debug.WriteLine("Registering observer");
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("sessionUpdated"), (NSNotification obj) =>
            {
                Debug.WriteLine("Got notif");
                var auth = SPTAuth.DefaultInstance;

                if (this.player == null) {
                    NSError error = null;
                    this.player = SPTAudioStreamingController.SharedInstance();

                    Debug.WriteLine("Is Player Good? " + (this.player != null));

                    // TODO Caching needs to be enabled here?
                    //bool success = this.player.StartWithClientId(auth.ClientID, out error);
                    bool success = this.player.StartWithClientId(auth.ClientID, null, true, out error);
                    if (success) {
                        this.player.Delegate = this;
                        this.player.PlaybackDelegate = new StreamingPlaybackDelegate();
                        // this.player.DiskCache = 1024 * 1024 * 64;  // TODO Caching
                        this.player.DiskCache = new SPTDiskCache(1024 * 1024 * 64);
                        this.player.LoginWithAccessToken(auth.Session.AccessToken);
                    } else {
                        this.player = null;
                        // TODO
                        Debug.WriteLine("Error init " + error.Description);
                    }
                }
            });
        }

        public override void AudioStreamingDidLogin(SPTAudioStreamingController audioStreaming)
        {
            Debug.WriteLine("Is Player Good 2? " + (this.player != null));

            this.player.PlaySpotifyURI("spotify:track:40pEs6TZQUjoyb7YLNCLbo", 0, 0, (NSError error1) =>
                       {
                           if (error1 != null) {
                               Debug.WriteLine("Err Playing: " + error1.DebugDescription);
                           }
                       });
            this.player.SetVolume(1, (arg0) => { });
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

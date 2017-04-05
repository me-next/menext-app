﻿using System;
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
        private SpotifyMusicServiceIos service;

        // TODO: Disabling for now because it breaks some songs
        // ex "G String Tuning Note" often doesn't work w/ cache
        public const bool SPOTIFY_CACHE_ENABLED = false;

        public StreamingDelegate(SpotifyMusicServiceIos service)
        {
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("sessionUpdated"), (NSNotification obj) =>
            {
                Debug.WriteLine("Got session updated notification", "stream");
                this.service = service;
                var auth = SPTAuth.DefaultInstance;

                if (this.Player == null) {
                    NSError error = null;
                    this.Player = SPTAudioStreamingController.SharedInstance();

                    Debug.WriteLine("Player is " + this.Player == null ? "Null" : "Not null", "stream");

                    bool success = this.Player.StartWithClientId(auth.ClientID, null, SPOTIFY_CACHE_ENABLED, out error);
                    if (success) {
                        this.Player.Delegate = this;
                        this.Player.PlaybackDelegate = new StreamingPlaybackDelegate(service);
                        if (SPOTIFY_CACHE_ENABLED) {
                            Debug.WriteLine("Spotify cache is enabled", "stream");
                            this.Player.DiskCache = new SPTDiskCache(1024 * 1024 * 64);
                        } else {
                            Debug.WriteLine("Spotify cache is disabled", "stream");
                        }

                    } else {
                        this.Player = null;
                        Debug.WriteLine("*** Error: " + error.Description, "stream");
                        return;
                    }
                } else {
                    Debug.WriteLine("Player was already not null", "stream");
                }

                Debug.WriteLine("Trying to login with access token", "stream");
                this.Player.LoginWithAccessToken(auth.Session.AccessToken);

                this.service.SomethingChanged();
            });
        }

        public override void AudioStreamingDidLogin(SPTAudioStreamingController audioStreaming)
        {
            Debug.Write("Logged in successfully!", "stream");

            this.Player.SetVolume(1, (arg0) => { });
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            AVAudioSession.SharedInstance().SetActive(true);

            this.service.SomethingChanged();
        }

        public override void AudioStreamingDidLogout(SPTAudioStreamingController audioStreaming)
        {
            this.service.SomethingChanged();
        }

        public override void AudioStreamingDidReceiveError(SPTAudioStreamingController audioStreaming, NSError error)
        {
            Debug.WriteLine("*** Error: " + error.Description, "stream");
        }

        public override void AudioStreamingDidReceiveMessage(SPTAudioStreamingController audioStreaming, string message)
        {
            Debug.WriteLine("Message: " + message, "stream");
        }
    }
}

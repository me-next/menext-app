using System;
using System.Collections.Generic;
using System.Diagnostics;
using AVFoundation;
using Foundation;
using MediaPlayer;
using MeNext.MusicService;
using MeNext.Spotify.iOS.Auth;
using MeNext.Spotify.iOS.Playback;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    // VERSION: Spotify iOS SDK Beta 25
    // Based mostly on https://github.com/spotify/ios-sdk/tree/2db9f565b45e683b4bb62c1ee1bdc34660f07c8f/Demo%20Projects/Simple%20Track%20Playback/Simple%20Track%20Playback
    // With a special mention of https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    /// <summary>
    /// Spotify music service for IOS.
    /// </summary>
    public class SpotifyMusicServiceIos : SpotifyMusicService
    {
        private readonly StreamingDelegate sd;
        private ISong playingSong;

        public SpotifyMusicServiceIos()
        {
            // This needs to happen BEFORE auth slash streaming delegate, so if they login with a pre-existing session
            // we get the memo and can pass it on to the existing web api.
            this.setupTokenUpdater();

            sd = new SpotifyAuth(this).CreateStreamingDelegate();
        }

        private void setupTokenUpdater()
        {
            // If we update our session, we need to create a new web api to use
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("sessionUpdated"), (NSNotification obj) =>
              {
                  var auth = SPTAuth.DefaultInstance;
                  this.SpotifyToken.UpdateAccessToken(auth.Session.AccessToken);
              });

            // TODO: Handle situation where session is invalidated
        }

        /// <summary>
        /// Checks if we are capable of playing at this very moment
        /// </summary>
        /// <value><c>true</c> if can play now; otherwise, <c>false</c>.</value>
        private bool CanPlayNow
        {
            get
            {
                return CanPlay;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:MeNext.Spotify.iOS.SpotifyMusicServiceIos"/> is playing.
        /// </summary>
        /// <value><c>true</c> if playing; otherwise, <c>false</c>.</value>
        public override bool Playing
        {
            get
            {
                if (this.CanPlayNow && this.sd.Player.PlaybackState != null) {
                    return this.sd.Player.PlaybackState.IsPlaying;
                }
                return false;
            }

            set
            {
                if (this.CanPlayNow
                    && this.sd.Player.PlaybackState != null
                    && this.sd.Player.PlaybackState.IsPlaying != value) {
                    this.sd.Player.SetIsPlaying(value, (NSError error) =>
                    {
                        if (error != null) {
                            Debug.WriteLine("Error setting playing to " + value + ": " + error.DebugDescription, "service");
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Gets or sets the playing position in seconds.
        /// </summary>
        /// <value>The playing position.</value>
        public override double PlayingPosition
        {
            get
            {
                if (this.CanPlayNow && this.sd.Player.PlaybackState != null) {
                    return this.sd.Player.PlaybackState.Position;
                }
                return 0.0;
            }

            set
            {
                if (this.CanPlayNow) {
                    this.sd.Player.SeekTo(value, (NSError error) =>
                    {
                        if (error != null) {
                            Debug.WriteLine("Error seeking to " + value + ": " + error.DebugDescription, "service");
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Gets the currently playing song.
        /// </summary>
        /// <value>The playing song.</value>
        public override ISong PlayingSong
        {
            get
            {
                return playingSong;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.iOS.SpotifyMusicServiceIos"/> is logged in.
        /// </summary>
        /// <value><c>true</c> if logged in; otherwise, <c>false</c>.</value>
        public override bool LoggedIn
        {
            get
            {
                return (this.sd.Player != null && this.sd.Player.LoggedIn);
            }
        }

        /// <summary>
        /// Gets or sets the volume on a scale of 0-1.
        /// </summary>
        /// <value>The volume.</value>
        public override double Volume
        {
            get
            {
                if (this.CanPlayNow) {
                    return this.sd.Player.Volume;
                }
                return 0d;
            }

            set
            {
                Debug.Assert(value >= 0 && value <= 1);
                if (this.CanPlayNow) {
                    this.sd.Player.SetVolume(value, (NSError error) => { });
                }
            }
        }

        /// <summary>
        /// Plays a song at given position in seconds.
        /// </summary>
        /// <param name="song">Song.</param>
        /// <param name="position">Position. Defaults to beginning = 0.</param>
        public override void PlaySong(ISong song, double position = 0)
        {
            if (song == null) {
                return;
            }
            Debug.WriteLine("Trying to play song: " + song.Name, "service");
            if (this.CanPlayNow) {
                sd.Player.PlaySpotifyURI(song.UniqueId, 0, position, (NSError error1) =>
                       {
                           if (error1 != null) {
                               this.playingSong = null;
                               Debug.WriteLine("*** Error Playing: " + error1.DebugDescription, "service");
                           } else {
                               this.playingSong = song;
                               Debug.WriteLine("Seemingly successfully played song", "service");
                           }
                       });
            } else {
                Debug.WriteLine("Whoops, can't play right now. Not logged in.", "service");
            }
        }

        public override bool LoginAvailable
        {
            get
            {
#if DEBUG
                return true;
#else
                return SpotifyAuth.HasSpotifyApp;
#endif
            }
        }

        /// <summary>
        /// Login to spotify.
        /// </summary>
        public override void Login()
        {
            Debug.WriteLine("Login request received in music service", "auth");
            SpotifyAuth.Login();
        }

        /// <summary>
        /// Logout of spotify.
        /// </summary>
        public override void Logout()
        {
            Debug.WriteLine("Logout request received in music service", "auth");
            if (this.LoggedIn) {
                this.sd.Player.Logout();
                Debug.WriteLine("Logged out.", "auth");
            } else {
                Debug.WriteLine("We weren't actually logged in though...", "auth");
            }
        }

        /// <summary>
        /// Allows the host to hear music playing.
        /// </summary>
        /// <param name="isHost">If set to <c>true</c> is host.</param>
        public override void SetIsHost(bool isHost)
        {
            if (isHost) {
                NSError error;
                Debug.WriteLine("Activated playback");
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                AVAudioSession.SharedInstance().SetActive(true, out error);
                if (error != null) {
                    Debug.WriteLine("*** ERROR SETTING ACTIVE: " + error.Description);
                }
                UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();
            } else {
                MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = new MPNowPlayingInfo();
                UIApplication.SharedApplication.EndReceivingRemoteControlEvents();
                AVAudioSession.SharedInstance().SetActive(false);
            }
        }
    }
}

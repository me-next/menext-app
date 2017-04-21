using System;
using MeNext.MusicService;

using Com.Spotify.Sdk.Android;
using Android.App;
using Android.Content;
using Com.Spotify.Sdk.Android.Player;

namespace MeNext.Spotify.Droid
{
    // Auth library: 1.0 (aar came from samples in Playback lib)
    // https://github.com/spotify/android-auth/releases
    //
    // Playback library: 24-noconnect-2.20b
    // https://github.com/spotify/android-sdk/releases
    /// <summary>
    /// Handles the Spotify music service on Android
    /// </summary>
    public class SpotifyMusicServiceDroid : SpotifyMusicService
    {
        private PlayerListener listener;
        internal Activity mainActivity;
        private ISong playingSong;
        private string lastPlayerAccessToken;

        // These are used for delaying a play until the player is ready during a token refresh
        private ISong delayedSong = null;
        private double delayedPosition = 0;
        internal bool DelayingSongPlay { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Spotify music service droid class.
        /// </summary>
        /// <param name="mainActivity">Main activity.</param>
        public SpotifyMusicServiceDroid(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
            this.listener = new PlayerListener(this, mainActivity);
        }

        /// <summary>
        /// Lets us know what the actual current access token is.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        internal void OnNewAccessToken(string accessToken)
        {
            this.lastPlayerAccessToken = accessToken;
        }

        // These guys pass on app status info from beyond
        // TODO Use a listener interface instead

        public void OnResume()
        {
            this.listener.OnResume();
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            this.listener.OnActivityResult(requestCode, resultCode, data);
        }

        public void OnPause()
        {
            this.listener.OnPause();
        }

        public void OnDestroy()
        {
            this.listener.OnDestroy();
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>The player.</value>
        private SpotifyPlayer Player
        {
            get
            {
                return this.listener.Player;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.Droid.SpotifyMusicServiceDroid"/> logged in.
        /// </summary>
        /// <value><c>true</c> if logged in; otherwise, <c>false</c>.</value>
        public override bool LoggedIn
        {
            get
            {
                return (this.Player != null && this.Player.IsLoggedIn);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:MeNext.Spotify.Droid.SpotifyMusicServiceDroid"/>
        /// is playing a song.
        /// </summary>
        /// <value><c>true</c> if playing; otherwise, <c>false</c>.</value>
        public override bool Playing
        {
            get
            {
                return (this.Player != null && this.Player.PlaybackState.IsPlaying);
            }

            set
            {
                if (this.Player != null && this.Player.PlaybackState.IsPlaying != value) {
                    if (value) {
                        this.Player.Resume(this.listener.operationCallback);
                    } else {
                        this.Player.Pause(this.listener.operationCallback);
                    }
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
                if (this.Player != null) {
                    return this.Player.PlaybackState.PositionMs / 1000.0;
                }
                return 0.0;
            }

            set
            {
                if (this.Player != null) {
                    this.Player.SeekToPosition(this.listener.operationCallback, (int) (value * 1000));
                }
            }
        }

        /// <summary>
        /// Gets the playing song.
        /// </summary>
        /// <value>The playing song.</value>
        public override ISong PlayingSong
        {
            get
            {
                return playingSong;
            }
        }

        private double _volume;

        /// <summary>
        /// Gets or sets the volume on a scale from 0-1.
        /// </summary>
        /// <value>The volume.</value>
        public override double Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                this._volume = value;
                this.listener.SetVolume(value);
            }
        }

        public override void Login()
        {
            this.listener.OpenLoginWindow();
        }

        public override void Logout()
        {
            this.SpotifyToken.NukeToken();
            this.Player.Logout();
        }

        /// <summary>
        /// Plays a song. Defaults to the start of the song.
        /// </summary>
        /// <param name="song">Song to play.</param>
        /// <param name="position">Position in song.</param>
        public override void PlaySong(ISong song, double position = 0)
        {
            if (song == null) {
                return;
            }
            if (this.LoggedIn) {
                this.delayedSong = song;
                this.delayedPosition = position;
                this.DelayingSongPlay = true;
                var updated = this.UpdatePlayerToken();
                if (!updated) {
                    this.ActuallyPlaySong();
                } else {
                    // The UpdatePlayerToken flow will (should) eventually call ActuallyPlaySong().
                }
            }
        }

        /// <summary>
        /// If the access token has changed, logout and re-login the player with the new token.
        /// This function relies on side effects from PlaySong(..). Do not expect it to work when called from any other
        /// method.
        /// </summary>
        private bool UpdatePlayerToken()
        {
            if (this.lastPlayerAccessToken != this.SpotifyToken.AccessToken) {
                // The rest of this flow takes place in PlayerListener.OnLoggedOut()
                this.Player.Logout();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Plays the currently delayed song, if one exists
        /// </summary>
        internal void ActuallyPlaySong()
        {
            if (this.delayedSong != null) {
                this.Player.PlayUri(this.listener.operationCallback, this.delayedSong.UniqueId, 0, (int) (this.delayedPosition * 1000));
                this.playingSong = this.delayedSong;
                this.delayedSong = null;
                this.delayedPosition = 0;
                this.DelayingSongPlay = false;
            }
        }

        public override void SetIsHost(bool isHost)
        {
            // TODO: Remote
        }
    }
}

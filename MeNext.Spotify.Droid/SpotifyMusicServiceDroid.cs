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
    public class SpotifyMusicServiceDroid : SpotifyMusicService
    {
        private PlayerListener listener;
        internal Activity mainActivity;
        private ISong playingSong;

        /// <summary>
        /// Initializes a new instance of the Spotify music service droid class.
        /// </summary>
        /// <param name="mainActivity">Main activity.</param>
        public SpotifyMusicServiceDroid(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
            this.listener = new PlayerListener(this);
        }

        internal void OnNewAccessToken(string accessToken)
        {
            this.webApi.updateAccessToken(accessToken);
        }

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

        /// <summary>
        /// Gets or sets the volume on a scale from 0-1.
        /// </summary>
        /// <value>The volume.</value>
        public override double Volume
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Login()
        {
            this.listener.OpenLoginWindow();
        }

        public override void Logout()
        {
            this.Player.Logout();
        }

        /// <summary>
        /// Plays the song. Defaults to beginning
        /// </summary>
        /// <param name="song">Song to play.</param>
        /// <param name="position">Position in song.</param>
        public override void PlaySong(ISong song, double position = 0)
        {
            if (this.Player != null) {
                this.Player.PlayUri(this.listener.operationCallback, song.UniqueId, 0, (int) (position * 1000));
                this.playingSong = song;
            }
        }

        public override void SetIsHost(bool isHost)
        {
            // TODO: Do something?
        }
    }
}

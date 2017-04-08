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

        public SpotifyMusicServiceDroid(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
            this.listener = new PlayerListener(this);
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

        private SpotifyPlayer Player
        {
            get
            {
                return this.listener.Player;
            }
        }

        public override bool LoggedIn
        {
            get
            {
                return (this.Player != null && this.Player.IsLoggedIn);
            }
        }

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

        public override ISong PlayingSong
        {
            get
            {
                return playingSong;
            }
        }

        public override void Login()
        {
            this.listener.OpenLoginWindow();
        }

        public override void Logout()
        {
            // TODO
            throw new NotImplementedException();
        }

        public override void PlaySong(ISong song, double position = 0)
        {
            if (this.Player != null) {
                this.Player.PlayUri(this.listener.operationCallback, song.UniqueId, 0, (int) (position * 1000));
                this.playingSong = song;
            }
        }
    }
}

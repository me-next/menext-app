using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using MeNext.MusicService;
using MeNext.Spotify.iOS.Auth;
using MeNext.Spotify.iOS.Playback;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    // TODO Document
    // Based heavily on https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    // and https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    public class SpotifyMusicService : IMusicService
    {
        private readonly List<IMusicServiceListener> listeners = new List<IMusicServiceListener>();

        private StreamingDelegate sd;

        private ISong playingSong;

        public SpotifyMusicService()
        {
            sd = new SpotifySetup().CreateStreamingDelegate();
        }

        /// <summary>
        /// Checks if we are capable of playing at this very moment
        /// </summary>
        /// <value><c>true</c> if can play now; otherwise, <c>false</c>.</value>
        private bool CanPlayNow
        {
            get
            {
                return LoggedIn;
            }
        }

        // =========================== //
        public bool CanPlay
        {
            get
            {
                // TODO: Check if the user has premium
                return LoggedIn;
            }
        }

        public bool CanSearch
        {
            get
            {
                // Any user can search (in principle)
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "Spotify";
            }
        }

        public bool Playing
        {
            get
            {
                if (this.CanPlayNow) {
                    return this.sd.Player.PlaybackState.IsPlaying;
                }
                return false;
            }

            set
            {
                if (this.CanPlayNow && this.sd.Player.PlaybackState.IsPlaying != value) {
                    // TODO Handle error
                    this.sd.Player.SetIsPlaying(value, (NSError arg0) => { });
                }
            }
        }

        public double PlayingPosition
        {
            get
            {
                if (this.CanPlayNow) {
                    return this.sd.Player.PlaybackState.Position;
                }
                return 0.0;
            }

            set
            {
                if (this.CanPlayNow) {
                    // TODO Handle error
                    this.sd.Player.SeekTo(value, (NSError arg0) => { });
                }
            }
        }

        public ISong PlayingSong
        {
            get
            {
                return playingSong;
            }
        }

        public bool HasUserLibrary
        {
            get
            {
                // TODO: Implement user library
                return false;
            }
        }

        public IResultList<IAlbum> UserLibraryAlbums
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        public IResultList<IArtist> UserLibraryArtists
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        public IResultList<IPlaylist> UserLibraryPlaylists
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        public bool LoggedIn
        {
            get
            {
                return (this.sd.Player != null && this.sd.Player.LoggedIn);
            }
        }
        public IAlbum GetAlbum(string uid)
        {
            return new SpotifyAlbum(uid);
        }

        public IArtist GetArtist(string uid)
        {
            return new SpotifyArtist(uid);
        }

        public IPlaylist GetPlaylist(string uid)
        {
            return new SpotifyPlaylist(uid);
        }

        public ISong GetSong(string uid)
        {
            return new SpotifySong(uid);
        }

        public void PlaySong(ISong song, double position = 0)
        {
            sd.Player.PlaySpotifyURI(song.UniqueId, 0, position, (NSError error1) =>
                       {
                           if (error1 != null) {
                               this.playingSong = null;
                               // TODO Handle error
                               Debug.WriteLine("Err Playing: " + error1.DebugDescription);
                           } else {
                               this.playingSong = song;
                           }
                       });
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            // TODO: Realify
            var result = new List<IAlbum>();
            result.Add(new SpotifyAlbum("spotify:album:0zdZSyxWaYmaRMPeUHcG1K"));
            result.Add(new SpotifyAlbum("spotify:album:7Mh7Q5DQIE9evMeGrKHjg8"));
            result.Add(new SpotifyAlbum("spotify:album:7uMMbwF64xfNT8VpAkbJAE"));
            return new SpotifySimpleResultList<IAlbum>(result);
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            // TODO: Realify
            var artists = new List<IArtist>();
            artists.Add(new SpotifyArtist("spotify:artist:5ksRONqssB7BR161NTtJAm"));
            artists.Add(new SpotifyArtist("spotify:artist:0p4nmQO2msCgU4IF37Wi3j"));
            artists.Add(new SpotifyArtist("spotify:artist:5rSXSAkZ67PYJSvpUpkOr7"));
            return new SpotifySimpleResultList<IArtist>(artists);
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            // TODO: Realify
            var result = new List<IPlaylist>();
            result.Add(new SpotifyPlaylist("spotify:user:drdanielfc:playlist:2kEtVOI82nPRSCXNO4xAd2"));
            result.Add(new SpotifyPlaylist("spotify:user:drdanielfc:playlist:4dfYJCt7vfTjBM3qdyHNUb"));
            result.Add(new SpotifyPlaylist("spotify:user:spotify:playlist:0UHbhCjKlXnFfv98bKrtKv"));
            return new SpotifySimpleResultList<IPlaylist>(result);
        }

        public IResultList<ISong> SearchSong(string query)
        {
            // TODO: Realify
            var result = new List<ISong>();
            result.Add(new SpotifySong("spotify:track:2Ml0l8YWJLQhPrRDLpQaDM"));
            result.Add(new SpotifySong("spotify:track:1BwaPm2VjiOenzjW1TOZuW"));
            result.Add(new SpotifySong("spotify:track:5USZyz6dnBEn1oLsKcAKQy"));
            result.Add(new SpotifySong("spotify:track:0mBL2JwjNYKtdFacHxvtJt"));
            result.Add(new SpotifySong("spotify:track:4EveU9Zb50mjgi5avDNqlK"));
            return new SpotifySimpleResultList<ISong>(result);
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            // No way to do this in the backend yet
            // Just do nothing
        }

        public void Login()
        {
            SpotifySetup.Login();
        }

        public void Logout()
        {
            // TODO: Test
            if (this.LoggedIn) {
                this.sd.Player.Logout();
            }
        }

        public void AddStatusListener(IMusicServiceListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveStatusListener(IMusicServiceListener listener)
        {
            listeners.Remove(listener);
        }
    }
}

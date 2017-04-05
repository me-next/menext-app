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
    // Based mostly on https://github.com/spotify/ios-sdk/tree/2db9f565b45e683b4bb62c1ee1bdc34660f07c8f/Demo%20Projects/Simple%20Track%20Playback/Simple%20Track%20Playback
    // With a special mention of https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    public class SpotifyMusicServiceIos : IMusicService
    {
        private readonly List<IMusicServiceListener> listeners = new List<IMusicServiceListener>();

        private StreamingDelegate sd;
        private ISong playingSong;
        private WebApi webApi;

        public SpotifyMusicServiceIos()
        {
            // This needs to happen BEFORE auth slash streaming delegate, so if they login with a pre-existing session
            // we get the memo and can pass it on to the existing web api.
            this.setupWebApi();

            sd = new SpotifyAuth(this).CreateStreamingDelegate();

        }

        private void setupWebApi()
        {
            // Default web api without any special access
            this.webApi = new WebApi();

            // If we update our session, we need to create a new web api to use
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("sessionUpdated"), (NSNotification obj) =>
              {
                  var auth = SPTAuth.DefaultInstance;
                  this.webApi.updateAccessToken(auth.Session.AccessToken, auth.Session.TokenType);
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

        public double PlayingPosition
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
                return webApi.GetUserLibraryPlaylists();
            }
        }

        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                return webApi.GetUserLibrarySongs();
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
            return webApi.metadata.GetAlbum(uid);
        }

        public IArtist GetArtist(string uid)
        {
            return webApi.metadata.GetArtist(uid);
        }

        public IPlaylist GetPlaylist(string uid)
        {
            return webApi.metadata.GetPlaylist(uid);
        }

        public ISong GetSong(string uid)
        {
            return webApi.metadata.GetSong(uid);
        }

        public IList<ISong> GetSongs(IList<string> uids)
        {
            return webApi.metadata.GetSongs(uids);
        }

        public IList<IArtist> GetArtists(IList<string> uids)
        {
            return webApi.metadata.GetArtists(uids);
        }

        public IList<IAlbum> GetAlbums(IList<string> uids)
        {
            return webApi.metadata.GetAlbums(uids);
        }

        public IList<IPlaylist> GetPlaylists(IList<string> uids)
        {
            return webApi.metadata.GetPlaylists(uids);
        }

        public void PlaySong(ISong song, double position = 0)
        {
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

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            return webApi.SearchAlbum(query);
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            return webApi.SearchArtist(query);
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            return webApi.SearchPlaylists(query);
        }

        public IResultList<ISong> SearchSong(string query)
        {
            return webApi.SearchSong(query);
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            // No way to do this in the backend yet
            // Just do nothing
        }

        public void Login()
        {
            Debug.WriteLine("Login request received in music service", "auth");
            SpotifyAuth.Login();
        }

        public void Logout()
        {
            // TODO: Test
            Debug.WriteLine("Logout request received in music service", "auth");
            if (this.LoggedIn) {
                this.sd.Player.Logout();
                Debug.WriteLine("Logged out.", "auth");
            } else {
                Debug.WriteLine("We weren't actually logged in though...", "auth");
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

        internal void SongEnds(string uri)
        {
            var x = this.GetSong(uri);
            foreach (var l in listeners) {
                l.SongEnds(this.GetSong(uri));
            }
        }

        internal void SomethingChanged()
        {
            foreach (var l in listeners) {
                l.SomethingChanged();
            }
        }
    }
}

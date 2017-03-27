using System;
using System.Collections.Generic;
using Foundation;
using MeNext.MusicService;
using MeNext.Spotify.iOS.Auth;

namespace MeNext.Spotify.iOS
{
    /// <summary>
    /// Interfaces with the 
    /// </summary>
    public class SpotifyMusicService : IMusicService
    {
        public SpotifyMusicService()
        {
            SPTAuth auth = SPTAuth.DefaultInstance;
            auth.ClientID = "b79f545d6c24407aa6bed17af62275d6";
            auth.RedirectURL = new Foundation.NSUrl("menext-spotify://callback");
            auth.SessionUserDefaultsKey = @"current session";
            auth.RequestedScopes = new NSObject[] { SpotifyConstants.SPTAuthStreamingScope };

            Console.WriteLine("Has Spotify: " + SPTAuth.SpotifyApplicationIsInstalled);
        }

        public bool CanPlay
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanSearch
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasUserLibrary
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Playing
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

        public double PlayingPosition
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

        public ISong PlayingSong
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IAlbum> UserLibraryAlbums
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IArtist> UserLibraryArtists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IPlaylist> UserLibraryPlaylists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddPlayStatusListener(ISongPlayListener listener)
        {
            //throw new NotImplementedException();
            // TODO
        }

        public IAlbum GetAlbum(string uid)
        {
            throw new NotImplementedException();
        }

        public IArtist GetArtist(string uid)
        {
            throw new NotImplementedException();
        }

        public IPlaylist GetPlaylist(string uid)
        {
            throw new NotImplementedException();
        }

        public ISong GetSong(string uid)
        {
            throw new NotImplementedException();
        }

        public void PlaySong(ISong song, double position = 0)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayStatusListener(ISongPlayListener listener)
        {
            throw new NotImplementedException();
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<ISong> SearchSong(string query)
        {
            throw new NotImplementedException();
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            throw new NotImplementedException();
        }
    }
}

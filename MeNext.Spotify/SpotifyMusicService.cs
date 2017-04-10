using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public abstract class SpotifyMusicService : IMusicService
    {
        protected WebApi webApi { get; }
        private readonly List<IMusicServiceListener> listeners = new List<IMusicServiceListener>();

        public string ClientId
        {
            get
            {
                return "b79f545d6c24407aa6bed17af62275d6";
            }
        }

        public string SpotifyCallback
        {
            get
            {
                return "menext-spotify://callback";
            }
        }

        protected SpotifyMusicService()
        {
            // Default web api without any special access
            this.webApi = new WebApi();
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

        public void AddStatusListener(IMusicServiceListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveStatusListener(IMusicServiceListener listener)
        {
            listeners.Remove(listener);
        }

        public void SongEnds(string uri)
        {
            var x = this.GetSong(uri);
            foreach (var l in listeners) {
                l.SongEnds(x);
            }
        }

        public void SomethingChanged()
        {
            foreach (var l in listeners) {
                l.SomethingChanged();
            }
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            // No way to do this in the backend yet
            // Jothing
        }

        public string Name
        {
            get
            {
                return "Spotify";
            }
        }

        public bool HasUserLibrary
        {
            get
            {
                return LoggedIn;
            }
        }

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

        public abstract bool LoggedIn { get; }
        public abstract bool Playing { get; set; }
        public abstract double PlayingPosition { get; set; }
        public abstract ISong PlayingSong { get; }
        public abstract double Volume { get; set; }

        public abstract void Login();
        public abstract void Logout();
        public abstract void PlaySong(ISong song, double position = 0);
    }
}

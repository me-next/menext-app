using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// The Spotify music service class links the Spotify user to MeNext.
    /// </summary>
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

        /// <summary>
        /// Gets the user's library albums.
        /// </summary>
        /// <value>The user's library albums.</value>
        public IResultList<IAlbum> UserLibraryAlbums
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the user's library artists.
        /// </summary>
        /// <value>The user library artists.</value>
        public IResultList<IArtist> UserLibraryArtists
        {
            get
            {
                // TODO: Implement user library
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the user's library playlists.
        /// </summary>
        /// <value>The user's library playlists.</value>
        public IResultList<IPlaylist> UserLibraryPlaylists
        {
            get
            {
                return webApi.GetUserLibraryPlaylists();
            }
        }

        /// <summary>
        /// Gets the user's library songs.
        /// </summary>
        /// <value>The user's library songs.</value>
        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                return webApi.GetUserLibrarySongs();
            }
        }

        /// <summary>
        /// Gets an album.
        /// </summary>
        /// <returns>The album.</returns>
        /// <param name="uid">album id.</param>
        public IAlbum GetAlbum(string uid)
        {
            return webApi.metadata.GetAlbum(uid);
        }

        /// <summary>
        /// Gets an artist.
        /// </summary>
        /// <returns>The artist.</returns>
        /// <param name="uid">artist id.</param>
        public IArtist GetArtist(string uid)
        {
            return webApi.metadata.GetArtist(uid);
        }

        /// <summary>
        /// Gets a playlist.
        /// </summary>
        /// <returns>The playlist.</returns>
        /// <param name="uid">playlist id.</param>
        public IPlaylist GetPlaylist(string uid)
        {
            return webApi.metadata.GetPlaylist(uid);
        }

        /// <summary>
        /// Gets a song.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">song id.</param>
        public ISong GetSong(string uid)
        {
            return webApi.metadata.GetSong(uid);
        }

        /// <summary>
        /// Gets a list of songs.
        /// </summary>
        /// <returns>The songs.</returns>
        /// <param name="uids">list of song Ids.</param>
        public IList<ISong> GetSongs(IList<string> uids)
        {
            return webApi.metadata.GetSongs(uids);
        }

        /// <summary>
        /// Gets a list of artists.
        /// </summary>
        /// <returns>The artists.</returns>
        /// <param name="uids">list of artist ids.</param>
        public IList<IArtist> GetArtists(IList<string> uids)
        {
            return webApi.metadata.GetArtists(uids);
        }

        /// <summary>
        /// Gets a list of albums.
        /// </summary>
        /// <returns>The albums.</returns>
        /// <param name="uids">list of album ids> .</param>
        public IList<IAlbum> GetAlbums(IList<string> uids)
        {
            return webApi.metadata.GetAlbums(uids);
        }

        /// <summary>
        /// Gets a list of playlists.
        /// </summary>
        /// <returns>The playlists.</returns>
        /// <param name="uids">list of playlist ids.</param>
        public IList<IPlaylist> GetPlaylists(IList<string> uids)
        {
            return webApi.metadata.GetPlaylists(uids);
        }

        /// <summary>
        /// Searches for albums.
        /// </summary>
        /// <returns>The album.</returns>
        /// <param name="query">Query for album.</param>
        public IResultList<IAlbum> SearchAlbum(string query)
        {
            return webApi.SearchAlbum(query);
        }

        /// <summary>
        /// Searches for artists.
        /// </summary>
        /// <returns>The artist.</returns>
        /// <param name="query">Query for artist.</param>
        public IResultList<IArtist> SearchArtist(string query)
        {
            return webApi.SearchArtist(query);
        }

        /// <summary>
        /// Searches for playlists.
        /// </summary>
        /// <returns>The playlist.</returns>
        /// <param name="query">Query for playlist.</param>
        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            return webApi.SearchPlaylists(query);
        }

        /// <summary>
        /// Searches for songs.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="query">Query for song.</param>
        public IResultList<ISong> SearchSong(string query)
        {
            return webApi.SearchSong(query);
        }

        /// <summary>
        /// Adds a status listener.
        /// </summary>
        /// <param name="listener">Listener to add.</param>
        public void AddStatusListener(IMusicServiceListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Removes a status listener.
        /// </summary>
        /// <param name="listener">Listener to remove.</param>
        public void RemoveStatusListener(IMusicServiceListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Updates listeners when song ends
        /// </summary>
        /// <param name="uri">Song's id.</param>
        public void SongEnds(string uri)
        {
            var x = this.GetSong(uri);
            foreach (var l in listeners) {
                l.SongEnds(x);
            }
        }

        /// <summary>
        /// Something has changed so update UI accordingly.
        /// </summary>
        public void SomethingChanged()
        {
            foreach (var l in listeners) {
                l.MusicServiceChange();
            }
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            // No way to do this in the backend yet
            // Jothing
        }

        /// <summary>
        /// Gets the name of the service
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return "Spotify";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.SpotifyMusicService"/> has user library.
        /// </summary>
        /// <value><c>true</c> if has user library; otherwise, <c>false</c>.</value>
        public bool HasUserLibrary
        {
            get
            {
                return LoggedIn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.SpotifyMusicService"/> can play music.
        /// </summary>
        /// <value><c>true</c> if can play; otherwise, <c>false</c>.</value>
        public bool CanPlay
        {
            get
            {
                // TODO: Check if the user has premium
                return LoggedIn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.SpotifyMusicService"/> can search.
        /// </summary>
        /// <value><c>true</c> if can search; otherwise, <c>false</c>.</value>
        public bool CanSearch
        {
            get
            {
                // Any user can search (in principle)
                return true;
            }
        }

        /// <summary>
        /// Gets recommendations based on a set of songs.
        /// </summary>
        /// <returns>The recommendations.</returns>
        /// <param name="seeds">Seeds.</param>
        public IList<ISong> GetRecommendations(IList<ISong> seeds)
        {
            return this.webApi.GetRecommendations(seeds);
        }

        public abstract bool LoggedIn { get; }
        public abstract bool Playing { get; set; }
        public abstract double PlayingPosition { get; set; }
        public abstract ISong PlayingSong { get; }
        public abstract double Volume { get; set; }

        public abstract void Login();
        public abstract void Logout();
        public abstract void PlaySong(ISong song, double position = 0);

        public abstract void SetIsHost(bool isHost);
    }
}

using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Represents a service which can play music
    /// </summary>
    public interface IMusicService
    {
        /// <summary>
        /// Gets the name of this service
        /// </summary>
        /// <value>The name of this service (ex Spotify).</value>
        String Name
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:com.danielcentore.MusicService.IMusicService"/> can
        /// play music
        /// </summary>
        /// <value><c>true</c> if can play; otherwise, <c>false</c>.</value>
        bool CanPlay
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:com.danielcentore.MusicService.IMusicService"/> can
        /// search for music.
        /// </summary>
        /// <value><c>true</c> if can search; otherwise, <c>false</c>.</value>
        bool CanSearch
        {
            get;
        }

        /// <summary>
        /// Searches for songs
        /// </summary>
        /// <returns>The list of songs.</returns>
        /// <param name="query">A string query.</param>
        IResultList<ISong> SearchSong(String query);

        /// <summary>
        /// Searches for artists
        /// </summary>
        /// <returns>The list of artists.</returns>
        /// <param name="query">A string query.</param>
        IResultList<IArtist> SearchArtist(String query);

        /// <summary>
        /// Searches for albums
        /// </summary>
        /// <returns>The list of artists.</returns>
        /// <param name="query">A string query.</param>
        IResultList<IAlbum> SearchAlbum(String query);

        /// <summary>
        /// Searches for playlists
        /// </summary>
        /// <returns>The list of playlists.</returns>
        /// <param name="query">A string query.</param>
        IResultList<IPlaylist> SearchPlaylists(String query);


        /// <summary>
        /// Gets a value indicating whether this <see cref="T:com.danielcentore.MusicService.IMusicService"/> provides
        /// access to a user library
        /// </summary>
        /// <value><c>true</c> if has user library; otherwise, <c>false</c>.</value>
        bool HasUserLibrary
        {
            get;
        }

        /// <summary>
        /// Gets a song based on its univeral id.
        /// Null if one does not exist.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">Uid.</param>
        ISong GetSong(String uid);

        /// <summary>
        /// Gets an artist based on its univeral id.
        /// Null if one does not exist.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">Uid.</param>
        IArtist GetArtist(String uid);

        /// <summary>
        /// Gets an album based on its univeral id.
        /// Null if one does not exist.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">Uid.</param>
        IAlbum GetAlbum(String uid);

        /// <summary>
        /// Gets a playlist based on its univeral id.
        /// Null if one does not exist.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">Uid.</param>
        IPlaylist GetPlaylist(String uid);

        /// <summary>
        /// Gets the user library songs.
        /// </summary>
        /// <value>The user library songs.</value>
        IResultList<ISong> UserLibrarySongs
        {
            get;
        }

        /// <summary>
        /// Gets the user library artists.
        /// </summary>
        /// <value>The user library artists.</value>
        IResultList<IArtist> UserLibraryArtists
        {
            get;
        }

        /// <summary>
        /// Gets the user library albums.
        /// </summary>
        /// <value>The user library albums.</value>
        IResultList<IAlbum> UserLibraryAlbums
        {
            get;
        }

        /// <summary>
        /// Gets the user library playlists.
        /// </summary>
        /// <value>The user library playlists.</value>
        IResultList<IPlaylist> UserLibraryPlaylists
        {
            get;
        }

        /// <summary>
        /// Plays a song. If the song is already playing, this behaves like a seek.
        /// </summary>
        /// <param name="song">Song.</param>
        /// <param name="position">Where to begin playing, in seconds</param>
        void PlaySong(ISong song, double position = 0);

        /// <summary>
        /// Adds a listener for when things change in the music service's status
        /// </summary>
        /// <param name="listener">Listener.</param>
        void AddStatusListener(IMusicServiceListener listener);

        /// <summary>
        /// Removes a listener for when things change in the music service's status
        /// </summary>
        /// <param name="listener">Listener.</param>
        void RemoveStatusListener(IMusicServiceListener listener);

        /// <summary>
        /// Gets the currently playing song.
        /// </summary>
        /// <value>The playing song.</value>
        ISong PlayingSong
        {
            get;
        }

        /// <summary>
        /// The current song's position. Setting it seeks.
        /// </summary>
        /// <value>The playing position.</value>
        double PlayingPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current song is playing (vs paused).
        /// </summary>
        /// <value><c>true</c> if playing; otherwise, <c>false</c>.</value>
        bool Playing
        {
            get;
            set;
        }

        /// <summary>
        /// Suggests that the list of songs be buffered.
        /// This cancels any existing buffer requests.
        /// This is just a performance suggestion so the implementation can optimise music playing. It might do nothing.
        /// </summary>
        /// <param name="songs">Songs.</param>
        void SuggestBuffer(List<ISong> songs);

        /// <summary>
        /// Checks if the current user is logged in to the service
        /// </summary>
        /// <value><c>true</c> if logged in; otherwise, <c>false</c>.</value>
        bool LoggedIn { get; }

        /// <summary>
        /// Requests that the user be logged in or re-logged in to the service
        /// </summary>
        void Login();

        /// <summary>
        /// Requests that the user be logged out of the service
        /// </summary>
        void Logout();
    }
}

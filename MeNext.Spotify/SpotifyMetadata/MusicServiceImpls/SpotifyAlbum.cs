using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Class that represents an album.
    /// </summary>
    public class SpotifyAlbum : IAlbum, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 20;
        private const string ENDPOINT_MULTIPLE = "albums";

        private MetadataFactory factory;

        private string uri;
        private List<string> songUids;
        private string name;
        /// <summary>
        /// Initializes a new instance of the Spotify album class.
        /// </summary>
        /// <param name="factory">Factory.</param>
        /// <param name="result">Result.</param>
        internal SpotifyAlbum(MetadataFactory factory, AlbumResult result)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.songUids = new List<string>();
            foreach (var track in result.tracks.items) {
                this.songUids.Add(track.uri);
            }
            this.name = result.name;

            // Submit items for caching
            foreach (var track in result.tracks.items) {
                this.factory.CacheSubmit(track);
            }
            foreach (var artist in result.artists) {
                this.factory.CacheSubmit(artist);
            }
        }

        internal SpotifyAlbum(MetadataFactory factory, PartialAlbumResult result)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.songUids = null;
            this.name = result.name;
        }

        /// <summary>
        /// Gets the name of the album.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets a list of the album's songs.
        /// </summary>
        /// <value>The songs.</value>
        public List<ISong> Songs
        {
            get
            {
                // TODO: If this is null we need to obtain them
                return factory.GetSongs(songUids);
            }
        }

        /// <summary>
        /// Gets the album's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        /// <summary>
        /// Gets the album's artwork.
        /// </summary>
        /// <returns>The album art.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public IImage GetAlbumArt(int width, int height)
        {
            // TODO
            return null;
        }

        /// <summary>
        /// Obtains a list of albums.
        /// </summary>
        /// <returns>The albums.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="sids">Sids is a queue of album ids.</param>
        internal static List<SpotifyAlbum> ObtainAlbums(MetadataFactory factory, Queue<string> sids)
        {
            var result = new List<SpotifyAlbum>();
            var items = factory.ObtainThings<AlbumsResult, AlbumResult>(sids, MAX_RESULTS_PER_QUERY, ENDPOINT_MULTIPLE);

            foreach (var item in items) {
                result.Add(new SpotifyAlbum(factory, item));
            }

            return result;
        }
    }
}

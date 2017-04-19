using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Class that represents an artist.
    /// </summary>
    public class SpotifyArtist : IArtist, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 50;
        private const string ENDPOINT_MULTIPLE = "artists";

        private MetadataFactory factory;
        private string name;
        private string uri;
        private List<string> albumUids;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.SpotifyArtist"/> class.
        /// </summary>
        /// <param name="factory">Factory.</param>
        /// <param name="result">Result.</param>
        internal SpotifyArtist(MetadataFactory factory, ArtistResult result)
        {
            this.factory = factory;
            this.name = result.name;
            this.uri = result.uri;
            this.albumUids = null;
        }

        public SpotifyArtist(MetadataFactory factory, PartialArtistResult result)
        {
            this.factory = factory;
            this.name = result.name;
            this.uri = result.uri;
            this.albumUids = null;
        }

        /// <summary>
        /// Gets the artist's albums.
        /// </summary>
        /// <value>The albums.</value>
        public List<IAlbum> Albums
        {
            get
            {
                // TODO Implement
                // https://developer.spotify.com/web-api/get-artists-albums/

                throw new NotImplementedException();


                //if (albumUids == null) {
                //    // We have not yet tried to obtain the albums for this artist

                //}
            }
        }

        /// <summary>
        /// Gets the artist's name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the artist's unique identifier.
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
        /// Gets the artist's album artwork.
        /// </summary>
        /// <returns>The artist art.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public IImage GetArtistArt(int width, int height)
        {
            // TODO
            return null;
        }

        /// <summary>
        /// Returns a list of artists based on the given ids.
        /// </summary>
        /// <returns>The artists.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="sids">Sids.</param>
        internal static List<SpotifyArtist> ObtainArtists(MetadataFactory factory, Queue<string> sids)
        {
            var result = new List<SpotifyArtist>();
            var items = factory.ObtainThings<ArtistsResult, ArtistResult>(sids, MAX_RESULTS_PER_QUERY, ENDPOINT_MULTIPLE);

            foreach (var item in items) {
                result.Add(new SpotifyArtist(factory, item));
            }

            return result;
        }

    }
}

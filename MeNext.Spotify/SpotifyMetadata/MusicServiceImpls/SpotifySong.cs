using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MeNext.MusicService;
using MeNext.Spotify;
using Newtonsoft.Json;
//using SpotifyAPI.Web.Models;

namespace MeNext.Spotify
{
    /// <summary>
    /// Class that represents a spotify song
    /// </summary>
    public class SpotifySong : ISong, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 50;
        private const string ENDPOINT_MULTIPLE = "tracks";
        //Important metadata for song
        private MetadataFactory factory;
        private string uri;
        private string name;
        private int trackNumber;
        private int diskNumber;
        private double duration;
        private List<string> artistUids;
        private string albumUid;

        /// <summary>
        /// Initializes a new instance of the Spotify song class.
        /// </summary>
        /// <param name="factory">Factory.</param>
        /// <param name="result">Result.</param>
        internal SpotifySong(MetadataFactory factory, TrackResult result)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.name = result.name;
            this.trackNumber = result.track_number;
            this.diskNumber = result.disc_number;
            this.duration = result.duration_ms / 1000.0;
            this.albumUid = result.album.uri;

            this.artistUids = new List<string>();
            foreach (var artist in result.artists) {
                this.artistUids.Add(artist.uri);
            }

            // Submit items for caching
            foreach (var artist in result.artists) {
                this.factory.CacheSubmit(artist);
            }

            this.factory.CacheSubmit(result.album);
        }

        /// <summary>
        /// Gets the song's album.
        /// </summary>
        /// <value>The album.</value>
        public IAlbum Album
        {
            get
            {
                return factory.GetAlbum(albumUid);
            }
        }

        /// <summary>
        /// Gets the song's artists.
        /// </summary>
        /// <value>The artist(s).</value>
        public List<IArtist> Artists
        {
            get
            {
                return factory.GetArtists(artistUids);
            }
        }

        /// <summary>
        /// Gets the song's disk number. 
        /// </summary>
        /// <value>The disk number.</value>
        public int DiskNumber
        {
            get
            {
                return this.diskNumber;
            }
        }

        /// <summary>
        /// Gets the song's duration in seconds.
        /// </summary>
        /// <value>The duration.</value>
        public double Duration
        {
            get
            {
                return this.duration;
            }
        }

        /// <summary>
        /// Gets the song's name.
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
        /// Gets the song's track number.
        /// </summary>
        /// <value>The track number.</value>
        public int TrackNumber
        {
            get
            {
                return this.trackNumber;
            }
        }

        /// <summary>
        /// Gets the song's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueId
        {
            get
            {
                return this.uri;
            }
        }

        /// <summary>
        /// Get a list of songs from a queue of song ids
        /// </summary>
        /// <returns>The songs.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="sids">Song ids.</param>
        internal static List<SpotifySong> ObtainSongs(MetadataFactory factory, Queue<string> sids)
        {
            var result = new List<SpotifySong>();
            var items = factory.ObtainThings<TracksResult, TrackResult>(sids, MAX_RESULTS_PER_QUERY, ENDPOINT_MULTIPLE);

            foreach (var item in items) {
                result.Add(new SpotifySong(factory, item));
            }

            return result;
        }

    }
}

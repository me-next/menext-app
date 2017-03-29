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
    public class SpotifySong : ISong, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 50;
        private const string ENDPOINT_MULTIPLE = "tracks";

        private MetadataFactory factory;
        private string uri;
        private string name;
        private int trackNumber;
        private int diskNumber;
        private double duration;
        private List<string> artistUids;
        private string albumUid;

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
        }

        public IAlbum Album
        {
            get
            {
                return factory.GetAlbum(albumUid);
            }
        }

        public List<IArtist> Artists
        {
            get
            {
                return factory.GetArtists(artistUids);
            }
        }

        public int DiskNumber
        {
            get
            {
                return this.diskNumber;
            }
        }

        public double Duration
        {
            get
            {
                return this.duration;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int TrackNumber
        {
            get
            {
                return this.trackNumber;
            }
        }

        public string UniqueId
        {
            get
            {
                return this.uri;
            }
        }

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

using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class SpotifyAlbum : IAlbum, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 20;
        private const string ENDPOINT_MULTIPLE = "albums";

        private MetadataFactory factory;

        private string uri;
        private List<string> songUids;
        private string name;

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

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public List<ISong> Songs
        {
            get
            {
                return factory.GetSongs(songUids);
            }
        }

        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        public IImage GetAlbumArt(int width, int height)
        {
            // TODO
            return null;
        }

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

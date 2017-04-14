using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class SpotifyArtist : IArtist, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 50;
        private const string ENDPOINT_MULTIPLE = "artists";

        private MetadataFactory factory;
        private string name;
        private string uri;
        private List<string> albumUids;

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

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        public IImage GetArtistArt(int width, int height)
        {
            // TODO
            return null;
        }

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

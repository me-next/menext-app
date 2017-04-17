using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Class that represents an artist and their spotify metadata.
    /// </summary>
    public class SpotifyArtist : IArtist, ISpotifyMetadata
    {
        private const int MAX_RESULTS_PER_QUERY = 50;
        private const string ENDPOINT_MULTIPLE = "artists";

        private MetadataFactory factory;
        private string name;
        private string uri;
        private List<string> albumUids;
        //Initializes an artist
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
        //Returns a list of the artist's albums
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

        //the artist's name
        public string Name
        {
            get
            {
                return name;
            }
        }

        //the artist's uid
        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        //the artist's artwork
        public IImage GetArtistArt(int width, int height)
        {
            // TODO
            return null;
        }

        //returns a list of artists based on the given ids.
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

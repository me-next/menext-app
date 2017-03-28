using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify.iOS
{
    public class SpotifyArtist : IArtist
    {
        private string spotifyUri;

        public SpotifyArtist(string spotifyUri)
        {
            this.spotifyUri = spotifyUri;
        }

        public List<IAlbum> Albums
        {
            get
            {
                // TODO
                var result = new List<IAlbum>();
                result.Add(new SpotifyAlbum("spotify:album:0zdZSyxWaYmaRMPeUHcG1K"));
                result.Add(new SpotifyAlbum("spotify:album:7Mh7Q5DQIE9evMeGrKHjg8"));
                result.Add(new SpotifyAlbum("spotify:album:7uMMbwF64xfNT8VpAkbJAE"));
                return result;
            }
        }

        public string Name
        {
            get
            {
                // TODO
                return "Artist Name";
            }
        }

        public string UniqueId
        {
            get
            {
                return spotifyUri;
            }
        }

        public IImage GetArtistArt(int width, int height)
        {
            // TODO
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class SpotifyAlbum : IAlbum, ISpotifyMetadata
    {
        private string spotifyUri;

        internal SpotifyAlbum(string spotifyUri)
        {
            this.spotifyUri = spotifyUri;
        }

        public string Name
        {
            get
            {
                // TODO
                return "Album Name";
            }
        }

        public List<ISong> Songs
        {
            get
            {
                // TODO
                var result = new List<ISong>();
                //result.Add(new SpotifySong("spotify:track:2Ml0l8YWJLQhPrRDLpQaDM"));
                //result.Add(new SpotifySong("spotify:track:1BwaPm2VjiOenzjW1TOZuW"));
                //result.Add(new SpotifySong("spotify:track:5USZyz6dnBEn1oLsKcAKQy"));
                //result.Add(new SpotifySong("spotify:track:0mBL2JwjNYKtdFacHxvtJt"));
                //result.Add(new SpotifySong("spotify:track:4EveU9Zb50mjgi5avDNqlK"));
                return result;
            }
        }

        public string UniqueId
        {
            get
            {
                return spotifyUri;
            }
        }

        public IImage GetAlbumArt(int width, int height)
        {
            // TODO
            return null;
        }
    }
}

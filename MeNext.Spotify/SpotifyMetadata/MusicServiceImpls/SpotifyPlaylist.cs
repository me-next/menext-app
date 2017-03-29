using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class SpotifyPlaylist : IPlaylist, ISpotifyMetadata
    {
        private readonly string spotifyUri;

        internal SpotifyPlaylist(string spotifyUri)
        {
            this.spotifyUri = spotifyUri;
        }

        public string Name
        {
            get
            {
                // TODO
                return "Playlist Name";
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
    }
}

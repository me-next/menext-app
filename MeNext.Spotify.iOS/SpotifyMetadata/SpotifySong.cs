using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify.iOS
{
    public class SpotifySong : ISong
    {
        private string spotifyUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.iOS.SpotifySong"/> class.
        /// </summary>
        /// <param name="spotifyUri">Spotify URI. Example: "spotify:track:5mw316TYTlvx1e3vBg1eRg"</param>
        public SpotifySong(string spotifyUri)
        {
            this.spotifyUri = spotifyUri;
        }

        public IAlbum Album
        {
            get
            {
                // TODO
                return new SpotifyAlbum("spotify:album:0C36RlW2Fa0C7n1JnWBBMP");
            }
        }

        public List<IArtist> Artists
        {
            get
            {
                // TODO
                var artists = new List<IArtist>();
                artists.Add(new SpotifyArtist("spotify:artist:5ksRONqssB7BR161NTtJAm"));
                artists.Add(new SpotifyArtist("spotify:artist:0p4nmQO2msCgU4IF37Wi3j"));
                artists.Add(new SpotifyArtist("spotify:artist:5rSXSAkZ67PYJSvpUpkOr7"));
                return artists;
            }
        }

        public int DiskNumber
        {
            get
            {
                // TODO
                return 1;
            }
        }

        public double Duration
        {
            get
            {
                // TODO
                return 500.0;
            }
        }

        public string Name
        {
            get
            {
                // TODO
                return "Song Name";
            }
        }

        public int TrackNumber
        {
            get
            {
                // TODO
                return 1;
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

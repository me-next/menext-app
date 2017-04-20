using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MeNext.MusicService;
using MeNext.Spotify;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    /// <summary>
    /// Class that represents a spotify playlist.
    /// </summary>
    public class SpotifyPlaylist : IPlaylist, ISpotifyMetadata
    {
        private MetadataFactory factory;
        private string uri;
        private string name;
        private IResultList<ISong> page1;

        /// <summary>
        /// Initializes a new instance of the Spotify playlist class.
        /// </summary>
        /// <param name="factory">Factory.</param>
        /// <param name="result">Result.</param>
        /// <param name="webApi">Web API.</param>
        internal SpotifyPlaylist(MetadataFactory factory, PlaylistResult result, WebApi webApi)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.name = result.name;

            PagingObjectResult<PlaylistTrackResult> tracks = result.tracks;

            var wrap = new PagingWrapper<ISong, PlaylistTrackResult>(tracks, webApi, false);
            this.page1 = wrap;

            // Submit items for caching
            foreach (var playlistTrack in tracks.items) {
                this.factory.CacheSubmit(playlistTrack);
            }
        }

        internal SpotifyPlaylist(MetadataFactory factory, PartialPlaylistResult result, WebApi webApi)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.name = result.name;

            var trackHref = result.tracks.href;

            var wrap = new PagingWrapper<ISong, PlaylistTrackResult>(trackHref, webApi, false);
            this.page1 = wrap;
        }

        /// <summary>
        /// Returns the playlist's name.
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
        /// Returns the playlist's songs.
        /// </summary>
        /// <value>The songs.</value>
        public IResultList<ISong> Songs
        {
            get
            {
                return this.page1;
            }
        }

        /// <summary>
        /// Returns the playlist's uid.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        // This one is a bit different than the other 3 obtainers, because the API is totally different
        /// <summary>
        /// Obtains a list of playlists from the given list of playlist ids.
        /// </summary>
        /// <returns>The playlists.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="uids">Uids is the playlist ids.</param>
        /// <param name="webApi">Web API.</param>
        internal static List<SpotifyPlaylist> ObtainPlaylists(MetadataFactory factory, IList<string> uids, WebApi webApi)
        {
            var result = new List<SpotifyPlaylist>();

            foreach (var uid in uids) {
                // Format example: spotify:user:wizzlersmate:playlist:1AVZz0mBuGbCEoNRQdYQju
                var lastColon = uid.LastIndexOf(':');
                var id = uid.Substring(lastColon + 1);
                var username = uid.Substring(13, lastColon - 9 - 13);

                var task = Task.Run(async () =>
                {
                    return await webApi.GetJson("/users/" + username + "/playlists/" + id);
                });
                var json = task.Result;
                var jsonResult = JsonConvert.DeserializeObject<PlaylistResult>(json);
                result.Add(new SpotifyPlaylist(factory, jsonResult, webApi));
            }

            return result;
        }

        public override int GetHashCode()
        {
            return this.UniqueId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return ((SpotifySong) obj).UniqueId == this.UniqueId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MeNext.MusicService;
using MeNext.Spotify;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class SpotifyPlaylist : IPlaylist, ISpotifyMetadata
    {
        private MetadataFactory factory;
        private string uri;
        private string name;
        private IResultList<ISong> page1;

        internal SpotifyPlaylist(MetadataFactory factory, PlaylistResult result, WebApi webApi)
        {
            this.factory = factory;
            this.uri = result.uri;
            this.name = result.name;

            PagingObjectResult<PlaylistTrackResult> tracks = result.tracks;

            // TODO: Do similar caching in the other classes
            foreach (var playlistTrack in tracks.items) {
                var trackResult = playlistTrack.track;
                var song = new SpotifySong(factory, trackResult);
                factory.CacheSubmit(song);
            }

            var wrap = new PagingWrapper<ISong, PlaylistTrackResult>(tracks, webApi, false);
            this.page1 = wrap;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public IResultList<ISong> Songs
        {
            get
            {
                return this.page1;
            }
        }

        public string UniqueId
        {
            get
            {
                return uri;
            }
        }

        // This one is a bit different than the other 3 obtainers, because the API is totally different
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
    }
}

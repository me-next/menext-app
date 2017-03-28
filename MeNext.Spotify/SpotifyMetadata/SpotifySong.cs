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

        private MetadataFactory factory;
        private string uid;
        private string name;
        private int trackNumber;
        private int diskNumber;
        private double duration;
        private List<string> artistUids;
        private string albumUid;

        //internal SpotifySong(MetadataFactory factory, string uid, string name, int trackNumber, int diskNumber, double duration, List<string> artistUids, string albumUid)
        //{
        //    this.factory = factory;
        //    this.uid = uid;
        //    this.name = name;
        //    this.trackNumber = trackNumber;
        //    this.diskNumber = diskNumber;
        //    this.duration = duration;
        //    this.artistUids = artistUids;
        //    this.albumUid = albumUid;
        //}

        internal SpotifySong(MetadataFactory factory, TrackResult result)
        {
            this.factory = factory;
            this.uid = result.uri;
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
                return this.uid;
            }
        }

        internal static List<SpotifySong> ObtainSongs(MetadataFactory factory, Queue<string> sids)// where T : ISpotifyMetadata
        {
            //Debug.Assert(typeof(T) == typeof(SpotifySong));

            WebApi webApi = factory.webApi;

            var result = new List<SpotifySong>();

            while (sids.Count > 0) {
                var ids = "";
                for (int i = 0; i < MAX_RESULTS_PER_QUERY && sids.Count > 0; ++i) {
                    var sid = sids.Dequeue();
                    ids += sid + ",";
                }
                ids = ids.Substring(0, ids.Length - 1);

                string endUri = string.Format("/tracks?ids={0}", ids);

                var task = Task.Run(async () =>
                {
                    return await webApi.GetJson(endUri);
                });

                var json = task.Result;
                var tracksResult = JsonConvert.DeserializeObject<TracksResult>(json);

                foreach (var trackResult in tracksResult.tracks) {
                    result.Add(new SpotifySong(factory, trackResult));
                }
            }

            return result;
        }

    }
}

﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MeNext.MusicService;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    /// <summary>
    /// Handles obtaining and caching metadata
    /// </summary>
    public class MetadataFactory
    {
        // TODO: Make the cache clean out old entries occassionally?
        private ConcurrentDictionary<string, IMetadata> cache = new ConcurrentDictionary<string, IMetadata>();

        public WebApi webApi { get; private set; }

        public MetadataFactory(WebApi webApi)
        {
            this.webApi = webApi;
        }

        /// <summary>
        /// Submits a chunk of our metadata to the cache.
        /// </summary>
        /// <param name="data">The metadata.</param>
        public void CacheSubmit(IMetadata data)
        {
            Debug.Assert(!cache.ContainsKey(data.UniqueId));
            cache[data.UniqueId] = data;
        }

        /// <summary>
        /// Submits a chunk of our metadata to the cache.
        /// </summary>
        /// <param name="data">The metadata.</param>
        public void CacheSubmit(IMetadataResult data)
        {
            var meta = data.ToMetadata(this.webApi, this);
            if (meta != null && !cache.ContainsKey(meta.UniqueId)) {
                this.CacheSubmit(meta);
            }
        }

        // Assumption: All uids are in fact of type T
        // TODO: Verify that?
        private List<Q> GetMany<T, Q>(IList<string> uids) where T : ISpotifyMetadata, Q
        {
            var result = new List<Q>();
            var absent = new List<string>();
            foreach (string uid in uids) {
                if (cache.ContainsKey(uid)) {
                    result.Add((T) cache[uid]);
                } else {
                    absent.Add(uid);
                }
            }

            // Bulk obtain members in absent and add to cache and result
            var obtained = Obtain<T>(absent);
            foreach (var thing in obtained) {
                result.Add((Q) thing);
                CacheSubmit(thing);
            }

            return result;
        }

        /// <summary>
        /// Obtains the actual objects from the interwebs.
        /// </summary>
        /// <returns>The metadata objects.</returns>
        /// <param name="uids">Uids.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private List<ISpotifyMetadata> Obtain<T>(IList<string> uids) where T : ISpotifyMetadata
        {
            var sids = new Queue<string>();
            foreach (var uid in uids) {
                var split = uid.Split(':');
                sids.Enqueue(split[2]);
            }

            var result = new List<ISpotifyMetadata>();
            if (typeof(T) == typeof(SpotifySong)) {
                var songs = SpotifySong.ObtainSongs(this, sids);
                result.AddRange(songs);
            } else if (typeof(T) == typeof(SpotifyAlbum)) {
                var albums = SpotifyAlbum.ObtainAlbums(this, sids);
                result.AddRange(albums);
            } else if (typeof(T) == typeof(SpotifyArtist)) {
                var artists = SpotifyArtist.ObtainArtists(this, sids);
                result.AddRange(artists);
            } else if (typeof(T) == typeof(SpotifyPlaylist)) {
                var playlists = SpotifyPlaylist.ObtainPlaylists(this, uids, webApi);
                result.AddRange(playlists);
            } else {
                throw new Exception("Invalid type T: " + typeof(T).Name);
            }
            return result;
        }

        internal List<Q> ObtainThings<T, Q>(Queue<string> sids, int resultsPerQuery, string endpoint) where T : IResultWithList<Q>
        {
            var result = new List<Q>();

            // While there are things which need obtaining
            while (sids.Count > 0) {
                // Compile a list of MAX_RESULTS_PER_QUERY of their ids 
                var ids = "";
                for (int i = 0; i < resultsPerQuery && sids.Count > 0; ++i) {
                    var sid = sids.Dequeue();
                    ids += sid + ",";
                }
                ids = ids.Substring(0, ids.Length - 1);

                // Get the json result
                string endUri = string.Format("/{0}?ids={1}", endpoint, ids);

                var task = Task.Run(async () =>
                {
                    return await webApi.GetJson(endUri);
                });

                var json = task.Result;
                var jsonResult = JsonConvert.DeserializeObject<T>(json);

                // Add the deserialised object to the list of deserialised objects
                result.AddRange(jsonResult.Items);
            }

            return result;
        }

        /// <summary>
        /// Gets songs based on list of song ids.
        /// </summary>
        /// <returns>The songs.</returns>
        /// <param name="uids">Song ids.</param>
        public List<ISong> GetSongs(IList<string> uids)
        {
            return GetMany<SpotifySong, ISong>(uids);
        }

        /// <summary>
        /// Gets albums based on list of album ids.
        /// </summary>
        /// <returns>The albums.</returns>
        /// <param name="uids">Album ids.</param>
        public List<IAlbum> GetAlbums(IList<string> uids)
        {
            return GetMany<SpotifyAlbum, IAlbum>(uids);
        }

        /// <summary>
        /// Gets artists based on list of artists ids.
        /// </summary>
        /// <returns>The artists.</returns>
        /// <param name="uids">Artist ids.</param>
        public List<IArtist> GetArtists(IList<string> uids)
        {
            return GetMany<SpotifyArtist, IArtist>(uids);
        }

        /// <summary>
        /// Gets playlists based on list of playlist ids.
        /// </summary>
        /// <returns>The playlists.</returns>
        /// <param name="uids">Playlist ids.</param>
        public List<IPlaylist> GetPlaylists(IList<string> uids)
        {
            return GetMany<SpotifyPlaylist, IPlaylist>(uids);
        }

        // Uses the corresponding GetMany function with a single uid, and then returns the singular result.
        // Just cuts down on code repetition
        private Q GetOne<T, Q>(string uid) where T : ISpotifyMetadata, Q
        {
            var list = new List<string>();
            list.Add(uid);
            return GetMany<T, Q>(list)[0];
        }

        /// <summary>
        /// Gets one song based on given song id.
        /// </summary>
        /// <returns>The song.</returns>
        /// <param name="uid">Uid.</param>
        public ISong GetSong(string uid)
        {
            if (uid == null) {
                return null;
            }
            return GetOne<SpotifySong, ISong>(uid);
        }

        /// <summary>
        /// Gets an album based on given album id.
        /// </summary>
        /// <returns>The album.</returns>
        /// <param name="uid">Album id.</param>
        public IAlbum GetAlbum(string uid)
        {
            if (uid == null) {
                return null;
            }
            return GetOne<SpotifyAlbum, IAlbum>(uid);
        }

        /// <summary>
        /// Gets an artist based on given artist id.
        /// </summary>
        /// <returns>The artist.</returns>
        /// <param name="uid">Artist id.</param>
        public IArtist GetArtist(string uid)
        {
            if (uid == null) {
                return null;
            }
            return GetOne<SpotifyArtist, IArtist>(uid);
        }

        /// <summary>
        /// Gets a playlist based on given playlist id.
        /// </summary>
        /// <returns>The playlist.</returns>
        /// <param name="uid">Playlist id.</param>
        public IPlaylist GetPlaylist(string uid)
        {
            if (uid == null) {
                return null;
            }
            return GetOne<SpotifyPlaylist, IPlaylist>(uid);
        }
    }
}

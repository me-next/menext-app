using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using MeNext.MusicService;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class MetadataFactory
    {
        private ConcurrentDictionary<string, ISpotifyMetadata> cache = new ConcurrentDictionary<string, ISpotifyMetadata>();

        public WebApi webApi { get; private set; }

        public MetadataFactory(WebApi webApi)
        {
            this.webApi = webApi;
        }

        // Assumption: All uids are in fact of type T
        private List<Q> GetMany<T, Q>(List<string> uids) where T : ISpotifyMetadata, Q
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
                cache[thing.UniqueId] = thing;
            }

            return result;
        }

        /// <summary>
        /// Obtains the actual objects from the interwebs
        /// </summary>
        /// <returns>The metadata objects.</returns>
        /// <param name="uids">Uids.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private List<ISpotifyMetadata> Obtain<T>(List<string> uids) where T : ISpotifyMetadata
        {
            // TODO: Add SpotifyPlaylist support, which does not have a "get multiple" endpoint
            // Then remove this assetion
            Debug.Assert(typeof(T) != typeof(SpotifyPlaylist));

            var sids = new Queue<string>();
            foreach (var uid in uids) {
                var split = uid.Split(':');
                sids.Enqueue(split[2]);
            }

            if (typeof(T) == typeof(SpotifySong)) {
                var songs = SpotifySong.ObtainSongs(this, sids);
                var result = new List<ISpotifyMetadata>();
                foreach (var song in songs) {
                    result.Add(song);
                }
                return result;
            } else if (typeof(T) == typeof(SpotifyAlbum)) {
                // TODO
            } else if (typeof(T) == typeof(SpotifyArtist)) {
                // TODO
            } else if (typeof(T) == typeof(SpotifyPlaylist)) {
                // TODO
            } else {
                throw new Exception("Invalid type T: " + typeof(T).Name);
            }

            return null;
        }

        private Q GetOne<T, Q>(string uid) where T : ISpotifyMetadata, Q
        {
            var list = new List<string>();
            list.Add(uid);
            return GetMany<T, Q>(list)[0];
        }

        public List<ISong> GetSongs(List<string> uids)
        {
            return GetMany<SpotifySong, ISong>(uids);
        }

        public List<IAlbum> GetAlbums(List<string> uids)
        {
            return GetMany<SpotifyAlbum, IAlbum>(uids);
        }

        public List<IArtist> GetArtists(List<string> uids)
        {
            return GetMany<SpotifyArtist, IArtist>(uids);
        }

        public List<IPlaylist> GetPlaylists(List<string> uids)
        {
            return GetMany<SpotifyPlaylist, IPlaylist>(uids);
        }

        public ISong GetSong(string uid)
        {
            return GetOne<SpotifySong, ISong>(uid);
        }

        public IAlbum GetAlbum(string uid)
        {
            return GetOne<SpotifyAlbum, IAlbum>(uid);
        }

        public IArtist GetArtist(string uid)
        {
            return GetOne<SpotifyArtist, IArtist>(uid);
        }

        public IPlaylist GetPlaylist(string uid)
        {
            return GetOne<SpotifyPlaylist, IPlaylist>(uid);
        }
    }
}

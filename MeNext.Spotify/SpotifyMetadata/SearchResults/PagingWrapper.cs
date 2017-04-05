using System;
using System.Collections.Generic;
using System.Diagnostics;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// This class wraps a PagingObjectResult, which comes from the Spotify API, in an IResultList, so it can
    /// be accessed agnostically.
    /// </summary>
    public class PagingWrapper<T, Q> : IResultList<T> where T : IMetadata where Q : IMetadataResult
    {
        private readonly WebApi webApi;
        private readonly bool isWrapped;

        private readonly int firstResult;
        private readonly bool hasNextPage;
        private readonly List<T> items;
        private readonly string nextPage;
        private readonly string prevPage;

        internal PagingWrapper(PagingObjectResult<Q> result, WebApi webApi, bool isWrapped)
        {
            this.webApi = webApi;
            this.isWrapped = isWrapped;

            this.firstResult = result.offset;
            this.hasNextPage = (result.next != null);
            this.nextPage = result.next;
            this.prevPage = result.previous;

            // TODO Use some of this info to help cache?
            this.items = new List<T>();
            var uids = new List<string>();
            foreach (var item in result.items) {
                uids.Add(item.uri);
            }
            if (typeof(T) == typeof(ISong)) {
                var songs = webApi.metadata.GetSongs(uids);
                foreach (var song in songs) {
                    this.items.Add((T) song);
                }
            } else if (typeof(T) == typeof(IAlbum)) {
                // TODO
                throw new NotImplementedException();
            } else if (typeof(T) == typeof(IArtist)) {
                // TODO
                throw new NotImplementedException();
            } else if (typeof(T) == typeof(IPlaylist)) {
                var playlists = webApi.metadata.GetPlaylists(uids);
                foreach (var pl in playlists) {
                    this.items.Add((T) pl);
                }
            } else {
                throw new Exception("Invalid type T: " + typeof(T).Name);
            }
        }

        public PageErrorType Error
        {
            get
            {
                return PageErrorType.SUCCESS;
            }
        }

        public int FirstResult
        {
            get
            {
                return firstResult;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return hasNextPage;
            }
        }

        public List<T> Items
        {
            get
            {
                return this.items;
            }
        }

        public IResultList<T> NextPage
        {
            get
            {
                if (nextPage == null) {
                    return null;
                }
                return webApi.PagingUri<T, Q>(nextPage, this.isWrapped);
            }
        }

        public IResultList<T> PreviousPage
        {
            get
            {
                if (prevPage == null) {
                    return null;
                }
                return webApi.PagingUri<T, Q>(prevPage, this.isWrapped);
            }
        }
    }
}

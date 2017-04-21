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

        private string delayedLoad;

        private int firstResult;
        private List<T> items;
        private string nextPage;
        private string prevPage;

        /// <summary>
        /// Dummy constructor for if we don't actually have any of the info yet, just where to get the first real page
        /// </summary>
        /// <param name="firstUrl">First URL.</param>
        /// <param name="webApi">Web API.</param>
        /// <param name="isWrapped">If set to <c>true</c> is wrapped.</param>
        internal PagingWrapper(string firstUrl, WebApi webApi, bool isWrapped)
        {
            // When this is the case, first access to a param blocks and fills in the real data
            this.webApi = webApi;
            this.isWrapped = isWrapped;

            this.delayedLoad = firstUrl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.PagingWrapper`2"/> class.
        /// </summary>
        /// <param name="result">Paging object with results.</param>
        /// <param name="webApi">Web API.</param>
        /// <param name="isWrapped">If set to <c>true</c> is wrapped.</param>
        internal PagingWrapper(PagingObjectResult<Q> result, WebApi webApi, bool isWrapped)
        {
            this.webApi = webApi;
            this.isWrapped = isWrapped;

            this.delayedLoad = null;

            this.firstResult = result.offset;
            this.nextPage = result.next;
            this.prevPage = result.previous;

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

        /// <summary>
        /// Checks if we had a delayed load for this page and, if so, load it.
        /// </summary>
        public void UndelayLoad()
        {
            if (delayedLoad != null) {
                var wrapper = (PagingWrapper<T, Q>) webApi.PagingUri<T, Q>(delayedLoad, this.isWrapped);
                this.firstResult = wrapper.firstResult;
                this.nextPage = wrapper.nextPage;
                this.prevPage = wrapper.prevPage;
                this.items = wrapper.items;

                this.delayedLoad = null;
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
                UndelayLoad();
                return firstResult;
            }
        }

        public bool HasNextPage
        {
            get
            {
                UndelayLoad();
                return (this.nextPage != null);
            }
        }

        public List<T> Items
        {
            get
            {
                UndelayLoad();
                return this.items;
            }
        }

        public IResultList<T> NextPage
        {
            get
            {
                UndelayLoad();
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
                UndelayLoad();
                if (prevPage == null) {
                    return null;
                }
                return webApi.PagingUri<T, Q>(prevPage, this.isWrapped);
            }
        }
    }
}

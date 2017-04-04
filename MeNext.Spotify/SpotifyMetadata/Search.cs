using System;
using System.Collections.Generic;
using System.Diagnostics;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public class Search<T, Q> : IResultList<T> where T : IMetadata where Q : IMetadataResult
    {
        private WebApi webApi;
        private bool isWrapped;

        private int firstResult;
        private bool hasNextPage;
        private List<T> items;
        private string nextPage;
        private string prevPage;


        public Search(PagingObjectResult<Q> searchResult, WebApi webApi, bool isWrapped)
        {
            this.webApi = webApi;
            this.isWrapped = isWrapped;

            this.firstResult = searchResult.offset;
            this.hasNextPage = (searchResult.next != null);
            this.nextPage = searchResult.next;
            this.prevPage = searchResult.previous;

            this.items = new List<T>();
            if (typeof(T) == typeof(ISong)) {
                var uids = new List<string>();
                foreach (var song in searchResult.items) {
                    uids.Add(song.uri);
                }
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
                // TODO
                throw new NotImplementedException();
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

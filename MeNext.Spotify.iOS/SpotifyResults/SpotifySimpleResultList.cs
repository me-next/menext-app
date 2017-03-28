using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify.iOS
{
    /// <summary>
    /// Represents a simple list of items with no paging
    /// </summary>
    public class SpotifySimpleResultList<K> : IResultList<K>
    {
        private List<K> items;
        private PageErrorType error;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.iOS.SpotifySimpleResultList`1"/> class when
        /// the operation has succeeded.
        /// </summary>
        /// <param name="items">Items.</param>
        public SpotifySimpleResultList(List<K> items)
        {
            this.items = items;
            this.error = PageErrorType.SUCCESS;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.iOS.SpotifySimpleResultList`1"/> class where
        /// the operation has failed.
        /// </summary>
        /// <param name="error">Error.</param>
        public SpotifySimpleResultList(PageErrorType error)
        {
            this.items = null;
            this.error = error;
        }

        public PageErrorType Error
        {
            get
            {
                return this.error;
            }
        }

        public int FirstResult
        {
            get
            {
                return 0;
            }
        }

        public int LastResult
        {
            get
            {
                return items.Count;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return false;
            }
        }

        public List<K> Items
        {
            get
            {
                return items;
            }
        }

        public IResultList<K> NextPage
        {
            get
            {
                return null;
            }
        }

        public IResultList<K> PreviousPage
        {
            get
            {
                return null;
            }
        }
    }
}

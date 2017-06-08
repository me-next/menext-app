using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.Spotify.iOS
{
    // TODO: This should be somewhere agnostic

    /// <summary>
    /// Represents a simple list of items with no paging
    /// </summary>
    public class SpotifySimpleResultList<K> : IResultList<K>
    {
        private List<K> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.iOS.SpotifySimpleResultList`1"/> class when
        /// the operation has succeeded.
        /// </summary>
        /// <param name="items">Items.</param>
        public SpotifySimpleResultList(List<K> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.Spotify.iOS.SpotifySimpleResultList`1"/> class where
        /// the operation has failed.
        /// </summary>
        /// <param name="error">Error.</param>
        public SpotifySimpleResultList()
        {
            this.items = null;
        }

        /// <summary>
        /// Gets the index of the first result within the list.
        /// </summary>
        /// <value>The first result.</value>
        public int FirstResult
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the index of the last result within the list.
        /// </summary>
        /// <value>The last result.</value>
        public int LastResult
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MeNext.Spotify.iOS.SpotifySimpleResultList`1"/> has a next page.
        /// </summary>
        /// <value><c>true</c> if has a next page; otherwise, <c>false</c>.</value>
        public bool HasNextPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items in a list.</value>
        public List<K> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Gets the next page.
        /// </summary>
        /// <value>The next page.</value>
        public IResultList<K> NextPage
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the previous page.
        /// </summary>
        /// <value>The previous page.</value>
        public IResultList<K> PreviousPage
        {
            get
            {
                return null;
            }
        }
    }
}

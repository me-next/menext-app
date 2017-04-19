using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    public class SimpleResultList<T> : IResultList<T>
    {
        // TODO: This should be an IEnumerable
        private List<T> items;
        /// <summary>
        /// Initializes a new instance of the class from a given list.
        /// </summary>
        /// <param name="items">the items to place in the simple list.</param>
        public SimpleResultList(List<T> items)
        {
            this.items = items;
        }
        /// <summary>
        /// Fufilling inheritance contract.
        /// If there is any error for the request it will return the type of error.
        /// If this isn't SUCCESS, then one shouldn't do anything with the rest of this object.
        /// </summary>
        /// <value>The error.</value>
        public PageErrorType Error
        {
            get
            {
                return PageErrorType.SUCCESS;
            }
        }
        /// <summary>
        /// Fufilling inheritance contract.
        /// Returns the index of the first item in this page.
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
        /// Fufilling inheritance contract.
        /// Indicates whether or not there is another page.
        /// </summary>
        /// <value><c>true</c> if has next page; otherwise, <c>false</c>.</value>
        public bool HasNextPage
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Fufilling inheritance contract.
        /// Returns a list of the class's items.
        /// </summary>
        /// <value>The items.</value>
        public List<T> Items
        {
            get
            {
                return items;
            }
        }
        /// <summary>
        /// Fufilling inheritance contract.
        /// </summary>
        /// <value>The next page.</value>
        public IResultList<T> NextPage
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Fufilling inheritance contract.
        /// </summary>
        /// <value>The previous page.</value>
        public IResultList<T> PreviousPage
        {
            get
            {
                return null;
            }
        }
    }
}

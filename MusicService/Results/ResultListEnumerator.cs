using System;
using System.Collections;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Enumerator of IResultLists
    /// </summary>
    public class ResultListEnumerator<T> : IEnumerator<T>
    {
        private IResultList<T> currentList;
        private List<T> allElements;
        private int pos = -1;

        /// <summary>
        /// Initializes a new result list enumerator from given list.
        /// </summary>
        /// <param name="resultList">List to enumerate through.</param>
        public ResultListEnumerator(IResultList<T> resultList)
        {
            this.currentList = resultList;
            this.allElements = new List<T>();
            this.allElements.AddRange(resultList.Items);
        }

        /// <summary>
        /// Returns the current Enum's value
        /// </summary>
        /// <value>The current element's value.</value>
        public T Current
        {
            get
            {
                try {
                    return allElements[pos];
                } catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Fulfilling inheritance contract.
        /// </summary>
        /// <value>The current value.</value>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Moves to the next element in the list.
        /// Will go to next page if needed.
        /// </summary>
        /// <returns><c>true</c>, if next was moved, <c>false</c> otherwise.</returns>
        public bool MoveNext()
        {
            ++pos;
            while (pos >= allElements.Count) {
                if (currentList.HasNextPage) {
                    currentList = currentList.NextPage;
                    allElements.AddRange(currentList.Items);
                } else {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Reset the Enumerator.
        /// </summary>
        public void Reset()
        {
            pos = -1;
        }
    }
}

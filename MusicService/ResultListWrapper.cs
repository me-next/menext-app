using System;
using System.Collections;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Wrapper to smooth use of result lists in different OS.
    /// </summary>
    public class ResultListWrapper<T> : IEnumerable<T>
    {
        private IResultList<T> resultList;
        /// <summary>
        /// Initializes a new instance of the class from a given IResultList.
        /// </summary>
        /// <param name="resultList">The result list to initialize from.</param>
        public ResultListWrapper(IResultList<T> resultList)
        {
            this.resultList = resultList;
        }
        /// <summary>
        /// Gets a result list enumerator of itself.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new ResultListEnumerator<T>(this.resultList);
        }
        /// <summary>
        /// Fulfilling inheritance contract.
        /// Gets enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    public class ResultListWrapper<T> : IEnumerable<T>
    {
        private IResultList<T> resultList;

        public ResultListWrapper(IResultList<T> resultList)
        {
            this.resultList = resultList;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ResultListEnumerator<T>(this.resultList);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

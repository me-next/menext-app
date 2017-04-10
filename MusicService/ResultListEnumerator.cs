using System;
using System.Collections;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    public class ResultListEnumerator<T> : IEnumerator<T>
    {
        private IResultList<T> currentList;
        private List<T> allElements;
        private int pos = -1;

        public ResultListEnumerator(IResultList<T> resultList)
        {
            this.currentList = resultList;
            this.allElements = new List<T>();
            this.allElements.AddRange(resultList.Items);
        }

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

        public void Reset()
        {
            pos = -1;
        }
    }
}

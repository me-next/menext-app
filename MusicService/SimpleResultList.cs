using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    public class SimpleResultList<T> : IResultList<T>
    {
        // TODO: This should be an IEnumerable
        private List<T> items;

        public SimpleResultList(List<T> items)
        {
            this.items = items;
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
                return 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return false;
            }
        }

        public List<T> Items
        {
            get
            {
                return items;
            }
        }

        public IResultList<T> NextPage
        {
            get
            {
                return null;
            }
        }

        public IResultList<T> PreviousPage
        {
            get
            {
                return null;
            }
        }
    }
}

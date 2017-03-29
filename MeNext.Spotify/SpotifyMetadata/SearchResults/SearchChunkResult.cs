using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MeNext.Spotify
{
    public class SearchChunkResult<Q> where Q : IMetadataResult
    {
        public static SearchChunkResult<T> CastTypeParam<T>(SearchChunkResult<Q> original) where T : IMetadataResult
        {
            Debug.Assert(typeof(T) == typeof(Q));

            var fresh = new SearchChunkResult<T>();
            fresh.href = original.href;
            fresh.limit = original.limit;
            fresh.next = original.next;
            fresh.offset = original.offset;
            fresh.previous = original.previous;
            fresh.total = original.total;


            fresh.items = new List<T>();
            foreach (var item in original.items) {
                // This casting is very kludgy but it works
                fresh.items.Add((T) (IMetadataResult) item);
            }

            return fresh;
        }

        public string href { get; set; }
        public IList<Q> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }
    }
}

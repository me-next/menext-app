using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MeNext.Spotify
{
    /// <summary>
    /// Represents a Spotify result with pages
    /// </summary>
    public class PagingObjectResult<Q> where Q : IMetadataResult
    {
        public string href { get; set; }
        public IList<Q> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }

        /// <summary>
        /// Casts a paging object result
        /// </summary>
        /// <returns>The new paging object.</returns>
        /// <param name="original">The original object.</param>
        /// <typeparam name="T">The type we want to cast it to. Must be equivalent to Q in runtime.</typeparam>
        public static PagingObjectResult<T> CastTypeParam<T>(PagingObjectResult<Q> original) where T : IMetadataResult
        {
            Debug.Assert(typeof(T) == typeof(Q));

            var fresh = new PagingObjectResult<T>();
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
    }
}

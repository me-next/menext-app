using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class Util
    {
        /// <summary>
        /// Casts all the members of a list to another type
        /// </summary>
        /// <returns>The casted members.</returns>
        /// <param name="list">The original list.</param>
        /// <typeparam name="T">The type we want to cast to.</typeparam>
        public static List<T> CastMembers<T>(IList<object> list)
        {
            var result = new List<T>();
            foreach (var x in list) {
                result.Add((T) x);
            }
            return result;
        }
    }
}

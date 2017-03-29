using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public class Util
    {
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

using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    /// <summary>
    /// Represents a result which has a main list of elements in it.
    /// </summary>
    public interface IResultWithList<T>
    {
        IList<T> Items { get; }
    }
}

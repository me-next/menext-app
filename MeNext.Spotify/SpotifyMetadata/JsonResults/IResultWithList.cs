using System;
using System.Collections.Generic;

namespace MeNext.Spotify
{
    public interface IResultWithList<T>
    {
        IList<T> Items { get; }
    }
}

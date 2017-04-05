using System;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    /// <summary>
    /// Represents a result which has a URI.
    /// </summary>
    public interface IMetadataResult
    {
        string uri { get; }

        /// <summary>
        /// Convert this IMetadataResult to IMetadata, if possible.
        /// </summary>
        /// <returns>The IMetadata, or null if not possible.</returns>
        IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata);

    }
}

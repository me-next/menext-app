using System;
using MeNext.MusicService;

namespace MeNext.Spotify
{
    public interface IMetadataResult
    {
        string uri { get; }

        /// <summary>
        /// CAN BE NULL IF NOT POSSIBLE
        /// </summary>
        /// <returns>The metadata.</returns>
        IMetadata ToMetadata(WebApi webApi, MetadataFactory metadata);

    }
}

using System;
namespace MeNext.Spotify
{
    public interface ISpotifyMetadata
    {
        /// <summary>
        /// Gets a Spotify unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId
        {
            get;
        }
    }
}

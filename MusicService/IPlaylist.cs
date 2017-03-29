using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Represents a playlist
    /// </summary>
    public interface IPlaylist : IMetadata
    {
        /// <summary>
        /// Gets the name of the playlist.
        /// </summary>
        /// <value>The name.</value>
        String Name
        {
            get;
        }

        /// <summary>
        /// Gets the playlist's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId
        {
            get;
        }

        /// <summary>
        /// Gets the list of songs in the playlist.
        /// </summary>
        /// <value>The songs.</value>
        List<ISong> Songs
        {
            get;
        }
    }
}

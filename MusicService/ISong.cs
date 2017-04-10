using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Represents a song.
    /// </summary>
    public interface ISong : IMetadata
    {
        /// <summary>
        /// Gets the name of the song.
        /// </summary>
        /// <value>The name.</value>
        String Name
        {
            get;
        }

        /// <summary>
        /// Gets the list of all the artists who contributed to this song.
        /// </summary>
        /// <value>The artists.</value>
        List<IArtist> Artists
        {
            get;
        }

        /// <summary>
        /// Gets the album this song is in.
        /// </summary>
        /// <value>The album.</value>
        IAlbum Album
        {
            get;
        }

        /// <summary>
        /// Gets the song's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId
        {
            get;
        }

        /// <summary>
        /// Gets the duration of the song in seconds.
        /// </summary>
        /// <value>The duration.</value>
        double Duration
        {
            get;
        }

        /// <summary>
        /// Gets the disk number.
        /// </summary>
        /// <value>The disk number.</value>
        int DiskNumber
        {
            get;
        }

        /// <summary>
        /// Gets the track number within the disk.
        /// </summary>
        /// <value>The track number.</value>
        int TrackNumber
        {
            get;
        }
    }
}

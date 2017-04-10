using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
    /// <summary>
    /// Represents an artist.
    /// </summary>
    public interface IArtist : IMetadata
    {
        /// <summary>
        /// Gets the artist name.
        /// </summary>
        /// <value>The name.</value>
        String Name
        {
            get;
        }

        /// <summary>
        /// Gets the artist's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId
        {
            get;
        }

        /// <summary>
        /// Gets all of the albums this artist has contributed to.
        /// </summary>
        /// <value>The albums.</value>
        List<IAlbum> Albums
        {
            get;
        }

        /// <summary>
        /// Gets the artist art which will look best at the given width and height, in pixels.
        /// Not necessarily actually the given width and height.
        /// </summary>
        /// <returns>The artist art.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        IImage GetArtistArt(int width, int height);
    }
}

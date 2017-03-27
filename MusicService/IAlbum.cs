using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
	/// <summary>
	/// Represents a music album.
	/// </summary>
	public interface IAlbum
	{
		/// <summary>
		/// Gets the name of the album.
		/// </summary>
		/// <value>The name.</value>
		String Name
		{
			get;
		}

		/// <summary>
		/// Gets the album's unique identifier.
		/// </summary>
		/// <value>The unique identifier.</value>
		IUniqueId UniqueId
		{
			get;
		}

		/// <summary>
		/// Gets the list of all songs in the album.
		/// </summary>
		/// <value>The songs.</value>
		List<ISong> Songs
		{
			get;
		}

		/// <summary>
		/// Gets the album art which will look best at the given width and height, in pixels.
		/// Not necessarily actually the given width and height.
		/// </summary>
		/// <returns>The artist art.</returns>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		IImage GetAlbumArt(int width, int height);
	}
}

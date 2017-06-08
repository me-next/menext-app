using System;
namespace MeNext.MusicService
{
	/// <summary>
	/// Represents a service's images
	/// </summary>
	public interface IImage
	{
		/// <summary>
		/// The Universal Resource Identifier for the location of the image
		/// </summary>
		/// <value>The URI.</value>
		String URI
		{
			get;
		}
	}
}

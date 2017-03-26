using System;
namespace MeNext.MusicService
{
	/// <summary>
	/// Represents a unique identifier for something.
	/// 
	/// This is an interface so that, in principle, one could encode multiple values into the Uid string and handle
	/// extracting them internally. In most cases, one would use some sort of unique id from the backend (ex A filename,
	/// Spotify's identifier).
	/// </summary>
	public interface IUniqueId
	{
		/// <summary>
		/// The unique id which will be used internally (ie in our backend)
		/// </summary>
		/// <value>The uid.</value>
		String Uid
		{
			get;
		}
	}
}

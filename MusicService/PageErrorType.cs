using System;
namespace MeNext.MusicService
{
	/// <summary>
	/// Defines possible errors for an IResultList
	/// </summary>
	public enum PageErrorType
	{
		/// <summary>
		/// There is no error
		/// </summary>
		SUCCESS,

		/// <summary>
		/// An unspecified error
		/// </summary>
		ERROR_GENERIC,

		/// <summary>
		/// There was trouble connecting to the network
		/// </summary>
		ERROR_NETWORK,

		/// <summary>
		/// This page does not exist
		/// </summary>
		ERROR_NONEXISTENT_PAGE,
	}
}

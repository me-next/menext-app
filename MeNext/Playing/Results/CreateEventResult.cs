using System;
namespace MeNext
{
	/// <summary>
	/// The result of a request to create an event
	/// </summary>
	public enum CreateEventResult
	{
		/// <summary>
		/// The event was successfully created
		/// </summary>
		SUCCESS,

		/// <summary>
		/// The event failed for an unspecified reason
		/// </summary>
		FAIL_GENERIC,

		/// <summary>
		/// An event with this name already exists
		/// </summary>
		FAIL_EVENT_EXISTS,

		/// <summary>
		/// There was trouble connecting to the server
		/// </summary>
		FAIL_NETWORK,
	}
}

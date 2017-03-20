using System;
namespace MeNext
{
	/// <summary>
	/// This contains all the latest status information after we poll the server
	/// </summary>
	public class StatusMessage
	{
		/// <summary>
		/// Whether the current event still exists or not. If this is false, we should go back to homepage.
		/// </summary>
		public bool EventActive { get; set; }

		/// <summary>
		/// The Id of the status message coming in. This is incremented for each server-side change.
		/// </summary>
		public long ChangeId { get; set; }

		/// <summary>
		/// This text is just to be used for debugging from the server
		/// </summary>
		public string TestingText { get; set; }

		/// <summary>
		/// The unique id of the current song
		/// </summary>
		public string CurrentSong { get; set; }

		/// <summary>
		/// The unix time in ms at which we began listening to CurrentSong. This should be updated when a new song
		/// begins playing or a seek occurs.
		/// </summary>
		public long SongStartTime { get; set; }

		/// <summary>
		/// The position within the song that we began listening at
		/// </summary>
		public double SongStartPos { get; set; }

		/// <summary>
		/// The server unix time in ms
		/// </summary>
		public long ServerTime { get; set; }

		/// <summary>
		/// The volume of the current song on a scale from 0-1
		/// </summary>
		public double Volume { get; set; }

		/// <summary>
		/// Whether the current song is playing or paused
		/// </summary>
		public bool Playing { get; set; }

		// TODO: Queues

		// TODO: Permissions
	}
}

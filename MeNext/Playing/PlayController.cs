using System;
using MeNext.MusicService;

namespace MeNext
{
	/// <summary>
	/// This class handles actually playing music. It should only be used by the host.
	/// </summary>
	public class PlayController
	{
		public StatusMessage PreviousMessage { get; set; }
		public StatusMessage CurrentMessage { get; set; }

		private readonly IMusicService musicService;

		public PlayController(IMusicService musicService)
		{
			this.musicService = musicService;
		}

		/// <summary>
		/// Updates the actual playing status based on the information in the latest status message
		/// </summary>
		/// <param name="message">Message.</param>
		public void UpdateActualPlaying(StatusMessage message)
		{
			PreviousMessage = CurrentMessage;
			CurrentMessage = message;

			if (CurrentMessage.CurrentSong != null)
			{
				var song = musicService.GetSong(CurrentMessage.CurrentSong);

				// Check if a seek has occurred or a new song is playing
				if (PreviousMessage == null
					|| PreviousMessage.SongStartTime != CurrentMessage.SongStartTime
					|| PreviousMessage.CurrentSong != CurrentMessage.CurrentSong)
				{
					var positionMs = CurrentMessage.ServerTime - CurrentMessage.SongStartTime;
					positionMs = Math.Max(0, positionMs);
					double positionSec = positionMs / 1000.0 + CurrentMessage.SongStartPos;

					musicService.PlaySong(song, positionSec);
				}
			}

			// Update playing status
			musicService.Playing = CurrentMessage.Playing;

			// TODO: Update volume
		}
	}
}

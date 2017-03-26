using System;
namespace MeNext.MusicService
{
	/// <summary>
	/// Listens to events about the current song playing status.
	/// </summary>
	public interface ISongPlayListener
	{
		/// <summary>
		/// Called when a song ends playing
		/// </summary>
		/// <param name="song">Song.</param>
		void SongEnds(ISong song);

		/// <summary>
		/// Called whenever something changed and we might need to update the now playing view
		/// </summary>
		void SomethingChanged();
	}
}

using System;
namespace MeNext.MusicService
{
    /// <summary>
    /// Listens to events about the current music service status.
    /// </summary>
    public interface IMusicServiceListener
    {
        /// <summary>
        /// Called when a song ends playing
        /// </summary>
        /// <param name="song">Song.</param>
        void SongEnds(ISong song);

        /// <summary>
        /// Called whenever something changed and we might need to update any of the UI
        /// </summary>
        void SomethingChanged();
    }
}

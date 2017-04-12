using System;
using MeNext.MusicService;
using SpotifyAPI.Web;

using System.Diagnostics;

namespace MeNext
{
    /// <summary>
    /// This class handles actually playing music. It should only be used by the host.
    /// </summary>
    public class PlayController : IPullUpdateObserver
    {
        private readonly IMusicService musicService;

        public PlayController(IMusicService musicService)
        {
            this.musicService = musicService;
            this.currentPullData = new PlayingResponse();
            this.previousPullData = new PlayingResponse();
        }

        /// <summary>
        /// Updates the actual playing status based on the information in the latest status message
        /// </summary>
        /// <param name="message">Message.</param>
        //public void UpdateActualPlaying(StatusMessage message)
        //{
        /*
        PreviousMessage = CurrentMessage;
        CurrentMessage = message;

        if (CurrentMessage.CurrentSong != null) {
            var song = musicService.GetSong(CurrentMessage.CurrentSong);

            // Check if a seek has occurred or a new song is playing
            if (PreviousMessage == null
                || PreviousMessage.SongStartTime != CurrentMessage.SongStartTime
                || PreviousMessage.CurrentSong != CurrentMessage.CurrentSong) {
                var positionMs = CurrentMessage.ServerTime - CurrentMessage.SongStartTime;
                positionMs = Math.Max(0, positionMs);
                double positionSec = positionMs / 1000.0 + CurrentMessage.SongStartPos;

                musicService.PlaySong(song, positionSec);
            }
        }

        // Update playing status
        musicService.Playing = CurrentMessage.Playing;
        */

        // TODO: Update volume
        //}

        private PlayingResponse previousPullData;
        private PlayingResponse currentPullData;

        public void OnNewPullData(PullResponse data)
        {
            // TODO: implement properly


            if (data == null) {
                return;
            }

            var playingInfo = data.Playing;

            // update old messages
            previousPullData = currentPullData;
            currentPullData = playingInfo;

            // check for relevant change
            if (currentPullData.CurrentSongID == previousPullData.CurrentSongID) {
                // if no change, and we aren't playing, request next song
                return;
            }

            // if we don't have a song, skip
            if (!currentPullData.HasSong) {
                return;
            }

            // lookup song
            var song = musicService.GetSong(currentPullData.CurrentSongID);
            musicService.PlaySong(song);

            Debug.WriteLine("Tried to play: " + song.UniqueId + " name: " + song.Name);
        }
    }
}

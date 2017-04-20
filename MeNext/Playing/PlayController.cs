using System;
using MeNext.MusicService;
using SpotifyAPI.Web;

using System.Diagnostics;
using System.Collections.Generic;

namespace MeNext
{
    /// <summary>
    /// This class handles actually playing music. It should only be used by the host.
    /// </summary>
    public class PlayController : IPullUpdateObserver
    {
        private static Random rnd = new Random();

        private readonly IMusicService musicService;
        private bool hasPlayedSong = false;
        private MainController controller;

        private List<ISong> manuallyPlayedSongs = new List<ISong>();
        private List<ISong> radioSongs = new List<ISong>();

        public PlayController(IMusicService musicService, MainController controller)
        {
            this.controller = controller;
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
            if (data == null) {
                return;
            }

            var playingInfo = data.Playing;

            // update old messages
            previousPullData = currentPullData;
            currentPullData = playingInfo;

            // if we don't have a song, play from radio
            if (!currentPullData.HasSong) {
                musicService.Playing = false;
                Debug.WriteLine("Submit a song from radio suggestions");

                var possibleSongs = musicService.GetRecommendations(this.manuallyPlayedSongs);

                foreach (var song in radioSongs) {
                    possibleSongs.Remove(song);
                }
                foreach (var song in manuallyPlayedSongs) {
                    possibleSongs.Remove(song);
                }
                if (possibleSongs.Count > 0) {
                    // If there's an available recommendation, play it
                    var radioSong = possibleSongs[rnd.Next(possibleSongs.Count)];
                    this.radioSongs.Add(radioSong);
                    this.controller.Event.RequestAddToPlayNext(radioSong);
                }

                return;
            }

            // check for relevant change
            if (currentPullData.CurrentSongID != previousPullData.CurrentSongID) {
                var song = musicService.GetSong(currentPullData.CurrentSongID);

                if (!radioSongs.Contains(song) && !manuallyPlayedSongs.Contains(song)) {
                    manuallyPlayedSongs.Add(song);
                }

                musicService.PlaySong(song);
                Debug.WriteLine("Tried to play: " + song.UniqueId + " name: " + song.Name);
            }
            musicService.Playing = currentPullData.Playing;
        }
    }
}

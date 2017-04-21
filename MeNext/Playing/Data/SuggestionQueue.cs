using System;
using System.Collections.Generic;

namespace MeNext
{
    /// <summary>
    /// Represents a suggestion queue
    /// </summary>
    public class SuggestionQueue
    {
        public List<SongResponse> Songs { get; private set; }

        public SuggestionQueue()
        {
            this.Songs = new List<SongResponse>();
        }

        /// <summary>
        /// Updates the suggestion queue, adding new songs to the bottom and removing nonexistent songs.
        /// </summary>
        /// <param name="queue">Queue.</param>
        public void UpdateQueue(QueueResponse queue)
        {
            // This algorithm is super inefficient but n is pretty small
            var queueSongs = new List<SongResponse>(queue.Songs);

            // Add missing songs
            foreach (var song in queueSongs) {
                var idx = Songs.FindIndex((obj) => obj.ID == song.ID);
                if (idx == -1) {
                    // Add the song to the end of the list
                    Songs.Add(song);
                } else {
                    // Update the old song with any new metadata, but in the same position
                    Songs[idx] = song;
                }
            }

            // Remove songs which were once present but alas, no longer are
            Songs.RemoveAll((oldSong) => queueSongs.Find((newSong) => oldSong.ID == newSong.ID) == null);
        }

        // TODO Test this
        /// <summary>
        /// Sorts the suggestion queue.
        /// Sorts based on the selected option of popularity, newest first, or oldest first
        /// </summary>
        /// <param name="how">How.</param>
        public void SortMe(SortOptions how)
        {
            switch (how) {
                case SortOptions.POPULARITY:
                    this.Songs.Sort((x, y) => x.TotalVotes.CompareTo(y.TotalVotes));
                    break;

                case SortOptions.NEWEST_FIRST:
                    this.Songs.Sort((x, y) => x.PosAdded.CompareTo(y.PosAdded));
                    break;

                case SortOptions.OLDEST_FIRST:
                    this.Songs.Sort((x, y) => x.PosAdded.CompareTo(y.PosAdded));
                    break;
            }
        }
    }

    /// <summary>
    /// Options for sorting
    /// </summary>
    public enum SortOptions
    {
        OLDEST_FIRST, NEWEST_FIRST, POPULARITY
    }
}

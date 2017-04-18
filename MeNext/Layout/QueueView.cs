using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using MeNext.MusicService;


namespace MeNext
{
    /// <summary>
    /// Represents the Queue tab which contains the Suggestion and Play next queues
    /// </summary>
    public class QueueView : ContentPage, IPullUpdateObserver
    {
        private MainController mainController;
        private ListsView<ISong> songList;

        public QueueView(MainController mainController)
        {
            this.Title = "Queue";
            this.mainController = mainController;

            NavigationPage.SetHasNavigationBar(this, false);

            this.mainController.Event.RegisterPullObserver(this);

            var suggestionQueue = new ResultsGroup<ISong>("Suggestions", new SongItemFactory());
            var playNextQueue = new ResultsGroup<ISong>("Play Next", new SongItemFactory());

            this.songList = new ListsView<ISong>(mainController, playNextQueue, suggestionQueue);

            //var songList = new SongListView(model, new BasicSongCellFactory());
            //songList.OnSongSelected += (song) =>
            //{
            //    Debug.WriteLine("selected song: " + song.Name);
            //};

            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    this.songList,
                }
            };
        }

        public void OnNewPullData(PullResponse data)
        {
            // Get play next song uids
            var playNextSongUids = new List<string>();
            foreach (var song in data.PlayNextQueue.Songs) {
                playNextSongUids.Add(song.ID);
            }

            // Get suggestions song uids
            var suggestSongUids = new List<string>();
            foreach (var song in this.mainController.Event.SuggestionQueue.Songs) {
                suggestSongUids.Add(song.ID);
            }

            // Get the combined list of songs in one shot and then discard them.
            // This allows us to make only a single metadata call to the Spotify API, and then the backend caches the
            // ISongs, so the next 2 calls to GetSongs don't trigger separate calls to the Spotify API which speeds
            // things up a smidge
            var combinedUids = new List<string>();
            combinedUids.AddRange(playNextSongUids);
            combinedUids.AddRange(suggestSongUids);
            mainController.musicService.GetSongs(combinedUids);

            // Get the individual play next and suggestion song sets
            var playNextSongs = mainController.musicService.GetSongs(playNextSongUids);
            var suggestSongs = mainController.musicService.GetSongs(suggestSongUids);

            this.UpdateSongLists(playNextSongs, suggestSongs);
        }

        private void UpdateSongLists(IEnumerable<ISong> playNext, IEnumerable<ISong> suggestions)
        {
            this.songList.UpdateLists(playNext, suggestions);
        }

    }
}
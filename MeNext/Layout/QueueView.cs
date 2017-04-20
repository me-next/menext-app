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
        private MainController controller;
        private ListsView<ISong> songList;

        public QueueView(MainController controller)
        {
            this.Title = "Queue";
            this.controller = controller;

            NavigationPage.SetHasNavigationBar(this, false);

            this.controller.Event.RegisterPullObserver(this);

            var suggestionQueue = new ResultsGroup<ISong>("Suggestions", new SongItemFactory(this.controller));
            var playNextQueue = new ResultsGroup<ISong>("Up Next", new SongItemFactory(this.controller));

            this.songList = new ListsView<ISong>(controller, playNextQueue, suggestionQueue);

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

            // Look up the metadata with spotify for each song.
            var suggestSongUids = new List<string>();
            foreach (var song in this.controller.Event.SuggestionQueue.Songs) {
                suggestSongUids.Add(song.ID);
            }

            // Get the combined list of songs in one shot and then discard them.
            // This allows us to make only a single metadata call to the Spotify API, and then the backend caches the
            // ISongs, so the next 2 calls to GetSongs don't trigger separate calls to the Spotify API which speeds
            // things up a smidge
            var combinedUids = new List<string>();
            combinedUids.AddRange(playNextSongUids);
            combinedUids.AddRange(suggestSongUids);
            controller.musicService.GetSongs(combinedUids);

            // Get the individual play next and suggestion song sets
            var playNextSongs = controller.musicService.GetSongs(playNextSongUids);
            var suggestSongs = controller.musicService.GetSongs(suggestSongUids);

            this.UpdateSongLists(playNextSongs, suggestSongs);
        }

        private void UpdateSongLists(IEnumerable<ISong> playNext, IEnumerable<ISong> suggestions)
        {
            this.songList.UpdateLists(playNext, suggestions);
        }

    }
}
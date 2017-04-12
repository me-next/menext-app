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

            this.mainController.RegisterObserver(this);

            var suggestionQueue = new ResultsGroup<ISong>("Suggestions", new SongItemFactory());
            var playNextQueue = new ResultsGroup<ISong>("Play Next", new SongItemFactory());

            this.songList = new ListsView<ISong>(playNextQueue, suggestionQueue);

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
            var queue = data.SuggestQueue;

            var songUids = new List<string>();

            // look up the metadata with spotify for each song
            foreach (var song in queue.Songs) {
                songUids.Add(song.ID);
            }

            var songs = mainController.musicService.GetSongs(songUids);

            this.UpdateSongLists(songs, songs);

            // TODO: Update the play next queue insetad of just reusing the suggestion queue
        }

        private void UpdateSongLists(IEnumerable<ISong> playNext, IEnumerable<ISong> suggestions)
        {
            this.songList.UpdateLists(playNext, suggestions);
            //this.songList.UpdateLists(
            //    new ResultsGroup<ISong>("Play Next", playNext, new SongItemFactory()),
            //    new ResultsGroup<ISong>("Suggestions", suggestions, new SongItemFactory())
            //);
        }

    }
}
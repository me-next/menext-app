using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;


namespace MeNext
{
    public class QueueView : ContentPage
    {
        private MainController mainController;

        public QueueView(MainController mainController)
        {
            // TODO: clean up this testing code
            this.Title = "Queue Placeholder";
            this.mainController = mainController;

            List<Song> songs = new List<Song>
            {
                new Song("A"),
                new Song("B"),
            };

            NavigationPage.SetHasNavigationBar(this, false);

            // suggestion queue model observes the pulls
            var model = new SuggestionQueueModel(songs);
            mainController.RegisterObserver(model);

            var songList = new SongListView(model, new BasicSongCellFactory());
            songList.OnSongSelected += (song) =>
            {
                Debug.WriteLine("selected song: " + song.Name);
            };

            int songCounter = 0;
            var addButton = new Button { Text = "addSong" };
            addButton.Clicked += (sender, e) =>
            {
                var song = new Song("song" + songCounter);
                model.Add(song);
                songCounter++;

                Debug.WriteLine("adding song: " + song.Name);
            };

            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    addButton,
                    new Label { Text = "queue holder" },
                    songList,
                }
            };
        }
    }

    /// <summary>
    /// Suggestion queue model does suggestion queue specific work. 
    /// </summary>
    public class SuggestionQueueModel : SongListModel
    {
        public SuggestionQueueModel(List<Song> songs) : base(songs)
        {
        }

        /// <summary>
        /// Called by MainController when new pull data comes in. This builds the model from the pull data. 
        /// </summary>
        /// <param name="data">Data.</param>
        public override void onNewPullData(PullResponse data)
        {
            // TODO: use ISongs, will need spotify lookup to get more detailed song info. 
            var queue = data.SuggestQueue;
            var songs = new List<Song>();

            foreach(var elem in queue.Songs) {
                songs.Add(new Song(elem.ID));
            }

            SetAll(songs);
        }
    }
}

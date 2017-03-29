using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using MeNext.MusicService;
using MeNext.SampleMusicService;


namespace MeNext
{
    public class SearchView : ContentPage
    {
        private SearchBar searchBar;
        private Label resultsLabel;
        private SongListModel model;
        private bool liveSearch;
        private MainController controller;

        public SearchView(MainController controller)
        {
            this.controller = controller;

            // TODO: clean up this testing code
            NavigationPage.SetHasNavigationBar(this, false);
            ISong selectedSong = null;
            liveSearch = false;

            resultsLabel = new Label
            {
                Text = ""
            };

            //TODO: hide playNextButton if user doesn't have permission to add to PN queue
            Button playNextButton = new Button
            {
                Text = "Add to PlayNext",
                BackgroundColor = Color.Orange,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            playNextButton.Clicked += (sender, e) =>
            {
                if (selectedSong == null) {
                    return;
                }
                //TODO: add song to actual playnext queue
                Debug.WriteLine("adding song to play next: " + selectedSong.Name);
            };

            Button suggestionButton = new Button
            {
                Text = "Add to Suggestions",
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            suggestionButton.Clicked += (sender, e) =>
            {
                if (selectedSong == null) {
                    return;
                }
                //TODO: add song to actual suggestion queue
                Debug.WriteLine("adding song to suggestions: " + selectedSong.Name);
            };

            StackLayout queueButtons = new StackLayout
            {
                Padding = 3,
                Orientation = StackOrientation.Horizontal,
                Children = {
                    playNextButton,
                    suggestionButton
                }
            };

            model = new SongListModel(new List<ISong>());
            SongListView songList = new SongListView(model, new BasicSongCellFactory());
            songList.OnSongSelected += (song) =>
            {
                Debug.WriteLine("selected song: " + song.Name);
                selectedSong = song;
            };

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                SearchCommand = new Command(() => SearchForSong(searchBar.Text))
            };
            searchBar.TextChanged += (sender, e) => TextChanged(searchBar.Text);

            //button for testing, can be removed/repurposed later
            Button toggleLive = new Button
            {
                Text = "Toggle Live Searching",
                BackgroundColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center
            };
            toggleLive.Clicked += (sender, e) =>
            {
                liveSearch = !liveSearch;
                if (liveSearch) {
                    toggleLive.BackgroundColor = Color.Green;
                } else {
                    toggleLive.BackgroundColor = Color.Red;
                }
            };

            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    queueButtons,
                    searchBar,
                    toggleLive,
                    resultsLabel,
                    songList,
                }
            };
        }

        /// <summary>
        /// The function to handle when search command (generally via keyboard search button) is sent to the search bar
        /// Takes in the text value from the search bar, searches with the music service, and updates the SongViewModel
        /// </summary>
        public void SearchForSong(string text)
        {
            //TODO: get songs from music service

            resultsLabel.Text = "";

            List<ISong> noSongs = new List<ISong>();

            //IMusicService service = new SampleMusicService.SampleMusicService();
            IResultList<ISong> results = controller.musicService.SearchSong(text);

            if (results == null || results.Items == null || results.Items.Count == 0) {
                resultsLabel.Text = "No Results.";
                model.SetAll(noSongs);
            } else {
                model.SetAll(results.Items);
            }

        }

        /// <summary>
        /// The function to handle when the text in the search bar changes. 
        /// If live searching is enabled, calls the SearchForSong function.
        /// </summary>
        private void TextChanged(string text)
        {
            if (liveSearch) {
                SearchForSong(text);
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using MeNext.MusicService;

namespace MeNext
{
    /// <summary>
    /// Search view.
    /// Contains a list of searched songs and an entry for song info to search.
    /// </summary>
    public class SearchView : ContentPage
    {
        private SearchBar searchBar;
        private Label resultsLabel;
        //private SongListModel model;
        private MainController controller;
        private ResultListView<ISong> songList;

        public SearchView(MainController controller)
        {
            this.controller = controller;

            NavigationPage.SetHasNavigationBar(this, false);

            resultsLabel = new Label
            {
                Text = ""
            };

            this.songList = new ResultListView<ISong>(this.controller, new SongItemFactory(controller));

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                SearchCommand = new Command(() => SearchForSong(searchBar.Text)),

                // Without this line, the search bar is invisible in Android 7
                // See https://bugzilla.xamarin.com/show_bug.cgi?id=43975
                HeightRequest = 40
            };
            searchBar.TextChanged += (sender, e) => TextChanged(searchBar.Text);

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    searchBar,
                    resultsLabel,
                    this.songList,
                }
            };
        }

        /// <summary>
        /// The function to handle when search command (generally via keyboard search button) is sent to the search bar
        /// Takes in the text value from the search bar, searches with the music service, and updates the SongViewModel
        /// </summary>
        public void SearchForSong(string text)
        {
            resultsLabel.Text = "";

            IResultList<ISong> results = controller.musicService.SearchSong(text);

            if (results == null || results.Items == null || results.Items.Count == 0) {
                resultsLabel.Text = "No Results.";
                //model.SetAll(noSongs);
                this.songList.UpdateResultList(new SimpleResultList<ISong>(new List<ISong>()));
            } else {
                //model.SetAll(results.Items);
                this.songList.UpdateResultList(results);
            }

        }

        /// <summary>
        /// The function to handle when the text in the search bar changes. 
        /// If live searching is enabled, calls the SearchForSong function.
        /// </summary>
        private void TextChanged(string text)
        {
            // TODO: Make this more efficient so we can have live searching
            // SearchForSong(text);
        }

    }

}

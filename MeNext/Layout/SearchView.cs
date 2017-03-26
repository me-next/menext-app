using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;


namespace MeNext
{
    public class SearchView : ContentPage
    {
        private SearchBar searchBar;
        private Label resultsLabel;
        private SongListModel model;

        public SearchView()
        {
            // TODO: clean up this testing code
            NavigationPage.SetHasNavigationBar(this, false);
            Song selectedSong = null;

            resultsLabel = new Label
            {
                Text = ""
            };

            Button playNextButton = new Button 
            { 
                Text = "Add to PlayNext", 
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
                //VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    playNextButton,
                    suggestionButton
                }
            };

            model = new SongListModel(new List<Song>());
            SongListView songList = new SongListView(model, new BasicSongCellFactory());
            songList.OnSongSelected += (song) =>
            {
                Debug.WriteLine("selected song: " + song.Name);
                selectedSong = song;
            };

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                SearchCommand = new Command(() => TextChanged(searchBar.Text))
            };
            searchBar.TextChanged += (sender, e) => TextChanged(searchBar.Text);

            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    queueButtons,
                    searchBar,
                    resultsLabel,
                    songList,
                }
            };
        }

        /// <summary>
        /// The function to handle when the text in the search bar changes
        /// Takes in the text value from the search bar, searches with the music service, and updates the SongViewModel
        /// </summary>
        public void TextChanged(string text)
        {
            //TODO: get songs from music service

            resultsLabel.Text = "";

            List<Song> songs = new List<Song>();

            List<Song> songs1 = new List<Song>
            {
                new Song("A"),
                new Song("B"),
                new Song("C"),
            };

            List<Song> songs2 = new List<Song>
            {
                new Song("1"),
                new Song("2"),
                new Song("3"),
            };

            List<Song> searchResults = new List<Song>();
            if (text == "") {
                resultsLabel.Text = "";
                searchResults = songs;
            } else if (text == "letters") {
                searchResults = songs1;
            } else if (text == "numbers") {
                searchResults = songs2;
            } else {
                //TODO: edit/remove once there are actual results from the music service
                resultsLabel.Text = "No Results.";
                searchResults = songs;
            }

            model.Clear();
            model.AddMultiple(searchResults);
        }

    }

}

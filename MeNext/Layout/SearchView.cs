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
		private SongListView songList;

		public SearchView()
		{
			// TODO: clean up this testing code
			this.Title = "Search";
			NavigationPage.SetHasNavigationBar(this, false);
			Song selectedSong = null;

			resultsLabel = new Label
			{
				Text = "Results will appear below.",
				//VerticalOptions = LayoutOptions.FillAndExpand,
				//FontSize = 25
			};

			Button playNextButton = new Button { Text = "Add to PlayNext", HorizontalOptions = LayoutOptions.StartAndExpand };
			playNextButton.Clicked += (sender, e) =>
			{
				if (selectedSong == null) return;
				//TODO: add song to actual playnext queue
				//Debug.WriteLine("adding song to play next: " + song.Name);
			};
			Button suggestionButton = new Button { Text = "Add to Suggestions", HorizontalOptions = LayoutOptions.EndAndExpand };
			suggestionButton.Clicked += (sender, e) =>
			{
				if (selectedSong == null) return;
				//TODO: add song to actual suggestion queue
				//Debug.WriteLine("adding song to suggestions: " + song.Name);
			};
			StackLayout queueButtons = new StackLayout
			{
				Padding = LayoutConsts.DEFAULT_PADDING,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					playNextButton,
					suggestionButton
				}
			};

			//var model = new SongListModel(new List<Song>());
			songList = new SongListView(new SongListModel(new List<Song>()), new BasicSongCellFactory());
			songList.OnSongSelected += (song) =>
			{
				Debug.WriteLine("selected song: " + song.Name);
				selectedSong = song;
			};

			searchBar = new SearchBar
			{
				Placeholder = "Enter search term",
				SearchCommand = new Command( () => TextChanged(searchBar.Text) )
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

		public void TextChanged(string text)
		{
			//TODO: get songs from music service
			resultsLabel.Text = "Searching for: " + text;

			List<Song> songs = new List<Song> { new Song("No"), new Song("Results"), };

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
			if (text == "letters")
			{
				searchResults = songs1;
			}
			else if (text == "numbers")
			{
				searchResults = songs2;
			}
			else
			{
				searchResults = songs;
			}

			//TODO: make this work with the SongListView better; use the Songs variable, or something similar?
			songList.ItemsSource = searchResults;
			//songList.Songs = new SongListModel(searchResults);
		}

	}

}

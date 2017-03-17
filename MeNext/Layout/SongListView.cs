using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeNext
{
	// Song is a POD class for song information.
	// TODO: maybe move this out of this file? Create a more legitimate version? Use the implementation
	// from menext/music-service?
	public class Song
	{
		public Song(string name)
		{
			this.Name = name;
		}

		public Song(string name, string artist)
		{
			this.Name = name;
			this.Artist = artist;
		}

		// should we provide getters on these?
		public string Name 
		{ 
			get; 
		}

		public string Artist
		{
			get;
		}

		public override string ToString()
		{
			return string.Format("[Song: Name={0}, Artist={1}]", Name, Artist);
		}
	};

	// SongCellFactory controls how the song gets rendered for the SongList.
	// Add buttons, click events etc. in here. 
	public abstract class SongCellFactory
	{
		public abstract ViewCell BuildView();
	};

	// DefaultSongCellFactory renders a basic view cell that only shows the song name. 
	public class BasicSongCellFactory : SongCellFactory
	{
		public override ViewCell BuildView()
		{
			// create views with bindings for displaying each property
			Label nameLabel = new Label();
			nameLabel.SetBinding(Label.TextProperty, "Name");

			return new ViewCell
			{
				View = new StackLayout
				{
					Padding = new Thickness(0, 5),
					Orientation = StackOrientation.Horizontal,
					Children = { nameLabel }
				},
			};
		}
	};

	// SongListModel is the model that backs the SongListView.
	// A controller updates this list. The view observes the model and updates as needed
	// This doesn't provide any priority-queue functionality, since the truth is always the server.
	public class SongListModel : ObservableCollection<Song>
	{
		public SongListModel(List<Song> songs) : base(songs)
		{
		}

		// TODO: implement update from client
		// TODO: have this take the concrete impl of the music-service results stuff
	};

	// SongListView displays a collection of songs. This is the "view". 
	// The SongCellFactory set in the constructor provides significant control of the way the list behaves. 
	public class SongListView : ListView
	{
		// TODO: include drag and drop endpoints for a drag-n-drop controller

		// TODO: create other constructors or expose setters?

		public SongListView(SongListModel songs, SongCellFactory factory)
		{
			// use the default factory
			SetItemTemplateWithFactory(factory);

			// set the model
			this.songs = songs;
			this.ItemsSource = this.songs;

			// add tap handler
			this.ItemTapped += OnItemTapped;
		}

		// set the ListView's ItemTemplate to use the provided SongFactory
		private void SetItemTemplateWithFactory(SongCellFactory factory)
		{
			this.ItemTemplate = new DataTemplate(() =>
			{
				return factory.BuildView();
			});
		}

		private SongListModel songs
		{
			// not sure that we need a "get"
			get;

			// set should clear all of the existing songs
			set;
		}

		// delegate for when a song is clicked
		public delegate void SongClickedEvent(Song song);

		// called when a song is tapped on once
		public event SongClickedEvent OnSongSelected;

		// wrapper around OnItemTapped that pulls out just the selected song
		private void OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			var song = (Song)e.Item;
			this.OnSongSelected(song);
		}
	}
}

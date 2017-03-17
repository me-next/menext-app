using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;

namespace MeNext
{
	// Song is a POD class for song information
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

	// SongFactory controls how the song gets rendered for the SongList.
	// Add buttons, click events etc. in here. 
	public abstract class SongFactory
	{
		public abstract ViewCell BuildView();
	};

	// renders a basic view cell
	public class DefaultSongFactory : SongFactory
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
					Children =
								{
									nameLabel,
								}
				},
			};
		}
	};

	// SongList manages presenting a list of songs. 
	// Down the road we will extend this to include drag and drop
	// 
	// TODO: provide custom model to make managing straight forwards
	public class SongList : ListView
	{
		public SongList(List<Song> songs, SongFactory factory)
		{
			SetItemTemplateWithFactory(factory);

			// TODO: look into using an observable list for this
			this.songs = new List<Song>();
			this.ItemsSource = this.songs;

			foreach (Song song in songs)
			{
				this.songs.Add(song);
			}

			// add tap'd handler
			this.ItemTapped += OnItemTapped;
		}

		public SongList(List<Song> songs)
		{
			// use the default factory
			SetItemTemplateWithFactory(new DefaultSongFactory());

			this.songs = new List<Song>();
			this.ItemsSource = this.songs;

			foreach (Song song in songs)
			{
				this.songs.Add(song);
			}

			// add tap'd handler
			this.ItemTapped += OnItemTapped;
		}

		// construct an empty song list with the default song factory
		public SongList()
		{
			// use the default factory
			SetItemTemplateWithFactory(new DefaultSongFactory());

			// set default song list
			this.songs = new List<Song>();
			this.ItemTapped += OnItemTapped;
		}

		// set the ListView's ItemTemplate to use the provided SongFactory
		private void SetItemTemplateWithFactory(SongFactory factory)
		{
			this.ItemTemplate = new DataTemplate(() =>
			{
				return factory.BuildView();
			});
		}

		// TODO: make this a custom list object
		private List<Song> songs
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

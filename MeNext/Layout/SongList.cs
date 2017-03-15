using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace MeNext
{
	// TODO: images? buttons on the sides?
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
	};

	// SongList manages presenting a list of songs. 
	// Down the road we will extend this to include 
	public class SongList : ListView
	{
		public SongList(List<Song> songs)
		{
			this.ItemTemplate = ItemTemplate = new DataTemplate(() =>
					{
						// Create views with bindings for displaying each property.
						Label nameLabel = new Label();
						nameLabel.SetBinding(Label.TextProperty, "Name");

						Label birthdayLabel = new Label();
						birthdayLabel.SetBinding(Label.TextProperty,
							new Binding("Birthday", BindingMode.OneWay,
								null, null, "Born {0:d}"));

						BoxView boxView = new BoxView();
						boxView.SetBinding(BoxView.ColorProperty, "FavoriteColor");

						// Return an assembled ViewCell.
						return new ViewCell
						{
							View = new StackLayout
							{
								Padding = new Thickness(0, 5),
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									boxView,
									new StackLayout
									{
										VerticalOptions = LayoutOptions.Center,
										Spacing = 0,
										Children =
										{
											nameLabel,
											birthdayLabel
										}
										}
								}
							}
						};
					});

			this.songs = songs;

			// TODO: look into copy / write mechanics messing with this
			this.ItemsSource = this.songs;
		}

		public List<Song> songs
		{
			// not sure that we need a "get"
			get;
			// set should clear all of the existing songs
			set;

		}


	}
}

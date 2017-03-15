using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace MeNext
{
	public class QueueView : ContentPage
	{
		
		/*
		private ListView getListView(List<Song> songs)
		{
			// Create the ListView.
			ListView ret = new ListView
			{
				// Source of data items.
				ItemsSource = songs,

				// Define template for displaying each item.
				// (Argument of DataTemplate constructor is called for 
				//      each item; it must return a Cell derivative.)
				ItemTemplate = new DataTemplate(() =>
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
					})
			};

			return ret;
		}

*/
		public QueueView()
		{
			this.Title = "Queue Placeholder";

			List<Song> songs = new List<Song>
			{
				new Song("A"),
				new Song("B"),
			};

			NavigationPage.SetHasNavigationBar(this, false);
			Content = new StackLayout
			{
				Padding = LayoutConsts.DEFAULT_PADDING,
				Children = {
					new Label { Text = "queue holder" },
					new SongList(songs),
				}
			};
		}
	}
}

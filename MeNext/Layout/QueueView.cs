using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;


namespace MeNext
{
	public class QueueView : ContentPage
	{
		public QueueView()
		{
			// TODO: clean up this testing code
			this.Title = "Queue Placeholder";

			List<Song> songs = new List<Song>
			{
				new Song("A"),
				new Song("B"),
			};

			NavigationPage.SetHasNavigationBar(this, false);
			var model = new SongListModel(songs);
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
}

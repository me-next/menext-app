using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace MeNext
{
	public class QueueView : ContentPage
	{
		public QueueView()
		{
			this.Title = "Queue Placeholder";

			List<Song> songs = new List<Song>
			{
				new Song("A"),
				new Song("B"),
			};

			NavigationPage.SetHasNavigationBar(this, false);
			var songList = new SongList(songs);
			songList.OnSongSelected += (song) =>
			{
				System.Diagnostics.Debug.WriteLine("selected song: " + song.Name);
			};

			Content = new StackLayout
			{
				Padding = LayoutConsts.DEFAULT_PADDING,
				Children = {
					new Label { Text = "queue holder" },
					songList,
				}
			};
		}
	}
}

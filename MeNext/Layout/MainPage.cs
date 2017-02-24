using System;

using Xamarin.Forms;

namespace MeNext.Layout
{
	public class MainPage : TabbedPage
	{
		public MainPage()
		{
			this.Title = "MeNext";

			var homeScreen = new NavigationPage(new HomeScreen());
			homeScreen.Title = "Home";
			Children.Add(homeScreen);

			var playingScreen = new NavigationPage(new PlaceholderScreen());
			playingScreen.Title = "Playing";
			Children.Add(playingScreen);

			var libraryScreen = new NavigationPage(new PlaceholderScreen());
			libraryScreen.Title = "Library";
			Children.Add(libraryScreen);

			var queueScreen = new NavigationPage(new PlaceholderScreen());
			queueScreen.Title = "Queue";
			Children.Add(queueScreen);
		}
	}
}


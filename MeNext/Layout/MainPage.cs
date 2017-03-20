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
			//homeScreen.Icon = "homeScreenIcon.png";  If we make this icon
			Children.Add(homeScreen);

			var playingScreen = new NavigationPage(new PlayingScreen());
			playingScreen.Title = "Playing"; 
			//playingScreen.Icon = "playingScreenIcon.png";  If we make this icon
			Children.Add(playingScreen);

			var libraryScreen = new NavigationPage(new PlaceholderScreen());
			libraryScreen.Title = "Library";
			//libraryScreen.Icon = "libraryScreenIcon.png";  If we make this icon
			Children.Add(libraryScreen);

			var queueScreen = new NavigationPage(new PlaceholderScreen());
			queueScreen.Title = "Queue";
			//queueScreen.Icon = "queueScreenIcon.png";
			Children.Add(queueScreen);
		}
	}
}


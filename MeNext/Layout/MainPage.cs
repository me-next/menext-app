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

			var searchScreen = new NavigationPage(new SearchView());
			searchScreen.Title = "Search";
			Children.Add(searchScreen);

			var queueScreen = new NavigationPage(new QueueView());
			queueScreen.Title = "Queue";
			Children.Add(queueScreen);

		}
	}
}


using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext.Layout
{
	/// <summary>
	/// The main tabbed page layout with stuff on it
	/// </summary>
	public class MainPage : TabbedPage
	{
		public IMusicService MusicService { get; set; }
		public MainController MainController { get; set; }

		public MainPage()
		{
			// Backend stuff
			MusicService = new SampleMusicService.SampleMusicService();
			MainController = new MainController(MusicService);
			MainController.RequestJoinEvent("testevent");		// TODO: Remove when we have UI for this

			// UI Stuff
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


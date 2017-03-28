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
        public MainController mainController { get; set; }

        public MainPage(MainController mainController)
        {
            // Backend stuff
            //MusicService = new SampleMusicService.SampleMusicService();
            this.mainController = mainController;
            //mainController.RequestJoinEvent("testevent");       // TODO: Remove when we have UI for this

            // UI Stuff
            this.Title = "MeNext";
            var homeScreen = new NavigationPage(new HomeScreen(mainController));
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

            var searchScreen = new NavigationPage(new SearchView());
            searchScreen.Title = "Search";
            //searchScreen.Icon = "searchScreenIcon.png";  If we make this icon
            Children.Add(searchScreen);

            var queueScreen = new NavigationPage(new QueueView(mainController));
            queueScreen.Title = "Queue";
            //queueScreen.Icon = "queueScreenIcon.png";
            Children.Add(queueScreen);

        }
    }
}


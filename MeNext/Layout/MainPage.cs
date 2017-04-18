using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext.Layout
{
    /// <summary>
    /// The main tabbed page layout with stuff on it
    /// </summary>
    public class MainPage : TabbedPage, IUIChangeListener
    {
        public IMusicService MusicService { get; set; }
        public MainController mainController { get; set; }

        private List<NavigationPage> pages = new List<NavigationPage>();
        private bool tabsShown;

        public MainPage(MainController mainController)
        {
            this.mainController = mainController;
            this.tabsShown = false;

            // UI Stuff
            NavigationPage.SetHasNavigationBar(this, false);
            this.Title = "MeNext";

            var homeScreen = new NavigationPage(new HomeScreen(mainController, mainController.NavPage));
            homeScreen.Title = "Home";
            homeScreen.Icon = "home_screen_icon";
            this.Children.Add(homeScreen);

            this.mainController.RegisterUiListenerDangerous(this);
        }

        public void SomethingChanged()
        {
            // Enable or disable available tabs
            if (mainController.InEvent != this.tabsShown) {
                this.tabsShown = mainController.InEvent;

                if (this.tabsShown) {
                    var playingScreen = new NavigationPage(new PlayingScreen(mainController));
                    playingScreen.Title = "Playing";
                    playingScreen.Icon = "play_screen_icon";
                    pages.Add(playingScreen);

                    var libraryScreen = new NavigationPage(new LibraryView(mainController));
                    libraryScreen.Title = "Library";
                    libraryScreen.Icon = "library_screen_icon";
                    pages.Add(libraryScreen);

                    var searchScreen = new NavigationPage(new SearchView(mainController));
                    searchScreen.Title = "Search";
                    searchScreen.Icon = "search_screen_icon";
                    pages.Add(searchScreen);

                    var queueScreen = new NavigationPage(new QueueView(mainController));
                    queueScreen.Title = "Queue";
                    queueScreen.Icon = "queue_screen_icon";
                    pages.Add(queueScreen);

                    foreach (var page in this.pages) {
                        this.Children.Add(page);
                    }
                } else {
                    foreach (var page in this.pages) {
                        this.Children.Remove(page);
                    }
                    this.pages.Clear();
                }
            }
        }
    }
}


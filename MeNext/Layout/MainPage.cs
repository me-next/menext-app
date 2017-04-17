using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext.Layout
{
    /// <summary>
    /// The main tabbed page layout with stuff on it.
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
            //homeScreen.Icon = "homeScreenIcon.png";
            this.Children.Add(homeScreen);

            this.mainController.RegisterUiListenerDangerous(this);
        }
        /// <summary>
        /// Something has changed. Update the UI accordingly.
        /// </summary>
        public void SomethingChanged()
        {
            // Enable or disable available tabs
            if (mainController.InEvent != this.tabsShown) {
                this.tabsShown = mainController.InEvent;
                // Show tabs if in event
                if (this.tabsShown) {
                    var playingScreen = new NavigationPage(new PlayingScreen(mainController));
                    playingScreen.Title = "Playing";
                    //playingScreen.Icon = "playingScreenIcon.png";
                    pages.Add(playingScreen);

                    var libraryScreen = new NavigationPage(new LibraryView(mainController));
                    libraryScreen.Title = "Library";
                    //libraryScreen.Icon = "libraryScreenIcon.png";
                    pages.Add(libraryScreen);

                    var searchScreen = new NavigationPage(new SearchView(mainController));
                    searchScreen.Title = "Search";
                    //searchScreen.Icon = "searchScreenIcon.png";
                    pages.Add(searchScreen);

                    var queueScreen = new NavigationPage(new QueueView(mainController));
                    queueScreen.Title = "Queue";
                    //queueScreen.Icon = "queueScreenIcon.png";
                    pages.Add(queueScreen);

                    foreach (var page in this.pages) {
                        this.Children.Add(page);
                    }
                } else { // No Event no tabs.
                    foreach (var page in this.pages) {
                        this.Children.Remove(page);
                    }
                    this.pages.Clear();
                }
            }
        }
    }
}


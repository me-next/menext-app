using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using MeNext.MusicService;


namespace MeNext
{
    /// <summary>
    /// Represents the Queue tab which contains the Suggestion and Play next queues
    /// </summary>
    public class LibraryView : ContentPage, IUIChangeListener
    {
        private MainController mainController;

        private Button loginSpotify;
        private Button playlistButton;
        private Button songButton;

        public LibraryView(MainController mainController)
        {
            this.Title = "Library";
            this.mainController = mainController;

            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };

            layout.Children.Add(loginSpotify = new Button
            {
                Text = "Login to Spotify",
                Command = new Command(() => this.mainController.musicService.Login())
            });

            layout.Children.Add(songButton = new Button
            {
                Text = "Songs",
                Command = new Command<ResultItemData>((obj) => Navigation.PushAsync(new SongLibraryView(mainController)))
            });

            layout.Children.Add(playlistButton = new Button
            {
                Text = "Playlists",
                Command = new Command(() => Navigation.PushAsync(new PlaylistLibraryView(mainController, this.Navigation)))
            });


            Content = layout;

            this.mainController.Event.RegisterUiListener(this);
            this.SomethingChanged();
        }
        /// <summary>
        /// User's Spotify account's login status has changed.
        /// Update UI based on if song can be added or if login is needed.
        /// </summary>
        public void SomethingChanged()
        {
            var logged = this.mainController.musicService.LoggedIn;
            loginSpotify.IsVisible = !logged;
            songButton.IsVisible = logged;
            playlistButton.IsVisible = logged;
        }
    }
    /// <summary>
    /// Content page that contains a view of a song library.
    /// Displays a ResultListView that contains the user's library's songs.
    /// </summary>
    public class SongLibraryView : ContentPage
    {
        public SongLibraryView(MainController controller)
        {
            this.Title = "Songs";

            var resultsView = new ResultListView<ISong>(controller, new SongItemFactory(controller));
            resultsView.UpdateResultList(controller.musicService.UserLibrarySongs);

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    resultsView
                }
            };

        }
    }

    public class PlaylistLibraryView : ContentPage
    {
        public PlaylistLibraryView(MainController controller, INavigation nav)
        {
            this.Title = "Playlists";

            var resultsView = new ResultListView<IPlaylist>(controller, new PlaylistItemFactory(controller, nav));
            resultsView.UpdateResultList(controller.musicService.UserLibraryPlaylists);

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    resultsView
                }
            };

        }
    }
}
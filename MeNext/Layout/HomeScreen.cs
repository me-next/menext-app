using System;

using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Home screen of the MeNext App.  Contains options to Join event, host event, login to spotify,
    /// logout of spotify, and leave event.  Buttons are created only if User should be able to access the button.
    /// i.e. cannot logout of spotify if the user isn't logged in.
    /// </summary>
    public class HomeScreen : ContentPage, IUIChangeListener
    {
        public const string SPOTIFY_WHY = "Logging in with Spotify Premium allows you to host events, where the " +
            "music comes out of your speakers. Otherwise, you must join someone else's event.";

        private MainController mc;

        private Label eventName;
        private Button joinEvent;
        private Button hostEvent;
        private Button loginSpotify;
        private Button logoutSpotify;
        private Button leaveEvent;
        private Label loginSpotifyWhy;

        public HomeScreen(MainController mc, NavigationPage nav)
        {
            this.mc = mc;
            var musicService = mc.musicService;

            this.Title = "Home";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout()
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };

            layout.Children.Add(eventName = new Label { HorizontalTextAlignment = TextAlignment.Center });

            layout.Children.Add(joinEvent = new Button
            {
                Text = "Join Event",
                Command = new Command(() => nav.Navigation.PushAsync(new JoinEvent(mc)))
            });

            layout.Children.Add(hostEvent = new Button
            {
                Text = "Host Event",
                Command = new Command(() => nav.Navigation.PushAsync(new HostEvent(mc)))
            });

            layout.Children.Add(leaveEvent = new Button
            {
                Text = "Leave Event",
                Command = new Command(() => mc.LeaveEvent())
            });

            layout.Children.Add(loginSpotify = new Button
            {
                Text = "Login with Spotify",
                Command = new Command(() => musicService.Login())
            });

            layout.Children.Add(loginSpotifyWhy = new Label
            {
                Text = SPOTIFY_WHY,
                HorizontalTextAlignment = TextAlignment.Center
            });

            layout.Children.Add(logoutSpotify = new Button
            {
                Text = "Logout of Spotify",
                Command = new Command(() => musicService.Logout())
            });

            Content = layout;

            mc.RegisterUiListenerDangerous(this);
            this.SomethingChanged();
        }
        /// <summary>
        /// Something has changed.  Updates UI to represent the new changes.
        /// Updates event name and visibility of buttons.
        /// </summary>
        public void SomethingChanged()
        {
            if (this.mc.InEvent) {
                this.eventName.Text = "Event Id: " + this.mc.Event.Slug;
            } else {
                this.eventName.Text = " ";
            }

            this.joinEvent.IsVisible = !this.mc.InEvent;
            this.hostEvent.IsVisible = !this.mc.InEvent && mc.musicService.LoggedIn;  // TODO Check premium too
            this.leaveEvent.IsVisible = this.mc.InEvent;
            this.loginSpotify.IsVisible = !mc.musicService.LoggedIn;
            this.loginSpotifyWhy.IsVisible = this.loginSpotify.IsVisible;
            this.logoutSpotify.IsVisible = mc.musicService.LoggedIn;
        }
    }
}

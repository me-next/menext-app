using System;

using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace MeNext
{
    /// <summary>
    /// Home screen of the MeNext App. Contains options to Join event, host event, login to spotify,
    /// logout of spotify, and leave event. Buttons are created only if User should be able to access the button.
    /// i.e. cannot logout of spotify if the user isn't logged in.
    /// </summary>
    public class HomeScreen : ContentPage, IUIChangeListener
    {
        public const string SPOTIFY_WHY = "Logging in with Spotify Premium allows you to host events, where the " +
            "music comes out of your speakers. Otherwise, you must join someone else's event.";

        private MainController mc;

        private Dictionary<string, Button> buttons;

        private Label eventName;
        private Button joinEvent;
        private Button hostEvent;
        private Button loginSpotify;
        private Button logoutSpotify;
        private Button leaveEvent;
        private Label loginSpotifyWhy;

        Label permissionsLabel;

        private AboutPage aboutPage;

        public HomeScreen(MainController mc, NavigationPage nav)
        {
            this.mc = mc;
            var musicService = mc.musicService;

            this.buttons = new Dictionary<string, Button>();

            this.Title = "Home";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout()
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };

            layout.Children.Add(eventName = new Label
            {
                FontSize = LayoutConsts.TITLE_FONT_SIZE,
                HorizontalTextAlignment = TextAlignment.Center,
            });

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

            layout.Children.Add(permissionsLabel = new Label
            {
                Text = "Permissions for everybody else:",
                HorizontalTextAlignment = TextAlignment.Center,
                IsVisible = false,
            });

            // TODO: do this properly

            var playPauseButt = new Button
            {
                Text = "Play/Pause",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
            };
            playPauseButt.Clicked += (sender, e) => TogglePermission(Permissions.PlayPause);
            layout.Children.Add(playPauseButt);
            buttons.Add(Permissions.PlayPause, playPauseButt);

            var skipButt = new Button
            {
                Text = "Skip songs",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
            };
            skipButt.Clicked += (sender, e) => TogglePermission(Permissions.Skip);
            layout.Children.Add(skipButt);
            buttons.Add(Permissions.Skip, skipButt);

            var nowButt = new Button
            {
                Text = "Add to up next",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
            };
            nowButt.Clicked += (sender, e) => TogglePermission(Permissions.PlayNext);
            layout.Children.Add(nowButt);
            buttons.Add(Permissions.PlayNext, nowButt);


            var volButt = new Button
            {
                Text = "Control volume",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
            };
            volButt.Clicked += (sender, e) => TogglePermission(Permissions.Volume);
            layout.Children.Add(volButt);
            buttons.Add(Permissions.Volume, volButt);

            aboutPage = new AboutPage(this);
            var aboutButton = new Button
            {
                Text = "About"
            };
            aboutButton.Clicked += (sender, e) => 
            {
                aboutPage.prevView = this.Content;
                this.Content = aboutPage;
            };
            layout.Children.Add(aboutButton);

            Content = layout;

            mc.RegisterUiListenerDangerous(this);
            this.SomethingChanged();
        }

        /// <summary>
        /// Toggles the value of a permission 
        /// </summary>
        /// <param name="which">Which.</param>
        public void TogglePermission(string which)
        {
            // try to flip the permission
            this.mc.Event.RequestSetPermission(which, !this.mc.Event.Permissions.GetPermission(which));
        }

        /// <summary>
        /// Helper function to set the button color
        /// </summary>
        /// <param name="which">Which.</param>
        private void SetButtonColorForField(string which)
        {
            // get the button
            Button result;
            var has = buttons.TryGetValue(which, out result);
            if (!has) {
                Debug.WriteLine("couldn't find key: " + which);
                return;
            }

            // now try to color the button
            var enabled = mc.Event.Permissions.GetPermission(which);
            if (enabled) {
                result.BackgroundColor = Color.Green;
            } else {
                result.BackgroundColor = Color.Red;
            }
        }
        /// <summary>
        /// Something has changed. Updates UI to represent the new changes.
        /// Updates event name and visibility of buttons.
        /// </summary>
        public void SomethingChanged()
        {
            if (this.mc.InEvent) {
                this.eventName.Text = "Event Id: " + this.mc.Event.Slug;
                this.eventName.Margin = new Thickness(0, 30, 0, 0);
            } else {
                this.eventName.Text = " ";
                this.eventName.Margin = new Thickness(0, 0, 0, 0);
            }

            // set visible only if host
            // can call event directly b/c we are 
            if (this.mc.InEvent) {
                permissionsLabel.IsVisible = this.mc.Event.IsHost;
                foreach (KeyValuePair<string, Button> entry in this.buttons) {
                    entry.Value.IsVisible = this.mc.Event.IsHost;
                }

                // now try setting color for each one
                SetButtonColorForField(Permissions.PlayNext);
                SetButtonColorForField(Permissions.PlayPause);
                SetButtonColorForField(Permissions.Skip);
                SetButtonColorForField(Permissions.Volume);

            } else {
                // if not in event, hide buttons
                foreach (KeyValuePair<string, Button> entry in this.buttons) {
                    entry.Value.IsVisible = false;
                }
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

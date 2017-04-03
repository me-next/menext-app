using System;

using Xamarin.Forms;

namespace MeNext
{
    public class HomeScreen : ContentPage, IUIChangeListener
    {
        private MainController mc;

        private Label loggedIn;

        public HomeScreen(MainController mc)
        {
            this.mc = mc;
            var musicService = mc.musicService;

            this.Title = "Home";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout()
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };

            layout.Children.Add(loggedIn = new Label());

            // TODO: Make these appear and dissappear based on availability
            layout.Children.Add(new Button
            {
                Text = "Join Event",
                Command = new Command(() => Navigation.PushAsync(new JoinEvent(mc)))
            });

            layout.Children.Add(new Button
            {
                Text = "Host Event",
                Command = new Command(() => Navigation.PushAsync(new HostEvent(mc)))
            });

            layout.Children.Add(new Button
            {
                Text = "Login with Spotify",
                Command = new Command(() => musicService.Login())
            });

            layout.Children.Add(new Button
            {
                Text = "Logout of Spotify",
                Command = new Command(() => musicService.Logout())
            });

            layout.Children.Add(new Button
            {
                Text = "End Event",
                Command = new Command(() => mc.RequestEndEvent())
            });

            Content = layout;

            mc.AddStatusListener(this);
            this.SomethingChanged();
        }

        public void SomethingChanged()
        {
            loggedIn.Text = "Spotify: " + (mc.musicService.LoggedIn ? "Logged In" : "Logged Out");
        }
    }
}

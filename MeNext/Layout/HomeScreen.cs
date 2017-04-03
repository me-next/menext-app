using System;

using Xamarin.Forms;

namespace MeNext
{
    public class HomeScreen : ContentPage
    {
        public HomeScreen(MainController mc)
        {
            var musicService = mc.musicService;

            this.Title = "Home";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout()
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };

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
                Text = "End Event",
                Command = new Command(() => mc.RequestEndEvent())
            });

            Content = layout;
        }
    }
}

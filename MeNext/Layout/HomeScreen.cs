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
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    new Label { Text = "Im a Home Screen" }
                }
            };
            var joinPage = new JoinEvent();
            var hostPage = new HostEvent(mc);
            var joinCommand = new Command(() => Navigation.PushAsync(joinPage));
            var hostCommand = new Command(() => Navigation.PushAsync(hostPage));
            layout.Children.Add(new Button { Text = "Host Event", Command = hostCommand });  //On Click opens HostEventScreen
            layout.Children.Add(new Button { Text = "Join Event", Command = joinCommand });  //OnClick open JoinEventScreen

            var loginCommand = new Command(() =>
            {
                musicService.Login();
            });
            layout.Children.Add(new Button
            {
                Text = (musicService.LoggedIn ? "Re-" : "") + "Login with Spotify",
                Command = loginCommand
            });

            Content = layout;
        }
    }
}

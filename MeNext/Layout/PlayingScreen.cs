using System;

using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage
    {
        public PlayingScreen()
        {
            this.Title = "Now Playing";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout
            {
                Children = {
                    new Label { Text = "Now Playing" }
                }
            };
            layout.Children.Add(new Button { Text = "<<" });//, Command = hostCommand });
            layout.Children.Add(new Button { Text = "Play" });//, Command = hostCommand });
            layout.Children.Add(new Button { Text = ">>" });//, Command = hostCommand });
            Content = layout;
        }
    }
}
using System;

using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage
    {
        public PlayingScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    new Label { Text = "Now Playing" }
                }
            };
        }
    }
}


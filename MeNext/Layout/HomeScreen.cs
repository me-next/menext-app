﻿using System;

using Xamarin.Forms;

namespace MeNext
{
    public class HomeScreen : ContentPage
    {
        public HomeScreen(MainController mainController)
        {
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
            var hostPage = new HostEvent();
            var joinCommand = new Command(() => Navigation.PushAsync(joinPage));
            var hostCommand = new Command(() => Navigation.PushAsync(hostPage));
            layout.Children.Add(new Button { Text = "Host Event", Command = hostCommand });  //On Click opens HostEventScreen
            layout.Children.Add(new Button { Text = "Join Event", Command = joinCommand });  //OnClick open JoinEventScreen

            // == TODO Delete this testing stuff == //
            var testCommand = new Command(() =>
            {
                var musicService = mainController.musicService;
            });
            layout.Children.Add(new Button
            {
                Text = "TESTING STUFF",
                Command = testCommand
            });
            // == End Testing stuff == //

            Content = layout;
        }
    }
}

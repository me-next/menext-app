using System;

using Xamarin.Forms;

namespace MeNext
{
    public class JoinedEvent : ContentPage
    {
        public JoinedEvent(MainController mc)
        {
            var layout = new StackLayout();
            layout.Children.Add(new Label { Text = "Successfully Joined the Event!\nThe Event's ID  = " + mc.EventSlug + "\n"});
            layout.Children.Add(new Button {
                Text = "Return to Home Page",
                Command = new Command(() => Navigation.PopAsync())});
            Content = layout;
        }
    }
}


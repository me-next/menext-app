using System;

using Xamarin.Forms;

namespace MeNext
{
    public class JoinEvent : ContentPage
    {
        public JoinEvent()
        {
            var layout = new StackLayout
            {
                Children = {
                    new Label { Text = "Join Event" }
                }

            };
            layout.Children.Add(new Entry { Text = "Event Name/Id Code" });
            var joinCommand = new Command((obj) => Navigation.PopAsync());
            layout.Children.Add(new Button { Text = "Join!", Command = joinCommand });
            Content = layout;
        }
    }
}

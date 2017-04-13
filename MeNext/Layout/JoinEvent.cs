using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MeNext
{
    public class JoinEvent : ContentPage
    {
        private Entry eventIDEntry;
        public JoinEvent(MainController mc)
        {
            this.Title = "Join event";
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };

            eventIDEntry = new Entry { Placeholder = "Event Id" };
            eventIDEntry.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                Debug.WriteLine("Text changed:" + e.ToString());
            };

            var joinCommand = new Command<commandClass>(JoinCommand);
            layout.Children.Add(eventIDEntry);
            layout.Children.Add(new Button
            {
                Text = "Join!",
                Command = joinCommand,
                CommandParameter = new commandClass(mc, eventIDEntry)
            });
            Content = layout;
        }

        void JoinCommand(commandClass cClass)
        {
            JoinEventClass joinEvent = new JoinEventClass(cClass.mc.RequestJoinEvent(eventIDEntry.Text.ToUpper()));
            Navigation.PopAsync();
            if (joinEvent.EventResult.ToString() == "SUCCESS") {

                //Navigation.PushAsync(new JoinedEvent(cClass.mc));
            } else {
                Debug.WriteLine("Couldn't join event successfully: " + joinEvent.EventResult.ToString());
                // TODO Error message
                // TODO Fail if we couldn't join!
            }
        }


    }
}

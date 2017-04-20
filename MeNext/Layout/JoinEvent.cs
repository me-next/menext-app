using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Content Page loaded when attempting to Join event.  
    /// Prompts User to enter the Event's ID.
    /// </summary>
    public class JoinEvent : ContentPage
    {
        // Text box to input the Event's ID
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

            var joinCommand = new Command<MainController>(JoinCommand);
            layout.Children.Add(eventIDEntry);
            layout.Children.Add(new Button
            {
                Text = "Join!",
                Command = joinCommand,
                CommandParameter = mc
            });
            Content = layout;
        }

        /// <summary>
        /// Command called when attempting to Join event.
        /// Calls the API join event request.
        /// </summary>
        /// <param name="mc">the mainController</param>
        void JoinCommand(MainController mc)
        {
            if (eventIDEntry.Text == null || eventIDEntry.Text == "") {
                // Can't join an event w/ no name
                return;
            }

            var joinEvent = mc.RequestJoinEvent(eventIDEntry.Text.Trim().ToLower());

            Navigation.PopAsync();
            if (joinEvent == JoinEventResult.SUCCESS) {

                //Navigation.PushAsync(new JoinedEvent(cClass.mc));
            } else {
                Debug.WriteLine("Couldn't join event successfully: " + joinEvent.ToString());
                // TODO Error message
                // TODO Fail if we couldn't join!
            }
        }
    }
}

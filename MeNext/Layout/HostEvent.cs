using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
    public class HostEvent : ContentPage
    {
        public Entry nameEntry;

        public HostEvent(MainController mc)
        {
            this.Title = "Host event";
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };

            nameEntry = new Entry { Placeholder = "Event Name", Text = "" };
            layout.Children.Add(nameEntry);

            var hostCommand = new Command<MainController>(HostCommand);
            layout.Children.Add(new Button
            {
                Text = "Host!",
                Command = hostCommand,
                CommandParameter = mc
            });
            Content = layout;
        }

        // Host Event works for randomized Event ids and unique new Event Ids.
        // Recieves blank Json response from server when attempting to host with in use Event id.
        // The blank response causes null errors @ line 175 in MainController.cs.  result is undefined.
        /// <summary>
        /// Create and host a new event.  
        /// </summary>
        /// <param name="mc">Mc.</param>
        void HostCommand(MainController mc)
        {
            CreateEventResult createEvent;
            if (nameEntry.Text != "") {
                createEvent = mc.RequestCreateEvent(nameEntry.Text.ToLower());
                if (createEvent == CreateEventResult.FAIL_EVENT_EXISTS) {
                    // That event name is taken.
                    // TODO: Implement warning to notify user that new name is needed.
                    // Right now it just changes the entry's text to implicate the new name.
                    nameEntry.Text = "";
                    nameEntry.Placeholder = "Try another name.";
                    return;
                } else if (createEvent == CreateEventResult.FAIL_GENERIC) {
                    nameEntry.Text = "";
                    // TODO This should be generic error, but right now try another name is also generic
                    nameEntry.Placeholder = "Try another name.";
                    //nameEntry.Placeholder = "Host name failed for unknown reason. Try again.";
                    return;
                }
            } else {
                createEvent = mc.RequestCreateEvent();
            }
            Navigation.PopAsync();
            if (createEvent == CreateEventResult.SUCCESS) {
                mc.Event.RequestEventPermissions();
                //Navigation.PopAsync();
                //Navigation.PushAsync(new JoinedEvent(mc));
            }
        }
    }
}

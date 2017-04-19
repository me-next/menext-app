using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
    public class HostEvent : ContentPage
    {   // int permissions = 0b00011
        int permissions = 3;
        public Entry nameEntry;
        // TODO Create class that stores permission instead of using bit masks
        // Less hardcoding = :)
        public HostEvent(MainController mc)
        {
            this.Title = "Host event";
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };
            nameEntry = new Entry { Placeholder = "Event Name", Text = "" };
            layout.Children.Add(nameEntry);
            var suggButt = new Button { Text = "Suggest Song", BackgroundColor = Color.Green };
            suggButt.Clicked += (sender, e) => AddPermission(sender, e, 1, mc);
            layout.Children.Add(suggButt);
            var nextButt = new Button { Text = "Play Next", BackgroundColor = Color.Green };
            nextButt.Clicked += (sender, e) => AddPermission(sender, e, 2, mc);
            layout.Children.Add(nextButt);
            var nowButt = new Button { Text = "Play Now", BackgroundColor = Color.Red };
            nowButt.Clicked += (sender, e) => AddPermission(sender, e, 4, mc);
            layout.Children.Add(nowButt);
            var volButt = new Button { Text = "Volume Control", BackgroundColor = Color.Red };
            volButt.Clicked += (sender, e) => AddPermission(sender, e, 8, mc);
            layout.Children.Add(volButt);
            var skipButt = new Button { Text = "Skip", BackgroundColor = Color.Red };
            skipButt.Clicked += (sender, e) => AddPermission(sender, e, 16, mc);
            layout.Children.Add(skipButt);
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
                if (createEvent.ToString() == "FAIL_EVENT_EXISTS") {
                    // That event name is taken.
                    // TODO: Implement warning to notify user that new name is needed.
                    // Right now it just changes the entry's text to implicate the new name.
                    nameEntry.Text = mc.EventName.ToString();
                    return;
                } else if (createEvent.ToString() == "FAIL_GENERIC") 
                {
                    nameEntry.Text = "";
                    nameEntry.Placeholder = "pick an unused name";
                    return;
                }
            } else {
                createEvent = mc.RequestCreateEvent();
            }
            Navigation.PopAsync();
            if (createEvent.ToString() == "SUCCESS") {
                mc.Event.RequestEventPermissions();
                //Navigation.PopAsync();
                //Navigation.PushAsync(new JoinedEvent(mc));
            }
        }
        //public Tuple<string, string, string> HostParty()
        //{
        //    string errormsg = "error";
        //    string partyID = "PID";
        //    string parsedJSON = "Parsed, Json, Text";
        //    // Call Host Event function on backend.
        //    // Recieve JSON from backend
        //    // Parse Json into PartyID and a general struct.
        //    // Error msg as needed.
        //    Tuple<string, string, string> reTuple = Tuple.Create(errormsg, partyID, parsedJSON);
        //    return reTuple;
        //}

        /// <summary>
        /// Adds the given permission.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        /// <param name="val">permission value.</param>
        /// <param name="mc">MainController mc.</param>
        public void AddPermission(object sender, EventArgs e, int val, MainController mc)
        {
            var thisButt = (Button) sender;
            var modifier = 0;
            if (thisButt.BackgroundColor.Equals(Color.Red)) {
                thisButt.BackgroundColor = Color.Green;
                modifier = 1;

            } else if (thisButt.BackgroundColor.Equals(Color.Green)) {
                thisButt.BackgroundColor = Color.Red;
                modifier = -1;
            }
            Debug.WriteLine("old_permissions = " + permissions);
            permissions += val * modifier;
            Debug.WriteLine("new_permissions = " + permissions);
        }
    }
}

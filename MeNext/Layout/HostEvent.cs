using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
    public class HostEvent : ContentPage
    {   //int permissions = 0b00011
        int permissions = 3;
        public HostEvent(MainController mc)
        {
            this.Title = "Host event";
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };
            var permissionText = "Suggest Song";
            var suggButt = new Button { Text = permissionText, BackgroundColor = Color.Green };
            suggButt.Clicked += (sender, e) => AddPermission(sender, e, 1, mc);
            layout.Children.Add(suggButt);
            permissionText = "Play Next";
            var nextButt = new Button { Text = permissionText, BackgroundColor = Color.Green };
            nextButt.Clicked += (sender, e) => AddPermission(sender, e, 2, mc);
            layout.Children.Add(nextButt);
            permissionText = "Play Now";
            var nowButt = new Button { Text = permissionText, BackgroundColor = Color.Red };
            nowButt.Clicked += (sender, e) => AddPermission(sender, e, 4, mc);
            layout.Children.Add(nowButt);
            permissionText = "Volume Control";
            var volButt = new Button { Text = permissionText, BackgroundColor = Color.Red };
            volButt.Clicked += (sender, e) => AddPermission(sender, e, 8, mc);
            layout.Children.Add(volButt);
            permissionText = "Skip";
            var skipButt = new Button { Text = permissionText, BackgroundColor = Color.Red };
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
        void HostCommand(MainController mc)
        {
            JoinEventClass joinEvent = new JoinEventClass(mc.RequestCreateEvent());
            Navigation.PopAsync();
            if (joinEvent.EventResult.ToString() == "SUCCESS") {
                mc.Event.RequestEventPermissions(permissions);
                //Navigation.PopAsync();
                //Navigation.PushAsync(new JoinedEvent(mc));
            }
        }
        public Tuple<string, string, string> HostParty()
        {
            string errormsg = "error";
            string partyID = "PID";
            string parsedJSON = "Parsed, Json, Text";
            //Call Host Event function on backend.
            //Recieve JSON from backend
            //Parse Json into PartyID and a general struct.
            //Error msg as needed.
            Tuple<string, string, string> reTuple = Tuple.Create(errormsg, partyID, parsedJSON);
            return reTuple;
        }
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
    public class HostEvent : ContentPage
    {
        public HostEvent(MainController mc)
        {
            var layout = new StackLayout
            {
                Children = {
                    new Label { Text = "Host Event!\nVarious permissions in a list\n" }
                }
            };
            var permissions = new List<string>();
            permissions.Add("Suggest Song");
            permissions.Add("Play Next");
            permissions.Add("Play Now");
            permissions.Add("Volume Control");
            permissions.Add("Skip"); //List is for future checkboxvar
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
            var returnval = mc.RequestCreateEvent("EventName");
            if (returnval.ToString() == "SUCCESS") 
            {
                Navigation.PopAsync();
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
    }
}

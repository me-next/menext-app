using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
	public class HostEvent : ContentPage
	{
		public HostEvent()
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
			var hostCommand = new Command((obj) => Navigation.PopAsync());
			layout.Children.Add(new Button { Text = "Host!", Command = hostCommand });

			Content = layout;
		}
		public Tuple<string, string, string> Host_Party()
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


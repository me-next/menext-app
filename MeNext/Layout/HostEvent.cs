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
    }
}


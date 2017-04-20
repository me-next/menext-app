using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
namespace MeNext
{
    public class HostEvent : ContentPage
    {
        public HostEvent(MainController mc)
        {
            this.Title = "Host event";
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
            };

            var hostCommand = new Command<MainController>(HostCommand);
            layout.Children.Add(new Button
            {
                Text = "Host!",
                Command = hostCommand,
                CommandParameter = mc
            });
            Content = layout;
        }
        /// <summary>
        /// Create and host a new event.  
        /// </summary>
        /// <param name="mc">Mc.</param>
        void HostCommand(MainController mc)
        {
            JoinEventClass joinEvent = new JoinEventClass(mc.RequestCreateEvent());
            Navigation.PopAsync();
            if (joinEvent.EventResult.ToString() == "SUCCESS") {
                mc.Event.RequestEventPermissions();
                //Navigation.PopAsync();
                //Navigation.PushAsync(new JoinedEvent(mc));
            }
        }
    }
}

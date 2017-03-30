using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MeNext
{
    public class JoinEvent : ContentPage
    {
        public JoinEvent(MainController mc)
        {
            var layout = new StackLayout();
            var eventName = new Entry { Placeholder = "Event ID Code", Text = "Test"};
            var joinCommand = new Command<commandClass>(JoinCommand);
            layout.Children.Add(new Label { Text = "Join Event" });
            layout.Children.Add(eventName);
            layout.Children.Add(new Button
            {
                Text = "Join!",
                Command = joinCommand,
                CommandParameter = new commandClass(mc, eventName)
            });
            Content = layout;
        }
        void JoinCommand(commandClass cClass)
        {
            JoinEventClass joinEvent = new JoinEventClass(cClass.mc.RequestJoinEvent(cClass.name));
            Debug.WriteLine("cClass.name = \'" + cClass.name + "\'\n");
            if (joinEvent.EventResult.ToString() == "SUCCESS") {
                Navigation.PopAsync();
                Navigation.PushAsync(new JoinedEvent(cClass.mc));
            }
            else {
                Debug.WriteLine("Oh no, couldn't join event successfully.  Error Message: " + joinEvent.EventResult.ToString() + "\n");
            }
        }


    }
}

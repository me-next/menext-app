using System;

using Xamarin.Forms;

namespace MeNext
{
	public class HostEvent : ContentPage
	{
		public HostEvent()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


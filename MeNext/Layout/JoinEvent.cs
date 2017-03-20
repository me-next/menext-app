using System;

using Xamarin.Forms;

namespace MeNext
{
	public class JoinEvent : ContentPage
	{
		public JoinEvent()
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


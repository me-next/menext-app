using System;

using Xamarin.Forms;

namespace MeNext
{
	public class PlayingScreen : ContentPage
	{
		public PlayingScreen()
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


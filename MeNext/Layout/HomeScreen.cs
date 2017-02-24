using System;

using Xamarin.Forms;

namespace MeNext
{
	public class HomeScreen : ContentPage
	{
		public HomeScreen()
		{
			this.Title = "Home";
			NavigationPage.SetHasNavigationBar(this, false);
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


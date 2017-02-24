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
				Padding = LayoutConsts.DEFAULT_PADDING,
				Children = {
					new Label { Text = "HomePage!" }
				}
			};
		}
	}
}


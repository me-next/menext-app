﻿using System;

using Xamarin.Forms;

namespace MeNext
{
	public class PlaceholderScreen : ContentPage
	{
		public PlaceholderScreen()
		{
			this.Title = "Placeholder";
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "I am a placeholder!" }
				}
			};
		}
	}
}


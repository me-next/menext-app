using System;
using Xamarin.Forms;

namespace MeNext
{
	public class LayoutConsts
	{
		private const int EDGE = 5;
		private const int IOS_TOP = 20;
		private static Thickness defaultPadding = new Thickness(EDGE, Device.OnPlatform(EDGE + IOS_TOP, EDGE, EDGE), EDGE, EDGE);

		public static Thickness DEFAULT_PADDING
		{
			get
			{
				return defaultPadding;
			}

			private set
			{
				defaultPadding = value;
			}
		}
	}
}

using System;
using Xamarin.Forms;

namespace MeNext
{
    public class LayoutConsts
    {
        private const int EDGE = 5;
        private const int IOS_TOP = 20;

        public static int ROW_HEIGHT
        {
            get
            {
                return 55;
            }
        }

        public static double BUTTON_WIDTH
        {
            get
            {
                return Device.OnPlatform<int>(40, 50, 0);
            }
        }

        public static double TITLE_FONT_SIZE
        {
            get
            {
                return 18;
            }
        }

        public static double SUBTITLE_FONT_SIZE
        {
            get
            {
                return 14;
            }
        }

        public static Thickness RIGHT_BUTTON_MARGIN
        {
            get
            {
                return new Thickness(0, 0, Device.OnPlatform(10, 0, 0), 0);
            }
        }

        public static int ICON_SIZE
        {
            get
            {
                return Device.OnPlatform(28, 20, 0);
            }
        }

        public static Thickness DEFAULT_PADDING
        {
            get
            {
                return new Thickness(EDGE, Device.OnPlatform(EDGE + IOS_TOP, EDGE, EDGE), EDGE, EDGE);
            }
        }
    }
}

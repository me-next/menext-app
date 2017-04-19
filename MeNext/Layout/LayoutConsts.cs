using System;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Layout constants.
    /// </summary>
    public class LayoutConsts
    {
        private const int EDGE = 5;
        private const int IOS_TOP = 20;

        /// <summary>
        /// Returns the heigh for rows.
        /// </summary>
        /// <value>The height for rows.</value>
        public static int ROW_HEIGHT
        {
            get
            {
                return 55;
            }
        }
        /// <summary>
        /// Width of buttons.
        /// </summary>
        /// <value>The width for buttons.</value>
        public static double BUTTON_WIDTH
        {
            get
            {
                return Device.OnPlatform<int>(40, 50, 0);
            }
        }
        /// <summary>
        /// The font size for titles.
        /// </summary>
        /// <value>The size for title fonts.</value>
        public static double TITLE_FONT_SIZE
        {
            get
            {
                return 18;
            }
        }
        /// <summary>
        /// The font size for subtitle.
        /// </summary>
        /// <value>The size for subtitle fonts.</value>
        public static double SUBTITLE_FONT_SIZE
        {
            get
            {
                return 14;
            }
        }
        /// <summary>
        /// The thickness for Button's margins.
        /// </summary>
        /// <value>The button's margins thickness. .</value>
        public static Thickness RIGHT_BUTTON_MARGIN
        {
            get
            {
                return new Thickness(0, 0, Device.OnPlatform(10, 0, 0), 0);
            }
        }
        /// <summary>
        /// Size for icons.
        /// </summary>
        /// <value>The size for icons.</value>
        public static int ICON_SIZE
        {
            get
            {
                return Device.OnPlatform(28, 20, 0);
            }
        }
        /// <summary>
        /// Gets the default padding.
        /// </summary>
        /// <value>The default padding in Thickness.</value>
        public static Thickness DEFAULT_PADDING
        {
            get
            {
                return new Thickness(EDGE, Device.OnPlatform(EDGE + IOS_TOP, EDGE, EDGE), EDGE, EDGE);
            }
        }
    }
}

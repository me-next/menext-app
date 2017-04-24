using System;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents a single command within a menu.
    /// </summary>
    public class MenuCommand
    {
        /// <summary>
        /// The text of the command
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The command itself, which is executed when the menu item is selected.
        /// </summary>
        public Command<ResultItemData> Command { get; set; }
    }
}

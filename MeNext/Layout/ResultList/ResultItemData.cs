using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents the data backing for a single item in a ListsView
    /// </summary>
    public class ResultItemData
    {
        /// <summary>
        /// The raw metadata item we are representing.
        /// </summary>
        /// <value>The item.</value>
        public IMetadata Item { get; private set; }

        // Title is accessed reflexively in ListsView (ie do not rename it)
        /// <summary>
        /// The main title which is big in the list element
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The subtitle which is small in the list element
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle { get; set; }

        /// <summary>
        /// The current suggestion status of the item. Set this to anything other than disable and it will be updated to
        /// the correct value automatically in ResultListCell.
        /// </summary>
        public SuggestSetting Suggest { get; set; }

        /// <summary>
        /// The command which is executed when we tap on the item. Set to null to do nothing.
        /// </summary>
        /// <value>The tap command.</value>
        public Command<ResultItemData> TapCommand { get; set; }

        /// <summary>
        /// The factory which is used to produce a menu for this cell when the menu button is tapped. Set to null to
        /// not show a menu button.
        /// </summary>
        public IMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Create a new ResultItemData which wraps a chunk of metadata.
        /// </summary>
        /// <param name="item">Item.</param>
        public ResultItemData(IMetadata item)
        {
            this.Item = item;

            this.Title = "";
            this.Subtitle = "";
            this.Suggest = SuggestSetting.DISABLE_SUGGEST;
            this.TapCommand = null;
            this.MenuHandler = null;
        }
    }

    /// <summary>
    /// What the "suggest" button should be set to presently
    /// </summary>
    public enum SuggestSetting
    {
        DISABLE_SUGGEST, SUGGEST, VOTE_NEUTRAL, VOTE_LIKE, VOTE_DISLIKE, NOW_PLAYING, UP_NEXT
    }
}

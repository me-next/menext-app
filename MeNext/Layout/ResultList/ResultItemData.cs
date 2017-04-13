using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents the data backing for a single item in a ListsView
    /// </summary>
    public class ResultItemData
    {
        public IMetadata Item { get; private set; }

        // Title is accessed reflexively in ListsView (ie do not rename it)
        public string Title { get; set; }
        public string Subtitle { get; set; }

        /// <summary>
        /// The current suggestion status of the item. Set this to anything other than disable and it will be updated to
        /// the correct value automatically in ResultListCell.
        /// </summary>
        public SuggestSetting Suggest { get; set; }


        public Command TapCommand { get; set; }    // TODO

        /// <summary>
        /// The menu command is the command that is run when the menu button is tapped. The command parameter is this
        /// very ResultItemData instance. If this is null, the menu icon is disabled.
        /// </summary>
        /// <value>The menu command.</value>
        public Command MenuCommand { get; set; }
        // TODO Disable the menu if this is null

        public ResultItemData(IMetadata item)
        {
            this.Item = item;

            this.Title = null;
            this.Subtitle = null;
            this.Suggest = SuggestSetting.DISABLE_SUGGEST;
            this.TapCommand = null;
            this.MenuCommand = null;
        }
    }

    /// <summary>
    /// What the "suggest" button should be set to presently
    /// </summary>
    public enum SuggestSetting
    {
        DISABLE_SUGGEST, SUGGEST, VOTE_NEUTRAL, VOTE_LIKE, VOTE_DISLIKE
    }
}

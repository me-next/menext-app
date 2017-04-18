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
        public IMetadata Item { get; private set; }

        // Title is accessed reflexively in ListsView (ie do not rename it)
        public string Title { get; set; }
        public string Subtitle { get; set; }

        /// <summary>
        /// The current suggestion status of the item. Set this to anything other than disable and it will be updated to
        /// the correct value automatically in ResultListCell.
        /// </summary>
        public SuggestSetting Suggest { get; set; }


        public Command<ResultItemData> TapCommand { get; set; }    // TODO

        public IMenuHandler MenuHandler { get; set; }

        public ResultItemData(IMetadata item)
        {
            this.Item = item;

            this.Title = null;
            this.Subtitle = null;
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
        DISABLE_SUGGEST, SUGGEST, VOTE_NEUTRAL, VOTE_LIKE, VOTE_DISLIKE, NOW_PLAYING
    }
}

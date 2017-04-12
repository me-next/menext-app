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

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public SuggestSetting Suggest { get; set; }
        public Command TapCommand { get; set; }

        public ResultItemData(IMetadata item)
        {
            this.Item = item;

            this.Title = null;
            this.Subtitle = null;
            this.Suggest = SuggestSetting.DISABLE_SUGGEST;
            this.TapCommand = null;
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

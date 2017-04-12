using System;
using System.Diagnostics;
using MeNext;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents a single row within a results list
    /// </summary>
    public class ResultListCell : ViewCell
    {
        public const string MENU_ICON = "⋮";
        public const string SUGGESTIONS_ADD = "+";
        public const string VOTE_YES = "\ud83d\udfdf";
        public const string VOTE_NO = "\ud83d\ude16";
        public const string VOTE_NEUTRAL = "\ud83d\ude10";

        private Label titleLabel;
        private Label subtitleLabel;
        private Button taskButton;
        private Button menuButton;

        public ResultListCell()
        {
            Debug.WriteLine("!! CREATING CELL !!");
            this.titleLabel = new Label();
            this.subtitleLabel = new Label();

            this.taskButton = new Button
            {
                Text = SUGGESTIONS_ADD,
                Command = new Command((obj) =>
                {
                    Debug.WriteLine("Task!");
                }),
            };

            this.menuButton = new Button
            {
                Text = MENU_ICON,
                Command = new Command((obj) =>
                {
                    Debug.WriteLine("Menu!");
                }),
            };

            this.View = new StackLayout
            {
                Padding = new Thickness(0, 5),
                Orientation = StackOrientation.Horizontal,
                Children = { titleLabel, subtitleLabel, taskButton, menuButton }
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            Debug.WriteLine("!! BINDING CONTEXT CHANGED !!");

            if (BindingContext != null) {
                Debug.WriteLine(BindingContext);

                var resultItem = ((ResultItemWrapper) BindingContext)?.ResultItem;

                if (resultItem == null) {
                    Debug.WriteLine("*** ERROR: ResultItem is null");
                } else {
                    this.titleLabel.Text = resultItem.Title;
                    this.subtitleLabel.Text = resultItem.Subtitle;

                    this.taskButton.IsVisible = (resultItem.Suggest != SuggestSetting.DISABLE_SUGGEST);
                    this.taskButton.Text = GetSuggestIcon(resultItem.Suggest);

                    // TODO Menu
                }
            }
        }

        private string GetSuggestIcon(SuggestSetting s)
        {
            switch (s) {
                case SuggestSetting.SUGGEST:
                    return SUGGESTIONS_ADD;

                case SuggestSetting.VOTE_LIKE:
                    return VOTE_YES;

                case SuggestSetting.VOTE_DISLIKE:
                    return VOTE_NO;

                case SuggestSetting.VOTE_NEUTRAL:
                    return VOTE_NEUTRAL;

                default:
                    return "ERROR";
            }
        }
    }


}

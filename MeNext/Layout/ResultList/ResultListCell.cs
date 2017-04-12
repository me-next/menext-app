using System;
using System.Diagnostics;
using MeNext;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    // Special thanks to
    // https://github.com/xamarin/xamarin-forms-samples/blob/master/UserInterface/ListView/BindingContextChanged/BindingContextChanged/CustomCell.cs

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
            this.titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold
            };
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
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { titleLabel, subtitleLabel }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Children = { taskButton, menuButton }
                    }

                }
            };
        }

        // This is called when the backing data for the cell changes
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null) {
                // Save the binding context
                var resultItem = (ResultItemData) BindingContext;

                // Update stuff
                this.titleLabel.Text = resultItem.Title;
                this.subtitleLabel.Text = resultItem.Subtitle;

                this.taskButton.IsVisible = (resultItem.Suggest != SuggestSetting.DISABLE_SUGGEST);
                this.taskButton.Text = GetSuggestIcon(resultItem.Suggest);

                // TODO Menu
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

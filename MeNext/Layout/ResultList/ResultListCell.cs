using System;
using System.Collections.Generic;
using System.Diagnostics;
using MeNext;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    // Special thanks to
    // https://github.com/xamarin/xamarin-forms-samples/blob/master/UserInterface/ListView/BindingContextChanged/BindingContextChanged/CustomCell.cs

    /// <summary>
    /// Represents a single row within a results list. It currently special cases ISong for the suggestion button which,
    /// while not great OO practice, but makes it easier to cache UI elements.
    /// </summary>
    public class ResultListCell : ViewCell, IUIChangeListener
    {
        public const string MENU_ICON = "⋮";
        public const string SUGGESTIONS_ADD = "+";
        public const string VOTE_YES = "\ud83d\ude0a";
        public const string VOTE_NO = "\ud83d\ude16";
        public const string VOTE_NEUTRAL = "\ud83d\ude10";

        public static int BUTTON_WIDTH = Device.OnPlatform<int>(30, 50, 0);

        private ResultItemData resultItem;

        private Label titleLabel;
        private Label subtitleLabel;
        private Button suggestButton;
        private Button menuButton;

        private MainController controller;

        public ResultListCell(MainController controller)
        {
            this.controller = controller;

            this.titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            this.subtitleLabel = new Label
            {
                LineBreakMode = LineBreakMode.TailTruncation
            };

            this.suggestButton = new Button
            {
                Text = SUGGESTIONS_ADD,
                WidthRequest = BUTTON_WIDTH,
                Command = new Command((obj) =>
                {
                    this.HandleSuggestion(resultItem.Suggest, (ISong) resultItem.Item);
                }),
            };

            this.menuButton = new Button
            {
                Text = MENU_ICON,
                WidthRequest = BUTTON_WIDTH,
                Command = new Command((obj) =>
                {
                    Debug.WriteLine("Menu!");
                    if (resultItem.MenuCommand != null && resultItem.MenuCommand.CanExecute(resultItem)) {
                        resultItem.MenuCommand.Execute(resultItem);
                    }
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
                        Children = { suggestButton, menuButton }
                    }
                }
            };

            var columns = new ColumnDefinitionCollection();
            columns.Add(new ColumnDefinition { Width = GridLength.Star });
            columns.Add(new ColumnDefinition { Width = GridLength.Auto });
            var rows = new RowDefinitionCollection();
            rows.Add(new RowDefinition { Height = GridLength.Auto });

            var metadata = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { titleLabel, subtitleLabel }
            };

            var buttons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Children = { suggestButton, menuButton },
                MinimumWidthRequest = 500,
            };

            var grid = new Grid
            {
                ColumnDefinitions = columns,
                RowDefinitions = rows,
            };
            grid.Children.Add(metadata, 0, 0);
            grid.Children.Add(buttons, 1, 0);

            this.View = grid;

            // This should be the last thing we do to avoid race conditions
            this.controller.Event.RegisterUiListener(this);
        }

        // This is called when the backing data for the cell changes
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null) {
                // Save the binding context
                this.resultItem = (ResultItemData) BindingContext;

                // Update stuff
                this.titleLabel.Text = resultItem.Title;
                this.subtitleLabel.Text = resultItem.Subtitle;

                this.suggestButton.IsVisible = (resultItem.Suggest != SuggestSetting.DISABLE_SUGGEST);
                this.UpdateSuggestionButton();

                if (this.suggestButton.IsVisible) {
                    // Make sure we can cast to an ISong if we have any setting other than disabling suggestions
                    Debug.Assert(resultItem.Item as ISong != null);
                }

                // TODO Menu
            }
        }

        private void UpdateSuggestionButton()
        {
            if (!this.suggestButton.IsVisible || this.controller.Event.LatestPull == null) {
                return;
            }
            var pull = this.controller.Event.LatestPull;
            var suggestions = new List<SongResponse>(pull.SuggestQueue.Songs);
            // TODO This could get slow w/ a ton of songs. Shared hashmap might be better.
            var item = suggestions.Find((obj) => obj.ID == this.resultItem.Item.UniqueId);
            if (item != null) {
                // The song has already been suggested
                if (item.Vote == 1) {
                    resultItem.Suggest = SuggestSetting.VOTE_LIKE;
                } else if (item.Vote == -1) {
                    resultItem.Suggest = SuggestSetting.VOTE_DISLIKE;
                } else {
                    resultItem.Suggest = SuggestSetting.VOTE_NEUTRAL;
                }
            } else {
                // The song is not yet suggested
                resultItem.Suggest = SuggestSetting.SUGGEST;
            }

            // Update the icon
            this.suggestButton.Text = GetSuggestIcon(resultItem.Suggest);
        }

        private void HandleSuggestion(SuggestSetting s, ISong song)
        {
            switch (s) {
                case SuggestSetting.SUGGEST:
                    controller.Event.RequestAddToSuggestions(song);
                    break;

                case SuggestSetting.VOTE_LIKE:
                    controller.Event.RequestThumbDown(song);
                    break;

                case SuggestSetting.VOTE_DISLIKE:
                    controller.Event.RequestThumbNeutral(song);
                    break;

                case SuggestSetting.VOTE_NEUTRAL:
                    controller.Event.RequestThumbUp(song);
                    break;
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

        public void SomethingChanged()
        {
            this.UpdateSuggestionButton();
        }
    }


}

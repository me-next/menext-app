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
    /// while not great OO practice, is necessary for performance reasons because this cell is cached.
    /// </summary>
    public class ResultListCell : ViewCell, IUIChangeListener
    {
        public const string MENU_ICON = "⋮";
        public const string SUGGESTIONS_ADD = "+";
        public const string VOTE_YES = "\ud83d\ude00";
        public const string VOTE_NO = "\ud83d\ude16";
        public const string VOTE_NEUTRAL = "\ud83d\ude10";
        public const string NOW_PLAYING = "\u25b6";

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
                LineBreakMode = LineBreakMode.TailTruncation,
                Text = "Placeholder",
                FontSize = LayoutConsts.TITLE_FONT_SIZE
            };
            this.subtitleLabel = new Label
            {
                LineBreakMode = LineBreakMode.TailTruncation,
                Text = "Placeholder",
                FontSize = LayoutConsts.SUBTITLE_FONT_SIZE
            };

            this.suggestButton = new Button
            {
                Text = SUGGESTIONS_ADD,
                FontSize = LayoutConsts.ICON_SIZE,
                WidthRequest = LayoutConsts.BUTTON_WIDTH,
                Command = new Command((obj) =>
                {
                    this.HandleSuggestion(resultItem.Suggest, (ISong) resultItem.Item);
                }),
            };

            this.menuButton = new Button
            {
                Text = MENU_ICON,
                FontSize = LayoutConsts.ICON_SIZE,
                WidthRequest = LayoutConsts.BUTTON_WIDTH,
                Margin = LayoutConsts.RIGHT_BUTTON_MARGIN,
                Command = new Command(async (obj) =>
                {
                    if (this.resultItem.MenuHandler == null) {
                        return;
                    }
                    var menu = this.resultItem.MenuHandler.ProduceMenu(this.resultItem);
                    if (menu.Count == 0) {
                        return;
                    }
                    var commands = new List<string>();
                    foreach (var item in menu) {
                        commands.Add(item.Title);
                    }
                    var action = await controller.NavPage.DisplayActionSheet(
                        "Menu: " + this.resultItem.Title,
                        "Cancel",
                        null,
                        commands.ToArray()
                    );
                    foreach (var item in menu) {
                        if (item.Title == action && item.Command.CanExecute(this.resultItem)) {
                            item.Command.Execute(this.resultItem);
                            Debug.WriteLine("Performed action " + action);
                        }
                    }
                }),
            };

            this.View = new StackLayout
            {
                Padding = new Thickness(10),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
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
                    },
                }
            };

            var columns = new ColumnDefinitionCollection();
            columns.Add(new ColumnDefinition
            {
                Width = GridLength.Star
            });
            columns.Add(new ColumnDefinition
            {
                Width = GridLength.Auto
            });
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

                this.menuButton.IsVisible = (this.resultItem.MenuHandler != null);
            }
        }
<<<<<<< HEAD
        /// <summary>
        /// Updates the suggestion button.
        /// </summary>
=======

        // TODO: Move the song specific stuff into a new class
>>>>>>> e4e58e36e25b9038174a3eaaebdde3fd0412d9b3
        private void UpdateSuggestionButton()
        {
            if (!this.suggestButton.IsVisible || this.controller.Event.LatestPull == null) {
                return;
            }
            var suggestions = this.controller.Event.SuggestionQueue.Songs;

            if (this.controller.Event?.LatestPull?.Playing?.CurrentSongID == this.resultItem.Item.UniqueId) {
                // This is the currently playing song
                resultItem.Suggest = SuggestSetting.NOW_PLAYING;
            } else {
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
            }

            // Update the icon
            this.suggestButton.Text = GetSuggestIcon(resultItem.Suggest);
        }
        /// <summary>
        /// Handles the suggestion.
        /// 
        /// </summary>
        /// <param name="s">SuggestedSetting s is "vote" on the song.</param>
        /// <param name="song">ISong song is the song thats been suggested.</param>
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
        /// <summary>
        /// Gets the suggest icon.
        /// </summary>
        /// <returns>The icon for suggest.</returns>
        /// <param name="s">SuggestSetting s is the current "vote" on the song.</param>
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

                case SuggestSetting.NOW_PLAYING:
                    return NOW_PLAYING;

                default:
                    return "ERROR";
            }
        }
        /// <summary>
        /// Update UI since something changed.
        /// </summary>
        public void SomethingChanged()
        {
            this.UpdateSuggestionButton();
        }


    }


}

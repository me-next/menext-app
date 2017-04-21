using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Handles producing the menu for songs in a result list
    /// </summary>
    public class SongMenuHandler : IMenuHandler
    {
        private MainController controller;

        private MenuCommand menuSuggest;
        private MenuCommand menuAddUpNext;
        private MenuCommand menuPlayNext;
        private MenuCommand menuPlayNow;
        //private MenuCommand menuRemoveSuggest;
        private MenuCommand menuRemoveUpNext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.SongMenuHandler"/> class.
        /// </summary>
        /// <param name="controller">The main controller.</param>
        public SongMenuHandler(MainController controller)
        {
            this.controller = controller;

            this.menuSuggest = new MenuCommand
            {
                Title = "Suggest",
                Command = new Command<ResultItemData>((obj) =>
                {
                    this.controller.Event.RequestAddToSuggestions((ISong) obj.Item);
                })
            };

            this.menuAddUpNext = new MenuCommand
            {
                Title = "Add to Up Next",
                Command = new Command<ResultItemData>((obj) =>
                {
                    this.controller.Event.RequestAddToPlayNext((ISong) obj.Item);
                })
            };

            this.menuPlayNext = new MenuCommand
            {
                Title = "Play Next",
                Command = new Command<ResultItemData>((obj) =>
                {
                    this.controller.Event.RequestAddTopOfPlayNext((ISong) obj.Item);
                })
            };

            this.menuPlayNow = new MenuCommand
            {
                Title = "Play Now",
                Command = new Command<ResultItemData>((obj) =>
                {
                    this.controller.Event.RequestPlayNow((ISong) obj.Item);
                })
            };

            //this.menuRemoveSuggest = new MenuCommand
            //{
            //    Title = "Remove from Suggestions",
            //    Command = new Command<ResultItemData>((obj) =>
            //    {
            //        this.controller.Event.RequestRemoveFromSuggestions((ISong) obj.Item);
            //    })
            //};

            this.menuRemoveUpNext = new MenuCommand
            {
                Title = "Remove from Up Next",
                Command = new Command<ResultItemData>((obj) =>
                {
                    this.controller.Event.RequestRemoveFromPlayNext((ISong) obj.Item);
                })
            };
        }

        public List<MenuCommand> ProduceMenu(ResultItemData data)
        {
            var from = (ISong) data.Item;

            var playNext = new List<SongResponse>();
            var suggestions = new List<SongResponse>();
            var isPlaying = false;

            var poll = this.controller.Event.LatestPull;
            if (poll != null) {
                playNext = new List<SongResponse>(poll.PlayNextQueue.Songs);
                suggestions = new List<SongResponse>(poll.SuggestQueue.Songs);
                isPlaying = (poll.Playing.CurrentSongID == from.UniqueId);
            }

            var inUpNext = playNext.Find((obj) => obj.ID == from.UniqueId) != null;
            var inSuggest = suggestions.Find((obj) => obj.ID == from.UniqueId) != null;

            var menu = new List<MenuCommand>();

            if (isPlaying) {
                // The song is currently playing
            } else if (inSuggest) {
                // The song is in the suggestion queue
                if (controller.Event.ThisHasPermission(Permissions.PlayNext)) {
                    menu.Add(menuAddUpNext);
                    menu.Add(menuPlayNext);
                    menu.Add(menuPlayNow);
                    //menu.Add(menuRemoveSuggest);
                }
            } else if (inUpNext) {
                // The song is in the up next queue
                if (controller.Event.ThisHasPermission(Permissions.PlayNext)) {
                    menu.Add(menuPlayNext);
                    menu.Add(menuPlayNow);
                    menu.Add(menuRemoveUpNext);
                }
            } else {
                // The song is not in any salient list
                menu.Add(menuSuggest);
                if (controller.Event.ThisHasPermission(Permissions.PlayNext)) {
                    menu.Add(menuAddUpNext);
                    menu.Add(menuPlayNext);
                    menu.Add(menuPlayNow);
                }
            }

            return menu;
        }
    }
}

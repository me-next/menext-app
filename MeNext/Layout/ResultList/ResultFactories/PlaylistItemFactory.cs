﻿using System;
using System.Collections;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Creates a ResultItemData to correspond with a playlist for use within a UI list
    /// </summary>
    public class PlaylistItemFactory : IResultItemFactory<IPlaylist>
    {
        public const string BULLET = "\u2022";
        private MainController controller;
        private INavigation nav;

        public PlaylistItemFactory(MainController controller, INavigation nav)
        {
            this.controller = controller;
            this.nav = nav;
        }

        public ResultItemData GetResultItem(IPlaylist from)
        {
            return new ResultItemData(from)
            {
                Title = from.Name,
                Subtitle = "",
                Suggest = SuggestSetting.DISABLE_SUGGEST,
                TapCommand = new Command<ResultItemData>((obj) => this.nav.PushAsync(new PlaylistSongsView(controller, from))),
                MenuHandler = null,
            };
        }
    }

    /// <summary>
    /// Represents a view containing songs which we navigate to from a playlist
    /// </summary>
    public class PlaylistSongsView : ContentPage
    {
        public PlaylistSongsView(MainController controller, IPlaylist from)
        {
            this.Title = from.Name;

            var resultsView = new ResultListView<ISong>(controller, new SongItemFactory(controller));
            resultsView.UpdateResultList(from.Songs);

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    resultsView
                }
            };

        }
    }
}

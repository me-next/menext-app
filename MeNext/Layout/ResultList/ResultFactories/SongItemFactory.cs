using System;
using System.Collections;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Creates a ResultItemData to correspond with a song for use within a UI list
    /// </summary>
    public class SongItemFactory : IResultItemFactory<ISong>
    {
        public const string BULLET = "\u2022";
        private MainController controller;

        public SongItemFactory(MainController controller)
        {
            this.controller = controller;
        }

        public ResultItemData GetResultItem(ISong from)
        {
            var artists = "";
            foreach (var artist in from.Artists) {
                artists += ", " + artist.Name;
            }
            if (artists.Length > 0) {
                artists = artists.Substring(2);
            }

            return new ResultItemData(from)
            {
                Title = from.Name,
                Subtitle = artists + " " + BULLET + " " + from.Album.Name,
                Suggest = SuggestSetting.SUGGEST,
                TapCommand = null,
                MenuHandler = new SongMenuHandler(this.controller),  // TODO reuse one instance?
            };
        }
    }
}

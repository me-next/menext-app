using System;
using MeNext.MusicService;

namespace MeNext
{
    public class SongItemFactory : ResultItemFactory<ISong>
    {
        public const string BULLET = "\u2022";

        public SongItemFactory()
        {
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
            };
        }
    }
}

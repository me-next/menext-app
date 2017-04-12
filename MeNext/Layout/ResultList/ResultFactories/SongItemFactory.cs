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
            return new ResultItemData(from)
            {
                Title = from.Name,
                // TODO List all the artists, not just the first one
                Subtitle = from.Artists[0].Name + " " + BULLET + " " + from.Album.Name,
                // TODO Make this follow what it actually is
                Suggest = SuggestSetting.SUGGEST,
                TapCommand = null,
            };
        }
    }
}

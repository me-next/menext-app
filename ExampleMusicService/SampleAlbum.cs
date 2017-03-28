using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.SampleMusicService
{
    public class SampleAlbum : IAlbum
    {
        String name;

        public SampleAlbum(String name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public List<ISong> Songs
        {
            get
            {
                var list = new List<ISong>();
                list.Add(new SampleSong("Diary of Horace Wimp"));
                list.Add(new SampleSong("Do Ya"));
                list.Add(new SampleSong("Evil Woman"));
                return list;
            }
        }

        public string UniqueId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IImage GetAlbumArt(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}

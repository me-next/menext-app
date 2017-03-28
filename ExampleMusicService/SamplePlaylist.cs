using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.SampleMusicService
{
    public class SamplePlaylist : IPlaylist
    {
        String name;

        public SamplePlaylist(String name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public List<ISong> Songs
        {
            get
            {
                var list = new List<ISong>();
                list.Add(new SampleSong("As Tears go By"));
                list.Add(new SampleSong("Alive"));
                list.Add(new SampleSong("Die Young"));
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
    }
}

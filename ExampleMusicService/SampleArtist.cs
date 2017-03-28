using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.SampleMusicService
{
    public class SampleArtist : IArtist
    {
        private String name;

        public SampleArtist(String name)
        {
            this.name = name;
        }

        public List<IAlbum> Albums
        {
            get
            {
                var list = new List<IAlbum>();
                list.Add(new SampleAlbum("Discovery"));
                list.Add(new SampleAlbum("A New World Record"));
                return list;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string UniqueId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IImage GetArtistArt(int width, int height)
        {
            return null;
        }
    }
}

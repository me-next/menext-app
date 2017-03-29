using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.SampleMusicService
{
    public class SampleMusicService : IMusicService
    {
        public bool CanPlay
        {
            get
            {
                return true;
            }
        }

        public bool CanSearch
        {
            get
            {
                return true;
            }
        }

        public bool HasUserLibrary
        {
            get
            {
                return false;
            }
        }

        public String Name
        {
            get
            {
                return "Sample Music Service";
            }
        }

        private bool playing = false;

        public bool Playing
        {
            get
            {
                return playing;
            }

            set
            {
                playing = value;
            }
        }

        private double position = 15.4;

        public double PlayingPosition
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        SampleSong song = new SampleSong("The Diary of Horace Wimp");

        public ISong PlayingSong
        {
            get
            {
                return song;
            }
        }

        public IResultList<IAlbum> UserLibraryAlbums
        {
            get
            {
                return null;
            }
        }

        public IResultList<IArtist> UserLibraryArtists
        {
            get
            {
                return null;
            }
        }

        public IResultList<IPlaylist> UserLibraryPlaylists
        {
            get
            {
                return null;
            }
        }

        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                return null;
            }
        }

        public bool LoggedIn
        {
            get
            {
                return true;
            }
        }

        public void AddStatusListener(IMusicServiceListener listener)
        {
        }

        public IAlbum GetAlbum(string uid)
        {
            return new SampleAlbum("Album" + uid);
        }

        public IArtist GetArtist(string uid)
        {
            return new SampleArtist("Artist" + uid);
        }

        public IPlaylist GetPlaylist(string uid)
        {
            return new SamplePlaylist("Plist" + uid);
        }

        public ISong GetSong(string uid)
        {
            return new SampleSong("Song" + uid);
        }

        public void PlaySong(ISong song, double position = 0)
        {
            this.song = (MeNext.SampleMusicService.SampleSong) song;
            this.position = position;
        }

        public void RemoveStatusListener(IMusicServiceListener listener)
        {
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            return null;
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            return null;
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            return null;
        }

        public IResultList<ISong> SearchSong(string query)
        {
            return null;
        }

        public void SuggestBuffer(List<ISong> songs)
        {
        }

        public void Login()
        {
        }

        public void Logout()
        {
        }

        public IList<ISong> GetSongs(IList<string> uids)
        {
            throw new NotImplementedException();
        }

        public IList<IArtist> GetArtists(IList<string> uids)
        {
            throw new NotImplementedException();
        }

        public IList<IAlbum> GetAlbums(IList<string> uids)
        {
            throw new NotImplementedException();
        }

        public IList<IPlaylist> GetPlaylists(IList<string> uids)
        {
            throw new NotImplementedException();
        }
    }
}

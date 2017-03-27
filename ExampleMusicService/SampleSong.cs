using System;
using System.Collections.Generic;
using MeNext.MusicService;

namespace MeNext.SampleMusicService
{
	public class SampleSong : ISong
	{
		private String name;

		public SampleSong(String name)
		{
			this.name = name;
		}

		public IAlbum Album
		{
			get
			{
				return new SampleAlbum("Discovery");
			}
		}

		public List<IArtist> Artists
		{
			get
			{
				var list = new List<IArtist>();
				list.Add(new SampleArtist("Electric Light Orchestra"));
				return list;
			}
		}

		public int DiskNumber
		{
			get
			{
				return 1;
			}

		}

		public double Duration
		{
			get
			{
				return 324.3;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public int TrackNumber
		{
			get
			{
				return 5;
			}
		}

		public IUniqueId UniqueId
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}

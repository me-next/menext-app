using System;

using Newtonsoft.Json;
using System.Collections.Generic;
namespace MeNext
{
    /// <summary>
    /// Parses a pull json response.
    /// </summary>
    public class PullResponse
    {
        [JsonProperty(PropertyName = "suggest")]
        public QueueResponse SuggestQueue;

        [JsonProperty(PropertyName = "change")]
        public UInt64 Change;

        [JsonProperty(PropertyName = "playing")]
        public PlayingResponse Playing;

        [JsonProperty(PropertyName = "playnext")]
        public QueueResponse PlayNextQueue;

        [JsonProperty(PropertyName = "permissions")]
        //public Permissions Permissions;
        public Dictionary<string, bool> Permissions;
    }
    /// <summary>
    /// Parses a song json response.
    /// </summary>
    public class SongResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string ID;

        [JsonProperty(PropertyName = "vote")]
        public int Vote;

        // TODO BACKEND
        public int TotalVotes;
        public long TimeAdded;
    }
    /// <summary>
    /// Parses a queue json response
    /// </summary>
    public class QueueResponse
    {
        [JsonProperty(PropertyName = "songs")]
        public SongResponse[] Songs;
    }
    /// <summary>
    /// Parses a playing json response
    /// </summary>
    public class PlayingResponse
    {
        [JsonProperty(PropertyName = "CurrentSongId")]
        public string CurrentSongID;

        // false when there is no song to play
        [JsonProperty(PropertyName = "HasSong")]
        public bool HasSong;
    }
}

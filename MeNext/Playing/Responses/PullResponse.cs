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
        public Dictionary<string, bool> Permissions;

        [JsonProperty(PropertyName = "error")]
        public String Error;
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

        [JsonProperty(PropertyName = "totalVotes")]
        public int TotalVotes;

        [JsonProperty(PropertyName = "posAdded")]
        public int PosAdded;
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

        public bool Playing;

        public double SongPos;

        public int Volume;

        // TODO: Do we still need these?
        public long CurrentTimeMs;
        public long SongStartTimeMs;
    }
}

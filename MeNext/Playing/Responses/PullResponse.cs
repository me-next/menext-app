using System;

using Newtonsoft.Json;
namespace MeNext
{
    public class PullResponse
    {
        [JsonProperty(PropertyName = "suggest")]
        public QueueResponse SuggestQueue;

        [JsonProperty(PropertyName = "change")]
        public UInt64 Change;

        [JsonProperty(PropertyName = "playing")]
        public PlayingResponse Playing;
    }

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

    public class QueueResponse
    {
        [JsonProperty(PropertyName = "songs")]
        public SongResponse[] Songs;
    }

    public class PlayingResponse
    {
        [JsonProperty(PropertyName = "CurrentSongId")]
        public string CurrentSongID;

        // false when there is no song to play
        [JsonProperty(PropertyName = "HasSong")]
        public bool HasSong;
    }
}

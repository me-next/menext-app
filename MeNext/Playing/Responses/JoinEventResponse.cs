using System;

using Newtonsoft.Json;

namespace MeNext
{
    /// <summary>
    /// The result from joining an event
    /// </summary>
    public class JoinEventResponse
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}

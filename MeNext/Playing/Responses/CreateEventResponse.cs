using System;

using Newtonsoft.Json;

namespace MeNext
{
    /// <summary>
    /// The response from creating an event
    /// </summary>
    public class CreateEventResponse
    {
        [JsonProperty(PropertyName = "pid")]
        public string EventID { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "alternative")]
        public string AlternativeName { get; set; }
    }
}

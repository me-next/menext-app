using System;

using Newtonsoft.Json;

namespace MeNext
{
    public class CreateEventResponse
    {
        public CreateEventResponse()
        {
        }

        [JsonProperty(PropertyName = "alternative")]
        public string AltID { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "pid")]
        public string EventID { get; set; }
    }
}

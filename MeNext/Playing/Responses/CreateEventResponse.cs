using System;

using Newtonsoft.Json;

namespace MeNext
{
    public class CreateEventResponse
    {
        public CreateEventResponse()
        {
        }

        [JsonProperty(PropertyName = "pid")]
        public string EventID { get; set; }
    }
}

using System;

using Newtonsoft.Json;


namespace MeNext
{
    public class JoinEventResponse
    {
        public JoinEventResponse()
        {
        }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}

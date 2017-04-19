using System;

using Newtonsoft.Json;

namespace MeNext
{
    public class CreateWithNameResponse
    {
        public CreateWithNameResponse()
        {
        }

        [JsonProperty(PropertyName = "alternative")]
        public string AltID { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

    }
}

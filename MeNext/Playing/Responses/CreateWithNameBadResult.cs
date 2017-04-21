using System;

using Newtonsoft.Json;

namespace MeNext
{
    /// <summary>
    /// The response from creating an event with a name when said creation failed
    /// </summary>
    public class CreateWithNameResponse
    {
        [JsonProperty(PropertyName = "alternative")]
        public string AltID { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

    }
}

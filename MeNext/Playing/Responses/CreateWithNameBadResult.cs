using System;

using Newtonsoft.Json;

namespace MeNext
{
	public class CreateWithNameBadResponse
	{
        public CreateWithNameBadResponse()
		{
		}

		[JsonProperty(PropertyName = "alternative")]
		public string AltID { get; set; }

		[JsonProperty(PropertyName = "error")]
		public string Error { get; set; }

	}
}

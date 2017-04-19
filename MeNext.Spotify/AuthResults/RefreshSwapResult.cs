using System;
using Newtonsoft.Json;

namespace MeNext.Spotify
{
    public class RefreshSwapResult
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken;

        [JsonProperty(PropertyName = "scope")]
        public string Scope;

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn;

        // This parameter might be null when calling /refresh, in which case we should continue to use the extant one
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshTokenEncrypted;
    }
}

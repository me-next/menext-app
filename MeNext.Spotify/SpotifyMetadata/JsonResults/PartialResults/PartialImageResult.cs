using System;
namespace MeNext.Spotify
{
    /// <summary>
    /// Holder for deserialized Spotify json.
    /// See https://developer.spotify.com/web-api/endpoint-reference/
    /// </summary>
    public class PartialImageResult
    {
        public int? height { get; set; }
        public int? width { get; set; }
        public string url { get; set; }
    }
}

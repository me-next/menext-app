using System;
namespace MeNext
{
    /// <summary>
    /// Important constants for dev stuff
    /// </summary>
    public class MeNextConsts
    {
#if DEBUG
        // Development server URL. Feel free to change this while you work.
        public const string SERVER_URL = "http://menext.danielcentore.com:8080";
#else
        // Production server URL. NEVER CHANGE.
        public const string SERVER_URL = "http://menext.danielcentore.com:8080";
#endif
    }
}

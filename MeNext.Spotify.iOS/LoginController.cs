using System;
using System.Diagnostics;
using MeNext.Spotify.iOS.Auth;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    public class LoginController
    {
        public LoginController()
        {
        }

        public void login()
        {
            SPTAuth auth = SPTAuth.DefaultInstance;

            if (SPTAuth.SupportsApplicationAuthentication) {
                // Auth using Spotify app
                Debug.WriteLine("Auth using spotify app");
                UIApplication.SharedApplication.OpenUrl(auth.SpotifyAppAuthenticationURL);
            } else {
                // App using web view
                Debug.WriteLine("ERR: Cannot auth using spotify app");
                // TODO: Use web auth view?
            }
        }
    }
}

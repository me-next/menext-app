using System;
using System.Diagnostics;
using Foundation;
using MeNext.Spotify.iOS.Auth;
using UIKit;

namespace MeNext.Spotify.iOS
{
    public class SpotifySetup : NSObject
    {
        public SpotifySetup()
        {

        }

        public static StreamingDelegate CreateStreamingDelegate()
        {
            var sd = new StreamingDelegate();

            SPTAuth auth = SPTAuth.DefaultInstance;

            // ClientID and RedirectURL come from http://developer.spotify.com/my-applications
            // TODO Move out of the service
            auth.ClientID = "b79f545d6c24407aa6bed17af62275d6";

            // SPTAuthStreamingScope = Audio playing
            auth.RequestedScopes = new NSObject[] { SpotifyConstants.SPTAuthStreamingScope };

            // Callback
            // TODO: Move out of the service
            auth.RedirectURL = new Foundation.NSUrl("menext-spotify://callback");

            // Enables SPTAuth to automatically store the session object for future use.
            auth.SessionUserDefaultsKey = @"SpotifySession";

            // TODO: Use a token swap service
            //auth.TokenSwapURL;
            //auth.TokenRefreshURL;

            // TODO remove
            Console.WriteLine("Has Spotify: " + SPTAuth.SpotifyApplicationIsInstalled);

            // Uncomment to auto login (if the user doesn't already have a session)
            //if (auth.Session == null || !auth.Session.IsValid) {
            //    Login();
            //}
            return sd;
        }

        // TODO: Allow us to login from another location
        public static void Login()
        {
            SPTAuth auth = SPTAuth.DefaultInstance;

            if (SPTAuth.SupportsApplicationAuthentication) {
                // Auth using Spotify app
                Debug.WriteLine("Auth using spotify app");
                UIApplication.SharedApplication.OpenUrl(auth.SpotifyAppAuthenticationURL);
            } else {
                // App using web view
                Debug.WriteLine("ERR: Cannot auth using spotify app");
                // TODO: Use web auth vi        }
            }
        }

        public bool OpenUrl(UIApplication app, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Debug.WriteLine("Got a URL: " + url.ToString());

            SPTAuth auth = SPTAuth.DefaultInstance;

            // This is the callback that's triggerred when auth is completed (or fails).
            SPTAuthCallback authCallback = (NSError error, SPTSession session) =>
            {
                if (error != null) {
                    Debug.WriteLine("*** Auth error: " + error.Description);
                } else {
                    auth.Session = session;
                }

                Debug.WriteLine("Posting the notif");
                NSNotificationCenter.DefaultCenter.PostNotificationName("sessionUpdated", this);
            };

            // Handle the callback from the authentication service
            if (auth.CanHandleURL(url)) {
                Debug.WriteLine("Handling it");
                auth.HandleAuthCallbackWithTriggeredAuthURL(url, authCallback);
                return true;
            }

            return false;
        }

        //public void startAuthenticationFlow()
        //{
        //    var spotifyAuthenticationViewController = SPTAuthViewController.AuthenticationViewController;
        //    spotifyAuthenticationViewController.Delegate = new AuthViewDelegate(this);
        //    spotifyAuthenticationViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
        //    spotifyAuthenticationViewController.DefinesPresentationContext = true;
        //    this.PresentViewController(spotifyAuthenticationViewController, fll);
        //}
    }
}
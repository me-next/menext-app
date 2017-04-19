using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundation;
using MeNext.Spotify.iOS.Auth;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    /// <summary>
    /// Spotify authorization.
    /// </summary>
    public class SpotifyAuth : NSObject
    {
        private SpotifyMusicServiceIos service;

        public SpotifyAuth(SpotifyMusicServiceIos service)
        {
            this.service = service;
        }

        public static void Login()
        {
            SPTAuth auth = SPTAuth.DefaultInstance;

            if (SPTAuth.SupportsApplicationAuthentication) {
                // Auth using Spotify app
                Debug.WriteLine("Spotify available; Using Spotify authentication.", "auth");
                UIApplication.SharedApplication.OpenUrl(auth.SpotifyAppAuthenticationURL);
            } else {
                // App using web view
                Debug.WriteLine("Spotify unavailable; Using web authentication.", "auth");
                UIApplication.SharedApplication.OpenUrl(new NSUrl(auth.SpotifyWebAuthenticationURL.AbsoluteString));
            }
        }

        public StreamingDelegate CreateStreamingDelegate()
        {
            Debug.WriteLine("Creating streaming delegate", "auth");

            var sd = new StreamingDelegate(this.service);

            SPTAuth auth = SPTAuth.DefaultInstance;

            // ClientID and RedirectURL come from http://developer.spotify.com/my-applications
            // TODO Move out of the service
            auth.ClientID = service.ClientId;

            // Request permission to do stuff
            // We should leave anything we don't use commented out
            auth.RequestedScopes = new NSObject[] {
                SpotifyConstants.SPTAuthStreamingScope,                   // Playing audio
                SpotifyConstants.SPTAuthPlaylistReadPrivateScope,
                SpotifyConstants.SPTAuthPlaylistReadCollaborativeScope,
                //SpotifyConstants.SPTAuthPlaylistModifyPublicScope,
                //SpotifyConstants.SPTAuthPlaylistModifyPrivateScope,
                //SpotifyConstants.SPTAuthUserFollowModifyScope,
                //SpotifyConstants.SPTAuthUserFollowReadScope,
                SpotifyConstants.SPTAuthUserLibraryReadScope,
                //SpotifyConstants.SPTAuthUserLibraryModifyScope,
                SpotifyConstants.SPTAuthUserReadPrivateScope,             // Gives access to user's country
                //SpotifyConstants.SPTAuthUserReadTopScope,
                //SpotifyConstants.SPTAuthUserReadBirthDateScope,
                SpotifyConstants.SPTAuthUserReadEmailScope
            };

            // Callback
            auth.RedirectURL = new Foundation.NSUrl(service.SpotifyCallback);

            // Enables SPTAuth to automatically store the session object for future use.
            auth.SessionUserDefaultsKey = @"SpotifySession";

            // Use our token swap service
            auth.TokenSwapURL = new NSUrl(SpotifyToken.TOKEN_SWAP_URL);
            auth.TokenRefreshURL = new NSUrl(SpotifyToken.TOKEN_REFRESH_URL);

            // If we have a valid session, notify that the session was updated so player gets setup
            if (auth.Session != null && auth.Session.IsValid) {
                Debug.WriteLine("We have an existing, valid session. Reusing it.", "auth");
                NSNotificationCenter.DefaultCenter.PostNotificationName("sessionUpdated", this);
            } else {
                Debug.WriteLine("We do not have a valid session.", "auth");
            }
            return sd;
        }

        /// <summary>
        /// Called when the application is opened by a URL.
        /// </summary>
        /// <returns><c>true</c>, if the app was opened successfully, <c>false</c> otherwise.</returns>
        /// <param name="url">URL that app is opened from.</param>
        public bool OpenUrl(UIApplication app, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Debug.WriteLine("OpenUrl called with url: " + url.AbsoluteString, "auth");

            SPTAuth auth = SPTAuth.DefaultInstance;

            // This is the callback that's triggerred when auth is completed (or fails).
            SPTAuthCallback authCallback = (NSError error, SPTSession session) =>
            {
                if (error != null) {
                    Debug.WriteLine("*** Authentication error: " + error.Description, "auth");
                } else {
                    auth.Session = session;
                    Debug.WriteLine("Authentication successful. Notifying the plebs.", "auth");
                    NSNotificationCenter.DefaultCenter.PostNotificationName("sessionUpdated", this);
                }
            };

            // Handle the callback from the authentication service
            if (auth.CanHandleURL(url)) {
                Debug.WriteLine("We can handle this Url.", "auth");
                auth.HandleAuthCallbackWithTriggeredAuthURL(url, authCallback);
                return true;
            }

            Debug.WriteLine("We cannot handle this Url.", "auth");
            return false;
        }
    }
}
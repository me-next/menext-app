using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundation;
using MeNext.Spotify.iOS.Auth;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    public class SpotifyAuth : NSObject
    {
        private SpotifyMusicServiceIos service;

        public SpotifyAuth(SpotifyMusicServiceIos service)
        {
            this.service = service;
        }

        public static void Login()
        {
            //if (!SPTAuth.SpotifyApplicationIsInstalled) {
            //    // TODO: Error message?
            //    return;
            //}

            SPTAuth auth = SPTAuth.DefaultInstance;

            if (SPTAuth.SupportsApplicationAuthentication && false) {
                // Auth using Spotify app
                Debug.WriteLine("Auth using spotify app");
                UIApplication.SharedApplication.OpenUrl(auth.SpotifyAppAuthenticationURL);
            } else {
                // App using web view
                Debug.WriteLine("Cannot auth using spotify app. Authing with web view.");
                UIApplication.SharedApplication.OpenUrl(new NSUrl(auth.SpotifyWebAuthenticationURL.AbsoluteString));
            }
        }

        public StreamingDelegate CreateStreamingDelegate()
        {
            var sd = new StreamingDelegate(this.service);

            SPTAuth auth = SPTAuth.DefaultInstance;

            // ClientID and RedirectURL come from http://developer.spotify.com/my-applications
            // TODO Move out of the service
            auth.ClientID = "b79f545d6c24407aa6bed17af62275d6";

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
                //SpotifyConstants.SPTAuthUserReadEmailScope
            };

            // Callback
            // TODO: Move out of the service
            auth.RedirectURL = new Foundation.NSUrl("menext-spotify://callback");

            // Enables SPTAuth to automatically store the session object for future use.
            auth.SessionUserDefaultsKey = @"SpotifySession";

            // TODO: Use a token swap service
            //auth.TokenSwapURL;
            //auth.TokenRefreshURL;

            // TODO remove
            // Console.WriteLine("Has Spotify: " + SPTAuth.SpotifyApplicationIsInstalled);

            // If we have a valid session, notify that the session was updated so player gets setup
            if (auth.Session != null && auth.Session.IsValid) {
                NSNotificationCenter.DefaultCenter.PostNotificationName("sessionUpdated", this);
            }
            return sd;
        }

        public bool OpenUrl(UIApplication app, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Debug.WriteLine("open URL called");
            SPTAuth auth = SPTAuth.DefaultInstance;

            // This is the callback that's triggerred when auth is completed (or fails).
            SPTAuthCallback authCallback = (NSError error, SPTSession session) =>
            {
                if (error != null) {
                    // TODO: Do something?
                    Debug.WriteLine("*** Auth error: " + error.Description);
                } else {
                    auth.Session = session;
                    Debug.WriteLine("auth good");
                }

                NSNotificationCenter.DefaultCenter.PostNotificationName("sessionUpdated", this);
            };

            // Handle the callback from the authentication service
            if (auth.CanHandleURL(url)) {
                auth.HandleAuthCallbackWithTriggeredAuthURL(url, authCallback);
                return true;
            }

            return false;
        }


        // Stolen from https://github.com/jguertl/SharePlugin/blob/5cd908cfe62d6f5d002823b4d712689dd1386a67/Share/Share.Plugin.iOSUnified/ShareImplementation.cs
        //public async Task<bool> OpenBrowser(string url, bool useSafari = true)
        //{
        //try {
        //    if (useSafari && UIDevice.CurrentDevice.CheckSystemVersion(9, 0)) {
        //        // create safari controller
        //        var sfViewController = new SFSafariViewController(new NSUrl(url), false);

        //        // show safari controller
        //        var vc = GetVisibleViewController();

        //        if (sfViewController.PopoverPresentationController != null) {
        //            sfViewController.PopoverPresentationController.SourceView = vc.View;
        //        }

        //        await vc.PresentViewControllerAsync(sfViewController, true);
        //    } else {
        //        UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        //    }

        //    return true;
        //} catch (Exception ex) {
        //    Console.WriteLine("Unable to open browser: " + ex.Message);
        //    return false;
        //}
        //}


        UIViewController GetVisibleViewController(UIViewController controller = null)
        {
            controller = controller ?? UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (controller.PresentedViewController == null)
                return controller;

            if (controller.PresentedViewController is UINavigationController) {
                return ((UINavigationController) controller.PresentedViewController).VisibleViewController;
            }

            if (controller.PresentedViewController is UITabBarController) {
                return ((UITabBarController) controller.PresentedViewController).SelectedViewController;
            }

            return GetVisibleViewController(controller.PresentedViewController);
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
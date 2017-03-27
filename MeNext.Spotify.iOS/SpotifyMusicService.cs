using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using MeNext.MusicService;
using MeNext.Spotify.iOS.Auth;
using MeNext.Spotify.iOS.Playback;
using SafariServices;
using UIKit;

namespace MeNext.Spotify.iOS
{
    // TODO Document
    // Based heavily on https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    // and https://developer.spotify.com/technologies/spotify-ios-sdk/tutorial/
    public class SpotifyMusicService : NSObject, IMusicService
    {
        StreamingDelegate sd;

        public SpotifyMusicService()
        {
            sd = new StreamingDelegate();

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

            var lc = new LoginController();
            lc.login();
        }

        public new bool OpenUrl(UIApplication app, NSUrl url, string sourceApplication, NSObject annotation)
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
        //    this.PresentViewController(spotifyAuthenticationViewController, false, null);
        //}


        // =========================== //
        public bool CanPlay
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanSearch
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasUserLibrary
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Playing
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double PlayingPosition
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ISong PlayingSong
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IAlbum> UserLibraryAlbums
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IArtist> UserLibraryArtists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<IPlaylist> UserLibraryPlaylists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultList<ISong> UserLibrarySongs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddPlayStatusListener(ISongPlayListener listener)
        {
            //throw new NotImplementedException();
            // TODO
        }

        public IAlbum GetAlbum(string uid)
        {
            throw new NotImplementedException();
        }

        public IArtist GetArtist(string uid)
        {
            throw new NotImplementedException();
        }

        public IPlaylist GetPlaylist(string uid)
        {
            throw new NotImplementedException();
        }

        public ISong GetSong(string uid)
        {
            throw new NotImplementedException();
        }

        public void PlaySong(ISong song, double position = 0)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayStatusListener(ISongPlayListener listener)
        {
            throw new NotImplementedException();
        }

        public IResultList<IAlbum> SearchAlbum(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<IArtist> SearchArtist(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<IPlaylist> SearchPlaylists(string query)
        {
            throw new NotImplementedException();
        }

        public IResultList<ISong> SearchSong(string query)
        {
            throw new NotImplementedException();
        }

        public void SuggestBuffer(List<ISong> songs)
        {
            throw new NotImplementedException();
        }
    }
}

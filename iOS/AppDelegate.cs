using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


using Foundation;
using MeNext.Spotify.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MeNext.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private static AppDelegate currentDelegate;

        private SpotifyMusicServiceIos musicService;
        private MainController mainController;
        private PollingTask pollingTask;

        public AppDelegate()
        {
            // create common music service objects
            // these will go through the PollingTask to the Poller
            musicService = new SpotifyMusicServiceIos();
            mainController = new MainController(this.musicService);

            Debug.Assert(currentDelegate == null);
            currentDelegate = this;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Begin polling
            InitPolling();

            // Boilerplate
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App(mainController));

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            if (this.musicService == null) {
                return false;
            }
            // TODO: Untie this from Spotify and make it platform neutral?
            return new SpotifyAuth(musicService).OpenUrl(application, url, sourceApplication, annotation);
        }

        /// <summary>
        /// Subscribes to the polling start and stop messages
        /// </summary>
        private void InitPolling()
        {
            MessagingCenter.Subscribe<StartPollMessage>(this, "StartPollMessage", async message =>
            {
                pollingTask = new PollingTask();
                await pollingTask.StartAsync(mainController);
            });

            MessagingCenter.Subscribe<StopPollMessage>(this, "StopPollMessage", message =>
            {
                pollingTask.Stop();
            });
        }

        // Unfortunately needs to be static, unless you can figure out some way to make the renderer get an object ref
        // on creation?
        /// <summary>
        /// Called when a remote control event is recv'd.
        /// </summary>
        /// <param name="theEvent">The event.</param>
        public static void RemoteControlReceived(UIEvent theEvent)
        {
            var evt = currentDelegate.mainController.Event;
            if (evt == null) {
                return;
            }
            switch (theEvent.Subtype) {
                case UIEventSubtype.RemoteControlNextTrack:
                    evt.RequestSkip();
                    break;

                case UIEventSubtype.RemoteControlPreviousTrack:
                    evt.RequestPrevious();
                    break;

                case UIEventSubtype.RemoteControlPause:
                    evt.RequestPause();
                    break;

                case UIEventSubtype.RemoteControlPlay:
                    evt.RequestPlay();
                    break;
            }
        }
    }
}

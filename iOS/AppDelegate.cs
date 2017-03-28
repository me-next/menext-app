using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


using Foundation;
using MeNext.Spotify.iOS;
using UIKit;
using Xamarin.Forms;

namespace MeNext.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SpotifyMusicService musicService;
        private MainController mainController;
        private PollingTask pollingTask;

        public AppDelegate()
        {
            Debug.WriteLine("in app delegate constructor");

            // create common music service objects
            // these will go through the PollingTask to the Poller
            //musicService = new SampleMusicService.SampleMusicService();
            musicService = new SpotifyMusicService();
            mainController = new MainController(this.musicService);
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
            return new SpotifySetup().OpenUrl(application, url, sourceApplication, annotation);
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
    }
}

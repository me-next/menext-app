using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using MeNext.MusicService;
using MeNext.Spotify.iOS;
using UIKit;
using Xamarin.Forms;

namespace MeNext.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private PollingTask pollingTask;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Begin polling
            InitPolling();

            // Initialise Spotify
            // This is not actually an error.
            // Building does some witchcraft to resolve the dependency stuff.
            IMusicService im = new SpotifyMusicService();

            // Boilerplate
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App(im));

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Subscribes to the polling start and stop messages
        /// </summary>
        private void InitPolling()
        {
            MessagingCenter.Subscribe<StartPollMessage>(this, "StartPollMessage", async message =>
            {
                pollingTask = new PollingTask();
                await pollingTask.StartAsync();
            });

            MessagingCenter.Subscribe<StopPollMessage>(this, "StopPollMessage", message =>
            {
                pollingTask.Stop();
            });
        }
    }
}

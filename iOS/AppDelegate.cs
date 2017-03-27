using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        SpotifyMusicService sms;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //// Begin polling
            //InitPolling();

            // Initialise Spotify
            // This is not actually an error.
            // Building does some witchcraft to resolve the dependency stuff.


            // Boilerplate
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App(this.sms));

            this.sms = new SpotifyMusicService();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Debug.WriteLine("AD Got URL: " + url);
            if (this.sms == null) {
                return false;
            }
            return this.sms.OpenUrl(application, url, sourceApplication, annotation);
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

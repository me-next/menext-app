using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


using Foundation;
using UIKit;
using Xamarin.Forms;
using MeNext;
using MeNext.MusicService;

namespace MeNext.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public AppDelegate()
        {
            Debug.WriteLine("in app delegate constructor");

            // create common music service objects
            // these will go through the PollingTask to the Poller
            musicService = new SampleMusicService.SampleMusicService();
            mainController = new MainController(this.musicService);
        }

        private IMusicService musicService;
        private MainController mainController;

        private PollingTask pollingTask;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            InitPolling();

            // Boilerplate UI stuff

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App(mainController));

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
                await pollingTask.StartAsync(mainController);
            });

            MessagingCenter.Subscribe<StopPollMessage>(this, "StopPollMessage", message =>
            {
                pollingTask.Stop();
            });
        }
    }
}

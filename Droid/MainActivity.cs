using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using MeNext.Spotify.Droid;
using Xamarin.Forms;

namespace MeNext.Droid
{
    [Activity(Label = "MeNext", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainController Controller;

        private SpotifyMusicServiceDroid sms;
        /// <summary>
        /// On create do this.
        /// </summary>
        /// <param name="bundle">Bundle for initializing.</param>
        protected override void OnCreate(Bundle bundle)
        {
            // We should only be making this activity once
            System.Diagnostics.Debug.Assert(Controller == null);

            this.sms = new SpotifyMusicServiceDroid(this);
            Controller = new MainController(this.sms);

            InitPolling();

            // Boilerplate UI stuff
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(Controller));
        }
        /// <summary>
        /// On resuming the app
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            this.sms.OnResume();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            this.sms.OnActivityResult(requestCode, resultCode, data);
        }
        /// <summary>
        /// On pausing the App.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            this.sms.OnPause();
        }
        /// <summary>
        /// On fully closing the app.
        /// </summary>
        protected override void OnDestroy()
        {
            // Super OnDestroy was called second in the example code
            // Not sure if it actually needs to be in reverse order
            // https://github.com/spotify/android-sdk/blob/master/samples/DemoProject/src/main/java/com/spotify/sdk/demo/DemoActivity.java
            this.sms.OnDestroy();
            base.OnDestroy();
        }

        /// <summary>
        /// Subscribes to the polling start and stop messages
        /// </summary>
        private void InitPolling()
        {
            MessagingCenter.Subscribe<StartPollMessage>(this, "StartPollMessage", message =>
            {
                var intent = new Intent(this, typeof(PollingService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopPollMessage>(this, "StopPollMessage", message =>
            {
                var intent = new Intent(this, typeof(PollingService));
                StopService(intent);
            });
        }
    }
}

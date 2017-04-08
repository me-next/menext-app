using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MeNext.Spotify.Droid;
using Xamarin.Forms;

namespace MeNext.Droid
{
    [Activity(Label = "MeNext", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private SpotifyMusicServiceDroid sms;
        protected override void OnCreate(Bundle bundle)
        {
            this.sms = new SpotifyMusicServiceDroid(this);

            InitPolling();

            // Boilerplate UI stuff
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new MainController(this.sms)));
        }

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

        protected override void OnPause()
        {
            base.OnPause();
            this.sms.OnPause();
        }

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

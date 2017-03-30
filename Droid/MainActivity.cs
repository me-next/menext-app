using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;

namespace MeNext.Droid
{
    [Activity(Label = "MeNext", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            InitPolling();

            // Boilerplate UI stuff
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(null));
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

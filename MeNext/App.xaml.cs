using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MeNext.Layout;
using MeNext.MusicService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MeNext
{
    public partial class App : Application
    {
        public App(IMusicService musicService)
        {
            InitializeComponent();

            // TODO: Remove once Droid is implemented
            if (musicService == null) {
                musicService = new SampleMusicService.SampleMusicService();
            }

            var mainController = new MainController(musicService);
            MainPage = new MainPage(mainController);
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
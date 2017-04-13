using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        private readonly IMusicService service;

        public PlayingScreen(MainController mainController)
        {
            this.service = mainController.musicService;

            this.Title = "Now Playing";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };

            layout.Children.Add(new Button
            {
                Text = "<<",
                Command = new Command(() => mainController.Event.RequestPrevious())
            });

            var playCommand = new Command(() =>
            {
                mainController.musicService.Playing = !mainController.musicService.Playing;
            });
            layout.Children.Add(new Button
            {
                Text = "Play/Pause",
                Command = playCommand
            });

            layout.Children.Add(new Button
            {
                Text = ">>",
                Command = new Command(() => mainController.Event.RequestSkip())
            });

            layout.Children.Add(this.songTitle = new Label { Text = "" });

            Content = layout;

            mainController.Event.RegisterUiListener(this);
        }

        public void SomethingChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (this.service.PlayingSong != null) {
                    this.songTitle.Text = this.service.PlayingSong.Name + " (" + this.service.PlayingPosition + "s)";
                } else {
                    this.songTitle.Text = "Nothing Playing";
                }
            });
        }
    }
}
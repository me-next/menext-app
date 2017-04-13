using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        private readonly MainController mainController;

        public PlayingScreen(MainController mainController)
        {
            this.mainController = mainController;

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

            layout.Children.Add(new Button
            {
                Text = "Play/Pause",
                Command = new Command(() => { })
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
                var playing = this.mainController.Event?.LatestPull?.Playing?.CurrentSongID;
                if (playing != null) {
                    var song = this.mainController.musicService.GetSong(playing);
                    this.songTitle.Text = song.Name;
                    // TODO Other metadata
                } else {
                    this.songTitle.Text = "Nothing Playing";
                }
            });
        }
    }
}
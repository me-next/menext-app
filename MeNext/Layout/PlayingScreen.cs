using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// The currently playing song screen.  Shows the currently playing song and other info.
    /// </summary>
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        private readonly MainController mainController;
        private Button prevButt;
        private Button playButt;
        private Button pauseButt;
        private Button nextButt;
        public PlayingScreen(MainController mainController)
        {
            this.mainController = mainController;

            this.Title = "Now Playing";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING
            };
            // Buttons to manipulate the playing music queue.
            prevButt = new Button
            {
                Text = "<<",
                Command = new Command(() => mainController.Event.RequestPrevious()),
                IsVisible = false
            };
            prevButt.Clicked += (sender, e) => SomethingChanged();
            layout.Children.Add(prevButt);
            playButt = new Button
            {
                Text = "Play",
                Command = new Command(PlayPause),
                IsVisible = false
            };
            layout.Children.Add(playButt);
            pauseButt = new Button
            {
                Text = "Pause",
                Command = new Command(PlayPause),
                IsVisible = false
            };
            layout.Children.Add(pauseButt);
            nextButt = new Button
            {
                Text = ">>",
                Command = new Command(() => mainController.Event.RequestSkip()),
                IsVisible = false
            };
            nextButt.Clicked += (sender, e) => SomethingChanged();
            layout.Children.Add(nextButt);

            layout.Children.Add(this.songTitle = new Label { Text = "" });

            Content = layout;

            mainController.Event.RegisterUiListener(this);
            this.SomethingChanged();
        }
        /// <summary>
        /// Something has changed.  Update UI accordingly.
        /// Shows buttons and song name when they are available to the User.
        /// </summary>
        public void SomethingChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var playing = this.mainController.Event?.LatestPull?.Playing?.CurrentSongID;
                // There is a song playing.
                if (playing != null) {
                    var song = this.mainController.musicService.GetSong(playing);
                    this.songTitle.Text = song.Name;
                    prevButt.IsVisible = true;
                    pauseButt.IsVisible = true;
                    nextButt.IsVisible = true;
                    // TODO Other metadata
                } else {
                    prevButt.IsVisible = true;
                    nextButt.IsVisible = true;
                    this.songTitle.Text = "Nothing Playing";
                }
            });
        }
        /// <summary>
        /// Play or Pause depending on the status of the Event.
        /// </summary>
        public void PlayPause()
        {
            // Music is playing
            if (this.mainController.musicService.Playing) {
                pauseButt.IsVisible = true;
                playButt.IsVisible = false;
                mainController.Event.RequestPlay();
            } else { // No music playing.
                pauseButt.IsVisible = false;
                playButt.IsVisible = true;
                mainController.Event.RequestPause();
            }
        }
    }
}
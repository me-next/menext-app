using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        //TODO: make album art work
        //private Image albumArt;
        //private readonly int artSize;
        private readonly MainController mainController;

        public PlayingScreen(MainController mainController)
        {
            this.mainController = mainController;

            this.Title = "Now Playing";
            NavigationPage.SetHasNavigationBar(this, false);

            var prevButton = new Button
            {
                Image = "previous_icon_50px.png",
                Command = new Command(() => mainController.Event.RequestPrevious())
            };

            var playButton = new Button
            {
                Image = "play_icon_50px.png",
                //TODO: add functionality, including switching image
                Command = new Command(() => { })
            };

            var nextButton = new Button
            {
                Image = "next_icon_50px.png",
                Command = new Command(() => mainController.Event.RequestSkip())
            };

            var buttons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    prevButton,
                    playButton,
                    nextButton
                }
            };

            this.songTitle = new Label { Text = "", HorizontalOptions = LayoutOptions.CenterAndExpand };
            //this.artSize = (int)(this.Width * 0.7);
            //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    //this.albumArt,
                    this.songTitle,
                    buttons
                }
            };

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
                    //this.albumArt = (Image)song.Album.GetAlbumArt(artSize, artSize);
                    // TODO Other metadata
                } else {
                    this.songTitle.Text = "Nothing Playing";
                    //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };
                }
            });
        }
    }
}
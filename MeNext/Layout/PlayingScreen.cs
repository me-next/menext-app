using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        private readonly Label artistLabel;
        private readonly Label albumTitle;
        //TODO: make album art work
        //private Image albumArt;
        //private readonly int artSize;
        private Button playButton;
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

            playButton = new Button
            {
                Image = "play_icon_50px.png",
                Command = new Command(() => {
                    if (mainController.musicService.Playing) {
                        mainController.Event.RequestPause();
                    } else {
                        mainController.Event.RequestPlay();
                    }
                })
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

            //TODO: add seeking/time slider

            this.songTitle = new Label { Text = "", Margin = new Thickness(0, 30, 0, 0), HorizontalOptions = LayoutOptions.CenterAndExpand };
            this.artistLabel = new Label { Text = "", HorizontalOptions = LayoutOptions.CenterAndExpand };
            this.albumTitle = new Label { Text = "", Margin = new Thickness(0, 0, 0, 30), HorizontalOptions = LayoutOptions.CenterAndExpand };

            //this.artSize = (int)(this.Width * 0.7);
            //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };

            this.Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    //this.albumArt,
                    this.songTitle,
                    this.artistLabel,
                    this.albumTitle,
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
                    this.albumTitle.Text = song.Album.Name;
                    //this.albumArt = (Image)song.Album.GetAlbumArt(artSize, artSize);
                    // TODO Other metadata
                } else {
                    this.songTitle.Text = "Nothing Playing";
                    this.songTitle.Margin = new Thickness(0, 30, 0, 30);
                    this.artistLabel.Text = "";
                    this.albumTitle.Text = "";
                    //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };
                }
                if (mainController.musicService.Playing) {
                    this.playButton.Image = "play_icon_50px.png";
                } else {
                    this.playButton.Image = "pause_icon_50px.png";
                }
            });
        }
    }
}
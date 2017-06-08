using System;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// The currently playing song screen. Shows the currently playing song and other info.
    /// </summary>
    public class PlayingScreen : ContentPage, IUIChangeListener
    {
        private readonly Label songTitle;
        private readonly Label artistLabel;
        private readonly Label albumTitle;
        //TODO: make album art work
        //private Image albumArt;
        //private readonly int artSize;

        private Button playButton;
        private Button prevButton;
        private Button nextButton;


        private readonly MainController mainController;
        //private Slider volumeSlider;
        private int lastVolumePull;

        public PlayingScreen(MainController mainController)
        {
            this.mainController = mainController;

            this.Title = "Now Playing";
            NavigationPage.SetHasNavigationBar(this, false);

            //volumeSlider = new Slider(0, 100, 50);
            //volumeSlider.ValueChanged += (sender, e) =>
            //{
            //    mainController.Event.RequestVolume(volumeSlider.Value);
            //};

            // Buttons to manipulate the playing music queue.
            this.prevButton = new Button
            {
                Image = "previous_icon_50px.png",
                Command = new Command(() => mainController.Event.RequestPrevious())
            };

            playButton = new Button
            {
                Image = "play_icon_50px.png",
                Command = new Command(() =>
                {
                    if (mainController.Event.Playing) {
                        mainController.Event.RequestPause();
                    } else {
                        mainController.Event.RequestPlay();
                    }
                })
            };

            this.nextButton = new Button
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
            this.songTitle = new Label
            {
                Text = "",
                Margin = new Thickness(0, 30, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = LayoutConsts.TITLE_FONT_SIZE,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.TailTruncation,
            };
            this.artistLabel = new Label
            {
                Text = "",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation,
            };
            this.albumTitle = new Label
            {
                Text = "",
                Margin = new Thickness(0, 0, 0, 30),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation,
            };

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
                    buttons,
                    //volumeSlider,
                }
            };

            mainController.Event.RegisterUiListener(this);
            this.SomethingChanged();
        }

        /// <summary>
        /// Something has changed. Update UI accordingly.
        /// Shows buttons and song name when they are available to the User.
        /// </summary>
        public void SomethingChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // What's visible
                this.playButton.IsVisible = mainController.Event.ThisHasPermission(Permissions.PlayPause);
                var skip = mainController.Event.ThisHasPermission(Permissions.Skip);
                this.prevButton.IsVisible = skip;
                this.nextButton.IsVisible = skip;
                //this.volumeSlider.IsVisible = mainController.Event.ThisHasPermission(Permissions.Volume);

                // Update metadata
                var playing = this.mainController.Event?.LatestPull?.Playing?.CurrentSongID;
                if (playing != null) {
                    // There is a song playing.
                    var song = this.mainController.musicService.GetSong(playing);

                    var artists = "";
                    foreach (var artist in song.Artists) {
                        artists += ", " + artist.Name;
                    }
                    if (artists.Length > 0) {
                        artists = artists.Substring(2);
                    }

                    this.songTitle.Text = song.Name;
                    this.artistLabel.Text = artists;
                    this.albumTitle.Text = song.Album.Name;
                    //this.albumArt = (Image)song.Album.GetAlbumArt(artSize, artSize);
                    // TODO Other metadata
                } else {
                    this.songTitle.Text = "Nothing Playing";
                    this.artistLabel.Text = "";
                    this.albumTitle.Text = "";
                    //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };
                }

                // Play button icon
                if (mainController.Event.Playing) {
                    this.playButton.Image = "pause_icon_50px.png";
                } else {
                    this.playButton.Image = "play_icon_50px.png";
                }

                // Adjust playback stuff
                if (mainController.Event?.LatestPull?.Playing != null) {
                    //var nominalVolume = mainController.Event.LatestPull.Playing.Volume;
                    //if (nominalVolume != this.lastVolumePull) {
                    //    this.volumeSlider.Value = nominalVolume;
                    //    this.lastVolumePull = nominalVolume;
                    //}
                }
            });
        }
    }
}
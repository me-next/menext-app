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
<<<<<<< Updated upstream
<<<<<<< HEAD

=======

            // Buttons to manipulate the playing music queue.
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
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
>>>>>>> d700bcc... * Comments * MainPage.cs: * HostEvent.cs: * JoinEvent.cs: * HomeScreen.cs: * SearchView.cs: * LibraryView.cs: * LayoutConsts.cs: * TestingScreen.cs: * PlayingScreen.cs: * SimpleResultList.cs: * ResultListWrapper.cs: * ResultListEnumerator.cs: * Resource.designer.cs: * ResultListCell.cs:
=======
>>>>>>> Stashed changes

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
                var playing = this.mainController.Event?.LatestPull?.Playing?.CurrentSongID;
                // There is a song playing.
                if (playing != null) {
                    var song = this.mainController.musicService.GetSong(playing);
                    this.songTitle.Text = song.Name;
<<<<<<< Updated upstream
<<<<<<< HEAD
                    this.albumTitle.Text = song.Album.Name;
                    //this.albumArt = (Image)song.Album.GetAlbumArt(artSize, artSize);
=======
                    prevButt.IsVisible = true;
                    pauseButt.IsVisible = true;
                    nextButt.IsVisible = true;
>>>>>>> d700bcc... * Comments * MainPage.cs: * HostEvent.cs: * JoinEvent.cs: * HomeScreen.cs: * SearchView.cs: * LibraryView.cs: * LayoutConsts.cs: * TestingScreen.cs: * PlayingScreen.cs: * SimpleResultList.cs: * ResultListWrapper.cs: * ResultListEnumerator.cs: * Resource.designer.cs: * ResultListCell.cs:
=======
                    this.albumTitle.Text = song.Album.Name;
                    //this.albumArt = (Image)song.Album.GetAlbumArt(artSize, artSize);

>>>>>>> Stashed changes
                    // TODO Other metadata
                } else {
                    this.songTitle.Text = "Nothing Playing";
                    this.songTitle.Margin = new Thickness(0, 30, 0, 30);
                    this.artistLabel.Text = "";
                    this.albumTitle.Text = "";
                    //this.albumArt = new Image { HeightRequest = artSize, WidthRequest = artSize };
                }
                if (mainController.musicService.Playing) {
                    this.playButton.Image = "pause_icon_50px.png";
                } else {
                    this.playButton.Image = "play_icon_50px.png";
                }
            });
        }
    }
}
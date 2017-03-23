using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MeNext
{
    // Song is a POD class for song information.
    // TODO: maybe move this out of this file? Create a more legitimate version? Use the implementation
    // from menext/music-service?
    public class Song
    {
        public Song(string name)
        {
            this.Name = name;
        }

        public Song(string name, string artist)
        {
            this.Name = name;
            this.Artist = artist;
        }

        public string Name
        {
            get;
        }

        public string Artist
        {
            get;
        }

        public override string ToString()
        {
            return string.Format("[Song: Name={0}, Artist={1}]", Name, Artist);
        }
    };

    /// <summary>
    /// SongCellFactory controls how the song gets rendered for the SongList.
    /// Add buttons, click events etc. in here. 
    /// </summary>
    public abstract class SongCellFactory
    {
        public abstract ViewCell BuildView();
    };

    /// <summary>
    /// DefaultSongCellFactory renders a basic view cell that only shows the song name. 
    /// </summary>
    public class BasicSongCellFactory : SongCellFactory
    {
        public override ViewCell BuildView()
        {
            // create views with bindings for displaying each property
            Label nameLabel = new Label();
            nameLabel.SetBinding(Label.TextProperty, "Name");

            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Horizontal,
                    Children = { nameLabel }
                },
            };
        }
    };

    /// <summary>
    /// SongListModel is the model that backs the SongListView.
    /// A controller updates this list. The view observes the model and updates as needed
    /// This doesn't provide any priority-queue functionality, since the truth is always the server.
    /// </summary>
    public class SongListModel : ObservableCollection<Song>
    {
        public SongListModel(List<Song> songs) : base(songs)
        {
        }

        /// <summary>
        /// Allows the addition of multiple songs to the observable model at once
        /// Functionally the same as looping through newSongs and calling the normal Add function on each
        ///   except that the NotifyCollectionChangedEvent triggers only once, instead of for each item
        /// </summary>
        public void AddMultiple(List<Song> newSongs)
        {
            this.CheckReentrancy();
            foreach (var song in newSongs) {
                this.Items.Add(song);
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        // TODO: implement update from client
        // TODO: have this take the concrete impl of the music-service results stuff
    };

    /// <summary>
    /// SongListView displays a collection of songs. This is the "view". 
    /// The SongCellFactory set in the constructor provides significant control of the way the list behaves. 
    /// </summary>
    public class SongListView : ListView
    {
        // TODO: include drag and drop endpoints for a drag-n-drop controller

        // TODO: create other constructors or expose setters?

        public SongListView(SongListModel songs, SongCellFactory factory)
        {
            // use the default factory
            SetItemTemplateWithFactory(factory);

            // set the model
            this.Songs = songs;
            this.ItemsSource = this.Songs;

            // add tap handler
            this.ItemTapped += OnItemTapped;
        }

        /// <summary>
        /// set the ListView's ItemTemplate to use the provided SongFactory
        /// </summary>
        /// <param name="factory">Factory.</param>
        private void SetItemTemplateWithFactory(SongCellFactory factory)
        {
            this.ItemTemplate = new DataTemplate(() =>
            {
                return factory.BuildView();
            });
        }

        private SongListModel Songs
        {
            get;

            // set should clear all of the existing songs
            set;
        }

        /// <summary>
        /// delegate for when a song is clicked
        /// </summary>
        public delegate void SongClickedEvent(Song song);

        /// <summary>
        /// called when a song is tapped on once
        /// </summary>
        public event SongClickedEvent OnSongSelected;

        /// <summary>
        /// wrapper around OnItemTapped that pulls out just the selected song
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var song = (Song) e.Item;
            this.OnSongSelected(song);
        }
    }
}

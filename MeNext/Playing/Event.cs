using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MeNext.MusicService;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents an in-progress event.
    /// </summary>
    public class Event
    {
        private List<IUIChangeListener> listeners = new List<IUIChangeListener>();

        /// <summary>
        /// Are we the host for the current event?
        /// </summary>
        /// <value><c>true</c> if is host; otherwise, <c>false</c>.</value>
        public bool IsHost { get; private set; }

        /// <summary>
        /// The current event's slug.
        /// </summary>
        /// <value>The event slug.</value>
        public string Slug { get; private set; }

        /// <summary>
        /// The change id from the post recent pull
        /// </summary>
        private UInt64 changeID;

        /// <summary>
        /// The pull observers.
        /// </summary>
        private List<IPullUpdateObserver> PullObservers;

        /// <summary>
        /// The most recent poll response
        /// </summary>
        public PullResponse LatestPull { get; private set; }

        /// <summary>
        /// The suggestion queue
        /// </summary>
        public SuggestionQueue SuggestionQueue { get; private set; }

        /// <summary>
        /// Permissions are stored in the pull information. Default to allow all permissions. 
        /// </summary>
        /// <value>The permissions.</value>
        public Permissions Permissions { get; set; }

        private MainController controller;

        private API Api
        {
            get
            {
                return this.controller.Api;
            }
        }

        /// <summary>
        /// Whether or not we are currently playing music
        /// </summary>
        public bool Playing
        {
            get
            {
                if (this.LatestPull == null) {
                    return false;
                }
                return this.LatestPull.Playing.Playing;
            }
        }

        /// <summary>
        /// Creates a new event representation
        /// </summary>
        /// <param name="controller">The main controller.</param>
        /// <param name="eventSlug">The event slug.</param>
        /// <param name="isHost">If set to <c>true</c>, we are the host.</param>
        public Event(MainController controller, string eventSlug, bool isHost)
        {
            this.PullObservers = new List<IPullUpdateObserver>();

            this.IsHost = isHost;
            this.Slug = eventSlug;

            this.changeID = 0;

            this.controller = controller;

            this.SuggestionQueue = new SuggestionQueue();

            // Let the music controller know whether or not it is currently hosting
            this.controller.musicService.SetIsHost(this.IsHost);

            // set up the play controller
            if (this.IsHost) {
                var playController = new PlayController(this.controller.musicService, this.controller);
                RegisterPullObserver(playController);
            }

            if (this.IsHost) {
                // TODO This should be server side
                this.RequestVolume(50);
            }

            // create permissions and have it listen for updates
            this.Permissions = new Permissions();
            this.RegisterPullObserver(Permissions);
        }

        /// <summary>
        /// Requests that we resume the current song. Continue playing if it is already playing.
        /// </summary>
        public void RequestPlay()
        {
            var task = Task.Run(async () =>
             {
                 if (this.LatestPull != null) {
                     return await Api.PlaySong(this.Slug, this.controller.UserKey);
                 }
                 return null;
             });

            if (task.IsFaulted) {
                Debug.WriteLine("Error requesting play:" + task.Exception.ToString());
                return;
            }

            Debug.WriteLine("Requested play song");
        }

        /// <summary>
        /// Requests that we pause. We should stay paused if it is already paused.
        /// </summary>
        public void RequestPause()
        {
            var task = Task.Run(async () =>
             {
                 if (this.LatestPull != null) {
                     return await Api.PauseSong(this.Slug, this.controller.UserKey, this.controller.musicService.PlayingPosition);
                 }
                 return null;
             });

            if (task.IsFaulted) {
                Debug.WriteLine("Error requesting pause:" + task.Exception.ToString());
                return;
            }

            Debug.WriteLine("Requested pause song");
        }

        /// <summary>
        /// Requests the volume on a scale from 0-100
        /// </summary>
        /// <param name="vol">Vol.</param>
        public void RequestVolume(int vol)
        {
            Task.Run(async () =>
             {
                 return await Api.SetVolume(this.Slug, this.controller.UserKey, vol);
             });
        }

        /// <summary>
        /// Requests that we skip the current song
        /// </summary>
        public void RequestSkip()
        {
            var task = Task.Run(async () =>
             {
                 if (this.LatestPull != null) {
                     return await Api.SkipSong(this.Slug, this.controller.UserKey, this.LatestPull.Playing.CurrentSongID);
                 }
                 return null;
             });

            if (task.IsFaulted) {
                Debug.WriteLine("Error requesting skip:" + task.Exception.ToString());
                return;
            }

            Debug.WriteLine("Requested next song");
        }

        /// <summary>
        /// Requests that we skip the current song
        /// </summary>
        public void SongEnded()
        {
            var task = Task.Run(async () =>
             {
                 if (this.LatestPull != null) {
                     return await Api.SongFinished(this.Slug, this.controller.UserKey, this.LatestPull.Playing.CurrentSongID);
                 }
                 return null;
             });

            if (task.IsFaulted) {
                Debug.WriteLine("Error requesting next at song end:" + task.Exception.ToString());
                return;
            }

            Debug.WriteLine("Requested next song at song end");
        }

        /// <summary>
        /// Requests that we jump back to the previous song
        /// </summary>
        public void RequestPrevious()
        {
            var task = Task.Run(async () =>
             {
                 if (this.LatestPull != null) {
                     return await Api.PrevSong(this.Slug, this.controller.UserKey, this.LatestPull.Playing.CurrentSongID);
                 }
                 return null;
             });

            if (task.IsFaulted) {
                Debug.WriteLine("Error requesting previous:" + task.Exception.ToString());
                return;
            }

            Debug.WriteLine("Requested previous song");
        }

        /// <summary>
        /// Requests that we adjust the volume
        /// </summary>
        /// <param name="vol">The volume on a scale from 0-100</param>
        public void RequestVolume(double vol)
        {
            var task = Task.Run(async () =>
            {
                return await Api.SetVolume(this.Slug, this.controller.UserKey, (int) vol);
            });
        }

        /// <summary>
        /// Requests that we seek to a position within the song.
        /// </summary>
        /// <param name="pos">The position (seconds)</param>
        public void RequestSeek(double pos)
        {
            // TODO
        }

        /// <summary>
        /// Requests that we add a song to the suggestions queue. If it already exists, do nothing.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestAddToSuggestions(ISong song)
        {
            var task = Task.Run(async () =>
            {
                return await Api.SuggestAddSong(this.Slug, this.controller.UserKey, song.UniqueId);
            });

            if (task.IsFaulted) {
                Debug.WriteLine("Failed to add song to suggestions!" + task.Exception.ToString());
            }

            Debug.WriteLine("Added " + song.UniqueId + " to suggestions");
        }

        /// <summary>
        /// Requests that we remove a song from the suggestions queue. If it doesn't exist, do nothing.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestRemoveFromSuggestions(ISong song)
        {
            // TODO
        }

        /// <summary>
        /// Requests that we add a song to the play next queue at the specified index. If the song already exists
        /// in the play next queue, do nothing.
        /// </summary>
        /// <param name="song">Song.</param>
        /// <param name="index">
        /// The index to insert the song at. 0=Next Song, 1=Song after that, etc. -1=Last Song,-2=song before that,
        /// etc.
        /// </param>
        public void RequestAddToPlayNext(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.AddPlayNext(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that we add a song to the top of the up next queue
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestAddTopOfPlayNext(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.AddTopPlayNext(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that we remove a song from the up next queue
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestRemovePlayNext(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.RemovePlayNext(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that we play a song now
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestPlayNow(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.PlayNow(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that we remove a song from the play next queue.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestRemoveFromPlayNext(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.RemovePlayNext(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that the current user thumbs up the song.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbUp(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.SuggestionUpvote(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that the current user thumbs down the song.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbDown(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.SuggestionDownvote(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Requests that the current user's thumb up or down information is cleared for the song.  
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbNeutral(ISong song)
        {
            Task.Run(async () =>
            {
                return await Api.SuggestionClearvote(this.Slug, this.controller.UserKey, song.UniqueId);
            });
        }

        /// <summary>
        /// Checks if the user can perform an action. Distinct from the event permissions.
        /// </summary>
        /// <returns><c>true</c>, if the user has permission for the action <c>false</c> otherwise.</returns>
        /// <param name="which">Which permission to check.</param>
        public bool ThisHasPermission(string which)
        {
            return this.IsHost || this.Permissions.GetPermission(which);
        }

        /// <summary>
        /// Try to set the permission
        /// </summary>
        /// <param name="which">Which permission.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public void RequestSetPermission(string which, bool value)
        {
            // try to set the permission, just leave it
            var task = Task.Run(async () =>
            {
                return await this.Api.SetPermission(this.Slug, this.controller.UserKey, which, value);
            });

            Debug.WriteLine("set permission result: " + task.Result);
        }

        /// <summary>
        /// Begins polling the server for the latest info
        /// </summary>
        public void StartPolling()
        {
            var message = new StartPollMessage(Slug, this.controller.UserKey);
            MessagingCenter.Send(message, "StartPollMessage");
            Debug.WriteLine("Starting to poll");
        }

        /// <summary>
        /// Stops polling the server
        /// </summary>
        public void StopPolling()
        {
            var message = new StopPollMessage();
            MessagingCenter.Send(message, "StopPollMessage");
        }

        /// <summary>
        /// Poll this instance. The main controller tracks the change ID. 
        /// The deserialized server response is sent to observers, who are responsible for acting on the info. 
        /// </summary>
        public void Poll()
        {
            var task = Task.Run(async () =>
            {
                return await Api.Pull(this.controller.UserKey, this.Slug, this.changeID);
            });

            var json = task.Result;
            if (json == null) {
                // if got null json, then we had an error with executing the pull
                // request to leave event
                this.controller.LeaveEvent();
                return;
            }
            // if there is no data, continue on
            // This is expected if change id is equal to ours
            if (json.Length == 0) {
                return;
            }

            if (task.IsFaulted) {
                Debug.WriteLine("*** Failed to pull: " + task.Exception.ToString());
            }

            // try to parse the pull
            Debug.WriteLine("Pull json: " + json + ". " + json.Length);

            var result = JsonConvert.DeserializeObject<PullResponse>(json);


            // update ourself
            this.changeID = result.Change;

            UpdatePullObservers(result);
        }

        /// <summary>
        /// Registers an observer on the pull.
        /// </summary>
        /// <param name="observer">Observer.</param>
        public void RegisterPullObserver(IPullUpdateObserver observer)
        {
            PullObservers.Add(observer);
        }

        /// <summary>
        /// Updates the pull observers.
        /// </summary>
        /// <param name="data">Data retrieved from the pull.</param>
        private void UpdatePullObservers(PullResponse data)
        {
            // This needs to be done before we inform anybody else
            // In case they use some of our processed data
            HandleNewPull(data);

            // We use a copy so listeners we call can create objects which register new listeners
            var copy = new List<IPullUpdateObserver>(this.PullObservers);

            foreach (var observer in copy) {
                observer.OnNewPullData(data);
            }
            this.controller.InformSomethingChanged();
        }

        void HandleNewPull(PullResponse data)
        {
            this.LatestPull = data;
            this.SuggestionQueue.UpdateQueue(this.LatestPull.SuggestQueue);
        }

        /// <summary>
        /// Called whenever anything has changed which might require a UI update.
        /// DO NOT CALL THIS METHOD TO FORCE A UI REFRESH.
        /// Instead, call controller.InformSomethingChanged() which calls this method as a side effect.
        /// </summary>
        public void SomethingChangedDangerous()
        {
            // We use a copy so listeners we call can create objects which register new listeners
            var copy = new List<IUIChangeListener>(listeners);

            foreach (var listener in copy) {
                listener.SomethingChanged();
            }
        }

        /// <summary>
        /// Registers the a user interface might need refresh listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        internal void RegisterUiListener(IUIChangeListener listener)
        {
            listeners.Add(listener);
        }


    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MeNext.MusicService;
using Xamarin.Forms;

using Newtonsoft.Json;

namespace MeNext
{

    /// <summary>
    /// Pull update observer gets data from pulls.
    /// </summary>
    public interface IPullUpdateObserver
    {
        void onNewPullData(PullResponse data);
    }

    /// <summary>
    /// The main controller the UI interfaces with to communicate with the backend and music player
    /// </summary>
    public class MainController : ISongPlayListener
    {
        private IMusicService musicService;
        private PlayController playController;
        private API api;

        /// <summary>
        /// Used for cancelling polling. We should generally just stop polling instead.
        /// </summary>
        private CancellationToken CancelToken { get; set; }

        /// <summary>
        /// Is there presently an active event?
        /// </summary>
        /// <value><c>true</c> if in event; otherwise, <c>false</c>.</value>
        public bool InEvent { get; private set; }

        /// <summary>
        /// Are we the host for the current event? Assumes there is an active event.
        /// </summary>
        /// <value><c>true</c> if is host; otherwise, <c>false</c>.</value>
        public bool IsHost { get; private set; }

        /// <summary>
        /// The current event's slug. Assumes there is an active event.
        /// </summary>
        /// <value>The event slug.</value>
        public string EventSlug { get; private set; }

        /// <summary>
        /// The display name for the user
        /// </summary>
        // TODO: Set this somewhere
        public string UserName { get; private set; }

        /// <summary>
        /// The user's secret key for accessing the event
        /// </summary>
        public string UserKey { get; private set; }

        /// <summary>
        /// The change identifier.
        /// </summary>
        private UInt64 changeID;

        /// <summary>
        /// The pull observers.
        /// </summary>
        private List<IPullUpdateObserver> PullObservers;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.MusicController"/> class.
        /// </summary>
        /// <param name="musicService">The music service we should be interfacing with</param>
        public MainController(IMusicService musicService)
        {
            this.musicService = musicService;

            SubscribePollingStatus();

            this.musicService.AddPlayStatusListener(this);
            this.playController = new PlayController(this.musicService);

            this.api = new API("http://menext.danielcentore.com:8080");
            this.UserKey = "potato";
            this.UserName = "bob";

            this.changeID = 0;

            this.PullObservers = new List<IPullUpdateObserver>();
        }

        /// <summary>
        /// Attempts to join an event.
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        /// <param name="slug">The event name.</param>
        public JoinEventResult RequestJoinEvent(string slug)
        {
           var task = Task.Run(async () =>
            {
                return await api.JoinParty(this.UserKey, this.UserName);
            });

            var json = task.Result;
            Debug.WriteLine("json: " + json);

            if(task.IsFaulted) {
                Debug.WriteLine("oh nose! error:" + task.Exception.ToString());
                return JoinEventResult.FAIL_GENERIC;
            }

            Debug.WriteLine("joined event" + slug);
            _ConfigureForEvent(this.UserKey, true, slug);
            return JoinEventResult.SUCCESS;
        }

        /// <summary>
        /// Attempts to create an event
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        /// <param name="slug">The event name.</param>
        public CreateEventResult RequestCreateEvent(string slug)
        {
            var task = Task.Run(async () =>
            {
                return await api.CreateParty(this.UserKey, this.UserName);
            });
            var json = task.Result;
            Debug.WriteLine("json: " + json);

            // TODO: real error check
            if (task.IsFaulted) {
                Debug.WriteLine("failed to join event!" + task.Exception.ToString());
                return CreateEventResult.FAIL_GENERIC;
            }

            var result = JsonConvert.DeserializeObject<CreateEventResponse>(json);


            _ConfigureForEvent(this.UserKey, true, result.EventID);
            return CreateEventResult.SUCCESS;
        }

        /// <summary>
        /// Attempts to end the current event.
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        public EndEventResult RequestEndEvent()
        {
            Debug.Assert(this.InEvent);
            _StopPolling();

            // TODO Send stuff to server, wait for response, and use real response below

            if (IsHost)
                this.musicService.Playing = false;
            this.InEvent = false;
            return EndEventResult.SUCCESS;
        }

        /// <summary>
        /// Leaves the current event
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        public LeaveEventResult LeaveEvent()
        {
            Debug.Assert(this.InEvent);
            _StopPolling();

            // If we are the host, leaving the event is equivalent to ending it.
            if (IsHost) {
                EndEventResult eer = RequestEndEvent();
                if (eer == EndEventResult.FAIL_NETWORK)
                    return LeaveEventResult.FAIL_NETWORK;
                if (eer != EndEventResult.SUCCESS)
                    return LeaveEventResult.FAIL_GENERIC;

                this.InEvent = false;
                return LeaveEventResult.SUCCESS;
            }

            // TODO Send stuff to server, wait for response, and use real response below


            this.InEvent = false;
            return LeaveEventResult.SUCCESS;
        }

        /// <summary>
        /// Sets up stuff whenever we join or create an event
        /// </summary>
        /// <param name="userKey">User key.</param>
        /// <param name="isHost">If set to <c>true</c> is host.</param>
        /// <param name="eventSlug">Event slug.</param>
        private void _ConfigureForEvent(string userKey, bool isHost, string eventSlug)
        {
            this.UserKey = userKey;
            this.IsHost = isHost;
            this.EventSlug = eventSlug;
            this.InEvent = true;
            _StartPolling();
        }

        /// <summary>
        /// Requests that we resume the current song. Continue playing if it is already playing.
        /// </summary>
        public void RequestPlay()
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we pause. We should stay paused if it is already paused.
        /// </summary>
        public void RequestPause()
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we skip the current song
        /// </summary>
        public void RequestSkip()
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we jump back to the previous song
        /// </summary>
        public void RequestPrevious()
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we adjust the volume
        /// </summary>
        /// <param name="vol">The volume on a scale from 0-1</param>
        public void RequestVolume(double vol)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we seek to a position within the song.
        /// </summary>
        /// <param name="pos">The position (seconds)</param>
        public void RequestSeek(double pos)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we add a song to the suggestions queue. If it already exists, do nothing.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestAddToSuggestions(ISong song)
        {
            var task = Task.Run(async () =>
            {
                return await api.SuggestAddSong(this.UserKey, this.EventSlug, song.UniqueId);
            });
            var json = task.Result;
            Debug.WriteLine("json: " + json);

            if (task.IsFaulted) {
                Debug.WriteLine("failed to add song!" + task.Exception.ToString());
            }
        }

        /// <summary>
        /// Requests that we remove a song from the suggestions queue. If it doesn't exist, do nothing.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestRemoveFromSuggestions(ISong song)
        {
            Debug.Assert(this.InEvent);
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
        public void RequestAddToPlayNext(ISong song, int index = -1)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that we remove a song from the play next queue.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestRemoveFromPlayNext(ISong song)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Atomic operation which is functionally equivalent to:
        /// <code>
        /// RequestRemoveFromPlayNext(song);
        /// RequestAddToPlayNext(song, index);
        /// </code>
        /// </summary>
        /// <param name="song">Song.</param>
        /// <param name="index">Index.</param>
        public void MoveWithinPlayNext(ISong song, int index)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that the current user thumbs up the song.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbUp(ISong song)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that the current user thumbs down the song.
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbDown(ISong song)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that the current user's thumb up or down information is cleared for the song.	
        /// </summary>
        /// <param name="song">Song.</param>
        public void RequestThumbNeutral(ISong song)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that the current event be given permissions
        /// </summary>
        public void RequestEventPermissions(/* TODO: permissions structure */)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Requests that a user be given permissions.
        /// </summary>
        /// <param name="username">Username.</param>
        public void RequestUserPermissions(string username /* TODO: permissions structure */)
        {
            Debug.Assert(this.InEvent);
        }

        /// <summary>
        /// Begins polling the server for the latest info
        /// </summary>
        private void _StartPolling()
        {
            Debug.Assert(this.InEvent);
            var message = new StartPollMessage(EventSlug, UserKey);
            MessagingCenter.Send(message, "StartPollMessage");
        }

        /// <summary>
        /// Stops polling the server
        /// </summary>
        private void _StopPolling()
        {
            Debug.Assert(this.InEvent);
            var message = new StopPollMessage();
            MessagingCenter.Send(message, "StopPollMessage");
        }

        /// <summary>
        /// Poll this instance. The main controller tracks the change ID. 
        /// 
        /// The deserialized server response is sent to observers, who are responsible for acting on the info. 
        /// </summary>
        public void Poll()
        {
            var task = Task.Run(async () =>
            {
                return await api.Pull(this.UserKey, this.EventSlug, this.changeID);
            });

            var json = task.Result;

            // if there is no data, continue on
            if (json.Length == 0) {
                return;
            }

            if (task.IsFaulted) {
                Debug.WriteLine("failed to pull!" + task.Exception.ToString());
            }

            // try to parse the pull
            Debug.WriteLine("pull json: " + json + ". " + json.Length);

            var result = JsonConvert.DeserializeObject<PullResponse>(json);

            // update ourself
            this.changeID = result.Change;

            UpdatePullObservers(result);
        }

        /// <summary>
        /// Updates the pull observers.
        /// </summary>
        /// <param name="data">Data retrieved from the pull.</param>
        private void UpdatePullObservers(PullResponse data)
        {
            foreach (var observer in this.PullObservers) {
                observer.onNewPullData(data);
            }
        }

        /// <summary>
        /// Registers an observer on the pull.
        /// </summary>
        /// <param name="observer">Observer.</param>
        public void RegisterObserver(IPullUpdateObserver observer)
        {
            // TODO: membership checking
            PullObservers.Add(observer);
        }

        /// <summary>
        /// Subscribes to the polling status.
        /// </summary>
        private void SubscribePollingStatus()
        {
            MessagingCenter.Subscribe<StatusMessage>(this, "StatusMessage", message =>
            {
                if (!this.InEvent)
                    return;

                // Print the debug text
                if (message.TestingText != null) {
                }

                // If the event is ended, we should leave and return to the homepage.
                if (!message.EventActive)
                    LeaveEvent();

                // TODO: Update stuff based on message

                // Update music playing if we are the host
                if (IsHost) {
                    playController.UpdateActualPlaying(message);
                }

                // Update UI
                this.SomethingChanged();
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message =>
            {
                // TODO: This is leftover from the tutorial. Remove if we don't decide to use it for anything.
            });
        }

        /// <summary>
        /// Called by the music service when a song ends
        /// </summary>
        public void SongEnds(ISong song)
        {
            // Tell the server we want to play the next song.
            // As host, we will have permission to do this.
            this.RequestSkip();
        }

        /// <summary>
        /// Called whenever anything has changed which might require a UI update
        /// </summary>
        public void SomethingChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // TODO: Update the UI
            });
        }
    }
}

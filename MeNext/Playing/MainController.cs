using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeNext.MusicService;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace MeNext
{
    /// <summary>
    /// The main controller the UI interfaces with to communicate with the backend and music player
    /// </summary>
    public class MainController : IMusicServiceListener
    {
        public IMusicService musicService;

        public API Api { get; private set; }
        private static Random random = new Random();
        private List<IUIChangeListener> listeners = new List<IUIChangeListener>();

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
        /// Used for cancelling polling. We should generally just stop polling instead.
        /// </summary>
        private CancellationToken CancelToken { get; set; }

        /// <summary>
        /// Is there presently an active event?
        /// </summary>
        /// <value><c>true</c> if in event; otherwise, <c>false</c>.</value>
        public bool InEvent
        {
            get
            {
                return (this.Event != null);
            }
        }

        /// <summary>
        /// The current event (or null if there is none)
        /// </summary>
        public Event Event { get; private set; }

        /// <summary>
        /// The navigation page which we use for full app navigations
        /// </summary>
        public NavigationPage NavPage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.MusicController"/> class.
        /// </summary>
        /// <param name="musicService">The music service we should be interfacing with</param>
        public MainController(IMusicService musicService)
        {
            this.musicService = musicService;

            this.musicService.AddStatusListener(this);

            this.Api = new API(MeNextConsts.SERVER_URL);

            this.UserKey = RandomString(6);

            // TODO Username?
            this.UserName = "bob";
            Debug.WriteLine("User key: " + this.UserKey);
        }

        /// <summary>
        /// Checks if a result succeeded or failed. If it failed, shows an err msg
        /// </summary>
        /// <returns><c>true</c>, if result succeeded, <c>false</c> otherwise.</returns>
        /// <param name="result">Result.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public bool OhShit<T>(Result<T> result)
        {
            if (result.HasError) {
                NavPage.DisplayAlert("\ud83d\ude35 " + result.ErrorMsg + " \ud83d\ude35", result.ErrorDetails, "Okie doke");
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Attempts to join an event.
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        /// <param name="slug">The event id.</param>
        public JoinEventResult RequestJoinEvent(string slug)
        {
            Debug.Assert(!this.InEvent);
            Debug.WriteLine("Requesting to join an event...");

            var task = Task.Run(async () =>
            {
                return await Api.JoinParty(slug, this.UserKey, this.UserName);
            });
            var r = task.Result;
            if (OhShit(r)) {
                return JoinEventResult.FAIL_GENERIC;
            }

            this.Event = new Event(this, slug, false);
            this.Event.StartPolling();
            this.InformSomethingChanged();

            Debug.WriteLine("Joined event\'" + slug + "\'");
            return JoinEventResult.SUCCESS;
        }

        /// <summary>
        /// Attempts to create an event
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        public CreateEventResult RequestCreateEvent()
        {
            Debug.Assert(!this.InEvent);

            var task = Task.Run(async () =>
            {
                return await Api.CreateParty(this.UserKey, this.UserName);
            });
            var r = task.Result;
            if (OhShit(r)) {
                return CreateEventResult.FAIL_GENERIC;
            }
            var json = r.Wrapped;

            var result = JsonConvert.DeserializeObject<CreateEventResponse>(json);
            if (!string.IsNullOrEmpty(result.Error)) {
                Debug.WriteLine("error creating event: " + result.Error);
                return CreateEventResult.FAIL_GENERIC;
            }

            this.Event = new Event(this, result.EventID, true);
            this.Event.StartPolling();
            this.InformSomethingChanged();

            return CreateEventResult.SUCCESS;
        }

        /// <summary>
        /// Attempts to create an event with a given name.
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        /// <param name="Eventname">Given event name.</param>
        public CreateEventResult RequestCreateEvent(string eventName)
        {
            Debug.Assert(!this.InEvent);
            Debug.WriteLine("going to create event with name");
            var task = Task.Run(async () =>
            {
                return await Api.CreateParty(this.UserKey, this.UserName, eventName);
            });
            var r = task.Result;
            if (OhShit(r)) {
                return CreateEventResult.FAIL_GENERIC;
            }
            var json = r.Wrapped;

            var result = JsonConvert.DeserializeObject<CreateEventResponse>(json);
            // Event creation failed server side. Assumes duplicate event name was used.
            if (!string.IsNullOrEmpty(result.Error)) {
                // TODO result.alternativeName
                Debug.WriteLine("issue creating event with name: " + result.Error);
                return CreateEventResult.FAIL_EVENT_EXISTS;
            }

            if (!task.IsFaulted) {
                //if(task.Status.ToString == "StatusInternalServerError")
                if (result?.EventID != null) {
                    this.Event = new Event(this, result.EventID, true);
                    this.Event.StartPolling();
                    this.InformSomethingChanged();
                    return CreateEventResult.SUCCESS;
                } else { return CreateEventResult.FAIL_GENERIC; }
            } else {
                Debug.WriteLine("*** Failed to create event!" + task.Exception.ToString());
                return CreateEventResult.FAIL_GENERIC;
            }
        }

        /// <summary>
        /// Attempts to end the current event.
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        public EndEventResult RequestEndEvent()
        {
            Debug.Assert(this.InEvent);
            Debug.WriteLine("going to end event!");
            this.Event.StopPolling();

            var task = Task.Run(async () =>
            {
                return await Api.EndEvent(this.Event.Slug, this.UserKey);
            });
            var r = task.Result;
            if (OhShit(r)) {
                return EndEventResult.FAIL_GENERIC;
            }
            // TODO Send stuff to server, wait for response, and use real response below

            if (this.Event.IsHost) {
                this.musicService.Playing = false;
            }
            this.Event = null;

            this.InformSomethingChanged();
            return EndEventResult.SUCCESS;
        }

        /// <summary>
        /// Leaves the current event
        /// </summary>
        /// <returns>The result of the attempt.</returns>
        public LeaveEventResult LeaveEvent()
        {
            Debug.Assert(this.InEvent);
            this.Event.StopPolling();

            Debug.WriteLine("going to leave event");
            // If we are the host, leaving the event is equivalent to ending it.
            if (this.Event.IsHost) {
                Debug.WriteLine("host leave");
                EndEventResult eer = RequestEndEvent();
                if (eer == EndEventResult.FAIL_NETWORK)
                    return LeaveEventResult.FAIL_NETWORK;
                if (eer != EndEventResult.SUCCESS)
                    return LeaveEventResult.FAIL_GENERIC;

                this.Event = null;
                return LeaveEventResult.SUCCESS;
            }

            // ask to leave the event
            var task = Task.Run(async () =>
            {
                return await Api.LeaveEvent(this.Event.Slug, this.UserKey);
            });
            var r = task.Result;
            if (OhShit(r)) {
                return LeaveEventResult.FAIL_GENERIC;
            }

            this.Event = null;

            this.InformSomethingChanged();
            return LeaveEventResult.SUCCESS;
        }

        /// <summary>
        /// Called by the music service when a song ends
        /// </summary>
        public void SongEnds(ISong song)
        {
            Debug.WriteLine("== SONG ENDED ==");
            this.Event?.SongEnded();
        }

        /// <summary>
        /// Called whenever anything has changed which might require a UI update
        /// </summary>
        public void InformSomethingChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // We use a copy so listeners we call can create objects which register new listeners
                var copy = new List<IUIChangeListener>(listeners);

                if (!this.InEvent) {
                    this.musicService.SetIsHost(false);
                }

                foreach (var listener in copy) {
                    listener.SomethingChanged();
                }

                this.Event?.SomethingChangedDangerous();
            });
        }

        /// <summary>
        /// Registers the a user interface might need refresh listener.
        /// DO NOT USE THIS METHOD IF YOUR OBJECT IS TO BE GARBAGE COLLECTED BETWEEN EVENTS.
        /// In fact, we assert that this is never called while an event is in progress.
        /// Instead, use MainController.Event.RegisterUiListener(..)
        /// </summary>
        /// <param name="listener">Listener.</param>
        internal void RegisterUiListenerDangerous(IUIChangeListener listener)
        {
            Debug.Assert(!this.InEvent);
            listeners.Add(listener);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Called by the music service if something has changed on their end
        public void MusicServiceChange()
        {
            this.InformSomethingChanged();
        }
    }

    /// <summary>
    /// Pull update observer gets data from pulls.
    /// </summary>
    public interface IPullUpdateObserver
    {
        void OnNewPullData(PullResponse data);
    }
}

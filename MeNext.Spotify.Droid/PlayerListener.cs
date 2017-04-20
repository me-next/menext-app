using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Util;
using Com.Spotify.Sdk.Android.Authentication;
using Com.Spotify.Sdk.Android.Player;
using Java.Lang;

namespace MeNext.Spotify.Droid
{
    // Based on https://github.com/spotify/android-sdk/blob/master/samples/DemoProject/src/main/java/com/spotify/sdk/demo/DemoActivity.java
    public class PlayerListener : Java.Lang.Object, IPlayerNotificationCallback, IConnectionStateCallback
    {
        public SpotifyPlayer Player;

        protected SpotifyMusicServiceDroid sms;
        protected BroadcastReceiver networkStateReceiver;
        public OperationCallback operationCallback;
        private Context context;

        /// <summary>
        /// Request code that will be passed together with authentication result to the onAuthenticationResult.
        /// Just a random uniquish number.
        /// </summary>
        private const int REQUEST_CODE = 43594;

        public PlayerListener(SpotifyMusicServiceDroid sms, Context context)
        {
            this.sms = sms;
            this.context = context;
            this.operationCallback = new OperationCallback(this);
        }

        // == Initialisation == //

        // TODO Create a listener type for this
        public void OnResume()
        {
            // Set up the broadcast receiver for network events. Note that we also unregister
            // this receiver again in onPause().

            networkStateReceiver = new NetworkStateWatcher(this);

            var filter = new IntentFilter(ConnectivityManager.ConnectivityAction);
            this.sms.mainActivity.RegisterReceiver(networkStateReceiver, filter);

            if (this.Player != null) {
                this.Player.AddNotificationCallback(this);
                this.Player.AddConnectionStateCallback(this);
            }

            this.sms.SomethingChanged();
        }

        // == Authentication == //

        // scopes: https://developer.spotify.com/web-api/using-scopes/
        // TODO Combine scope usage across platforms?

        /// <summary>
        /// Opens the Spotify login window.
        /// </summary>
        public void OpenLoginWindow()
        {
            var request = new AuthenticationRequest.Builder(sms.ClientId, AuthenticationResponse.Type.Code, sms.SpotifyCallback)
                .SetScopes(new string[] {
                    "streaming",
                    "playlist-read-private",
                    "playlist-read-collaborative",
                    "user-library-read",
                    "user-read-private",
                    "user-read-email" })
                .Build();

            AuthenticationClient.OpenLoginActivity(this.sms.mainActivity, REQUEST_CODE, request);
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_CODE) {
                var response = AuthenticationClient.GetResponse((int) resultCode, data);
                var type = response.GetType();
                if (type == AuthenticationResponse.Type.Code) {
                    OnAuthenticationComplete(response);
                } else if (type == AuthenticationResponse.Type.Error) {
                    // TODO: Show err message?
                    Log.Debug("PlayerListener", "Auth error: " + response.Error);
                } else {
                    // Most likely auth flow was cancelled
                    Log.Debug("PlayerListener", "Auth result: " + response.GetType());
                }
            }
            this.sms.SomethingChanged();
        }

        /// <summary>
        /// Called when authentication has completed.
        /// </summary>
        /// <param name="response">Response.</param>
        void OnAuthenticationComplete(AuthenticationResponse response)
        {
            // Once we have obtained an authorization token, we can proceed with creating a Player.
            Log.Debug("PlayerListener", "Got code");
            this.sms.SpotifyToken.UpdateTokens(response.Code);
            var accessToken = this.sms.SpotifyToken.AccessToken;

            if (this.Player == null) {
                var playerConfig = new Com.Spotify.Sdk.Android.Player.Config(
                    this.sms.mainActivity.ApplicationContext, accessToken, this.sms.ClientId
                );
                // TODO Same issue as iOS w/ short songs
                playerConfig.UseCache(false);

                // Since the Player is a static singleton owned by the Spotify class, we pass "this" as
                // the second argument in order to refcount it properly. Note that the method
                // Spotify.destroyPlayer() also takes an Object argument, which must be the same as the
                // one passed in here. If you pass different instances to Spotify.getPlayer() and
                // Spotify.destroyPlayer(), that will definitely result in resource leaks.
                this.Player = Com.Spotify.Sdk.Android.Player.Spotify.GetPlayer(playerConfig, this, new PlayerInitObserver(this));
            } else {
                this.Player.Login(accessToken);
            }

            this.sms.SomethingChanged();
        }

        public void SetVolume(double vol)
        {
            // TODO Instead of setting system volume, set stream volume
            AudioManager audio = (AudioManager) this.context.GetSystemService(Context.AudioService);
            var maxVol = audio.GetStreamMaxVolume(Stream.Music);
            audio.SetStreamVolume(Stream.Music, (int) (vol * maxVol), 0);
        }

        // == Callback methods == //

        /// <summary>
        /// Update when logging in.
        /// </summary>
        public void OnLoggedIn()
        {
            Log.Debug("PlayerListener", "Login complete");

            // Let the sms know what our new auth token is
            var accessToken = this.sms.SpotifyToken.AccessToken;
            this.sms.OnNewAccessToken(accessToken);

            // Causes a delayed song play (if one exists) to play during a token refresh
            // Does nothing if this is just a normal login
            this.sms.ActuallyPlaySong();

            this.sms.SomethingChanged();
        }

        /// <summary>
        /// Update when logging out
        /// </summary>
        public void OnLoggedOut()
        {
            Log.Debug("PlayerListener", "Logout complete");
            if (this.sms.DelayingSongPlay) {
                // If we are delaying a song play, then we are in the midst of a token refresh and should log back in
                this.Player.Login(this.sms.SpotifyToken.AccessToken);
            }
            this.sms.SomethingChanged();
        }

        /// <summary>
        /// Update on login failure
        /// </summary>
        /// <param name="error">Error.</param>
        public void OnLoginFailed(Com.Spotify.Sdk.Android.Player.Error error)
        {
            Log.Debug("PlayerListener", "*** Login error: " + error);
            this.sms.SomethingChanged();
        }

        /// <summary>
        /// Update for temporary errors
        /// </summary>
        public void OnTemporaryError()
        {
            Log.Debug("PlayerListener", "*** Temporary error (what does that mean??)");
            this.sms.SomethingChanged();
        }

        public void OnConnectionMessage(string msg)
        {
            Log.Debug("PlayerListener", "Incomming connection msg: " + msg);
        }

        // == Destruction == //

        internal void OnPause()
        {
            this.sms.mainActivity.UnregisterReceiver(this.networkStateReceiver);

            // Note that calling Spotify.destroyPlayer() will also remove any callbacks on whatever
            // instance was passed as the refcounted owner. So in the case of this particular example,
            // it's not strictly necessary to call these methods, however it is generally good practice
            // and also will prevent your application from doing extra work in the ba when
            // paused.
            if (this.Player != null) {
                this.Player.RemoveNotificationCallback(this);
                this.Player.RemoveConnectionStateCallback(this);
            }

            this.sms.SomethingChanged();
        }

        internal void OnDestroy()
        {
            // *** ULTRA-IMPORTANT ***
            // ALWAYS call this in your onDestroy() method, otherwise you will leak native resources!
            // This is an unfortunate necessity due to the different memory management models of
            // Java's garbage collector and C++ RAII.
            // For more information, see the documentation on Spotify.destroyPlayer().
            Com.Spotify.Sdk.Android.Player.Spotify.DestroyPlayer(this);
        }

        public void OnPlaybackEvent(PlayerEvent pEvent)
        {
            // Remember kids, always use the English locale when changing case for non-UI strings!
            // Otherwise you'll end up with mysterious errors when running in the Turkish locale.
            // See: http://java.sys-con.com/node/46241


            if (pEvent == PlayerEvent.KSpPlaybackNotifyTrackDelivered) {
                // TODO This happens slightly before the song ends, so do fix
                Log.Debug("PlayerListener", "Track Delivered");
                var endingSong = this.sms.PlayingSong.UniqueId;
                this.sms.SongEnds(endingSong);
            }

            Log.Debug("PlayerListener", "Playback Event: " + pEvent);
            this.sms.SomethingChanged();

        }

        public void OnPlaybackError(Com.Spotify.Sdk.Android.Player.Error error)
        {
            Log.Debug("PlayerListener", "*** Playback error: " + error);
            this.sms.SomethingChanged();
        }

        /// <summary>
		/// Registering for connectivity changes in Android does not actually deliver them to us in the delivered intent.
        /// </summary>
        /// <returns>Connectivity state to be passed to the SDK</returns>
        /// <param name="context">Android context</param>
        private Connectivity GetNetworkConnectivity(Context context)
        {
            var connectivityManager = (ConnectivityManager) context.GetSystemService(Context.ConnectivityService);
            var activeNetwork = connectivityManager.ActiveNetworkInfo;
            if (activeNetwork != null && activeNetwork.IsConnected) {
                return Connectivity.FromNetworkType((int) activeNetwork.Type);
            } else {
                return Connectivity.Offline;
            }
        }

        public class PlayerInitObserver : Java.Lang.Object, SpotifyPlayer.IInitializationObserver
        {
            private readonly PlayerListener listener;

            public PlayerInitObserver(PlayerListener listener)
            {
                this.listener = listener;
            }

            public void OnInitialized(SpotifyPlayer p0)
            {
                Log.Debug("PlayerInitObserver", "Player initialialised");
                var player = this.listener.Player;
                player.SetConnectivityStatus(this.listener.operationCallback,
                                             this.listener.GetNetworkConnectivity(this.listener.sms.mainActivity));
                player.AddNotificationCallback(this.listener);
                player.AddConnectionStateCallback(this.listener);
                this.listener.sms.SomethingChanged();
            }

            public void OnError(Throwable error)
            {
                Log.Debug("PlayerInitObserver", "*** Error: " + error.Message);
                this.listener.sms.SomethingChanged();
            }
        }

        public class OperationCallback : Java.Lang.Object, IPlayerOperationCallback
        {
            private readonly PlayerListener listener;

            public OperationCallback(PlayerListener listener)
            {
                this.listener = listener;
            }

            public void OnError(Com.Spotify.Sdk.Android.Player.Error error)
            {
                Log.Debug("OperationCallback", "*** Error: " + error);
                this.listener.sms.SomethingChanged();
            }

            public void OnSuccess()
            {
                Log.Debug("OperationCallback", "OK!");
                this.listener.sms.SomethingChanged();
            }
        }

        public class NetworkStateWatcher : BroadcastReceiver
        {
            private readonly PlayerListener listener;

            public NetworkStateWatcher(PlayerListener listener)
            {
                this.listener = listener;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var player = this.listener.Player;
                if (player != null) {
                    var baseContext = this.listener.sms.mainActivity.BaseContext;
                    var connectivity = this.listener.GetNetworkConnectivity(baseContext);
                    Log.Debug("NetworkStateWatcher", "Network state changed: " + connectivity);
                    player.SetConnectivityStatus(this.listener.operationCallback, connectivity);
                }
                this.listener.sms.SomethingChanged();
            }
        }
    }
}

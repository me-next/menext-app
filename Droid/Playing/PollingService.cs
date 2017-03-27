using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;

namespace MeNext.Droid
{
    /// <summary>
    /// Class which handles background polling for Android
    /// </summary>
    [Service]
    public class PollingService : Service
    {
        private CancellationTokenSource cancelToken;

        /// <summary>
        /// Begins polling
        /// </summary>
        /// <returns></returns>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            cancelToken = new CancellationTokenSource();

            Task.Run(() =>
            {
                try {
                    // Invoke shared code
                    // TODO: get the music service from the main app
                    var poller = new Poller(null);
                    poller.PollForever(cancelToken.Token).Wait();
                } catch (System.OperationCanceledException) {
                } finally {
                    if (cancelToken.IsCancellationRequested) {
                        Device.BeginInvokeOnMainThread(
                            () => MessagingCenter.Send(new CancelledMessage(), "CancelledMessage")
                        );
                    }
                }
            }, cancelToken.Token);

            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Stops polling
        /// </summary>
        public override void OnDestroy()
        {
            if (cancelToken != null) {
                cancelToken.Token.ThrowIfCancellationRequested();

                cancelToken.Cancel();
            }
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}

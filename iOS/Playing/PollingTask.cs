using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace MeNext.iOS
{
	/// <summary>
	/// Class which handles background polling from iOS
	/// </summary>
	public class PollingTask
	{
		private nint taskId;
		private CancellationTokenSource cancelToken;

		/// <summary>
		/// Begins polling
		/// </summary>
		/// <returns>The async.</returns>
		public async Task StartAsync()
		{
			cancelToken = new CancellationTokenSource();

			taskId = UIApplication.SharedApplication.BeginBackgroundTask("PollingTask", OnExpiration);

			try
			{
				// Invoke shared code
				var poller = new Poller();
				await poller.PollForever(cancelToken.Token);
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				if (cancelToken.IsCancellationRequested)
				{
					Device.BeginInvokeOnMainThread(
						() => MessagingCenter.Send(new CancelledMessage(), "CancelledMessage")
					);
				}
			}

			UIApplication.SharedApplication.EndBackgroundTask(taskId);
		}

		/// <summary>
		/// Stops polling
		/// </summary>
		public void Stop()
		{
			cancelToken.Cancel();
		}

		void OnExpiration()
		{
			cancelToken.Cancel();
		}
	}
}

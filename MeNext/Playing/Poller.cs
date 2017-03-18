﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeNext
{
	public class Poller
	{
		private const int MS_PER_POLL = 200;

		/// <summary>
		/// Polls the server until we stop polling
		/// 
		/// https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/messaging-center/
		/// 
		/// Also see:
		/// http://stackoverflow.com/questions/17119075/do-you-have-to-put-task-run-in-a-method-to-make-it-async
		/// 
		/// 
		/// </summary>
		/// <returns>The forever.</returns>
		/// <param name="token">Token.</param>
		public async Task PollForever(CancellationToken token)
		{
			// TODO: Disable polling when the app goes to background and we are not the host?

			// TODO: Fix this warning?
			await Task.Run(async () =>
			{
				long i = 0;
				for (;;)
				{
					++i;
					token.ThrowIfCancellationRequested();

					await Task.Delay(MS_PER_POLL);

					// TODO: Obtain a real status message
					var message = new StatusMessage
					{
						TestingText = "Test: " + i,
						EventActive = true,
						ChangeId = i
					};

					Device.BeginInvokeOnMainThread(() =>
					{
						MessagingCenter.Send<StatusMessage>(message, "StatusMessage");
					});
				}
			}, token);
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Net.Http;

namespace MeNext
{
    /// <summary>
    /// Handles polling the backend server
    /// </summary>
    public class Poller
    {
        public MainController mainController;
        private const int MS_PER_POLL = 200;

        public Poller(MainController mainController)
        {
            this.mainController = mainController;
        }

        /// <summary>
        /// Polls the server until we stop polling
        /// 
        /// https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/messaging-center/
        /// 
        /// Also see:
        /// http://stackoverflow.com/questions/17119075/do-you-have-to-put-task-run-in-a-method-to-make-it-async
        /// </summary>
        /// <returns>The forever.</returns>
        /// <param name="token">Token.</param>
        public async Task PollForever(CancellationToken token)
        {
            // TODO: Disable polling when the app goes to background and we are not the host?

            await Task.Run(async () =>
            {
                for (;;) {
                    token.ThrowIfCancellationRequested();

                    mainController.Event.Poll();


                    await Task.Delay(MS_PER_POLL);
                }
            }, token);
        }
    }
}

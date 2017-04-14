using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using MediaPlayer;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MeNext.Layout.FullWrapperView), typeof(MeNext.iOS.NowPlayingRenderer))]

namespace MeNext.iOS
{
    public class NowPlayingRenderer : Xamarin.Forms.Platform.iOS.NavigationRenderer
    {
        public NowPlayingRenderer()
        {
            if (MPNowPlayingInfoCenter.DefaultCenter == null) {
                Debug.WriteLine("NULL BEFORE EVERYTHING");
            } else {
                Debug.WriteLine("NOT NULL BEFORE EVERYTHING");
            }

            NSError error;
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            AVAudioSession.SharedInstance().SetActive(true, out error);
            if (error != null) {
                Debug.WriteLine("*** ERROR SETTING ACTIVE: " + error.Description);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            Debug.WriteLine("Registered for remote control");


            UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();

            this.BecomeFirstResponder();
        }

        public override void RemoteControlReceived(UIEvent theEvent)
        {
            Debug.WriteLine("Recvd remote control thingy");

            base.RemoteControlReceived(theEvent);
        }
    }
}
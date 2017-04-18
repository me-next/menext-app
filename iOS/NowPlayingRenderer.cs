using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using MediaPlayer;
using MeNext.Spotify.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MeNext.Layout.FullWrapperView), typeof(MeNext.iOS.NowPlayingRenderer))]

namespace MeNext.iOS
{
    /// <summary>
    /// This class handles interfacing with the media remote control (ex when you swipe up). It needs to be a native
    /// view, hence being a renderer.
    /// </summary>
    public class NowPlayingRenderer : NavigationRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.iOS.NowPlayingRenderer"/> class. Registers itself as
        /// the remote control responder.
        /// </summary>
        public NowPlayingRenderer()
        {
            AVAudioSession.SharedInstance().WeakDelegate = this;
            this.BecomeFirstResponder();
        }

        // We need to do this otherwise becoming first responder will fail
        public override bool CanBecomeFirstResponder
        {
            get
            {
                return true;
            }
        }

        // Called when we recv a remote event
        public override void RemoteControlReceived(UIEvent theEvent)
        {
            // Pass the message on to the app
            AppDelegate.RemoteControlReceived(theEvent);

            base.RemoteControlReceived(theEvent);
        }
    }
}
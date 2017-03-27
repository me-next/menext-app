using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace MeNext.Spotify.iOS.Playback
{
    [Native]
    public enum SpPlaybackEvent : long
    {
    	NotifyPlay,
    	NotifyPause,
    	NotifyTrackChanged,
    	NotifyNext,
    	NotifyPrev,
    	NotifyShuffleOn,
    	NotifyShuffleOff,
    	NotifyRepeatOn,
    	NotifyRepeatOff,
    	NotifyBecameActive,
    	NotifyBecameInactive,
    	NotifyLostPermission,
    	EventAudioFlush,
    	NotifyAudioDeliveryDone,
    	NotifyContextChanged,
    	NotifyTrackDelivered,
    	NotifyMetadataChanged
    }

    [Native]
    public enum SpErrorCode : long
    {
    	ErrorOk = 0,
    	ErrorFailed,
    	ErrorInitFailed,
    	ErrorWrongAPIVersion,
    	ErrorNullArgument,
    	ErrorInvalidArgument,
    	ErrorUninitialized,
    	ErrorAlreadyInitialized,
    	ErrorLoginBadCredentials,
    	ErrorNeedsPremium,
    	ErrorTravelRestriction,
    	ErrorApplicationBanned,
    	ErrorGeneralLoginError,
    	ErrorUnsupported,
    	ErrorNotActiveDevice,
    	ErrorAPIRateLimited,
    	ErrorPlaybackErrorStart = 1000,
    	ErrorGeneralPlaybackError,
    	ErrorPlaybackRateLimited,
    	ErrorPlaybackCappingLimitReached,
    	ErrorAdIsPlaying,
    	ErrorCorruptTrack,
    	ErrorContextFailed,
    	ErrorPrefetchItemUnavailable,
    	AlreadyPrefetching,
    	StorageWriteError,
    	PrefetchDownloadFailed
    }

    static class CFunctions
    {
    	// extern NSError * NSErrorFromSPErrorCode (SpErrorCode code);
    	//[DllImport("__Internal")]
    	//[Verify(PlatformInvoke)]
    	//static extern NSError NSErrorFromSPErrorCode(SpErrorCode code);
    }

    [Native]
    public enum SPTBitrate : long
    {
    	Low = 0,
    	Normal = 1,
    	High = 2
    }

    [Native]
    public enum SPTRepeatMode : long
    {
    	Off = 0,
    	Context = 1,
    	One = 2
    }
}

using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace MeNext.Spotify.iOS.Playback
{
    //[Static]
    //[Verify(ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern double SpotifyAudioPlaybackVersionNumber;
        [Field("SpotifyAudioPlaybackVersionNumber", "__Internal")]
        double SpotifyAudioPlaybackVersionNumber { get; }

        // extern const unsigned char [] SpotifyAudioPlaybackVersionString;
        //[Field("SpotifyAudioPlaybackVersionString", "__Internal")]
        //byte[] SpotifyAudioPlaybackVersionString { get; }
    }

    // @interface SPTCircularBuffer : NSObject
    [BaseType(typeof(NSObject))]
    interface SPTCircularBuffer
    {
        // -(id)initWithMaximumLength:(NSUInteger)size;
        [Export("initWithMaximumLength:")]
        IntPtr Constructor(nuint size);

        // -(void)clear;
        [Export("clear")]
        void Clear();

        // -(NSUInteger)attemptAppendData:(const void *)data ofLength:(NSUInteger)dataLength;
        //[Export("attemptAppendData:ofLength:")]
        //unsafe nuint AttemptAppendData(void* data, nuint dataLength);

        // -(NSUInteger)attemptAppendData:(const void *)data ofLength:(NSUInteger)dataLength chunkSize:(NSUInteger)chunkSize;
        //[Export("attemptAppendData:ofLength:chunkSize:")]
        //unsafe nuint AttemptAppendData(void* data, nuint dataLength, nuint chunkSize);

        // -(NSUInteger)readDataOfLength:(NSUInteger)desiredLength intoAllocatedBuffer:(void **)outBuffer;
        //[Export("readDataOfLength:intoAllocatedBuffer:")]
        //unsafe nuint ReadDataOfLength(nuint desiredLength, void** outBuffer);

        // @property (readonly) NSUInteger length;
        [Export("length")]
        nuint Length { get; }

        // @property (readonly, nonatomic) NSUInteger maximumLength;
        [Export("maximumLength")]
        nuint MaximumLength { get; }
    }

    // @protocol SPTCoreAudioControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SPTCoreAudioControllerDelegate
    {
        // @optional -(void)coreAudioController:(SPTCoreAudioController *)controller didOutputAudioOfDuration:(NSTimeInterval)audioDuration;
        [Export("coreAudioController:didOutputAudioOfDuration:")]
        void DidOutputAudioOfDuration(SPTCoreAudioController controller, double audioDuration);
    }

    // @interface SPTCoreAudioController : NSObject
    [BaseType(typeof(NSObject))]
    interface SPTCoreAudioController
    {
        // -(void)clearAudioBuffers;
        [Export("clearAudioBuffers")]
        void ClearAudioBuffers();

        // -(NSInteger)attemptToDeliverAudioFrames:(const void *)audioFrames ofCount:(NSInteger)frameCount streamDescription:(AudioStreamBasicDescription)audioDescription;
        //[Export("attemptToDeliverAudioFrames:ofCount:streamDescription:")]
        //unsafe nint AttemptToDeliverAudioFrames(void* audioFrames, nint frameCount, AudioStreamBasicDescription audioDescription);

        // -(uint32_t)bytesInAudioBuffer;
        [Export("bytesInAudioBuffer")]
        //[Verify(MethodToProperty)]
        uint BytesInAudioBuffer { get; }

        // -(BOOL)connectOutputBus:(UInt32)sourceOutputBusNumber ofNode:(AUNode)sourceNode toInputBus:(UInt32)destinationInputBusNumber ofNode:(AUNode)destinationNode inGraph:(AUGraph)graph error:(NSError **)error;
        //[Export("connectOutputBus:ofNode:toInputBus:ofNode:inGraph:error:")]
        //unsafe bool ConnectOutputBus(uint sourceOutputBusNumber, int sourceNode, uint destinationInputBusNumber, int destinationNode, AUGraph* graph, out NSError error);

        // -(void)disposeOfCustomNodesInGraph:(AUGraph)graph;
        //[Export("disposeOfCustomNodesInGraph:")]
        //unsafe void DisposeOfCustomNodesInGraph(AUGraph* graph);

        // @property (readwrite, nonatomic) double volume;
        [Export("volume")]
        double Volume { get; set; }

        // @property (readwrite, nonatomic) BOOL audioOutputEnabled;
        [Export("audioOutputEnabled")]
        bool AudioOutputEnabled { get; set; }

        [Wrap("WeakDelegate")]
        SPTCoreAudioControllerDelegate Delegate { get; set; }

        // @property (readwrite, nonatomic, weak) id<SPTCoreAudioControllerDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @property (readwrite, nonatomic) UIBackgroundTaskIdentifier backgroundPlaybackTask;
        [Export("backgroundPlaybackTask")]
        nuint BackgroundPlaybackTask { get; set; }
    }

    // typedef void (^SPTErrorableOperationCallback)(NSError *);
    delegate void SPTErrorableOperationCallback(NSError arg0);

    // @protocol SPTDiskCaching <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SPTDiskCaching
    {
        // @required -(BOOL)allocateCacheWithKey:(NSString *)key size:(NSUInteger)size;
        [Abstract]
        [Export("allocateCacheWithKey:size:")]
        bool AllocateCacheWithKey(string key, nuint size);

        // @required -(NSData *)readCacheDataWithKey:(NSString *)key length:(NSUInteger)length offset:(NSUInteger)offset;
        [Abstract]
        [Export("readCacheDataWithKey:length:offset:")]
        NSData ReadCacheDataWithKey(string key, nuint length, nuint offset);

        // @required -(BOOL)writeCacheDataWithKey:(NSString *)key data:(NSData *)data offset:(NSUInteger)offset;
        [Abstract]
        [Export("writeCacheDataWithKey:data:offset:")]
        bool WriteCacheDataWithKey(string key, NSData data, nuint offset);

        // @required -(void)closeCacheWithKey:(NSString *)key;
        [Abstract]
        [Export("closeCacheWithKey:")]
        void CloseCacheWithKey(string key);
    }

    interface ISPTDiskCaching : SPTDiskCaching
    { }

    // @interface SPTDiskCache : NSObject <SPTDiskCaching>
    [BaseType(typeof(NSObject))]
    interface SPTDiskCache : ISPTDiskCaching
    {
        // -(instancetype)initWithCapacity:(NSUInteger)capacity;
        [Export("initWithCapacity:")]
        IntPtr Constructor(nuint capacity);

        // @property (readonly, nonatomic) NSUInteger capacity;
        [Export("capacity")]
        nuint Capacity { get; }
    }

    // @interface SPTPlaybackTrack : NSObject
    [BaseType(typeof(NSObject))]
    interface SPTPlaybackTrack
    {
        // @property (readonly) NSString * _Nonnull name;
        [Export("name")]
        string Name { get; }

        // @property (readonly) NSString * _Nonnull uri;
        [Export("uri")]
        string Uri { get; }

        // @property (readonly) NSString * _Nonnull playbackSourceUri;
        [Export("playbackSourceUri")]
        string PlaybackSourceUri { get; }

        // @property (readonly) NSString * _Nonnull playbackSourceName;
        [Export("playbackSourceName")]
        string PlaybackSourceName { get; }

        // @property (readonly) NSString * _Nonnull artistName;
        [Export("artistName")]
        string ArtistName { get; }

        // @property (readonly) NSString * _Nonnull artistUri;
        [Export("artistUri")]
        string ArtistUri { get; }

        // @property (readonly) NSString * _Nonnull albumName;
        [Export("albumName")]
        string AlbumName { get; }

        // @property (readonly) NSString * _Nonnull albumUri;
        [Export("albumUri")]
        string AlbumUri { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable albumCoverArtURL;
        [NullAllowed, Export("albumCoverArtURL")]
        string AlbumCoverArtURL { get; }

        // @property (readonly) NSTimeInterval duration;
        [Export("duration")]
        double Duration { get; }

        // @property (readonly) NSUInteger indexInContext;
        [Export("indexInContext")]
        nuint IndexInContext { get; }

        // -(instancetype _Nullable)initWithName:(NSString * _Nonnull)name uri:(NSString * _Nonnull)uri playbackSourceUri:(NSString * _Nonnull)playbackSourceUri playbackSourceName:(NSString * _Nonnull)playbackSourceName artistName:(NSString * _Nonnull)artistName artistUri:(NSString * _Nonnull)artistUri albumName:(NSString * _Nonnull)albumName albumUri:(NSString * _Nonnull)albumUri albumCoverArtURL:(NSString * _Nullable)albumCoverArtURL duration:(NSTimeInterval)duration indexInContext:(NSUInteger)indexInContext;
        [Export("initWithName:uri:playbackSourceUri:playbackSourceName:artistName:artistUri:albumName:albumUri:albumCoverArtURL:duration:indexInContext:")]
        IntPtr Constructor(string name, string uri, string playbackSourceUri, string playbackSourceName, string artistName, string artistUri, string albumName, string albumUri, [NullAllowed] string albumCoverArtURL, double duration, nuint indexInContext);
    }

    // @interface SPTPlaybackMetadata : NSObject
    [BaseType(typeof(NSObject))]
    interface SPTPlaybackMetadata
    {
        // @property (readonly) SPTPlaybackTrack * _Nullable prevTrack;
        [NullAllowed, Export("prevTrack")]
        SPTPlaybackTrack PrevTrack { get; }

        // @property (readonly) SPTPlaybackTrack * _Nullable currentTrack;
        [NullAllowed, Export("currentTrack")]
        SPTPlaybackTrack CurrentTrack { get; }

        // @property (readonly) SPTPlaybackTrack * _Nullable nextTrack;
        [NullAllowed, Export("nextTrack")]
        SPTPlaybackTrack NextTrack { get; }

        // -(instancetype _Nullable)initWithPrevTrack:(SPTPlaybackTrack * _Nullable)prevTrack currentTrack:(SPTPlaybackTrack * _Nullable)currentTrack nextTrack:(SPTPlaybackTrack * _Nullable)nextTrack;
        [Export("initWithPrevTrack:currentTrack:nextTrack:")]
        IntPtr Constructor([NullAllowed] SPTPlaybackTrack prevTrack, [NullAllowed] SPTPlaybackTrack currentTrack, [NullAllowed] SPTPlaybackTrack nextTrack);
    }

    // @interface SPTPlaybackState : NSObject
    [BaseType(typeof(NSObject))]
    interface SPTPlaybackState
    {
        // @property (readonly, nonatomic) BOOL isPlaying;
        [Export("isPlaying")]
        bool IsPlaying { get; }

        // @property (readonly, nonatomic) BOOL isRepeating;
        [Export("isRepeating")]
        bool IsRepeating { get; }

        // @property (readonly, nonatomic) BOOL isShuffling;
        [Export("isShuffling")]
        bool IsShuffling { get; }

        // @property (readonly, nonatomic) BOOL isActiveDevice;
        [Export("isActiveDevice")]
        bool IsActiveDevice { get; }

        // @property (readonly, nonatomic) NSTimeInterval position;
        [Export("position")]
        double Position { get; }

        // -(instancetype _Nullable)initWithIsPlaying:(BOOL)isPlaying isRepeating:(BOOL)isRepeating isShuffling:(BOOL)isShuffling isActiveDevice:(BOOL)isActiveDevice position:(NSTimeInterval)position;
        [Export("initWithIsPlaying:isRepeating:isShuffling:isActiveDevice:position:")]
        IntPtr Constructor(bool isPlaying, bool isRepeating, bool isShuffling, bool isActiveDevice, double position);
    }

    // @interface SPTAudioStreamingController : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SPTAudioStreamingController
    {
        // +(instancetype)sharedInstance;
        [Static]
        [Export("sharedInstance")]
        SPTAudioStreamingController SharedInstance();

        // -(BOOL)startWithClientId:(NSString *)clientId audioController:(SPTCoreAudioController *)audioController allowCaching:(BOOL)allowCaching error:(NSError **)error;
        [Export("startWithClientId:audioController:allowCaching:error:")]
        bool StartWithClientId(string clientId, [NullAllowed] SPTCoreAudioController audioController, bool allowCaching, out NSError error);

        // -(BOOL)startWithClientId:(NSString *)clientId error:(NSError **)error;
        [Export("startWithClientId:error:")]
        bool StartWithClientId(string clientId, out NSError error);

        // -(BOOL)stopWithError:(NSError **)error;
        [Export("stopWithError:")]
        bool StopWithError(out NSError error);

        // -(void)loginWithAccessToken:(NSString *)accessToken;
        [Export("loginWithAccessToken:")]
        void LoginWithAccessToken(string accessToken);

        // -(void)logout;
        [Export("logout")]
        void Logout();

        // @property (readonly, assign, atomic) BOOL initialized;
        [Export("initialized")]
        bool Initialized { get; }

        // @property (readonly, atomic) BOOL loggedIn;
        [Export("loggedIn")]
        bool LoggedIn { get; }

        [Wrap("WeakDelegate")]
        SPTAudioStreamingDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<SPTAudioStreamingDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        [Wrap("WeakPlaybackDelegate")]
        SPTAudioStreamingPlaybackDelegate PlaybackDelegate { get; set; }

        // @property (nonatomic, weak) id<SPTAudioStreamingPlaybackDelegate> playbackDelegate;
        [NullAllowed, Export("playbackDelegate", ArgumentSemantic.Weak)]
        NSObject WeakPlaybackDelegate { get; set; }

        // @property (nonatomic, strong) id<SPTDiskCaching> diskCache;
        [Export("diskCache", ArgumentSemantic.Strong)]
        ISPTDiskCaching DiskCache { get; set; }

        // -(void)setVolume:(SPTVolume)volume callback:(SPTErrorableOperationCallback)block;
        [Export("setVolume:callback:")]
        void SetVolume(double volume, SPTErrorableOperationCallback block);

        // -(void)setTargetBitrate:(SPTBitrate)bitrate callback:(SPTErrorableOperationCallback)block;
        [Export("setTargetBitrate:callback:")]
        void SetTargetBitrate(SPTBitrate bitrate, SPTErrorableOperationCallback block);

        // -(void)seekTo:(NSTimeInterval)position callback:(SPTErrorableOperationCallback)block;
        [Export("seekTo:callback:")]
        void SeekTo(double position, SPTErrorableOperationCallback block);

        // -(void)setIsPlaying:(BOOL)playing callback:(SPTErrorableOperationCallback)block;
        [Export("setIsPlaying:callback:")]
        void SetIsPlaying(bool playing, SPTErrorableOperationCallback block);

        // -(void)playSpotifyURI:(NSString *)spotifyUri startingWithIndex:(NSUInteger)index startingWithPosition:(NSTimeInterval)position callback:(SPTErrorableOperationCallback)block;
        [Export("playSpotifyURI:startingWithIndex:startingWithPosition:callback:")]
        void PlaySpotifyURI(string spotifyUri, nuint index, double position, SPTErrorableOperationCallback block);

        // -(void)queueSpotifyURI:(NSString *)spotifyUri callback:(SPTErrorableOperationCallback)block;
        [Export("queueSpotifyURI:callback:")]
        void QueueSpotifyURI(string spotifyUri, SPTErrorableOperationCallback block);

        // -(void)skipNext:(SPTErrorableOperationCallback)block;
        [Export("skipNext:")]
        void SkipNext(SPTErrorableOperationCallback block);

        // -(void)skipPrevious:(SPTErrorableOperationCallback)block;
        [Export("skipPrevious:")]
        void SkipPrevious(SPTErrorableOperationCallback block);

        // -(void)setShuffle:(BOOL)enable callback:(SPTErrorableOperationCallback)block;
        [Export("setShuffle:callback:")]
        void SetShuffle(bool enable, SPTErrorableOperationCallback block);

        // -(void)setRepeat:(SPTRepeatMode)mode callback:(SPTErrorableOperationCallback)block;
        [Export("setRepeat:callback:")]
        void SetRepeat(SPTRepeatMode mode, SPTErrorableOperationCallback block);

        // @property (readonly, atomic) SPTVolume volume;
        [Export("volume")]
        double Volume { get; }

        // @property (readonly, atomic) SPTPlaybackMetadata * metadata;
        [Export("metadata")]
        SPTPlaybackMetadata Metadata { get; }

        // @property (readonly, atomic) SPTPlaybackState * playbackState;
        [Export("playbackState")]
        SPTPlaybackState PlaybackState { get; }

        // @property (readonly, atomic) SPTBitrate targetBitrate;
        [Export("targetBitrate")]
        SPTBitrate TargetBitrate { get; }
    }

    // @protocol SPTAudioStreamingDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SPTAudioStreamingDelegate
    {
        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didReceiveError:(NSError *)error;
        [Export("audioStreaming:didReceiveError:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, NSError error);
        void AudioStreamingDidReceiveError(SPTAudioStreamingController audioStreaming, NSError error);

        // @optional -(void)audioStreamingDidLogin:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidLogin:")]
        void AudioStreamingDidLogin(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidLogout:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidLogout:")]
        void AudioStreamingDidLogout(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidEncounterTemporaryConnectionError:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidEncounterTemporaryConnectionError:")]
        void AudioStreamingDidEncounterTemporaryConnectionError(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didReceiveMessage:(NSString *)message;
        [Export("audioStreaming:didReceiveMessage:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, string message);
        void AudioStreamingDidReceiveMessage(SPTAudioStreamingController audioStreaming, string message);

        // @optional -(void)audioStreamingDidDisconnect:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidDisconnect:")]
        void AudioStreamingDidDisconnect(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidReconnect:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidReconnect:")]
        void AudioStreamingDidReconnect(SPTAudioStreamingController audioStreaming);
    }

    // @protocol SPTAudioStreamingPlaybackDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SPTAudioStreamingPlaybackDelegate
    {
        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didReceivePlaybackEvent:(SpPlaybackEvent)event;
        [Export("audioStreaming:didReceivePlaybackEvent:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, SpPlaybackEvent @event);
        void AudioStreamingDidReceivePlaybackEvent(SPTAudioStreamingController audioStreaming, SpPlaybackEvent @event);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangePosition:(NSTimeInterval)position;
        [Export("audioStreaming:didChangePosition:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, double position);
        void AudioStreamingDidChangePosition(SPTAudioStreamingController audioStreaming, double position);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangePlaybackStatus:(BOOL)isPlaying;
        [Export("audioStreaming:didChangePlaybackStatus:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, bool isPlaying);
        void AudioStreamingDidChangePlaybackStatus(SPTAudioStreamingController audioStreaming, bool isPlaying);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didSeekToPosition:(NSTimeInterval)position;
        [Export("audioStreaming:didSeekToPosition:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, double position);
        void AudioStreamingDidSeekToPosition(SPTAudioStreamingController audioStreaming, double position);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangeVolume:(SPTVolume)volume;
        [Export("audioStreaming:didChangeVolume:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, double volume);
        void AudioStreamingDidChangeVolume(SPTAudioStreamingController audioStreaming, double volume);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangeShuffleStatus:(BOOL)enabled;
        [Export("audioStreaming:didChangeShuffleStatus:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, bool enabled);
        void AudioStreamingDidChangeShuffleStatus(SPTAudioStreamingController audioStreaming, bool enabled);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangeRepeatStatus:(SPTRepeatMode)repeateMode;
        [Export("audioStreaming:didChangeRepeatStatus:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, SPTRepeatMode repeateMode);
        void AudioStreamingDidChangeRepeatStatus(SPTAudioStreamingController audioStreaming, SPTRepeatMode repeateMode);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didChangeMetadata:(SPTPlaybackMetadata *)metadata;
        [Export("audioStreaming:didChangeMetadata:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, SPTPlaybackMetadata metadata);
        void AudioStreamingDidChangeMetadata(SPTAudioStreamingController audioStreaming, SPTPlaybackMetadata metadata);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didStartPlayingTrack:(NSString *)trackUri;
        [Export("audioStreaming:didStartPlayingTrack:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, string trackUri);
        void AudioStreamingDidStartPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri);

        // @optional -(void)audioStreaming:(SPTAudioStreamingController *)audioStreaming didStopPlayingTrack:(NSString *)trackUri;
        [Export("audioStreaming:didStopPlayingTrack:")]
        //void AudioStreaming(SPTAudioStreamingController audioStreaming, string trackUri);
        void AudioStreamingDidStopPlayingTrack(SPTAudioStreamingController audioStreaming, string trackUri);

        // @optional -(void)audioStreamingDidSkipToNextTrack:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidSkipToNextTrack:")]
        void AudioStreamingDidSkipToNextTrack(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidSkipToPreviousTrack:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidSkipToPreviousTrack:")]
        void AudioStreamingDidSkipToPreviousTrack(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidBecomeActivePlaybackDevice:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidBecomeActivePlaybackDevice:")]
        void AudioStreamingDidBecomeActivePlaybackDevice(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidBecomeInactivePlaybackDevice:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidBecomeInactivePlaybackDevice:")]
        void AudioStreamingDidBecomeInactivePlaybackDevice(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidLosePermissionForPlayback:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidLosePermissionForPlayback:")]
        void AudioStreamingDidLosePermissionForPlayback(SPTAudioStreamingController audioStreaming);

        // @optional -(void)audioStreamingDidPopQueue:(SPTAudioStreamingController *)audioStreaming;
        [Export("audioStreamingDidPopQueue:")]
        void AudioStreamingDidPopQueue(SPTAudioStreamingController audioStreaming);
    }

    [Static]
    //[Verify(ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern const NSInteger SPTErrorCodeNoError;
        [Field("SPTErrorCodeNoError", "__Internal")]
        nint SPTErrorCodeNoError { get; }

        // extern const NSInteger SPTErrorCodeFailed;
        [Field("SPTErrorCodeFailed", "__Internal")]
        nint SPTErrorCodeFailed { get; }

        // extern const NSInteger SPTErrorCodeInitFailed;
        [Field("SPTErrorCodeInitFailed", "__Internal")]
        nint SPTErrorCodeInitFailed { get; }

        // extern const NSInteger SPTErrorCodeWrongAPIVersion;
        [Field("SPTErrorCodeWrongAPIVersion", "__Internal")]
        nint SPTErrorCodeWrongAPIVersion { get; }

        // extern const NSInteger SPTErrorCodeNullArgument;
        [Field("SPTErrorCodeNullArgument", "__Internal")]
        nint SPTErrorCodeNullArgument { get; }

        // extern const NSInteger SPTErrorCodeInvalidArgument;
        [Field("SPTErrorCodeInvalidArgument", "__Internal")]
        nint SPTErrorCodeInvalidArgument { get; }

        // extern const NSInteger SPTErrorCodeUninitialized;
        [Field("SPTErrorCodeUninitialized", "__Internal")]
        nint SPTErrorCodeUninitialized { get; }

        // extern const NSInteger SPTErrorCodeAlreadyInitialized;
        [Field("SPTErrorCodeAlreadyInitialized", "__Internal")]
        nint SPTErrorCodeAlreadyInitialized { get; }

        // extern const NSInteger SPTErrorCodeBadCredentials;
        [Field("SPTErrorCodeBadCredentials", "__Internal")]
        nint SPTErrorCodeBadCredentials { get; }

        // extern const NSInteger SPTErrorCodeNeedsPremium;
        [Field("SPTErrorCodeNeedsPremium", "__Internal")]
        nint SPTErrorCodeNeedsPremium { get; }

        // extern const NSInteger SPTErrorCodeTravelRestriction;
        [Field("SPTErrorCodeTravelRestriction", "__Internal")]
        nint SPTErrorCodeTravelRestriction { get; }

        // extern const NSInteger SPTErrorCodeApplicationBanned;
        [Field("SPTErrorCodeApplicationBanned", "__Internal")]
        nint SPTErrorCodeApplicationBanned { get; }

        // extern const NSInteger SPTErrorCodeGeneralLoginError;
        [Field("SPTErrorCodeGeneralLoginError", "__Internal")]
        nint SPTErrorCodeGeneralLoginError { get; }

        // extern const NSInteger SPTErrorCodeUnsupported;
        [Field("SPTErrorCodeUnsupported", "__Internal")]
        nint SPTErrorCodeUnsupported { get; }

        // extern const NSInteger SPTErrorCodeNotActiveDevice;
        [Field("SPTErrorCodeNotActiveDevice", "__Internal")]
        nint SPTErrorCodeNotActiveDevice { get; }

        // extern const NSInteger SPTErrorCodeGeneralPlaybackError;
        [Field("SPTErrorCodeGeneralPlaybackError", "__Internal")]
        nint SPTErrorCodeGeneralPlaybackError { get; }

        // extern const NSInteger SPTErrorCodePlaybackRateLimited;
        [Field("SPTErrorCodePlaybackRateLimited", "__Internal")]
        nint SPTErrorCodePlaybackRateLimited { get; }

        // extern const NSInteger SPTErrorCodeTrackUnavailable;
        //[Field("SPTErrorCodeTrackUnavailable", "__Internal")]
        //nint SPTErrorCodeTrackUnavailable { get; }
    }
}

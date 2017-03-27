using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using StoreKit;

namespace MeNext.Spotify.iOS.Auth
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://developer.xamarin.com/guides/ios/advanced_topics/binding_objective-c/
    //


    [Static]
    //[Verify(ConstantsInterfaceAssociation)]
    public partial interface SpotifyConstants
    {
        // extern double SpotifyAuthenticationVersionNumber;
        [Field("SpotifyAuthenticationVersionNumber", "__Internal")]
        [Static]
        double SpotifyAuthenticationVersionNumber { get; }

        // extern const unsigned char [] SpotifyAuthenticationVersionString;
        [Field("SpotifyAuthenticationVersionString", "__Internal")]
        [Static]
        IntPtr SpotifyAuthenticationVersionString { get; }

        // extern NSString *const SPTAuthStreamingScope;
        [Field("SPTAuthStreamingScope", "__Internal")]
        [Static]
        NSString SPTAuthStreamingScope { get; }

        // extern NSString *const SPTAuthPlaylistReadPrivateScope;
        [Field("SPTAuthPlaylistReadPrivateScope", "__Internal")]
        [Static]
        NSString SPTAuthPlaylistReadPrivateScope { get; }

        // extern NSString *const SPTAuthPlaylistReadCollaborativeScope;
        [Field("SPTAuthPlaylistReadCollaborativeScope", "__Internal")]
        [Static]
        NSString SPTAuthPlaylistReadCollaborativeScope { get; }

        // extern NSString *const SPTAuthPlaylistModifyPublicScope;
        [Field("SPTAuthPlaylistModifyPublicScope", "__Internal")]
        [Static]
        NSString SPTAuthPlaylistModifyPublicScope { get; }

        // extern NSString *const SPTAuthPlaylistModifyPrivateScope;
        [Field("SPTAuthPlaylistModifyPrivateScope", "__Internal")]
        [Static]
        NSString SPTAuthPlaylistModifyPrivateScope { get; }

        // extern NSString *const SPTAuthUserFollowModifyScope;
        [Field("SPTAuthUserFollowModifyScope", "__Internal")]
        [Static]
        NSString SPTAuthUserFollowModifyScope { get; }

        // extern NSString *const SPTAuthUserFollowReadScope;
        [Field("SPTAuthUserFollowReadScope", "__Internal")]
        [Static]
        NSString SPTAuthUserFollowReadScope { get; }

        // extern NSString *const SPTAuthUserLibraryReadScope;
        [Field("SPTAuthUserLibraryReadScope", "__Internal")]
        [Static]
        NSString SPTAuthUserLibraryReadScope { get; }

        // extern NSString *const SPTAuthUserLibraryModifyScope;
        [Field("SPTAuthUserLibraryModifyScope", "__Internal")]
        [Static]
        NSString SPTAuthUserLibraryModifyScope { get; }

        // extern NSString *const SPTAuthUserReadPrivateScope;
        [Field("SPTAuthUserReadPrivateScope", "__Internal")]
        [Static]
        NSString SPTAuthUserReadPrivateScope { get; }

        // extern NSString *const SPTAuthUserReadTopScope;
        [Field("SPTAuthUserReadTopScope", "__Internal")]
        [Static]
        NSString SPTAuthUserReadTopScope { get; }

        // extern NSString *const SPTAuthUserReadBirthDateScope;
        [Field("SPTAuthUserReadBirthDateScope", "__Internal")]
        [Static]
        NSString SPTAuthUserReadBirthDateScope { get; }

        // extern NSString *const SPTAuthUserReadEmailScope;
        [Field("SPTAuthUserReadEmailScope", "__Internal")]
        [Static]
        NSString SPTAuthUserReadEmailScope { get; }

        // extern NSString *const SPTAuthSessionUserDefaultsKey;
        //[Field("SPTAuthSessionUserDefaultsKey", "__Internal")]
        //NSString SPTAuthSessionUserDefaultsKey { get; }
    }

    // @interface SPTAuth : NSObject
    [BaseType(typeof(NSObject))]
    public interface SPTAuth
    {
        // +(SPTAuth *)defaultInstance;
        [Static]
        [Export("defaultInstance")]
        //[Verify(MethodToProperty)]
        SPTAuth DefaultInstance { get; }

        // @property (readwrite, strong) NSString * clientID;
        [Export("clientID", ArgumentSemantic.Strong)]
        string ClientID { get; set; }

        // @property (readwrite, strong) NSURL * redirectURL;
        [Export("redirectURL", ArgumentSemantic.Strong)]
        NSUrl RedirectURL { get; set; }

        // @property (readwrite, strong) NSArray * requestedScopes;
        [Export("requestedScopes", ArgumentSemantic.Strong)]
        //[Verify(StronglyTypedNSArray)]
        NSObject[] RequestedScopes { get; set; }

        // @property (readwrite, strong) SPTSession * session;
        [Export("session", ArgumentSemantic.Strong)]
        SPTSession Session { get; set; }

        // @property (readwrite, strong) NSString * sessionUserDefaultsKey;
        [Export("sessionUserDefaultsKey", ArgumentSemantic.Strong)]
        string SessionUserDefaultsKey { get; set; }

        // @property (readwrite, strong) NSURL * tokenSwapURL;
        [Export("tokenSwapURL", ArgumentSemantic.Strong)]
        NSUrl TokenSwapURL { get; set; }

        // @property (readwrite, strong) NSURL * tokenRefreshURL;
        [Export("tokenRefreshURL", ArgumentSemantic.Strong)]
        NSUrl TokenRefreshURL { get; set; }

        // @property (readonly) BOOL hasTokenSwapService;
        [Export("hasTokenSwapService")]
        bool HasTokenSwapService { get; }

        // @property (readonly) BOOL hasTokenRefreshService;
        [Export("hasTokenRefreshService")]
        bool HasTokenRefreshService { get; }

        // +(NSURL *)loginURLForClientId:(NSString *)clientId withRedirectURL:(NSURL *)redirectURL scopes:(NSArray *)scopes responseType:(NSString *)responseType;
        [Static]
        [Export("loginURLForClientId:withRedirectURL:scopes:responseType:")]
        //[Verify(StronglyTypedNSArray)]
        NSUrl LoginURLForClientId(string clientId, NSUrl redirectURL, NSObject[] scopes, string responseType);

        // +(NSURL *)loginURLForClientId:(NSString *)clientId withRedirectURL:(NSURL *)redirectURL scopes:(NSArray *)scopes responseType:(NSString *)responseType campaignId:(NSString *)campaignId;
        [Static]
        [Export("loginURLForClientId:withRedirectURL:scopes:responseType:campaignId:")]
        //[Verify(StronglyTypedNSArray)]
        NSUrl LoginURLForClientId(string clientId, NSUrl redirectURL, NSObject[] scopes, string responseType, string campaignId);

        // -(NSURL *)spotifyWebAuthenticationURL;
        [Export("spotifyWebAuthenticationURL")]
        //[Verify(MethodToProperty)]
        NSUrl SpotifyWebAuthenticationURL { get; }

        // -(NSURL *)spotifyAppAuthenticationURL;
        [Export("spotifyAppAuthenticationURL")]
        //[Verify(MethodToProperty)]
        NSUrl SpotifyAppAuthenticationURL { get; }

        // -(BOOL)canHandleURL:(NSURL *)callbackURL;
        [Export("canHandleURL:")]
        bool CanHandleURL(NSUrl callbackURL);

        // -(void)handleAuthCallbackWithTriggeredAuthURL:(NSURL *)url callback:(SPTAuthCallback)block;
        [Export("handleAuthCallbackWithTriggeredAuthURL:callback:")]
        void HandleAuthCallbackWithTriggeredAuthURL(NSUrl url, SPTAuthCallback block);

        // +(BOOL)supportsApplicationAuthentication;
        [Static]
        [Export("supportsApplicationAuthentication")]
        //[Verify(MethodToProperty)]
        bool SupportsApplicationAuthentication { get; }

        // +(BOOL)spotifyApplicationIsInstalled;
        [Static]
        [Export("spotifyApplicationIsInstalled")]
        //[Verify(MethodToProperty)]
        bool SpotifyApplicationIsInstalled { get; }

        // -(void)renewSession:(SPTSession *)session callback:(SPTAuthCallback)block;
        [Export("renewSession:callback:")]
        void RenewSession(SPTSession session, SPTAuthCallback block);
    }

    // typedef void (^SPTAuthCallback)(NSError *, SPTSession *);
    public delegate void SPTAuthCallback(NSError arg0, SPTSession arg1);

    // @interface SPTSession : NSObject <NSSecureCoding>
    [BaseType(typeof(NSObject))]
    public interface SPTSession : INSSecureCoding
    {
        // -(instancetype)initWithUserName:(NSString *)userName accessToken:(NSString *)accessToken expirationDate:(NSDate *)expirationDate;
        [Export("initWithUserName:accessToken:expirationDate:")]
        IntPtr Constructor(string userName, string accessToken, NSDate expirationDate);

        // -(instancetype)initWithUserName:(NSString *)userName accessToken:(NSString *)accessToken encryptedRefreshToken:(NSString *)encryptedRefreshToken expirationDate:(NSDate *)expirationDate;
        [Export("initWithUserName:accessToken:encryptedRefreshToken:expirationDate:")]
        IntPtr Constructor(string userName, string accessToken, string encryptedRefreshToken, NSDate expirationDate);

        // -(instancetype)initWithUserName:(NSString *)userName accessToken:(NSString *)accessToken expirationTimeInterval:(NSTimeInterval)timeInterval;
        [Export("initWithUserName:accessToken:expirationTimeInterval:")]
        IntPtr Constructor(string userName, string accessToken, double timeInterval);

        // -(BOOL)isValid;
        [Export("isValid")]
        //[Verify(MethodToProperty)]
        bool IsValid { get; }

        // @property (readonly, copy, nonatomic) NSString * canonicalUsername;
        [Export("canonicalUsername")]
        string CanonicalUsername { get; }

        // @property (readonly, copy, nonatomic) NSString * accessToken;
        [Export("accessToken")]
        string AccessToken { get; }

        // @property (readonly, copy, nonatomic) NSString * encryptedRefreshToken;
        [Export("encryptedRefreshToken")]
        string EncryptedRefreshToken { get; }

        // @property (readonly, copy, nonatomic) NSDate * expirationDate;
        [Export("expirationDate", ArgumentSemantic.Copy)]
        NSDate ExpirationDate { get; }

        // @property (readonly, copy, nonatomic) NSString * tokenType;
        [Export("tokenType")]
        string TokenType { get; }
    }

    // @interface SPTConnectButton : UIControl
    [BaseType(typeof(UIControl))]
    public interface SPTConnectButton
    {
    }

    // @protocol SPTAuthViewDelegate
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    public interface SPTAuthViewDelegate
    {
        // @required -(void)authenticationViewController:(SPTAuthViewController *)authenticationViewController didLoginWithSession:(SPTSession *)session;
        [Abstract]
        [Export("authenticationViewController:didLoginWithSession:")]
        void AuthenticationViewController(SPTAuthViewController authenticationViewController, SPTSession session);

        // @required -(void)authenticationViewController:(SPTAuthViewController *)authenticationViewController didFailToLogin:(NSError *)error;
        [Abstract]
        [Export("authenticationViewController:didFailToLogin:")]
        void AuthenticationViewController(SPTAuthViewController authenticationViewController, NSError error);

        // @required -(void)authenticationViewControllerDidCancelLogin:(SPTAuthViewController *)authenticationViewController;
        [Abstract]
        [Export("authenticationViewControllerDidCancelLogin:")]
        void AuthenticationViewControllerDidCancelLogin(SPTAuthViewController authenticationViewController);
    }

    // @interface SPTAuthViewController : UIViewController
    [BaseType(typeof(UIViewController))]
    public interface SPTAuthViewController
    {
        [Wrap("WeakDelegate")]
        SPTAuthViewDelegate Delegate { get; set; }

        // @property (assign, nonatomic) id<SPTAuthViewDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(SPTAuthViewController *)authenticationViewController;
        [Static]
        [Export("authenticationViewController")]
        //[Verify(MethodToProperty)]
        SPTAuthViewController AuthenticationViewController { get; }

        // +(SPTAuthViewController *)authenticationViewControllerWithAuth:(SPTAuth *)auth;
        [Static]
        [Export("authenticationViewControllerWithAuth:")]
        SPTAuthViewController AuthenticationViewControllerWithAuth(SPTAuth auth);

        // -(void)clearCookies:(void (^)())callback;
        [Export("clearCookies:")]
        void ClearCookies(Action callback);
    }

    // @interface SPTStoreViewController : SKStoreProductViewController
    [BaseType(typeof(SKStoreProductViewController))]
    [DisableDefaultCtor]
    public interface SPTStoreViewController
    {
        // @property (readonly, copy, nonatomic) NSString * campaignToken;
        [Export("campaignToken")]
        string CampaignToken { get; }

        // -(instancetype)initWithCampaignToken:(NSString *)campaignToken storeDelegate:(id<SPTStoreControllerDelegate>)storeDelegate __attribute__((objc_designated_initializer));
        [Export("initWithCampaignToken:storeDelegate:")]
        [DesignatedInitializer]
        IntPtr Constructor(string campaignToken, SPTStoreControllerDelegate storeDelegate);
    }

    // @protocol SPTStoreControllerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    public interface SPTStoreControllerDelegate
    {
        // @required -(void)productViewControllerDidFinish:(SPTStoreViewController *)viewController;
        [Abstract]
        [Export("productViewControllerDidFinish:")]
        void ProductViewControllerDidFinish(SPTStoreViewController viewController);
    }

    // @interface SPTEmbeddedImages : NSObject
    [BaseType(typeof(NSObject))]
    public interface SPTEmbeddedImages
    {
        // +(UIImage *)buttonImage;
        [Static]
        [Export("buttonImage")]
        //[Verify(MethodToProperty)]
        UIImage ButtonImage { get; }

        // +(UIImage *)closeImage;
        [Static]
        [Export("closeImage")]
        //[Verify(MethodToProperty)]
        UIImage CloseImage { get; }

        // +(UIImage *)newButtonImage;
        [Static]
        [Export("newButtonImage")]
        //[Verify(MethodToProperty)]
        UIImage NewButtonImage { get; }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using TeBSYammer.Authentication;
using Foundation;
using UIKit;
using OpenId.AppAuth;
using TeBSYammer.DependencyServices;
using TeBSYammer.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(YammerAuth))]
namespace TeBSYammer.iOS.DependencyServices
{
    public class YammerAuth : IYammerAuth
    {
        public Task Authenticate(Action<string, string> success, Action<Exception> error)
        {
            try
            {
                var clientID = $"{AppSettings.ClientID}.apps.Yammerusercontent.com";
                var redirectUrl = $"TeBSYammer://com.TebsYammer";
                var oauthYammerHandler = new YammerOAuthAuthenticationHandler(AppSettings.ClientID, AppSettings.ClientSecret, reDirectUrl: redirectUrl);
                oauthYammerHandler.Login(success, error);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                error(ex);
                return Task.FromException(ex);
            }
        }

        public bool Callback(string url)
        {
            if (url.ToLower().Contains("tebsyammer"))
            {
                // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
                Uri uri_netfx = new Uri(url);

                // load redirect_url Page
                YammerOAuthAuthenticationHandler._authenticator.OnPageLoading(uri_netfx);

                CloseBrowser();

                return true;
            }
            return false;
        }

        public Task CloseBrowser()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            return vc.DismissViewControllerAsync(false);
        }

        #region OpenID.AppAuth.Android
        //public async Task Authenticate(Action<string, string> success, Action<Exception> error)
        //{
        //    var issuer = new NSUrl("https://www.Yammer.com");
        //    var redirectURI = new NSUrl($"https://tebsyammer/");

        //    var configuration = await AuthorizationService.DiscoverServiceConfigurationForIssuerAsync(issuer);
            

        //    // builds authentication request
        //    var request = new AuthorizationRequest(configuration,
        //        AppSettings.ClientID,
        //        AppSettings.ClientSecret,
        //        null,
        //      redirectURI,
        //      ResponseType.Code,
        //      null);

        //    // performs authentication request
        //    var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

        //    appDelegate.CurrentAuthorizationFlow = AuthState.PresentAuthorizationRequest(request,
        //      GetTopViewController(),
        //      (authState, e) =>
        //    {
        //        if (authState != null)
        //        {
        //            success?.Invoke(authState.LastTokenResponse.AccessToken,"");
        //        }
        //    });
        //}

        UIViewController GetTopViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }


        #endregion
    }
}

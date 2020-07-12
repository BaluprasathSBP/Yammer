using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Tools.Authentication;
using Foundation;
using OpenId.AppAuth;
using UIKit;
using VMS.DependencyServices;
using VMS.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleAuth))]
namespace VMS.iOS.DependencyServices
{
  public class GoogleAuth : IGoogleAuth
  {
    public Task Authenticate(Action<string, string> success, Action<Exception> error)
    {
      try
      {
        var clientID = $"{AppSettings.Current.GoogleAuthIOSKey}.apps.googleusercontent.com";
        var redirectUrl = $"com.googleusercontent.apps.{AppSettings.Current.GoogleAuthIOSKey}:/oauth2redirect";
        var oauthGoogleHandler = new GoogleOAuthAuthenticationHandler(AppSettings.Current.GoogleAuthIOSKey, null, reDirectUrl: redirectUrl);
        oauthGoogleHandler.Login(success, error);
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
      if (url.Contains("com.googleusercontent.apps"))
      {
        // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
        Uri uri_netfx = new Uri(url);

        // load redirect_url Page
        GoogleOAuthAuthenticationHandler._authenticator.OnPageLoading(uri_netfx);

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
    //public async Task Authenticate(Action<string> success, Action<Exception> error)
    //{
    //  var issuer = new NSUrl("https://accounts.google.com");
    //  var redirectURI = new NSUrl($"com.googleusercontent.apps.318327418820-56q3binm9m08oc98rvcu5vvlmnappca7:/oauthredirect");

    //  var configuration = await AuthorizationService.DiscoverServiceConfigurationForIssuerAsync(issuer);

    //  // builds authentication request
    //  var request = new AuthorizationRequest(configuration,
    //    $"318327418820-56q3binm9m08oc98rvcu5vvlmnappca7.apps.googleusercontent.com",
    //    new string[] { Scope.OpenId, Scope.Profile, Scope.Email },
    //    redirectURI,
    //    ResponseType.Code,
    //    null);

    //  // performs authentication request
    //  var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

    //  appDelegate.CurrentAuthorizationFlow = AuthState.PresentAuthorizationRequest(request,
    //    GetTopViewController(),
    //    (authState, e) =>
    //  {
    //    if (authState != null)
    //    {
    //      success?.Invoke(authState.LastTokenResponse.AccessToken);
    //    }
    //  });
    //}

    //UIViewController GetTopViewController()
    //{
    //  var window = UIApplication.SharedApplication.KeyWindow;
    //  var vc = window.RootViewController;
    //  while (vc.PresentedViewController != null)
    //    vc = vc.PresentedViewController;

    //  if (vc is UINavigationController navController)
    //    vc = navController.ViewControllers.Last();

    //  return vc;
    //}

    #endregion
  }
}

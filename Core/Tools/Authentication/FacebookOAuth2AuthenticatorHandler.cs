using System;
using Xamarin.Auth;

namespace Core.Tools.Authentication
{
  public class FacebookOAuth2AuthenticatorHandler: BaseOAuthAuthenticatorHandler
  {
    Action<string, string> _successCallback;
    Action<Exception> _errorCallback;

    OAuth2Authenticator _authenticator;

    public FacebookOAuth2AuthenticatorHandler(string clientId, string scope = "email"): base(clientId, scope)
    {

    }

    public override void Login(Action<string, string> success, Action<Exception> error)
    {
      _successCallback = success;
      _errorCallback = error;

      _authenticator = null;
      _authenticator = new OAuth2Authenticator(
        ClientId,
        Scope,
        new Uri("https://www.facebook.com/dialog/oauth"),
        new Uri("https://www.facebook.com/connect/login_success.html"),
        null,
        false);

      _authenticator.Completed += (sender, e) =>
      {
        string token = null;

        if (e.Account != null && e.Account.Properties.ContainsKey("access_token"))
        {
          token = e.Account.Properties["access_token"];
        }
        _successCallback?.Invoke(token, null);
      };

      _authenticator.Error += (sender, e) =>
      {
        _errorCallback?.Invoke(e.Exception);
      };

      Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
      presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
      presenter.Login(_authenticator);
    }
  }
}

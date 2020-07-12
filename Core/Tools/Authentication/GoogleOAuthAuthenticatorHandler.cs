using System;
using Xamarin.Auth;

namespace Core.Tools.Authentication
{
  public class GoogleOAuthAuthenticationHandler : BaseOAuthAuthenticatorHandler
  {
    Action<string, string> _successCallback;
    Action<Exception> _errorCallback;

    public static OAuth2Authenticator _authenticator;
    string _token;

    public GoogleOAuthAuthenticationHandler(
      string clientId,
      string appSecret,
      string scope = "email openid profile",
      string reDirectUrl = null)
        : base(clientId, scope, null, appSecret, reDirectUrl)
    {

    }

    public override void Login(Action<string, string> success, Action<Exception> error)
    {     
      _successCallback = success;
      _errorCallback = error;

      _authenticator = null;
      _authenticator = new OAuth2Authenticator(
        ClientId,
        "",
        Scope,
        new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
        new Uri(RedirectUrl),
        new Uri("https://oauth2.googleapis.com/token"),
        isUsingNativeUI: true)
      {
        ShowErrors = true,

      };

      _authenticator.Completed += (sender, e) =>
      {
        if (e.Account != null && e.Account.Properties.ContainsKey("access_token"))
        {
          _token = e.Account.Properties["access_token"];
        }
        
        _successCallback?.Invoke(_token, null);
      };

      _authenticator.Error += (sender, e) =>
      {
        //There might be a better way in the future other than ignoring invalid_request error
        if (string.IsNullOrEmpty(_token) || !e.Message.Contains("invalid_request"))
        {
          _errorCallback?.Invoke(e.Exception);
        }
      };

      Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
      presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
      presenter.Login(_authenticator);
    }
  }
}

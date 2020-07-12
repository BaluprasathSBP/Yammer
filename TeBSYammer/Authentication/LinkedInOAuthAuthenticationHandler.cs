using System;
using Xamarin.Auth;

namespace Core.Tools.Authentication
{
  public class LinkedInOAuthAuthenticationHandler: BaseOAuthAuthenticatorHandler
  {
    Action<string, string> _successCallback;
    Action<Exception> _errorCallback;

    OAuth2Authenticator _authenticator;
    string _token;

    public LinkedInOAuthAuthenticationHandler(
      string clientId, 
      string appSecret,
      string scope = "r_liteprofile r_emailaddress") 
        : base(clientId, scope, null, appSecret)
    {
    }

    public override void Login(Action<string, string> success, Action<Exception> error)
    {
      _successCallback = success;
      _errorCallback = error;

      _authenticator = null;
      _authenticator = new OAuth2Authenticator(
        ClientId,
        ApiSecret,
        Scope,
        new Uri("https://www.linkedin.com/oauth/v2/authorization"),
        new Uri("https://www.volunteer.sg/signin-linkedin"),
        new Uri("https://www.linkedin.com/oauth/v2/accessToken"))
      {
        ShowErrors = false,
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
        //HACK There might be a better way in the future other than ignoring invalid_request error
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

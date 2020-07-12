using System;
using Xamarin.Auth;

namespace Core.Tools.Authentication
{
  public class TwitterOAuthAuthenticatorHandler: BaseOAuthAuthenticatorHandler
  {
    Action<string, string> _successCallback;
    Action<Exception> _errorCallback;

    OAuth1Authenticator _authenticator;

    public TwitterOAuthAuthenticatorHandler(string apiKey, string apiSecret) 
      : base(null, null, apiKey, apiSecret)
    {
    }

    public override void Login(Action<string, string> success, Action<Exception> error)
    {
      _successCallback = success;
      _errorCallback = error;

      _authenticator = null;
      _authenticator = new OAuth1Authenticator(
        ApiKey, 
        ApiSecret,
        new Uri("https://api.twitter.com/oauth/request_token"),
        new Uri("https://api.twitter.com/oauth/authorize"),
        new Uri("https://api.twitter.com/oauth/access_token"),
        new Uri("https://mobile.twitter.com/home"));

      _authenticator.Completed += (sender, e) =>
      {
        string token = null;
        string tokenSecret = null;

        if (e.Account != null)
        {
          if (e.Account.Properties.ContainsKey("oauth_token"))
          {
            token = e.Account.Properties["oauth_token"];
          }

          if (e.Account.Properties.ContainsKey("oauth_token_secret"))
          {
            tokenSecret = e.Account.Properties["oauth_token_secret"];
          }
        }


        _successCallback?.Invoke(token, tokenSecret);
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

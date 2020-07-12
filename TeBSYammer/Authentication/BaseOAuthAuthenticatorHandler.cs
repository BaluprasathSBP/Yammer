using System;
using Xamarin.Auth;

namespace TeBSYammer.Authentication
{
  public abstract class BaseOAuthAuthenticatorHandler : IOAuthAuthenticatorHandler
  {
    public string ClientId { get; set; }
    public string Scope { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string RedirectUrl { get; set; }

    protected BaseOAuthAuthenticatorHandler()
    {

    }

    protected BaseOAuthAuthenticatorHandler(
      string clientId,
      string scope,
      string apiKey = null,
      string apiSecret = null,
      string redirectUrl = null)
    {
      ClientId = clientId;
      Scope = scope;
      ApiKey = apiKey;
      ApiSecret = apiSecret;
      RedirectUrl = redirectUrl;
    }

    public abstract void Login(Action<string, string> success, Action<Exception> error);
  }
}

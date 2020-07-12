using System;
using Xamarin.Auth;
using Newtonsoft.Json;
using Microsoft.CSharp;

namespace TeBSYammer.Authentication
{
    public class YammerOAuthAuthenticationHandler : BaseOAuthAuthenticatorHandler
    {
        Action<string, string> _successCallback;
        Action<Exception> _errorCallback;

        public static OAuth2Authenticator _authenticator;
        string _token;

        public YammerOAuthAuthenticationHandler(
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
              ApiSecret,
              "",
              new Uri("https://www.yammer.com/oauth2/authorize"),
              new Uri(RedirectUrl),
              new Uri("https://www.yammer.com/oauth2/access_token.json"),
              isUsingNativeUI: true)
            {
                ShowErrors = true,

            };

            _authenticator.Completed += async (sender, e) =>
            {
                if (e.Account != null && e.Account.Properties.ContainsKey("access_token"))
                {
                    _token = e.Account.Properties["access_token"];
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(_token);
                    await AppProperties.AddOrUpdate("access_token", _token);
                    await AppProperties.AddOrUpdate("token", data["token"].ToString());

                    AppSettings.Token = data["token"].ToString();
                    //await AppProperties.AddOrUpdate("username", data["user_id"]);
                    //await AppProperties.AddOrUpdate("network_permalink", data["network_permalink"]);
                    //await AppProperties.AddOrUpdate("network_id", data["network_id"]);
                    //await AppProperties.AddOrUpdate("user_id", data["user_id"]);

                }
                if (e.Account != null && e.Account.Properties.ContainsKey("network"))
                {
                    var network = e.Account.Properties["network"];
                    //await AppProperties.AddOrUpdate("network", network);
                }
                if (e.Account != null && e.Account.Properties.ContainsKey("user"))
                {
                    var user = e.Account.Properties["user"];
                    //await AppProperties.AddOrUpdate("user", user);
                    //dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(user);
                    //await AppProperties.AddOrUpdate("full_name", data["full_name"]);
                    //await AppProperties.AddOrUpdate("network_domains", data["network_domains"]);
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Network.Http;

namespace Core.Network.Api
{
    public class BaseClient
    {
        protected readonly IApiContext ApiContext;
        protected readonly ITokenProvider TokenProvider;

        protected IHttpClient Client;

        protected bool AddAuthBeader { get; set; }

        protected Action UnAuthorizedEventHandler { get; set; }

        protected bool ShouldAutomaticallyGetToken { get; set; }

        protected string Token { get; set; }

        protected BaseClient(IApiContext apiContext,
                             ITokenProvider tokenProvider,
                             string baseURL = null,
                             bool addAuthBearer = false,
                             bool shouldAutomaticallyGetToken = false,
                             Action unAuthorizeAccessHandler = null,
                             string token = null)
        {
            ApiContext = apiContext;
            TokenProvider = tokenProvider;
            Client = new HttpClient(baseURL ?? apiContext.BaseURL)
            {
                UnAuthorizedEventHandler = unAuthorizeAccessHandler
            };
            AddAuthBeader = addAuthBearer;
            ShouldAutomaticallyGetToken = shouldAutomaticallyGetToken;
            Token = token;
        }

        protected async Task<T> GetAsync<T>(string resource,
                                            Dictionary<string, string> extraParams = null,
                                            Dictionary<string, string> extraHeaders = null) where T : class
        {
            var request = await CreateRequest(resource, Method.GET);

            if (extraParams != null)
            {
                foreach (var param in extraParams)
                {
                    request.AddParams(param.Key, param.Value);
                }
            }

            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            return await Client.ExecuteAsync<T>(request); ;
        }

        protected async Task<T> PostAsync<T>(string resource, object model) where T : class
        {
            var request = await CreateRequest(resource, Method.POST);

            request.AddJsonBody(model);

            return await Client.ExecuteAsync<T>(request);
        }

        protected async Task<T> PostQueryAsync<T>(string resource,
          object model = null,
          Dictionary<string, string> extraParams = null) where T : class
        {
            var request = await CreateRequest(resource, Method.POST);

            if (model != null)
            {
                request.AddJsonBody(model);
            }

            if (extraParams != null)
            {
                foreach (var param in extraParams)
                {
                    request.AddParams(param.Key, param.Value);
                }
            }

            return await Client.ExecuteAsync<T>(request);
        }

        protected async Task<T> PutAsync<T>(string resource, object model) where T : class
        {
            var request = await CreateRequest(resource, Method.PUT);

            request.AddJsonBody(model);

            return await Client.ExecuteAsync<T>(request);
        }

        protected async Task DeleteAsync(string resource)
        {
            var request = await CreateRequest(resource, Method.DELETE);

            await Client.ExecuteAsync<object>(request);
        }

        protected async Task<T> DeleteAsync<T>(string resource, object model) where T : class
        {
            var request = await CreateRequest(resource, Method.DELETE);

            request.AddJsonBody(model);

            return await Client.ExecuteAsync<T>(request);
        }

        protected async Task<T> PostFileAsync<T>(string resource,
          FileObject file,
          Dictionary<string, string> extraParams = null) where T : class
        {
            var request = await CreateRequest(resource, Method.POST);

            request.FileObject = file;

            if (extraParams != null)
            {
                foreach (var param in extraParams)
                {
                    request.AddParams(param.Key, param.Value);
                }
            }

            return await Client.ExecuteAsync<T>(request);
        }

        protected async Task<IHttpRequest> CreateRequest(string resource, Method method)
        {
            var request = new HttpRequest(resource, method);

            request.AddHeader("Authorization", $"Bearer {Token}");

            //if (AddAuthBeader)
            //{
            //  var tokenExpired = TokenProvider.Token?.Expiry.CompareTo(DateTime.UtcNow) < 0;
            //  var isNullToken = TokenProvider.Token == null || string.IsNullOrEmpty(TokenProvider.Token.AccessToken);
            //  var isTokenPublic = TokenProvider.Token?.IsPublic ?? true;

            //  if (isNullToken || tokenExpired)
            //  {
            //    Token token = null;

            //    if (ShouldAutomaticallyGetToken)
            //    {
            //      try
            //      {
            //        if (isTokenPublic || isNullToken)
            //        {
            //          token = await GetToken();
            //        }
            //        else if (!isTokenPublic && tokenExpired)
            //        {
            //          token = await RefreshToken();
            //        }
            //      }
            //      catch
            //      {
            //      }
            //    }

            //    TokenProvider.Token = token;

            //    if (token == null)
            //    {
            //      if (!isTokenPublic && tokenExpired)
            //      {
            //        UnAuthorizedEventHandler?.Invoke();
            //      }

            //      throw new NullOrExpiredTokenException("Token is null or empty");
            //    }
            //  }

            //}

            //request.AddHeader("DeviceID", ApiContext?.DeviceID ?? "");

            return request;
        }

        protected virtual Task<Token> GetToken()
        {
            throw new Exception("GetToken should be implemented");
        }

        protected virtual Task<Token> RefreshToken()
        {
            throw new Exception("RefreshToken should be implemented");
        }

        public class NullOrExpiredTokenException : Exception
        {
            public NullOrExpiredTokenException(string message) : base(message)
            {

            }
        }
    }
}

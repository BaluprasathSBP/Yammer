using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Core.Tools;
using Newtonsoft.Json;

namespace Core.Network.Http
{
  public class HttpClient: IHttpClient
  {
    public HttpClient()
    {
    }

    public HttpClient(string baseURL)
    {
      BaseURL = baseURL;
    }

    public string BaseURL { get; set; }

    public Action UnAuthorizedEventHandler { get; set; }

    public async Task<T> ExecuteAsync<T>(IHttpRequest restRequest) where T: class
    {
     return await InternalExecuteAsyncWithRetry<T>(restRequest);
    }

    public Task<byte[]> DownloadAsync(IHttpRequest restReques)
    {
      return InternalExecuteAsyncWithRetry<byte[]>(restReques);
    }

    async Task<T> InternalExecuteAsyncWithRetry<T>(IHttpRequest restRequest) where T : class
    {
      T result = default(T);

      for (int i = 1; i <= restRequest.MaxRetries; i++)
      {
        try
        {
          result = await InternalExecuteAsync<T>(restRequest);

          break;
        }
        catch (Exception e)
        {
          if (i == restRequest.MaxRetries)
          {
            throw e;
          }
          Thread.Sleep(100);
        }
      }
      return result;
    }

    async Task<T> InternalExecuteAsync<T>(IHttpRequest restRequest) where T : class
    {
      var finalUri = BuildUri(restRequest);

#if DEBUG
      Console.WriteLine($"ExecuteAsync: {restRequest.Method}: {finalUri}");
      Console.WriteLine($"Request:{Environment.NewLine}{restRequest}");
#endif
      object data = default(T);

      var provider = Xamarin.Forms.DependencyService.Get<IHttpClientProvider>();

      var client = new System.Net.Http.HttpClient(new HttpClientHandler())
      {
        Timeout = TimeSpan.FromMinutes(1.5),
        DefaultRequestHeaders =
        {
          CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate"),
          Pragma = { NameValueHeaderValue.Parse("no-cache")}
        }
      };

      var request = new HttpRequestMessage(restRequest.Method.ToHttpMethod(), finalUri);
      request.Headers.CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate");

      try
      {

        request.Content = GetContent(restRequest);

        foreach (var header in restRequest.Headers)
        {
          request.Headers.Add(header.Key, header.Value);
        }

        var response = await client.SendAsync(request);

        var content = "";

        if (response.IsSuccessStatusCode)
        {
          if (typeof(T) == typeof(FileObject))
          {
            var file = new FileObject
            {
              Data = await response.Content.ReadAsByteArrayAsync(),
              FileName = response.Content.Headers.ContentDisposition.FileName.Trim('"')
            };

            data = file;
          }
          else
          {
            content = await response.Content.ReadAsStringAsync();

#if DEBUG
            Console.WriteLine($"{response.StatusCode} : {content}");
#endif

            data = (typeof(T).IsPrimitive || typeof(T) == typeof(string)) ?
                content as T
                  : JsonConvert.DeserializeObject<T>(
                    content,
                    new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
          }
        }
        else
        {
          if (response.StatusCode == HttpStatusCode.Unauthorized)
          {
            UnAuthorizedEventHandler?.Invoke();
          }
          else
          {
            content = await response.Content.ReadAsStringAsync();
          }
          throw new HttpServerException(response.StatusCode, content);
        }
      }
      catch (HttpServerException e)
      {
        throw new HttpServerException(e.StatusCode, e.Message);
      }
      catch (Exception e)
      {
#if DEBUG
        Console.WriteLine($"{e.Message} : {e.StackTrace}");
#endif

        throw new HttpConnectionException();
      }

      return (T)data;
    }

    #region

    HttpContent GetContent(IHttpRequest restRequest)
    {
      HttpContent content = null;

      if (restRequest.FileObject != null)
      {
        var multipartContent = new MultipartFormDataContent();

        var file = restRequest.FileObject;

        file.Stream.Position = 0;
        file.Stream.Seek(0, SeekOrigin.Begin);

        var streamContent = new StreamContent(file.Stream);

        streamContent.Headers.ContentDisposition
         = ContentDispositionHeaderValue.Parse("form-data");

        streamContent
          .Headers
          .ContentDisposition
          .Parameters
          .Add(new NameValueHeaderValue("name", "contentFile"));

        var filename = HttpUtility.UrlEncode(string.Join("", file.FileName.Split(Path.GetInvalidFileNameChars())));

        streamContent
          .Headers
          .ContentDisposition
          .Parameters
          .Add(new NameValueHeaderValue("filename", "\"" + filename + "\""));

        multipartContent.Add(streamContent);

        content = multipartContent;
      }
      else if (restRequest.RequestBody != null)
      {
        content = new StringContent(restRequest.RequestBody, Encoding.UTF8, "application/json");
      }

      return content;
    }

    Uri BuildUri(IHttpRequest request)
    {
      UriBuilder uriBuilder = new UriBuilder($"{BaseURL}{ReplaceUrlSegments(request)}");

      StringBuilder query = new StringBuilder();

      foreach (var param in request.Params)
      {
        if (query.Length > 0)
        {
          query.Append("&");
        }
        query.Append($"{param.Key}={param.Value}");
      }
      uriBuilder.Query = query.ToString();

      return new Uri(uriBuilder.ToString());
    }

    string ReplaceUrlSegments(IHttpRequest request)
    {
      var resourceUrl = request.Resource;

      foreach (var segment in request.UrlSegments)
      {
        resourceUrl = resourceUrl.Replace(segment.Key, segment.Value);
      }
      return resourceUrl;
    }

    #endregion
  }
}

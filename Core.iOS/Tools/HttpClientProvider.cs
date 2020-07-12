using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.iOS.Tools;
using Core.Tools;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientProvider))]
namespace Core.iOS.Tools
{
  public class HttpClientProvider : IHttpClientProvider
  {
    public HttpClient Client(TimeSpan? timeout = null)
    {
      var configuration = NSUrlSessionConfiguration.DefaultSessionConfiguration;
      configuration.TimeoutIntervalForRequest = timeout?.Seconds ?? 100;
      var handler = new NSUrlSessionHandler(configuration)
      {
        DisableCaching = true
      };
      return new HttpClient(handler)
      {
        DefaultRequestHeaders =
        {
          CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate"),
          Pragma = { NameValueHeaderValue.Parse("no-cache")}
        }
      };
    }
  }
}

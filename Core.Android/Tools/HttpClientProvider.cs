using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Droid.Tools;
using Core.Tools;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientProvider))]
namespace Core.Droid.Tools
{
  public class HttpClientProvider : IHttpClientProvider
  {
    public HttpClient Client(TimeSpan? timeout = null)
    {
      return new HttpClient
      {
        Timeout = timeout ?? TimeSpan.FromSeconds(100),
        DefaultRequestHeaders =
        {
          CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate"),
          Pragma = { NameValueHeaderValue.Parse("no-cache")}
        }
      };
    }
  }
}

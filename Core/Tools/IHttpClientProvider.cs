using System;
using System.Net.Http;

namespace Core.Tools
{
  public interface IHttpClientProvider
  {
    HttpClient Client(TimeSpan? timeout = null);
  }
}

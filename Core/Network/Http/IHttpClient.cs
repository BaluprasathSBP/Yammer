using System;
using System.Threading.Tasks;

namespace Core.Network.Http
{
  public interface IHttpClient
  {
    string BaseURL { get; set; }

    Action UnAuthorizedEventHandler { get; set; }

    Task<T> ExecuteAsync<T>(IHttpRequest restRequest) where T : class;

    Task<byte[]> DownloadAsync(IHttpRequest restReques);
  }
}

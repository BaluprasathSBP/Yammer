using System.Collections.Generic;

namespace Core.Network.Http
{
  public interface IHttpRequest
  {
    string Resource { get; set; }

    Method Method { get; set; }

    string RequestBody { get; set; }

    string ContentType { get; set; }

    int MaxRetries { get; set; }

    IDictionary<string, string> Headers { get; set; }

    IDictionary<string, string> UrlSegments { get; set; }

    IDictionary<string, string> Params { get; set; }

    IHttpRequest AddJsonBody(object obj);

    IHttpRequest AddBody(string body);

    IHttpRequest AddHeader(string key, string value);

    IHttpRequest AddUrlSegment(string key, string value);

    IHttpRequest AddParams(string key, string value);

    FileObject FileObject { get; set; }
  }
}

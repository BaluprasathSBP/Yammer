using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Core.Network.Http
{
  public class HttpRequest: IHttpRequest
  {
    #region IRequest Properties

    public string Resource { get; set; }

    public Method Method { get; set; }

    public string RequestBody { get; set; }

    public string ContentType { get; set; }

    public int MaxRetries { get; set; } = 3;

    public IDictionary<string, string> Headers { get; set; }

    public IDictionary<string, string> UrlSegments { get; set; }

    public IDictionary<string, string> Params { get; set; }

    public FileObject FileObject { get; set; }

    public byte[] Data { get; set; }

    #endregion

    #region Initializers

    public HttpRequest()
    {
      Headers = new Dictionary<string, string>();
      UrlSegments = new Dictionary<string, string>();
      Params = new Dictionary<string, string>();
    }

    public HttpRequest(string resource) : this()
    {
      Resource = resource;
    }

    public HttpRequest(string resource, Method method) : this(resource)
    {
      Method = method;
    }

    #endregion

    #region IRequest Methods

    public IHttpRequest AddHeader(string key, string value)
    {
      Headers.Add(key, value);
      return this;
    }

    public IHttpRequest AddJsonBody(object obj)
    {
      ContentType = "application/json";
      RequestBody = JsonConvert.SerializeObject(obj);
      return this;
    }

    public IHttpRequest AddBody(string body)
    {
      RequestBody = body;
      return this;
    }

    public IHttpRequest AddParams(string key, string value)
    {
      Params.Add(key, value);
      return this;
    }

    public IHttpRequest AddUrlSegment(string key, string value)
    {
      UrlSegments.Add(key, value);
      return this;
    }

    #endregion

    #region Overridden Methods

    public override string ToString()
    {
      return
          $"-- Resource: {Resource}{System.Environment.NewLine}"
        + $"-- Method: {Method}{System.Environment.NewLine}"
        + $"-- Params: {string.Join(";", Params.Select(x => x.Key + "=" + x.Value).ToArray())}"
        + $"-- Headers: {string.Join(";", Headers.Select(x => x.Key + "=" + x.Value).ToArray())}"
        + $"-- UrlSegments: {string.Join(";", UrlSegments.Select(x => x.Key + "=" + x.Value).ToArray())}"
        + $"-- ContentType: {ContentType}{System.Environment.NewLine}"
        + $"-- RequestBody: {RequestBody}{System.Environment.NewLine}";
    }

    #endregion
  }
}

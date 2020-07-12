using System;
using System.Net.Http;

namespace Core.Network.Http
{
  public enum Method
  {
    GET,
    POST,
    PUT,
    DELETE,
    PATCH
  }

  public static class MethodExtensions
  {
    public static HttpMethod ToHttpMethod(this Method method)
    {
      switch (method)
      {
        case Method.GET:
          return HttpMethod.Get;
        case Method.POST:
          return HttpMethod.Post;
        case Method.PUT:
          return HttpMethod.Put;
        case Method.DELETE:
          return HttpMethod.Delete;
        default:
          return HttpMethod.Get;
      }
    }
  }
}

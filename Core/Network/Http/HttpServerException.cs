using System;
using System.Net;

namespace Core.Network.Http
{
  public class HttpServerException : Exception
  {
    public HttpStatusCode StatusCode { get; set; }

    public HttpServerException(HttpStatusCode statusCode, string message) : base(message)
    {
      StatusCode = statusCode;
    }
  }
}

using System;
namespace Core.Network.Http
{
  public class HttpConnectionException : Exception
  {
    public HttpConnectionException()
    {
    }

    public HttpConnectionException(string message): base(message)
    {
    }
  }
}

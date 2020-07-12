using System;

namespace Core.Network.Api
{
  public interface IAuthApiContext : IApiContext
  {
  }

  public class AuthApiContext : IAuthApiContext
  {
    public string DeviceID { get; set; }
    public string BaseURL { get; private set; }

    public AuthApiContext(string baseURL)
    {
      BaseURL = baseURL;
    }
  }
}

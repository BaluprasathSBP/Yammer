using System;

namespace Core.Network.Api
{
  public interface ILocationApiContext : IApiContext
  {
  }

  public class LocationApiContext : ILocationApiContext
  {
    public string DeviceID { get; set; }
    public string BaseURL { get; private set; }

    public LocationApiContext(string baseURL)
    {
      BaseURL = baseURL;
    }
  }
}

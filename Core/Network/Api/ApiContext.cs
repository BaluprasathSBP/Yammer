namespace Core.Network.Api
{
  public interface IApiContext
  {
    string DeviceID { get; set; }
    string BaseURL { get; }
  }

  public class ApiContext: IApiContext
  {
    public string DeviceID { get; set; }
    public string BaseURL { get; private set; }

    public ApiContext(string baseURL)
    {
      BaseURL = baseURL;
    }
  }
}

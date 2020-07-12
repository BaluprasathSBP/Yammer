using System;

namespace Core.Network.Api
{
  public class Token
  {
    public bool IsPublic { get; set; }
    public string AccessToken { get; set; }
    public DateTime Expiry { get; set; }
  }
}

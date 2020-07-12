using System;

namespace Core.Network.Api
{
  public interface ITokenProvider
  {
    Token Token { get; set; }
  }

  public class TokenProvider : ITokenProvider
  {
    public Token Token { get; set; }
  }
}

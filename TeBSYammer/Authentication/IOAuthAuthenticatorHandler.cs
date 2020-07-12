using System;

namespace TeBSYammer.Authentication
{
  public interface IOAuthAuthenticatorHandler
  {
    void Login(Action<string, string> success, Action<Exception> error);
  }
}


using System;

namespace Core.Tools.Authentication
{
  public interface IOAuthAuthenticatorHandler
  {
    void Login(Action<string, string> success, Action<Exception> error);
  }
}


using System;
using System.Threading.Tasks;

namespace TeBSYammer.DependencyServices
{
  public interface IYammerAuth
  {
    Task Authenticate(Action<string,string> success, Action<Exception> error);
    bool Callback(string url);
  }
}

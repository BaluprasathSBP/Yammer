using System;
using System.Threading.Tasks;
using Core.Tools.Authentication;

namespace VMS.DependencyServices
{
  public interface IGoogleAuth
  {
    Task Authenticate(Action<string,string> success, Action<Exception> error);
    bool Callback(string url);
  }
}

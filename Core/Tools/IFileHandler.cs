using System;
using System.Threading.Tasks;

namespace Core.Tools
{
  public enum SystemDirectory
  {
    Internal,
    Cache,
    External
  }

  public interface IFileHandler
  {
    string Save(SystemDirectory dir, string filename, byte[] file, string folder = null);
    void Delete(string path);
  }
}

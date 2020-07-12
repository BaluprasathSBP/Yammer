using System;
using System.IO;
using Core.iOS.Tools;
using Core.Tools;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHandler))]
namespace Core.iOS.Tools
{
  public class FileHandler : IFileHandler
  {
    public void Delete(string path)
    {
      try
      {
        if (File.Exists(path))
        {
          File.Delete(path);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.StackTrace);
      }
    }

    public string Save(SystemDirectory dir, string filename, byte[] content, string folder = null)
    {
      var path = PathFromDirectory(dir);

      if (!string.IsNullOrEmpty(folder))
      {
        path = Path.Combine(path, folder);
      }

      path = Path.Combine(path, filename);

      FileInfo info = new FileInfo(path);
      if (!info.Directory.Exists)
      {
        info.Directory.Create();
      }

      File.WriteAllBytes(info.FullName, content);
      return path;
    }

    string PathFromDirectory(SystemDirectory dir)
    {
      return dir == SystemDirectory.Cache
           ? FileSystem.CacheDirectory
           : FileSystem.AppDataDirectory;
    }
  }
}

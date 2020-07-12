using System;
using Core.Droid.Utils;
using Core.Tools;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(FileHandler))]
namespace Core.Droid.Utils
{
  public class FileHandler : IFileHandler
  {
    public void Delete(string path)
    {
      if (File.Exists(path))
      {
        File.Delete(path);
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
           : dir == SystemDirectory.Internal ? FileSystem.AppDataDirectory
           : Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
    }
  }
}

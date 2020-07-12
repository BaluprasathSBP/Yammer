using System;
using Android.Content;
using Android.OS;
using Core.Droid.Tools;
using Core.Tools;
using Java.IO;
using Java.Lang;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(PlatformCommonUtils))]
namespace Core.Droid.Tools
{
  public class PlatformCommonUtils : IPlatformCommonUtils
  {
    static Context _context;

    public static void Init(Context context)
    {
      _context = context;
    }

    public bool IsRooted()
    {
      bool isEmulator = IsEmulator();
      if (isEmulator)
      {
        return false;
      }

      // Check from build info
      var buildTags = Build.Tags;
      if (   buildTags != null
          && buildTags.Contains("test-keys"))
      {
        return true;
      }

      if (new File("/system/app/Superuser.apk").Exists())
      {
        return true;
      }

      var pathList = new string[]
      {
        "/system/xbin/su",
        "/system/bin/su",
        "/data/local/su",
        "/su/bin/su"
      };

      foreach(var path in pathList)
      {
        if (new File(path).Exists())
        {
          return true;
        }
      }

      Java.Lang.Process process = null;
      try
      {
        process = Runtime.GetRuntime().Exec(new string[] { "/system/xbin/which", "su" });
        BufferedReader br = new BufferedReader(new InputStreamReader(process.InputStream));
        if (br.ReadLine() != null)
        {
          return true;
        }
      }
      catch (Throwable)
      {
      }
      finally
      {
        if (process != null)
        {
          process.Destroy();
        }
      }

      return false;
    }

    public bool IsEmulator()
    {
      return Build.Product.Equals("sdk")
          || Build.Product.Contains("_sdk")
          || Build.Product.Contains("sdk_")
          || Secure.GetString(_context.ContentResolver, "android_id") == null;
    }
  }
}

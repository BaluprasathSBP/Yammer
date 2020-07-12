using System;
using System.IO;
using Core.iOS.Tools;
using Core.Tools;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformCommonUtils))]
namespace Core.iOS.Tools
{
  public class PlatformCommonUtils : IPlatformCommonUtils
  {
    public bool IsRooted()
    {
      if (!IsEmulator())
      {
        var pathList = new string[]
        {
          "/Applications/Cydia.app",
          "/Library/MobileSubstrate/MobileSubstrate.dylib",
          "/bin/bash",
          "/usr/sbin/sshd",
          "/etc/apt",
          "/usr/bin/ssh"
        };

        foreach(var path in pathList)
        {
          if (File.Exists(path))
          {
            return true;
          }
        }

        // Check if the app can access outside of its sandbox
        try
        {
          File.WriteAllText("/private/jailbreak.txt", "Jailbreak.");
          return true;
        }
        catch(Exception e)
        {
        }

        // Check if the app can open a Cydia's URL scheme
        if (UIApplication.SharedApplication.CanOpenUrl(new Foundation.NSUrl(@"cydia://package/com.example.package")))
        {
          return true;
        }
      }
      return false;
    }

    public bool IsEmulator()
    {
      return Runtime.Arch == Arch.SIMULATOR;
    }
  }
}

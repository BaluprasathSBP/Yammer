using System;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Webkit;
using Core.Tools;
using Plugin.CurrentActivity;

namespace Core.Droid.Tools
{
  public abstract class DocumentHelper: IDocumentHelper
  {
    public void OpenLocalDocument(string path, Action completed = null)
    {
      var fileProvider = GetFileProvider();
      var file = new Java.IO.File(path);

      var context = Android.App.Application.Context;
      Android.Net.Uri uri = FileProvider.GetUriForFile(context, fileProvider, file);
      string extension = MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
      string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);

      Intent viewIntent = new Intent(Intent.ActionView);
      viewIntent.SetDataAndType(uri, mimeType);
      viewIntent.AddFlags(ActivityFlags.NoHistory);
      viewIntent.AddFlags(ActivityFlags.GrantReadUriPermission);

      Context ctx = CrossCurrentActivity.Current.Activity;
      var apps = ctx.PackageManager.QueryIntentActivities(viewIntent, PackageInfoFlags.MatchDefaultOnly);

      if (apps.Any())
      {
        CrossCurrentActivity.Current.Activity.StartActivity(Intent.CreateChooser(viewIntent, "Choose App to View"));
      }
      else
      {
        throw new Exception("No application can perform this action");
      }

      completed?.Invoke();
    }

    protected abstract string GetFileProvider();
  }
}

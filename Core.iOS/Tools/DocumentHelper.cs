using System;
using Core.iOS.Tools;
using Core.Tools;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.IO;

[assembly:Dependency(typeof(DocumentHelper))]
namespace Core.iOS.Tools
{
  public class DocumentHelper: UIDocumentInteractionControllerDelegate, IDocumentHelper
  {
    Action _completed;
    UIViewController _viewController;
    
    public void OpenLocalDocument(string path,Action completed = null)
    {
      _completed = completed;

      var fileUrl = NSUrl.CreateFileUrl(new string[] { path });
      var uidic = UIDocumentInteractionController.FromUrl(fileUrl);

      _viewController = Platform.GetCurrentViewController();

      uidic.Delegate = this;

      if (!uidic.PresentPreview(true))
      {
        var fileData = NSData.FromFile(path);
        UIActivityViewController activityViewController
          = new UIActivityViewController(new NSObject[] { fileUrl }, null);

        var window = UIApplication.SharedApplication.KeyWindow;
        var vc = window.RootViewController;
        vc.PresentViewController(activityViewController, true, null);
      }
    }

    public override void DidEndPreview(UIDocumentInteractionController controller)
    {
      _completed?.Invoke();
    }

    public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
    {
      return _viewController;
    }

    [Export("documentInteractionControllerRectForPreview:")]
    public override CoreGraphics.CGRect RectangleForPreview(UIDocumentInteractionController controller)
    {
      return _viewController.View.Frame;
    }

    [Export("documentInteractionControllerViewForPreview:")]
    public override UIView ViewForPreview(UIDocumentInteractionController controller)
    {
      return _viewController.View;
    }
  }
}

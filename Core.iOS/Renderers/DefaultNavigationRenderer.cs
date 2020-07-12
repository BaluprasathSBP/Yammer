using System;
using Core.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(DefaultPageRenderer))]
namespace Core.iOS.Renderers
{
  public class DefaultPageRenderer : PageRenderer
  {
    public override void WillMoveToParentViewController(UIViewController parent)
    {
      base.WillMoveToParentViewController(parent);
      parent.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
    }
  }
}

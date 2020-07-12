using Core.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(DefaultWebViewRenderer))]
namespace Core.iOS.Renderers
{
  public class DefaultWebViewRenderer : WkWebViewRenderer
  {
    protected override void OnElementChanged(VisualElementChangedEventArgs e)
    {
      base.OnElementChanged(e);

      if (NativeView != null)
      {
        NativeView.Opaque = false;
        NativeView.BackgroundColor = UIColor.Clear;
      }
    }
  }
}

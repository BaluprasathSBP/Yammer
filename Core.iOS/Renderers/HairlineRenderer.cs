using Core.Controls;
using Core.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Hairline), typeof(HairlineRenderer))]
namespace Core.iOS.Renderers
{
  public class HairlineRenderer : ViewRenderer<Hairline, UIView>
  {
    UIView _view;

    protected override void OnElementChanged(ElementChangedEventArgs<Hairline> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement == null)
      {
        return;
      }

      if (Control == null)
      {
        _view = new UIView();
        SetNativeControl(_view);
      }

      if (Element == null)
      {
        return;
      }

      var rect = _view.Frame;
      rect.Height = new System.nfloat(Element.HeightRequest) / UIScreen.MainScreen.Scale;

      _view.Frame = rect;
    }
  }
}

using System.ComponentModel;
using Android.Content;
using Core.Controls;
using Core.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Hairline),
                          typeof(HairlineRenderer))]
namespace Core.Droid.Renderers
{
  public class HairlineRenderer : ViewRenderer<Hairline, Android.Views.View>
  {
    Android.Views.View _view;

    public HairlineRenderer (Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Hairline> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement == null)
      {
        return;
      }

      if (Control == null)
      {
        SetNativeControl(_view = new Android.Views.View(Context));
      }

      UpdateHeight();
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || Element == null)
      {
        return;
      }

      if (e.PropertyName == nameof(Element.Height))
      {
        UpdateHeight();
      }
    }

    void UpdateHeight()
    {
      if (_view == null)
      {
        return;
      }

      var p = _view.LayoutParameters;
      p.Height = 1;
      Element.HeightRequest = 1;

      _view.LayoutParameters = p;
    }
  }
}

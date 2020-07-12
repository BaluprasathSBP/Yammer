using System;
using Android.Content;
using Core.Droid.Renderers;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(DefaultButtonRenderer))]
namespace Core.Droid.Renderers
{
  public class DefaultButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer
  {
    public DefaultButtonRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || Element == null)
      {
        return;
      }
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);
    }
  }
}

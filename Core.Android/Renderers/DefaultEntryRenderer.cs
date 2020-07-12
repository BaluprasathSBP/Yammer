using System;
using Android.Content;
using Core.Droid.Renderers;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(DefaultEntryRenderer))]
namespace Core.Droid.Renderers
{
  public class DefaultEntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
  {
    public DefaultEntryRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);

      if (Control == null
          || Element == null)
      {
        return;
      }
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);
    }
  }
}

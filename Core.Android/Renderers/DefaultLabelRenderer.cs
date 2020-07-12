using System.ComponentModel;
using Android.Content;
using Android.Util;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label),
                          typeof(Core.Droid.Renderers.DefaultLabelRenderer))]
namespace Core.Droid.Renderers
{
  public class DefaultLabelRenderer : Xamarin.Forms.Platform.Android.LabelRenderer
  {
    readonly bool _shouldOverwriteTypeface = false;

    public DefaultLabelRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || Element == null)
      {
        return;
      }

      UpdateFont();
      UpdateFormattedText();

      if (_shouldOverwriteTypeface)
      {
        Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                       Element.FontAttributes);
      }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || Element == null)
      {
        return;
      }

      if (e.PropertyName == Label.FontSizeProperty.PropertyName)
      {
        UpdateFont();
      }
      else if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
      {
        UpdateFormattedText();
      }
    }

    void UpdateFormattedText()
    {
      if (Element.FormattedText != null)
      {
        Control.TextFormatted = Element.FormattedText.ToAttributed(Context,
                                                                   Element.FontAttributes,
                                                                   Element.TextColor,
                                                                   Element.FontSize);
      }
    }

    void UpdateFont()
    {
      Control.SetTextSize(ComplexUnitType.Dip, (float)Element.FontSize);
    }
  }
}

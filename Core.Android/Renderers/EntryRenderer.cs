using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Android.Views;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Core.Controls.Entry),
                          typeof(Core.Droid.Renderers.EntryRenderer))]
namespace Core.Droid.Renderers
{
  public class EntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
  {
    #region Properties

    protected Controls.Entry BaseElement
    {
      get { return Element as Controls.Entry; }
    }

    #endregion

    #region Initializations

    public EntryRenderer(Context context) : base(context)
    {
    }

    #endregion

    #region Life-cycle methods

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }

      if (e.PropertyName == Controls.Entry.BackgroundDrawableProperty.PropertyName)
      {
        UpdateBackgroundDrawable();
      }
      else if (e.PropertyName == Entry.FontSizeProperty.PropertyName)
      {
        UpdateFont();
      }
      else if (e.PropertyName == Controls.Entry.MaxLengthProperty.PropertyName)
      {
        UpdateMaxLength();
      }
      else if (e.PropertyName == Controls.Entry.PaddingProperty.PropertyName)
      {
        UpdatePadding();
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }


      Control.Background = null;
      Control.Gravity = GravityFlags.CenterVertical;
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);
      UpdatePadding();
      UpdateBackgroundDrawable();
      UpdateFont();
      UpdateMaxLength();
    }

    #endregion

    #region Internal methods

    void UpdateBackgroundDrawable()
    {
      var drawableName = BaseElement.BackgroundDrawable?.File;
      if (string.IsNullOrEmpty(drawableName))
      {
        return;
      }

      Drawable drawable = Context.GetDrawable(drawableName);
      Control.Background = drawable;

      if (BaseElement.Padding.Equals(default(Thickness)))
      {
        float dpi = Resources.DisplayMetrics.Density;
        int padding = (int)(8 * dpi);
        Control.SetPadding(padding, 0, padding, 0);
      }
      else
      {
        UpdatePadding();
      }
    }

    void UpdateFont()
    {
      Control.SetTextSize(ComplexUnitType.Dip, (float)Element.FontSize);
    }

    void UpdateMaxLength()
    {
      if (BaseElement.MaxLength == 0)
      {
        return;
      }

      var currentFilters = new List<IInputFilter>(Control?.GetFilters() ?? new IInputFilter[0]);

      for (var i = 0; i < currentFilters.Count; i++)
      {
        if (currentFilters[i] is InputFilterLengthFilter)
        {
          currentFilters.RemoveAt(i);
          break;
        }
      }

      currentFilters.Add(new InputFilterLengthFilter(BaseElement.MaxLength));
      Control.SetFilters(currentFilters.ToArray());

      var currentControlText = Control.Text;

      if (currentControlText.Length > BaseElement.MaxLength)
      {
        Control.Text = currentControlText.Substring(0, BaseElement.MaxLength);
      }
    }

    void UpdatePadding()
    {
      float dpi = Resources.DisplayMetrics.Density;

      Control.SetPadding((int)(BaseElement.Padding.Left * dpi),
                         (int)(BaseElement.Padding.Top * dpi),
                         (int)(BaseElement.Padding.Right * dpi),
                         (int)(BaseElement.Padding.Bottom * dpi));
    }

    #endregion
  }
}

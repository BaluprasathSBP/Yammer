using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Core.Controls.DatePicker),
                          typeof(Core.Droid.Renderers.DatePickerRenderer))]
namespace Core.Droid.Renderers
{
  public class DatePickerRenderer: Xamarin.Forms.Platform.Android.DatePickerRenderer
  {
    #region Properties

    protected Controls.DatePicker BaseElement
    {
      get { return Element as Controls.DatePicker; }
    }

    #endregion

    #region Initializations

    public DatePickerRenderer(Context context) : base(context)
    {
    }

    #endregion

    #region Life-cycle methods

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (Control == null
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
      else if (e.PropertyName == Controls.Entry.PaddingProperty.PropertyName)
      {
        UpdatePadding();
      }
      else if (   e.PropertyName == nameof(Element.IsFocused)
               && Element.IsFocused)
      {
        BaseElement.NullableDate = Element.Date;
      }
      else if (e.PropertyName == nameof(Element.Date))
      {
        BaseElement.NullableDate = Element.Date;
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
    {
      base.OnElementChanged(e);

      if (Control == null
          || BaseElement == null)
      {
        return;
      }

      Control.Focusable = false;
      Control.Background = null;
      Control.Gravity = GravityFlags.CenterVertical;
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);
      UpdatePadding();
      UpdateBackgroundDrawable();
      UpdateFont();
    }

    protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
    {
      var view = BaseElement;
      var dialog = new DatePickerDialog(Context, (o, e) =>
      {
        view.Date = e.Date;
        ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
      }, year, month, day);

      dialog.SetButton2("Clear", (sender, e) =>
      {
        view.Date = DateTime.Now;
        view.NullableDate = null;
        ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
      });

      return dialog;
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

using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Util;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Core.Controls.Button),
                          typeof(Core.Droid.Renderers.ButtonRenderer))]
namespace Core.Droid.Renderers
{
  public class ButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer
  {
    #region Properties

    Controls.Button BaseElement
    {
      get { return Element as Controls.Button; }
    }

    #endregion

    #region Initializations

    public ButtonRenderer(Context context) : base(context)
    {
    }

    #endregion

    #region Life-cycle methods

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || Element == null)
      {
        return;
      }

      if (   e.PropertyName == Controls.Button.NormalBackgroundDrawableProperty.PropertyName
          || e.PropertyName == Controls.Button.PressedBackgroundDrawableProperty.PropertyName
          || e.PropertyName == Controls.Button.DisabledBackgroundDrawableProperty.PropertyName
          || e.PropertyName == Controls.Button.BackgroundColorProperty.PropertyName
          || e.PropertyName == Controls.Button.PressedBackgroundColorProperty.PropertyName
          || e.PropertyName == Controls.Button.DisabledTextColorProperty.PropertyName)
      {
        UpdateBackgroundStateDrawable();
      }
      else if (   e.PropertyName == Controls.Button.PressedTextColorProperty.PropertyName
               || e.PropertyName == Controls.Button.DisabledTextColorProperty.PropertyName
               || e.PropertyName == Controls.Button.TextColorProperty.PropertyName)
      {
        UpdateTextColor();
      }
      else if (e.PropertyName == Controls.Button.FontSizeProperty.PropertyName)
      {
        UpdateFont();
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || Element == null)
      {
        return;
      }

      Control.SetPadding(0, 0, 0, 0);
      Control.SetAllCaps(false);
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);

      UpdateBackgroundStateDrawable();
      UpdateTextColor();
      UpdateFont();
    }

    #endregion

    #region Internal methods

    void UpdateBackgroundStateDrawable()
    {
      string normalDrawableName = BaseElement.NormalBackgroundDrawable?.File;
      string pressedDrawableName = BaseElement.PressedBackgroundDrawable?.File;
      string disabledDrawableName = BaseElement.DisabledBackgroundDrawable?.File;

      Android.Graphics.Color normalColor = BaseElement.BackgroundColor.ToAndroid();
      Android.Graphics.Color pressedColor = BaseElement.PressedBackgroundColor.ToAndroid();
      Android.Graphics.Color disabledColor = BaseElement.DisabledBackgroundColor.ToAndroid();

      Drawable disabledDrawable = null;
      Drawable pressedDrawable = null;
      Drawable normalDrawable = null;

      // Disabled state
      if (!string.IsNullOrEmpty(disabledDrawableName))
      {
        disabledDrawable = Context.GetDrawable(disabledDrawableName);
      }
      else if (disabledColor != 0)
      {
        disabledDrawable = new ColorDrawable(disabledColor);
      }

      // Press state
      if (!string.IsNullOrEmpty(pressedDrawableName))
      {
        pressedDrawable = Context.GetDrawable(pressedDrawableName);
      }
      else if (pressedColor != 0)
      {
        pressedDrawable = new ColorDrawable(pressedColor);
      }

      // Normal state
      if (normalDrawableName != null)
      {
        normalDrawable = Context.GetDrawable(normalDrawableName);
      }
      else
      {
        normalDrawable = new ColorDrawable(normalColor);
      }

      var stateListDrawable = new StateListDrawable();

      if (disabledDrawable != null)
      {
        stateListDrawable.AddState(
          new[] { -Android.Resource.Attribute.StateEnabled },
          disabledDrawable);
      }

      if (pressedDrawable != null)
      {
        stateListDrawable.AddState(
          new[] { Android.Resource.Attribute.StateEnabled,
                  Android.Resource.Attribute.StatePressed},
          pressedDrawable);
      }

      stateListDrawable.AddState(
          new[] { Android.Resource.Attribute.StateEnabled },
          normalDrawable);

      Control.Background = stateListDrawable;
    }

    void UpdateTextColor ()
    {
      int normalColor = BaseElement.TextColor.ToAndroid();
      int selectedColor = BaseElement.SelectedTextColor.ToAndroid();
      int disabledColor = BaseElement.DisabledTextColor.ToAndroid();

      var stateListColor = new ColorStateList(new int[][] {
        new int[] { -Android.Resource.Attribute.StateEnabled
        },
        new int[] { Android.Resource.Attribute.StateEnabled,
                    Android.Resource.Attribute.StatePressed
        },
        new int[] { Android.Resource.Attribute.StateEnabled,
                    Android.Resource.Attribute.StateSelected
        },
        new int[] { Android.Resource.Attribute.StateEnabled
        }
      },
      new int[] {
        disabledColor == 0 ? normalColor : disabledColor,
        selectedColor == 0 ? normalColor : selectedColor,
        selectedColor == 0 ? normalColor : selectedColor,
        normalColor,
      });

      Control.SetTextColor(stateListColor);
    }

    void UpdateFont()
    {
      Control.SetTextSize(ComplexUnitType.Dip, (float)Element.FontSize);
    }

    #endregion
  }
}

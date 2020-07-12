using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Text;
using Android.Util;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor),
                          typeof(Core.Droid.Renderers.EditorRenderer))]
namespace Core.Droid.Renderers
{
  public class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
  {
    #region Properties

    Controls.Editor BaseElement
    {
      get { return Element as Controls.Editor; }
    }

    #endregion

    public EditorRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }
      Control.Background = null;
      Control.Typeface = TypefaceFactory.GetTypefaceForFontAttribute(Context,
                                                                     Element.FontAttributes);

      UpdateFont();
      UpdateMaxLength();
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }

      if (e.PropertyName == Editor.FontSizeProperty.PropertyName)
      {
        UpdateFont();
      }
      else if (e.PropertyName == Controls.Editor.MaxLengthProperty.PropertyName)
      {
        UpdateMaxLength();
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
  }
}

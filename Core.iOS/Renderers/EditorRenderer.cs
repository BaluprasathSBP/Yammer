using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor),
                          typeof(Core.iOS.Renderers.EditorRenderer))]
namespace Core.iOS.Renderers
{
  public class EditorRenderer : Xamarin.Forms.Platform.iOS.EditorRenderer
  {
    #region Properties

    Controls.Editor BaseElement
    {
      get { return Element as Controls.Editor; }
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

      if (e.PropertyName == Controls.Editor.MaxLengthProperty.PropertyName)
      {
        UpdateMaxLength();
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }

      UpdateMaxLength();
    }

    #endregion

    void UpdateMaxLength()
    {
      Control.ShouldChangeText = (textField, range, replacementString) =>
      {
        if (BaseElement.MaxLength == 0)
        {
          return true;
        }
        var newLength = textField.Text.Length + replacementString.Length - range.Length;
        return newLength <= BaseElement.MaxLength;
      };
    }
  }
}

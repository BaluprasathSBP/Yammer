using System;
using System.ComponentModel;
using CoreGraphics;
using Core.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Core.Controls.Entry),
                          typeof(Core.iOS.Renderers.EntryRenderer))]
namespace Core.iOS.Renderers
{
  public class EntryRenderer : Xamarin.Forms.Platform.iOS.EntryRenderer
  {
    #region Properties

    Controls.Entry BaseElement
    {
      get { return Element as Controls.Entry; }
    }

    IElementController ElementController => Element as IElementController;

    #endregion

    bool _disposed;

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

      var textField = new PaddedTextField();
      textField.EditingChanged += OnEditingChanged;
      textField.EditingDidEnd += OnEditingEnded;
      textField.ShouldReturn = OnShouldReturn;
      textField.SecureTextEntry = BaseElement.IsPassword;

      SetNativeControl(textField);

      Control.Placeholder = BaseElement.Placeholder;
      Control.Text = BaseElement.Text;
      Control.BorderStyle = UITextBorderStyle.None;

      UpdateBackgroundDrawable();
      UpdateMaxLength();
      UpdatePadding();
      UpdateKeyboard();
    }

    protected override void Dispose(bool disposing)
    {
      if (_disposed)
      {
        return;
      }

      if (disposing)
      {
        if (Control != null)
        {
          Control.EditingChanged -= OnEditingChanged;
          Control.EditingDidEnd -= OnEditingEnded;
        }
      }

      base.Dispose(disposing);
    }

    #endregion

    #region Internal methods

    void OnEditingChanged(object sender, EventArgs eventArgs)
    {
      ElementController.SetValueFromRenderer(Xamarin.Forms.Entry.TextProperty, Control.Text);
    }

    void OnEditingEnded(object sender, EventArgs e)
    {
      var controlText = Control.Text ?? string.Empty;
      var entryText = Element.Text ?? string.Empty;
      if (controlText != entryText)
      {
        ElementController.SetValueFromRenderer(Xamarin.Forms.Entry.TextProperty, controlText);
      }

      ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
    }

    void UpdateBackgroundDrawable ()
    {
      var drawableName = BaseElement.BackgroundDrawable?.File;
      if (string.IsNullOrEmpty(drawableName))
      {
        return;
      }

      UIImage source = UIImage.FromBundle(drawableName);
      if (!Equals(BaseElement.EdgeInsets, default(EdgeInsets)))
      {
        var mode = BaseElement.ResizeMode == ResizeMode.Stretch
                  ? UIImageResizingMode.Stretch
                  : UIImageResizingMode.Tile;

        source = source.CreateResizableImage(
          new UIEdgeInsets(BaseElement.EdgeInsets.Top,
                           BaseElement.EdgeInsets.Left,
                           BaseElement.EdgeInsets.Bottom,
                           BaseElement.EdgeInsets.Right),
          mode
        );
      }
      Control.Background = source;
    }

    void UpdateMaxLength()
    {
      Control.ShouldChangeCharacters = (textField, range, replacementString) =>
      {
        if (BaseElement.MaxLength == 0)
        {
          return true;
        }
        var newLength = textField.Text.Length + replacementString.Length - range.Length;
        return newLength <= BaseElement.MaxLength;
      };
    }

    void UpdatePadding()
    {
      if (Control is PaddedTextField textField)
      {
        textField.TextInsets = new UIEdgeInsets((System.nfloat)BaseElement.Padding.Top,
                                                (System.nfloat)BaseElement.Padding.Left,
                                                (System.nfloat)BaseElement.Padding.Bottom,
                                                (System.nfloat)BaseElement.Padding.Right);
      }
    }

    void UpdateKeyboard()
    {
      var keyboard = Element.Keyboard;
      Control.ApplyKeyboard(keyboard);
      Control.ReloadInputViews();
    }

    #endregion

    public class PaddedTextField: UITextField 
    {
      UIEdgeInsets _textInsets;
      public UIEdgeInsets TextInsets 
      { 
        get => _textInsets; 
        set
        {
          _textInsets = value;
          SetNeedsDisplay();
        }
      }

      public override CGRect TextRect(CGRect forBounds)
      {
        return TextInsets.InsetRect(forBounds);
      }

      public override CGRect EditingRect(CGRect forBounds)
      {
        return TextInsets.InsetRect(forBounds);
      }

      public override CGRect PlaceholderRect(CGRect forBounds)
      {
        return TextInsets.InsetRect(forBounds);
      }

      public override void DrawText(CGRect rect)
      {
        base.DrawText(TextInsets.InsetRect(rect));
      }
    }
  }
}

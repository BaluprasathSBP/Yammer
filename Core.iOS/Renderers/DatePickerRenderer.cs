using System;
using System.ComponentModel;
using System.Drawing;
using Foundation;
using Core.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static Core.iOS.Renderers.EntryRenderer;

[assembly: ExportRenderer(typeof(Core.Controls.DatePicker),
                          typeof(Core.iOS.Renderers.DatePickerRenderer))]
namespace Core.iOS.Renderers
{
  public class DatePickerRenderer : ViewRenderer<Xamarin.Forms.DatePicker, UITextField>
  {
    #region Properties

    Controls.DatePicker BaseElement
    {
      get { return Element as Controls.DatePicker; }
    }

    IElementController ElementController => Element as IElementController;

    #endregion

    UIDatePicker _picker;
    UIColor _defaultTextColor;

    #region Life-cycle methods

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (Control == null
          || BaseElement == null)
      {
        return;
      }

      if (e.PropertyName == Controls.DatePicker.BackgroundDrawableProperty.PropertyName)
      {
        UpdateBackgroundDrawable();
      }
      else if (e.PropertyName == Controls.DatePicker.PaddingProperty.PropertyName)
      {
        UpdatePadding();
      }
      if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName 
          || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
      {
        BaseElement.NullableDate = Element.Date;
        UpdateDateFromModel(true);
      }
      else if (e.PropertyName == Xamarin.Forms.DatePicker.MinimumDateProperty.PropertyName)
      {
        UpdateMinimumDate();
      }
      else if (e.PropertyName == Xamarin.Forms.DatePicker.MaximumDateProperty.PropertyName)
      {
        UpdateMaximumDate();
      }
      else if (e.PropertyName == Xamarin.Forms.DatePicker.TextColorProperty.PropertyName 
               || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
      {
        UpdateTextColor();
      }
      else if (   e.PropertyName == nameof(Element.IsFocused)
               && Element.IsFocused)
      {
        BaseElement.NullableDate = Element.Date;
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement == null)
      {
        return;
      }
        
      if (Control == null)
      {
        SetNativeControl(CreateEntry());
      }

      UpdateBackgroundDrawable();
      UpdatePadding();
      UpdateDateFromModel(false);
      UpdateMinimumDate();
      UpdateMaximumDate();
      UpdateTextColor();
    }

    #endregion

    #region Internal Methods

    UITextField CreateEntry()
    {
      var entry = new PaddedTextField();

      entry.EditingDidBegin += OnStarted;
      entry.EditingDidEnd += OnEnded;

      _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };
      _picker.ValueChanged += HandleValueChanged;

      var width = UIScreen.MainScreen.Bounds.Width;
      var toolbar = new UIToolbar(new RectangleF(0, 0, (float)width, 44))
      {
        BarStyle = UIBarStyle.Default,
        Translucent = true
      };
      var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
      var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => entry.ResignFirstResponder());
      var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Done, 
        (sender, e) => 
        { 
          _picker.Date = DateTime.Now.ToNSDate();
          BaseElement.Date = DateTime.Now;
          BaseElement.NullableDate = null;
          entry.ResignFirstResponder();
        });

      toolbar.SetItems(new[] { clearButton, spacer, doneButton }, false);

      entry.InputView = _picker;
      entry.InputAccessoryView = toolbar;

      entry.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
      entry.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

      _defaultTextColor = entry.TextColor;
      return entry;
    }

    void HandleValueChanged(object sender, EventArgs e)
    {
      ElementController?.SetValueFromRenderer(Xamarin.Forms.DatePicker.DateProperty, _picker.Date.ToDateTime().Date);
    }

    void OnEnded(object sender, EventArgs eventArgs)
    {
      ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
    }

    void OnStarted(object sender, EventArgs eventArgs)
    {
      ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
    }

    void UpdateMaximumDate()
    {
      _picker.MaximumDate = Element.MaximumDate.ToNSDate();
    }

    void UpdateMinimumDate()
    {
      _picker.MinimumDate = Element.MinimumDate.ToNSDate();
    }

    void UpdateTextColor()
    {
      var textColor = Element.TextColor;

      if (textColor.IsDefault || (!Element.IsEnabled))
      {
        Control.TextColor = _defaultTextColor;
      }
      else
      {
        Control.TextColor = textColor.ToUIColor();
      }
      Control.Text = Control.Text;
    }

    void UpdateBackgroundDrawable()
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

    void UpdateDateFromModel(bool animate)
    {
      if (_picker.Date.ToDateTime().Date != Element.Date.Date)
        _picker.SetDate(Element.Date.ToNSDate(), animate);

      Control.Text = Element.Date.ToString(Element.Format);
    }

    #endregion
  }
}

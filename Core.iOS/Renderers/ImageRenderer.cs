using System.ComponentModel;
using Core.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Core.Controls.Image),
                          typeof(Core.iOS.Renderers.ImageRenderer))]
namespace Core.iOS.Renderers
{
  public class ImageRenderer : Xamarin.Forms.Platform.iOS.ImageRenderer
  {
    #region Properties

    Controls.Image BaseElement
    {
      get { return Element as Controls.Image; }
    }

    #endregion

    public ImageRenderer()
    {
    }

    #region Life-cycle methods

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }

      if (e.PropertyName == Controls.Image.ResizableSourceProperty.PropertyName)
      {
        UpdateBackgroundDrawable();
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
    {
      base.OnElementChanged(e);

      if (   Control == null
          || BaseElement == null)
      {
        return;
      }

      UpdateBackgroundDrawable();
    }

    #endregion

    #region Internal methods

    void UpdateBackgroundDrawable ()
    {
      string drawableName = BaseElement.ResizableSource?.File;
      if (string.IsNullOrEmpty(drawableName))
      {
        return;
      }

      // TODO Provide property for UIImage resizable insets
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
        Control.ContentMode = UIViewContentMode.ScaleToFill;
      }
      Control.Image = source;
    }

    #endregion
  }
}

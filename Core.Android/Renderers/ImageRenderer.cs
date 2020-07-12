using System;
using System.ComponentModel;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Core.Controls.Image),
                          typeof(Core.Droid.Renderers.ImageRenderer))]
namespace Core.Droid.Renderers
{
  public class ImageRenderer : Xamarin.Forms.Platform.Android.ImageRenderer
  {
    #region Properties

    Controls.Image BaseElement
    {
      get { return Element as Controls.Image; }
    }

    #endregion

    public ImageRenderer (Context context) : base(context)
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

    void UpdateBackgroundDrawable()
    {
      string drawableName = BaseElement.ResizableSource?.File;
      if (   Context == null 
          || string.IsNullOrEmpty(drawableName))
      {
        return;
      }

      var drawable = Context.GetDrawable(drawableName);
      Control.SetImageDrawable(null);
      Control.SetBackground(drawable);
    }
  }
}

using System;
using Xamarin.Forms;

namespace Core.Controls
{
  public class Image : Xamarin.Forms.Image
  {
    #region Bindable Properties

    public static readonly BindableProperty ResizableSourceProperty = BindableProperty.Create(
      propertyName: "ResizableSource",
      returnType: typeof(FileImageSource),
      declaringType: typeof(Image),
      defaultValue: default(FileImageSource));

    public static readonly BindableProperty EdgeInsetsProperty = BindableProperty.Create(
      propertyName: nameof(EdgeInsets),
      returnType: typeof(EdgeInsets),
      declaringType: typeof(Entry),
      defaultValue: default(EdgeInsets));

    public static readonly BindableProperty ResizeModeProperty = BindableProperty.Create(
      propertyName: nameof(ResizeMode),
      returnType: typeof(ResizeMode),
      declaringType: typeof(Entry),
      defaultValue: default(ResizeMode));

    public FileImageSource ResizableSource
    {
      get { return (FileImageSource)GetValue(ResizableSourceProperty); }
      set { SetValue(ResizableSourceProperty, value); }
    }

    public EdgeInsets EdgeInsets
    {
      get { return (EdgeInsets)GetValue(EdgeInsetsProperty); }
      set { SetValue(EdgeInsetsProperty, value); }
    }

    public ResizeMode ResizeMode
    {
      get { return (ResizeMode)GetValue(ResizeModeProperty); }
      set { SetValue(ResizeModeProperty, value); }
    }

    #endregion
  }
}

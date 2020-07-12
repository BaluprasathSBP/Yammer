using Xamarin.Forms;

namespace Core.Controls
{
  public class Entry : Xamarin.Forms.Entry
  {
    public static readonly BindableProperty BackgroundDrawableProperty = BindableProperty.Create(
      propertyName: "BackgroundDrawable",
      returnType: typeof(FileImageSource),
      declaringType: typeof(Entry),
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

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
      propertyName: nameof(MaxLength),
      returnType: typeof(int),
      declaringType: typeof(Entry),
      defaultValue: default(int));

    public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
      propertyName: nameof(Padding),
      returnType: typeof(Thickness),
      declaringType: typeof(Entry),
      defaultValue: default(Thickness));

    public FileImageSource BackgroundDrawable
    {
      get { return (FileImageSource)GetValue(BackgroundDrawableProperty); }
      set { SetValue(BackgroundDrawableProperty, value); }
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

    public int MaxLength
    {
      get { return (int)GetValue(MaxLengthProperty); }
      set { SetValue(MaxLengthProperty, value); }
    }

    public Thickness Padding
    {
      get { return (Thickness)GetValue(PaddingProperty); }
      set { SetValue(PaddingProperty, value); }
    }
  }
}

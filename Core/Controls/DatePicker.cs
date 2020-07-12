using System;
using Xamarin.Forms;

namespace Core.Controls
{
  public class DatePicker: Xamarin.Forms.DatePicker
  {
    public static readonly BindableProperty BackgroundDrawableProperty = BindableProperty.Create(
      propertyName: "BackgroundDrawable",
      returnType: typeof(FileImageSource),
      declaringType: typeof(DatePicker),
      defaultValue: default(FileImageSource));

    public static readonly BindableProperty EdgeInsetsProperty = BindableProperty.Create(
      propertyName: nameof(EdgeInsets),
      returnType: typeof(EdgeInsets),
      declaringType: typeof(DatePicker),
      defaultValue: default(EdgeInsets));

    public static readonly BindableProperty ResizeModeProperty = BindableProperty.Create(
      propertyName: nameof(ResizeMode),
      returnType: typeof(ResizeMode),
      declaringType: typeof(DatePicker),
      defaultValue: default(ResizeMode));

    public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
      propertyName: nameof(Padding),
      returnType: typeof(Thickness),
      declaringType: typeof(DatePicker),
      defaultValue: default(Thickness));

    public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(
      propertyName: nameof(NullableDate),
      returnType: typeof(DateTime?),
      declaringType: typeof(DatePicker),
      defaultValue: null);

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

    public Thickness Padding
    {
      get { return (Thickness)GetValue(PaddingProperty); }
      set { SetValue(PaddingProperty, value); }
    }

    public DateTime? NullableDate
    {
      get { return (DateTime?)GetValue(NullableDateProperty); }
      set { SetValue(NullableDateProperty, value); }
    }

    public DatePicker()
    {
    }
  }
}

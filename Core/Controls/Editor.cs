using Xamarin.Forms;

namespace Core.Controls
{
  public class Editor : Xamarin.Forms.Editor
  {
    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
      propertyName: nameof(MaxLength),
      returnType: typeof(int),
      declaringType: typeof(Editor),
      defaultValue: default(int));

    public int MaxLength
    {
      get { return (int)GetValue(MaxLengthProperty); }
      set { SetValue(MaxLengthProperty, value); }
    }
  }
}

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Core.Utils
{
  public class AllCapsStringConverter: IValueConverter
  {
    public AllCapsStringConverter()
    {
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is string str ? str.ToUpper() : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value as string;
    }
  }
}

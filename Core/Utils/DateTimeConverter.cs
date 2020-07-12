using System;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Core.Utils
{
  public class DateTimeConverter: IsoDateTimeConverter
  {
    public DateTimeConverter()
    {
    }

    public DateTimeConverter(string format)
    {
      DateTimeFormat = format;
      Culture = new CultureInfo("en-US");
    }
  }
}

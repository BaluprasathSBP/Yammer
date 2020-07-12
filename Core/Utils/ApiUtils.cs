using System;
namespace Core.Utils
{
  public static class ApiUtils
  {
    public static string ApiDateFormat { get; set; } = "yyyyMMddhhmmss.ffffff";

    public static string ToApiDateTimeFormat(this DateTime? date)
    {
      if (date != null)
      {
        return date?.ToUniversalTime().ToString(ApiDateFormat);
      }
      return string.Empty;
    }

    public static string ToApiLocalDateTimeFormat(this DateTime? date)
    {
      if (date != null)
      {
        return date?.ToString(ApiDateFormat);
      }
      return string.Empty;
    }

    public static string ToApiDateTimeFormat(this DateTimeOffset? date)
    {
      if (date != null)
      {
        return date?.ToUniversalTime().ToString(ApiDateFormat);
      }
      return string.Empty;
    }

    public static string ToApiDateTimeFormat(this DateTime date)
    {
      return date.ToUniversalTime().ToString(ApiDateFormat);
    }

    public static string ToApiDateTimeFormat(this DateTimeOffset date)
    {
      return date.ToUniversalTime().ToString(ApiDateFormat);
    }

    public static object ToApiDateFormat(DateTime? date)
    {
      return date?.ToUniversalTime().ToString(ApiDateFormat);
    }

    public static object ToApiDateFormatWithoutUTC(DateTime? date)
    {
      return date?.ToString(ApiDateFormat);
    }
  }
}

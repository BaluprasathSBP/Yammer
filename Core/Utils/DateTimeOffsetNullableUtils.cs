using System;
namespace Core.Utils
{
  public static class DateTimeOffsetNullableUtils
  {
    public static DateTimeOffset? EarliestDateTime(params DateTimeOffset?[] dateTimes)
    {
      DateTimeOffset? earliest = null;

      if (dateTimes.Length == 0)
      {
        return null;
      }

      earliest = dateTimes[0];

      for (var cnt = 1; cnt < dateTimes.Length; cnt++)
      {
        var date = dateTimes[cnt];

        if (date == null)
        {
          continue;
        }

        if (earliest == null || date?.CompareTo(earliest.Value) < 0)
        {
          earliest = date;
        }
      }
      return earliest;
    }

    public static DateTimeOffset? LatesttDateTime(params DateTimeOffset?[] dateTimes)
    {
      DateTimeOffset? latest = null;

      if (dateTimes.Length == 0)
      {
        return null;
      }

      latest = dateTimes[0];

      for (var cnt = 1; cnt < dateTimes.Length; cnt++)
      {
        var date = dateTimes[cnt];

        if (date == null)
        {
          continue;
        }

        if (latest == null || date?.CompareTo(latest.Value) > 0)
        {
          latest = date;
        }
      }
      return latest;
    }
  }
}

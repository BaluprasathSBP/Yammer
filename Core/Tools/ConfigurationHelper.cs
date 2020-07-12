using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Core.Tools
{
  public static class ConfigurationHelper
  {
    public static T LoadConfigration<T>(Assembly assembly, string reosourceId)
    {
      var config = default(T);

      try
      {

        using (Stream s = assembly.GetManifestResourceStream(reosourceId))
        {
          StreamReader reader = new StreamReader(s);
          string text = reader.ReadToEnd();
          config = JsonConvert.DeserializeObject<T>(text);
        }
      }
      catch(Exception e)
      {
      }
      return config;
    }
  }
}

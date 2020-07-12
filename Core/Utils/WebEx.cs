using System;
using System.Net;
using System.Collections.Generic;

namespace Core.Utils
{
  public static class WebEx
  {
    static char[] AmpersandChars = new char[] { '&' };
    /*
     * mc++
     * never used
    static char[] EqualsChars = new char[] { '=' };
    */

    public static IDictionary<string, string> FormDecode(string encodedString)
    {
      #region
      ///-------------------------------------------------------------------------------------------------
      /// Pull Request - manually added/fixed
      ///		bug fix in SslCertificateEqualityComparer #76
      ///		https://github.com/xamarin/Xamarin.Auth/pull/76
      /*
      var inputs = new Dictionary<string, string> ();
      if (encodedString.StartsWith ("?") || encodedString.StartsWith ("#")) {
        encodedString = encodedString.Substring (1);
      }
      var parts = encodedString.Split (AmpersandChars);
      foreach (var p in parts) {
        var kv = p.Split (EqualsChars);
        var k = Uri.UnescapeDataString (kv[0]);
        var v = kv.Length > 1 ? Uri.UnescapeDataString (kv[1]) : "";
        inputs[k] = v;
      }
      */
      var inputs = new Dictionary<string, string>();

      if (encodedString.Length > 0)
      {
        char firstChar = encodedString[0];
        if (firstChar == '?' || firstChar == '#')
        {
          encodedString = encodedString.Substring(1);
        }

        if (encodedString.Length > 0)
        {
          var parts = encodedString.Split(AmpersandChars, StringSplitOptions.RemoveEmptyEntries);

          foreach (var part in parts)
          {
            var equalsIndex = part.IndexOf('=');

            string key;
            string value;
            if (equalsIndex >= 0)
            {
              key = Uri.UnescapeDataString(part.Substring(0, equalsIndex));
              value = Uri.UnescapeDataString(part.Substring(equalsIndex + 1));
            }
            else
            {
              key = Uri.UnescapeDataString(part);
              value = string.Empty;
            }

            inputs[key] = value;
          }
        }
      }
      ///-------------------------------------------------------------------------------------------------
      #endregion

      return inputs;
    }
  }
}

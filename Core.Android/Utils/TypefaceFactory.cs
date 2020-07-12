using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Core.Droid;

namespace Core.Droid.Utils
{
  public static class TypefaceFactory
  {
    /* Constants */

    public const int RobotoBold = 0;
    public const int RobotoBoldItalic = 1;
    public const int RobotoItalic = 2;
    public const int RobotoLight = 3;
    public const int RobotoLightItalic = 4;
    public const int RobotoMedium = 5;
    public const int RobotoMediumItalic = 6;
    public const int RobotoRegular = 7;
    public const int RobotoThin = 8;

    /* Static properties */

    static IDictionary<string, Typeface> TypefaceCache = new Dictionary<string, Typeface>();
    static IDictionary<string, string> TypefacePathCache = new Dictionary<string, string>();

    static int[] RawRes = {
      Resource.Raw.font_roboto_bold,
      Resource.Raw.font_roboto_bolditalic,
      Resource.Raw.font_roboto_italic,
      Resource.Raw.font_roboto_light,
      Resource.Raw.font_roboto_lightitalic,
      Resource.Raw.font_roboto_medium,
      Resource.Raw.font_roboto_mediumitalic,
      Resource.Raw.font_roboto_regular,
      Resource.Raw.font_roboto_thin,
    };

    /* Static methods */

    public static Typeface GetTypefaceForFontAttribute(Context context, FontAttributes attr)
    {
      return GetTypeFaceForId(context,  attr == FontAttributes.Bold ? RobotoBold
                                      : attr == FontAttributes.Italic ? RobotoItalic
                                      : RobotoRegular);
    }

    /// <summary>
    /// Gets the type face for identifier.
    /// </summary>
    /// <returns>The type face for identifier.</returns>
    /// <param name="context">Context.</param>
    /// <param name="typefaceID">Typeface identifier.</param>
    public static Typeface GetTypeFaceForId(Context context, int typefaceID)
    {
      return GetTypefaceForRawResId(context, RawRes[typefaceID]);
    }

    /// <summary>
    /// Gets the type face path for identifier.
    /// </summary>
    /// <returns>The type face path for identifier.</returns>
    /// <param name="context">Context.</param>
    /// <param name="typefaceID">Typeface identifier.</param>
    public static string GetTypeFacePathForId(Context context, int typefaceID)
    {
      int typefaceRawResID = RawRes[typefaceID];
      string key = context.Resources.GetResourceEntryName(typefaceRawResID);

      if (!TypefacePathCache.TryGetValue(key, out string typefacePath))
      {
        GetTypefaceForRawResId(context, typefaceRawResID);
      }
      return TypefacePathCache[key];
    }

    /// <summary>
    /// Gets the typeface for raw res identifier.
    /// </summary>
    /// <returns>The typeface for raw res identifier.</returns>
    /// <param name="context">Context.</param>
    /// <param name="typefaceRawResID">Typeface raw res identifier.</param>
    public static Typeface GetTypefaceForRawResId(Context context,
        int typefaceRawResID)
    {
      string key = context.Resources.GetResourceEntryName(typefaceRawResID);

      if (!TypefaceCache.TryGetValue(key, out Typeface typeface))
      {
        string path = GetDirectory(context) + "/rawfont-" + key + ".ttf";
        if (!File.Exists(path))
        {
          using(var input = context.Resources.OpenRawResource(typefaceRawResID))
          {
            using(var output = new BufferedStream(new FileStream(path,
                                                             FileMode.OpenOrCreate,
                                                             FileAccess.Write,
                                                             FileShare.None)))
            {
              byte[] buffer = new byte[1024];
              int len;
              while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
              {
                output.Write(buffer, 0, len);
              }
            }
          }
        }

        typeface = Typeface.CreateFromFile(path);

        TypefaceCache[key] = typeface;
        TypefacePathCache[key] = path;
      }
      return typeface;
    }

    /// <summary>
    /// Gets the directory.
    /// </summary>
    /// <returns>The directory.</returns>
    /// <param name="context">Context.</param>
    private static string GetDirectory(Context context)
    {
      return context.CacheDir.Path;
    }
  }
}

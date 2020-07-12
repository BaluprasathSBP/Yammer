using System;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.Utils
{
  [ContentProperty("Text")]
  public class TranslateExtension : IMarkupExtension
  {
    readonly CultureInfo ci;

    public static ResourceManager ResourceManager { get; set; }

    public TranslateExtension()
    {
      if (   Device.RuntimePlatform == Device.iOS 
          || Device.RuntimePlatform == Device.Android)
      {
        var localizationService = DependencyService.Get<ILocalize>();

        ci = localizationService.GetCurrentCultureInfo();
      }
    }

    public static void Init(ResourceManager resourceManager)
    {
      ResourceManager = resourceManager;
    }

    public string Text { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
      if (ResourceManager == null)
      {
        throw new Exception("Please provide A resource Manager for the Transalte Extension");
      }
      if (Text == null)
      {
        return "";
      }

      var translation = ResourceManager.GetString(Text, ci);

      if (translation == null)
      {
#if DEBUG
        throw new ArgumentException($"Key {Text} was not found in resources " +
                                    "'{ResourceId}' for culture '{ci.Name}'");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
      }
      return translation;
    }
  }
}

using System.Globalization;

namespace Core.Utils
{
  public interface ILocalize
  {
    CultureInfo GetCurrentCultureInfo();

    void SetLocale(CultureInfo ci);

    CultureInfo[] GetAllCultures();
  }
}

using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Core.Utils
{
  public static class NavigationUtils
  {
    public static bool IsPageAtTop(this INavigation navigation, Type typeOfPage)
    {
      var stack = navigation.NavigationStack;

      if (stack.Count == 0)
      {
        return false;
      }
      return (stack[stack.Count - 1].GetType() == typeOfPage);
    }

    public static async Task PopToPage(this INavigation navigation, Type typeOfPage)
    {
      var stack = navigation.NavigationStack;
      var backCount = 0;

      for(var cnt = stack.Count - 1; cnt >= 0; cnt--)
      {
        if(stack[cnt].GetType() == typeOfPage)
        {
          break;
        }

        backCount++;
      }

      for (var counter = 1; counter < backCount; counter++)
      {
        navigation.RemovePage(stack[stack.Count - 2]);
      }

      await navigation.PopAsync(true);
    }

    public static async Task Pop(this INavigation navigation, int popCount)
    {
      var stack = navigation.NavigationStack;
      var backCount = popCount;

      for (var counter = 1; counter < backCount; counter++)
      {
        navigation.RemovePage(stack[stack.Count - 2]);
      }

      await navigation.PopAsync(true);
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Core.Controls
{
  public class HorizontalListView : View
  {
    public static readonly BindableProperty ItemsSourceProperty =
      BindableProperty.Create(nameof(ItemsSource),
                              typeof(IEnumerable),
                              typeof(HorizontalListView),
                              default(IEnumerable<object>),
                              BindingMode.TwoWay);

    public static readonly BindableProperty ItemTemplateProperty =
      BindableProperty.Create(nameof(ItemTemplate),
                              typeof(DataTemplate),
                              typeof(HorizontalListView), default(DataTemplate));

    public static readonly BindableProperty ItemWidthProperty =
      BindableProperty.Create(nameof(ItemWidth),
                              typeof(int),
                              typeof(HorizontalListView),
                              default(int));

    public static readonly BindableProperty ItemHeightProperty =
      BindableProperty.Create(nameof(ItemHeight),
                              typeof(int),
                              typeof(HorizontalListView),
                              default(int));

    public IEnumerable ItemsSource
    {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    public DataTemplate ItemTemplate
    {
      get { return (DataTemplate)GetValue(ItemTemplateProperty); }
      set { SetValue(ItemTemplateProperty, value); }
    }

    public int ItemWidth
    {
      get { return (int)GetValue(ItemWidthProperty); }
      set { SetValue(ItemWidthProperty, value); }
    }

    public int ItemHeight
    {
      get { return (int)GetValue(ItemHeightProperty); }
      set { SetValue(ItemHeightProperty, value); }
    }
  }
}

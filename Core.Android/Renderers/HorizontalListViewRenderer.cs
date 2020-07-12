using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Core.Controls;
using Core.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace Core.Droid.Renderers
{
  public class HorizontalListViewRenderer : ViewRenderer<HorizontalListView, RecyclerView>
  {
    public HorizontalListViewRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
    {
      base.OnElementChanged(e);

      if (Control == null)
      {
        var view = new RecyclerView(Context)
        {
          HorizontalScrollBarEnabled = false
        };
        view.SetLayoutManager(new LinearLayoutManager(Context,
                                                      LinearLayoutManager.Horizontal,
                                                      false));
        SetNativeControl(view);

        NotifyDataSetChanged();
      }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      if (e.PropertyName == HorizontalListView.ItemsSourceProperty.PropertyName)
      {
        NotifyDataSetChanged();
      }
    }

    void NotifyDataSetChanged()
    {
      var adapter = new RecyclerAdapter(Element, Element.ItemsSource?.Cast<object>()?.ToList());
      Control.SetAdapter(adapter);
    }

    class RecyclerAdapter : RecyclerView.Adapter
    {
      IList _dataSet;
      HorizontalListView _listView;

      int _templateIncrementer = 0;
      IDictionary<DataTemplate, int> _templateTypeID = new Dictionary<DataTemplate, int>();

      public RecyclerAdapter(HorizontalListView listView, IList dataSet)
      {
        _listView = listView;
        _dataSet = dataSet;
      }

      public override int ItemCount => _dataSet?.Count ?? 0;

      public override int GetItemViewType(int position)
      {
        var selector = _listView.ItemTemplate as DataTemplateSelector;
        var template = selector.SelectTemplate(_dataSet[position], _listView.Parent);

        if (!_templateTypeID.TryGetValue(template, out int key))
        {
          key = _templateIncrementer;
          _templateTypeID[template] = key;
          _templateIncrementer++;
        }
        return key;
      }

      public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
      {
        var selector = _listView.ItemTemplate as DataTemplateSelector;
        var template = _templateTypeID.FirstOrDefault(e => e.Value == viewType).Key;
        var viewCell = template.CreateContent() as ViewCell;

        if (Platform.GetRenderer(viewCell.View) == null)
        {
          Platform.SetRenderer(viewCell.View, Platform.CreateRendererWithContext(viewCell.View,
                                                                                 parent.Context));
        }
        var renderer = Platform.GetRenderer(viewCell.View);
        viewCell.Parent = _listView;

        var metrics = parent.Context.Resources.DisplayMetrics;
        var width = (int)(_listView.ItemWidth * metrics.Density);
        var height = (int)(_listView.ItemHeight * metrics.Density);

        viewCell.View.Layout(new Rectangle(0, 0, _listView.ItemWidth, _listView.ItemHeight));

        var layoutParams = new LayoutParams(width, height);

        var viewGroup = renderer.View;
        viewGroup.LayoutParameters = layoutParams;

        return new RecyclerViewHolder(viewCell, viewGroup);
      }

      public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
      {
        var data = _dataSet[position];
        (holder as RecyclerViewHolder).OnBind(data);
      }
    }

    class RecyclerViewHolder : RecyclerView.ViewHolder
    {
      ViewCell _viewCell;

      public RecyclerViewHolder(ViewCell viewCell, Android.Views.View itemView) : base(itemView)
      {
        _viewCell = viewCell;
      }

      public void OnBind(object dataContext)
      {
        _viewCell.View.BindingContext = dataContext;
      }
    }
  }
}

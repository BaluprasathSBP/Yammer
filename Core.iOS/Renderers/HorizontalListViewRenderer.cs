using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using Core.Controls;
using Core.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HorizontalListView),
                          typeof(HorizontalListViewRenderer))]
namespace Core.iOS.Renderers
{
  public class HorizontalListViewRenderer : ViewRenderer<HorizontalListView, UICollectionView>
  {

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == HorizontalListView.ItemsSourceProperty.PropertyName)
      {
        InvalidateDataSource();
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
    {
      base.OnElementChanged(e);

      if (Control == null)
      {
        var layout = new UICollectionViewFlowLayout
        {
          ScrollDirection = UICollectionViewScrollDirection.Horizontal
        };

        if (e.NewElement != null)
        {
          layout.ItemSize = new CGSize(e.NewElement.ItemWidth, e.NewElement.ItemHeight);
          layout.MinimumInteritemSpacing = 0;
          layout.MinimumLineSpacing = 0;

          var rect = new CGRect(0, 0, 100, 100);
          SetNativeControl(new UICollectionView(rect, layout));
          Control.BackgroundColor = e.NewElement?.BackgroundColor.ToUIColor();
          Control.ShowsHorizontalScrollIndicator = false;
          Control.RegisterClassForCell(typeof(InternalViewCell), nameof(InternalViewCell));
          InvalidateDataSource();
        }
      }
    }

    void InvalidateDataSource()
    {
      Control.DataSource = new ViewSource(Element as HorizontalListView);
    }

    class ViewSource : UICollectionViewDataSource
    {
      readonly HorizontalListView _listView;

      readonly IList _dataSource;

      public ViewSource(HorizontalListView view)
      {
        _listView = view;
        _dataSource = view.ItemsSource?.Cast<object>()?.ToList();
      }

      public override nint NumberOfSections(UICollectionView collectionView)
      {
        return 1;
      }

      public override nint GetItemsCount(UICollectionView collectionView, nint section)
      {
        return _dataSource?.Count ?? 0;
      }

      public override UICollectionViewCell GetCell(UICollectionView collectionView,
                                                   NSIndexPath indexPath)
      {
        InternalViewCell cell = (InternalViewCell)collectionView.DequeueReusableCell(
          nameof(InternalViewCell),
          indexPath);

        var dataContext = _dataSource[indexPath.Row];
        if (dataContext != null)
        {
          var dataTemplate = _listView.ItemTemplate;
          ViewCell viewCell;
          var selector = dataTemplate as DataTemplateSelector;
          if (selector != null)
          {
            var template = selector.SelectTemplate(_dataSource[indexPath.Row], _listView.Parent);
            viewCell = template.CreateContent() as ViewCell;
          }
          else
          {
            viewCell = dataTemplate?.CreateContent() as ViewCell;
          }

          cell.UpdateUi(viewCell, dataContext, _listView);
        }
        return cell;
      }
    }

    class InternalViewCell : UICollectionViewCell
    {
      public InternalViewCell(IntPtr p) : base(p)
      {
      }

      public void UpdateUi(ViewCell viewCell, object dataContext, HorizontalListView view)
      {
        viewCell.BindingContext = dataContext;
        viewCell.Parent = view;

        var height = (int)((view.ItemHeight + viewCell.View.Margin.Top + viewCell.View.Margin.Bottom));
        var width = (int)((view.ItemWidth + viewCell.View.Margin.Left + viewCell.View.Margin.Right));
        viewCell.View.Layout(new Rectangle(0, 0, width, height));

        if (Platform.GetRenderer(viewCell.View) == null)
        {
          Platform.SetRenderer(viewCell.View, Platform.CreateRenderer(viewCell.View));
        }
        var renderer = Platform.GetRenderer(viewCell.View).NativeView;

        foreach (UIView subView in ContentView.Subviews)
        {
          subView.RemoveFromSuperview();
        }
        renderer.ContentMode = UIViewContentMode.ScaleAspectFit;
        ContentView.AddSubview(renderer);
      }
    }
  }
}

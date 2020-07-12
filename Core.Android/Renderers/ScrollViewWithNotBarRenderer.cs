using System;
using Core.Controls;
using Core.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;


[assembly: ExportRenderer(typeof(ScrollViewWithNotBar), typeof(ScrollViewWithNotBarRenderer))]

namespace Core.Droid.Renderers
{
  public class ScrollViewWithNotBarRenderer : ScrollViewRenderer
  {
    protected override void OnElementChanged(VisualElementChangedEventArgs e)
    {
      base.OnElementChanged(e);

      if (e.OldElement != null || this.Element == null)
        return;

      if (e.OldElement != null)
        e.OldElement.PropertyChanged -= OnElementPropertyChanged;

      e.NewElement.PropertyChanged += OnElementPropertyChanged;
      this.HorizontalScrollBarEnabled = false;
      this.VerticalScrollBarEnabled = false;
    }

    protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {

      this.HorizontalScrollBarEnabled = false;
      this.VerticalScrollBarEnabled = false;
      if (ChildCount > 0)
      {
        GetChildAt(0).HorizontalScrollBarEnabled = false;
        GetChildAt(0).VerticalScrollBarEnabled = false;
      }

    }
  }
}
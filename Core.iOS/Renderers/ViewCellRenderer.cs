using Core.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ViewCell),
                          typeof(ViewCellRenderer))]
namespace Core.iOS.Renderers
{
  public class ViewCellRenderer : Xamarin.Forms.Platform.iOS.ViewCellRenderer
  {
    #region Callback methods

    public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
    {
      var viewCell = base.GetCell(item, reusableCell, tv);
      if (viewCell != null)
      {
        viewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
      }
      return viewCell;
    }

    #endregion
  }
}

using System;
using Xamarin3United.PCL;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin3United.PCL.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Linq;

[assembly: ExportRenderer (typeof(ContactViewCell), typeof(ContactViewCellRenderer))]
namespace Xamarin3United.PCL.iOS
{
	public class ContactViewCellRenderer:ViewCellRenderer
	{
		public ContactViewCellRenderer ()
		{
		}

		public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			UITableViewCell tvc = base.GetCell (item, reusableCell, tv);
			tvc.Layer.CornerRadius = 9;
			tvc.Layer.MasksToBounds = true;

			return tvc;
		}
	}
}


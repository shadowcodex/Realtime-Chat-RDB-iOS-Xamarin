using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;

namespace RCRDBXamariniOS
{
	public class TableSource : UITableViewSource {

		LinkedList<Message> TableItems;
		string CellIdentifier = "TableCell";
		nfloat height = 175;


		public TableSource (LinkedList<Message> items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{			
			return TableItems.Count;
		}


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			var cell = tableView.DequeueReusableCell (CellIdentifier) as MessageCell;
			Message item = Messages.FindMessage (TableItems, indexPath.Row);				

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new MessageCell ( CellIdentifier); }
				
			cell.UpdateCell (item.name, item.message, item.date);

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (CellIdentifier) as MessageCell;

			Message item = Messages.FindMessage (TableItems, indexPath.Row);				

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new MessageCell ( CellIdentifier); }

			cell.UpdateCell (item.name, item.message, item.date);

			CGSize size = new CGSize (tableView.Bounds.Width - 40, float.MaxValue);
			height = cell.messageLabel.Text.StringSize(cell.messageLabel.Font, size, UILineBreakMode.WordWrap).Height;
			Console.WriteLine("set height to" + (height + 44));
			return height + 44;
		}



	}
}


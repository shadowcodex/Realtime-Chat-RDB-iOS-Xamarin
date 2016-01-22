using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Drawing;

namespace RCRDBXamariniOS
{
	// This class represents the styling of a message cell. 
	public class MessageCell : UITableViewCell
	{
		public nfloat RowHeight = 0;
		public UILabel nameLabel, messageLabel, dateLabel;


		public MessageCell (string cellId) : base (UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			ContentView.BackgroundColor = UIColor.FromRGB (255, 255, 255);

			nameLabel = new UILabel () {
				Font = UIFont.FromName("GillSans", 14f),
				TextColor = UIColor.FromRGB (0, 0, 0),
				BackgroundColor = UIColor.Clear
			};
			dateLabel = new UILabel () {
				Font = UIFont.FromName("GillSans", 10f),
				TextColor = UIColor.FromRGB (0, 128, 128),
				BackgroundColor = UIColor.Clear
			};
			messageLabel = new UILabel () {
				Font = UIFont.FromName("GillSans", 18f),
				TextColor = UIColor.FromRGB (0, 0, 0),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear,
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap
			};					

			ContentView.SizeToFit ();
			ContentView.AddSubviews(new UIView[] {dateLabel, messageLabel, nameLabel});
			this.SizeToFit ();
			Console.WriteLine ("Content " + ContentView.Frame.Height);



			RowHeight = this.Frame.Bottom;
			Console.WriteLine (RowHeight);


		}

		public void UpdateCell (string name, string message, DateTime date)
		{
			nameLabel.Text = " - " + name + ":";
			messageLabel.Text = message;
			dateLabel.Text = "[" + date.ToShortDateString() + ":" + date.ToLongTimeString() + "]";
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			nameLabel.Frame = new CGRect (100, 0, ContentView.Bounds.Width, 33);
			messageLabel.Frame = new CGRect (5, 5, ContentView.Bounds.Width, ContentView.Bounds.Height);
			dateLabel.Frame = new CGRect (5, 0, ContentView.Bounds.Width, 33);
		}

	}
}


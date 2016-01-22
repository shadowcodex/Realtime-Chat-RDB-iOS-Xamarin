using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using static RCRDBXamariniOS.TableSource;
using Quobject.SocketIoClientDotNet.Client;

namespace RCRDBXamariniOS
{
	public partial class ViewController : UIViewController
	{		
		string theurl { get; set; }


		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			theurl = "http://realtime-chat.unrestrictedcoding.com";

			Messages messages = new Messages (InvokeOnMainThread, MessageBoard, theurl, Username);
			SocketUtil socket = new SocketUtil (theurl, messages);


			ChangeName.TouchUpInside += (sender, ea) => {
				messages.SetUsername();
			};

			SendMessage.TouchUpInside += (sender, ea) => {
				messages.Send(MessageText.Text);
				MessageText.Text = "";
			};

			MessageText.ShouldReturn = (sender) =>
			{
				sender.ResignFirstResponder();
				messages.Send(sender.Text);
				sender.Text = "";
				return false;
			};
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}			

	}
}


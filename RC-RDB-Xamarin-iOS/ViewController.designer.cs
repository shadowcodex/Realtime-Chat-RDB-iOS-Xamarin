// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace RCRDBXamariniOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ChangeName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView MessageBoard { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField MessageText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SendMessage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Username { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ChangeName != null) {
				ChangeName.Dispose ();
				ChangeName = null;
			}
			if (MessageBoard != null) {
				MessageBoard.Dispose ();
				MessageBoard = null;
			}
			if (MessageText != null) {
				MessageText.Dispose ();
				MessageText = null;
			}
			if (SendMessage != null) {
				SendMessage.Dispose ();
				SendMessage = null;
			}
			if (Username != null) {
				Username.Dispose ();
				Username = null;
			}
		}
	}
}

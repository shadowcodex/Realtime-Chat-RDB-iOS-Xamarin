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
		LinkedList<string> messages = new LinkedList<string>();
		string theurl = "";
		string usernamee = "";

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			theurl = "http://realtime-chat.unrestrictedcoding.com";

			GetMessages ();
			SetUsername ();

			Socket socket = IO.Socket ("http://realtime-chat.unrestrictedcoding.com");
			SetupSockets (socket);

			ChangeName.TouchUpInside += (sender, ea) => {
				SetUsername();
			};

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		private void SetupSockets(Socket socket) 
		{
			socket.Connect ();


			socket.On ("new_message", data => {				
				// get the json data from the server message
				JsonValue tempdata = JsonValue.Parse(data.ToString());
				JsonValue thedata = tempdata["new_val"];
					
				// Add message to messages
				messages.AddFirst (thedata ["name"].ToString () + thedata ["message"].ToString ());
				InvokeOnMainThread ( () => {
					SetMessages (messages);
				});

			});
		}

		private void SetUsername()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Set Username";
			alert.AddButton("OK");
			alert.AddButton("Cancel");
			alert.Message = "Please Enter a Username.";
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			alert.Clicked += (object s, UIButtonEventArgs ev) =>
			{
				if(ev.ButtonIndex ==0)
				{
					usernamee = alert.GetTextField(0).Text;
					Username.Text = usernamee;
				}
			};
			alert.Show();
		}

		public async void GetMessages()
		{			
			JsonValue values = await FetchMessages (theurl+"/message/all");
			System.Diagnostics.Debug.WriteLine("test2");
			foreach (JsonValue value in values) {
				messages.AddLast (value ["name"].ToString() + value ["message"].ToString());
			}
			SetMessages (messages);

		}

		private async Task<JsonValue> FetchMessages (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ())
			{
				
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ())
				{
					
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					Console.Out.WriteLine("Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}
		}

		private void SetMessages(LinkedList<string> mes)
		{
			System.Diagnostics.Debug.WriteLine("here");
			string[] MessageArray = new string[mes.Count];
			mes.CopyTo (MessageArray, 0);
			MessageBoard.Source = new TableSource(MessageArray);
			MessageBoard.ReloadData ();
		}
	}
}


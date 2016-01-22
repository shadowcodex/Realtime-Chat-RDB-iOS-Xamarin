using System;
using System.Collections.Generic;
using UIKit;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace RCRDBXamariniOS
{
	public class Messages
	{
		LinkedList<string> messages = new LinkedList<string>();
		private UITableView MessageBoard;
		private Action<Action> invokeui;
		string source { get; }
		private string usernamee { get; set; }
		private UILabel Username { get; set; }

		public Messages (Action<Action> invokeui, UITableView MessageBoard, string url, UILabel Username)
		{
			this.MessageBoard = MessageBoard;
			this.Username = Username;
			this.invokeui = invokeui;
			this.source = url;
			GetMessages ();
			SetUsername ();
		}
			
		// Refresh the message list with what is in the message array
		public void SetMessages()
		{			
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);
			MessageBoard.Source = new TableSource(MessageArray);
			MessageBoard.ReloadData ();
		}
			
		// Refresh the message list with what was sent to the function
		public void SetMessages(LinkedList<string> messages)
		{
			this.messages = messages;
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);
			MessageBoard.Source = new TableSource(MessageArray);
			MessageBoard.ReloadData ();
		}

		// Refresh the message list from inside a thread with what is in the message array
		public void SetMessagesUI()
		{		
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);	
			invokeui ( () => {				
				MessageBoard.Source = new TableSource(MessageArray);
				MessageBoard.ReloadData ();
			});

		}

		// Refresh the message list from inside a thread with what was sent to the function
		public void SetMessagesUI(LinkedList<string> messages)
		{
			this.messages = messages;
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);
			invokeui ( () => {				
				MessageBoard.Source = new TableSource(MessageArray);
				MessageBoard.ReloadData ();
			});
		}

		public async void Send(string message)
		{
			//{message: message, name: name} 
			var values = new NameValueCollection();
			values["message"] = message;
			values["name"] = usernamee;
			SendMessage (source + "/message/send", values);
		}

		private void SendMessage(string url, NameValueCollection message)
		{
			try {				

				using (var client = new WebClient())
				{	
					Uri uri = new Uri(url);
					var response =  client.UploadValues(uri, message);		
				}


			}

			catch (WebException exception) {
				Console.WriteLine (exception.ToString ());
			}
				

		}

		// Retrive last 100 messages from server and store into messages
		public async void GetMessages()
		{			
			JsonValue values = await FetchMessages (source+"/message/all");
			foreach (JsonValue value in values) {
				messages.AddLast (value ["name"].ToString() + value ["message"].ToString());
			}
			SetMessages ();

		}

		// Call any http open API to get JSON data back
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

		// Mutator method to add new messages
		public void Add(string item)
		{
			messages.AddFirst (item);
		}

		public void SetUsername()
		{
			invokeui ( () => {				
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
			});
		}
	}
}


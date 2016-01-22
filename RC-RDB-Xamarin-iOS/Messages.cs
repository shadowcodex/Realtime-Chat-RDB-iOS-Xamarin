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
		LinkedList<Message> messages = new LinkedList<Message>();
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
			MessageBoard.Source = new TableSource(messages);
			MessageBoard.ReloadData ();
		}
			
		// Refresh the message list with what was sent to the function
		public void SetMessages(LinkedList<Message> messages)
		{
			this.messages = messages;
			MessageBoard.Source = new TableSource(messages);
			MessageBoard.ReloadData ();
		}

		// Refresh the message list from inside a thread with what is in the message array
		public void SetMessagesUI()
		{						
			invokeui ( () => {				
				MessageBoard.Source = new TableSource(messages);
				MessageBoard.ReloadData ();
			});

		}

		// Refresh the message list from inside a thread with what was sent to the function
		public void SetMessagesUI(LinkedList<Message> messages)
		{
			this.messages = messages;
			invokeui ( () => {				
				MessageBoard.Source = new TableSource(messages);
				MessageBoard.ReloadData ();
			});
		}

		public void Send(string message)
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
				
				// Strip annoying quotes off of the values.
				// Name
				string name = value ["name"].ToString ();
				name = name.Substring(1, name.Length -2);

				// Message
				string message = value ["message"].ToString ();
				message = message.Substring (1, message.Length - 2);

				// TimeStamp
				string datestring = value ["date"].ToString ();
				datestring = datestring.Substring (1, datestring.Length - 2);
				DateTime date = Convert.ToDateTime (datestring);

				// Add message to message list
				messages.AddLast(new Message(message, name, date));
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
		public void Add(Message item)
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

		public static Message FindMessage(LinkedList<Message> messages, int index)
		{
			Message message = new Message();
			int count = 0;
			Boolean found = false;
			foreach (Message Item in messages) {
				if (count == index) {
					found = true;
					message = Item;
					break;
				}
				count++;
			}

			if (found)
				return message;
			else
				return null;
		}
	}
}


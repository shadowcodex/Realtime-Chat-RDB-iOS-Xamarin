using System;
using Quobject.SocketIoClientDotNet.Client;
using System.Json;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace RCRDBXamariniOS
{
	public class sockets
	{
		Socket socket { get; }
		string source { get; }
		LinkedList<string> messages { get; set; }
		private UITableView MessageBoard;
		private Action<Action> begininvoke;

		public sockets(string url, LinkedList<string> messages, UITableView MessageBoard, Action<Action> begininvoke)
		{
			this.begininvoke = begininvoke;
			this.messages = messages;
			this.MessageBoard = MessageBoard;
			this.source = url;
			socket = IO.Socket (url);
			socket.Connect ();
		}

		private void RecieveMessagesIO() 
		{
			socket.On ("new_message", data => {				
				// get the json data from the server message
				JsonValue tempdata = JsonValue.Parse(data.ToString());
				JsonValue thedata = tempdata["new_val"];

				// Add message to messages
				messages.AddFirst (thedata ["name"].ToString () + thedata ["message"].ToString ());
				begininvoke ( () => {
					SetMessages ();
				});

			});
		}

		// move to message handling class
		public void SetMessages()
		{			
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);
			MessageBoard.Source = new TableSource(MessageArray);
			MessageBoard.ReloadData ();
		}

		// move to message handling class
		public override void SetMessages(LinkedList<string> messages)
		{
			this.messages = messages;
			string[] MessageArray = new string[messages.Count];
			messages.CopyTo (MessageArray, 0);
			MessageBoard.Source = new TableSource(MessageArray);
			MessageBoard.ReloadData ();
		}

	}
}


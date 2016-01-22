using System;
using Quobject.SocketIoClientDotNet.Client;
using System.Json;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace RCRDBXamariniOS
{
	public class SocketUtil
	{
		
		Socket socket { get; }
		string source { get; }
		Messages Messages { get; set; }

		public SocketUtil(string url, Messages Messages)
		{
			this.Messages = Messages;
			this.source = url;
			socket = IO.Socket (url);
			socket.Connect ();
			ReceiveMessagesIO ();
		}

		private void ReceiveMessagesIO() 
		{
			socket.On ("new_message", data => {	
				System.Diagnostics.Debug.WriteLine("Message Received!");
				// get the json data from the server message
				JsonValue tempdata = JsonValue.Parse(data.ToString());
				JsonValue thedata = tempdata["new_val"];

				// Add message to messages
				Messages.Add (thedata ["name"].ToString () + thedata ["message"].ToString ());
				Messages.SetMessagesUI ();

			});
		}
	}
}


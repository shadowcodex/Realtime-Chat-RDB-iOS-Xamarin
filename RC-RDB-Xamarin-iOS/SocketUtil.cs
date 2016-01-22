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

				// Strip annoying quotes off of the values.
				// Name
				string name = thedata ["name"].ToString ();
				name = name.Substring(1, name.Length -2);

				// Message
				string message = thedata ["message"].ToString ();
				message = message.Substring (1, message.Length - 2);

				// TimeStamp
				string datestring = thedata ["date"].ToString ();
				datestring = datestring.Substring (1, datestring.Length - 2);
				DateTime date = Convert.ToDateTime (datestring);

				// Add message to messages
				Messages.Add (new Message(message, name, date));
				Messages.SetMessagesUI ();

			});
		}
	}
}


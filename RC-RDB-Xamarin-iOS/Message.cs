using System;

namespace RCRDBXamariniOS
{
	// This class is a message. It stores and holds the information needed for a message.
	public class Message
	{
		public string message { get; set; }
		public string name { get; set; }
		public DateTime date { get; set; }
		public Message (string message, string name, DateTime date)
		{
			this.message = message;
			this.name = name;
			this.date = date;
		}

		public Message()
		{
			// Nothing, just blank dude...
		}
	}
}


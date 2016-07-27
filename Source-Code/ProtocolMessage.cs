using System;
using System.Collections.Generic;
using System.Text;

namespace ProtocolLookup
{
	public class ProtocolMessage
	{
		public string MessageId = "0";
		public string MessageType = "Integer";
		public string MessageName = "Player Id";
		public string MessageDescription = "The player's id";
		public string RequiredOrOptional = "Required";

		public ProtocolMessage(string MessageId_, string MessageType_, string MessageName_, string MessageDescription_)
		{
			this.MessageId = MessageId_;
			this.MessageType = MessageType_;
			this.MessageName = MessageName_;
			this.MessageDescription = MessageDescription_;
		}

		public static ProtocolMessage FromLine(string LineToFormat, bool RequiredOrOptionalParam)
		{
			string[] LineFormat = LineToFormat.Split('|');

			//ID
			string Id = LineFormat[1].Trim().Trim('`');
			//TYPE
			string Type = LineFormat[2].Trim().Trim('`');
			//NAME
			string Name = LineFormat[3].Trim();

			ProtocolMessage Formatted;

			if (RequiredOrOptionalParam)
			{
				//REQUIRED?
				string RequiredOrOption = LineFormat[4].Trim();
				//DESC
				string Description = LineFormat[5].Trim();
				Formatted = new ProtocolMessage(Id, Type, Name, Description);
				Formatted.RequiredOrOptional = RequiredOrOption;
			} else {
				//DESC
				string Description = LineFormat[4].Trim();
				//CREATE
				Formatted = new ProtocolMessage(Id, Type, Name, Description);
			}

			return Formatted;
		}
	}
}

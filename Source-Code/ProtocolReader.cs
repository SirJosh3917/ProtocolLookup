using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ProtocolLookup
{
    public static class ProtocolReader
    {
		private static string[] LatestProtocol = null;
		public static string[] LatestProtocolDownloaded { get { return LatestProtocol; } }

		/// <summary>
		/// Downloads the protocol from the github page
		/// </summary>
		/// <returns></returns>
		public static string[] GetProtocol()
		{
			WebClient downloadProtocol = new WebClient();
			downloadProtocol.DownloadFile("https://raw.githubusercontent.com/Tunous/EverybodyEditsProtocol/master/README.md", "protocol.txt");

			string[] fileData = File.ReadAllLines("protocol.txt");
			File.Delete("protocol.txt");

			LatestProtocol = fileData;

			return fileData;
		}

		public static List<ProtocolMessage> GetMessage(string ReadOrSendMessage, string MessageName, out bool MessageFound, out string MessageDescription)
		{
			//Reuse the latest protocol downloaded for efficiency//
			if (LatestProtocolDownloaded == null)
				GetProtocol();

			List<ProtocolMessage> Messages = new List<ProtocolMessage>();

			MessageFound = false;
			MessageDescription = "Message Not Found";
			int StepOn = 0;
			bool specifyRequired = false;

			foreach(string i in LatestProtocolDownloaded)
			{

				if(MessageFound)
				{
					switch(StepOn)
					{
						case 1:
							MessageDescription = i;
							break;
						case 3:
							if (i.Contains("Required?"))
								specifyRequired = true;
							break;
						case 5:
							StepOn--;

							if(i.Trim() == "")
								return Messages;

							Messages.Add(ProtocolMessage.FromLine(i, specifyRequired));

							break;
					}
					StepOn++;
				}

				if(i == "### <a id=\"" + ReadOrSendMessage + "m-" + MessageName + "\">\"" + MessageName + "\"</a>")
				{
					MessageFound = true;
					StepOn++;
				}

			}

			return Messages;
		}

		public static List<ProtocolMessage> GetRecievedMessage(string MessageName, out bool MessageFound, out string MessageDescription)
		{
			return GetMessage("r", MessageName, out MessageFound, out MessageDescription);
		}

		public static List<ProtocolMessage> GetSendMessage(string MessageName, out bool MessageFound, out string MessageDescription)
		{
			return GetMessage("s", MessageName, out MessageFound, out MessageDescription);
		}
    }
}

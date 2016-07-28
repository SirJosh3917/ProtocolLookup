using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolLookup;
using PlayerIOClient;

namespace InitWidthHeightExample
{
	class Program
	{

		static uint WidthAt = 0,
			HeightAt = 0;

		static void Main(string[] args)
		{
			string Email = "";
			string Password = "";
			string WorldID = "";

			Console.WriteLine("Initializing ProtocolLookup");

			ProtocolReader.GetProtocol();

			Console.WriteLine("Checking message \"init\" for world width and height...");
			
			//Get Messages
			bool BooleanMessageFound;
			string StringMessageDescription;
			List<ProtocolMessage> Messages = ProtocolReader.GetRecievedMessage("init", out BooleanMessageFound, out StringMessageDescription);

			//Look through each message
			foreach (ProtocolMessage i in Messages)
			{
				//Check to see if it contains 'World'
				if (i.MessageName.ToLower().Contains("world"))
				{

					//Check to see if it contains 'Width'
					if (i.MessageName.ToLower().Contains("width"))
					{
						//Set the variable
						WidthAt = Convert.ToUInt32(i.MessageId);

						Console.WriteLine(@"Found Width:");
						Console.WriteLine(i.MessageId + " | " + i.MessageType + " | " + i.MessageName + " | " + i.MessageDescription);
					}

					//Check to see if it contains 'Height'
					if (i.MessageName.ToLower().Contains("height"))
					{
						//Set the variable
						HeightAt = Convert.ToUInt32(i.MessageId);

						Console.WriteLine(@"Found Height:");
						Console.WriteLine(i.MessageId + " | " + i.MessageType + " | " + i.MessageName + " | " + i.MessageDescription);
					}
				}
			}

			Console.WriteLine("Logging In...");

			Client client = PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", Email, Password, null);

			Console.WriteLine("Logged In! Joining World...");

			Connection con = client.Multiplayer.CreateJoinRoom(WorldID, "EverybodyEdits" + client.BigDB.Load("config", "config")["version"], true, null, null);

			Console.WriteLine("Joined World, now 'joining' world...");

			con.OnMessage += con_OnMessage;
			con.Send("init");

			Console.WriteLine("Init sent\r\nPress any key to close and stop bot.");

			Console.ReadKey();

			con.Disconnect();
			client.Logout();
		}

		static void con_OnMessage(object sender, Message e)
		{
			switch(e.Type)
			{
				case "init":
					Console.WriteLine("World width: " + e[WidthAt].ToString());
					Console.WriteLine("World height: " + e[HeightAt].ToString());
					break;
			}
		}
	}
}

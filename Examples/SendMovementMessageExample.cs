using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolLookup;
using PlayerIOClient;
using System.Threading;

namespace SendMovementMessageExample
{
	class Program
	{

		static uint WidthAt = 0,
			HeightAt = 0;

		static Connection con;

		static object[] SendMovement = null;


		static void Main(string[] args)
		{
			string Email = "robot@ee.com";
			string Password = "ee";
			string WorldID = "PW4pVxLOk6cEI";
			int X = 16;
			int Y = 16;

			Console.WriteLine("Initializing ProtocolLookup");

			ProtocolReader.GetProtocol();

			Console.WriteLine("Checking message \"m\" for correct sending...");

			List<object> SendM = new List<object>();

			//Get Messages
			bool BooleanMessageFound;
			string StringMessageDescription;
			List<ProtocolMessage> Messages = ProtocolReader.GetSendMessage("m", out BooleanMessageFound, out StringMessageDescription);

			//Look through each message
			foreach (ProtocolMessage i in Messages)
			{
				//Find X
				if(i.MessageName.ToLower() == "x")
					SendM.Add(X);

				//Find Y
				if(i.MessageName.ToLower() == "y")
					SendM.Add(Y);

				if(!(i.MessageName.ToLower() == "x" || i.MessageName.ToLower() == "y"))
				{
					//Fill in everything else with a "valid null" so to speak

					switch(i.MessageType.ToLower())
					{
						case "double":
						case "integer":
						case "uint":
							SendM.Add(0);
							break;
						case "boolean":
							SendM.Add(false);
							break;
						case "string":
							SendM.Add("");
							break;
						case "bytearray":
							SendM.Add(new byte[1]);
							break;
					}
				}
			}

			//Format list into proper object[]

			SendMovement = new object[SendM.Count];

			for (int a = 0; a < SendM.Count; a++)
				SendMovement[a] = SendM[a];

			Console.WriteLine("Logging In...");

			Client client = PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", Email, Password, null);

			Console.WriteLine("Logged In! Joining World...");

			con = client.Multiplayer.CreateJoinRoom(WorldID, "EverybodyEdits" + client.BigDB.Load("config", "config")["version"], true, null, null);

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
					Console.WriteLine("Recieved \"Init\"");
					con.Send("init2");
					break;
				case "m":
					Console.WriteLine("Somebody moved, constantly sending movement");
					int a = 0;
					while(true)
					{
						con.Send("m", SendMovement);
						Thread.Sleep(100);
						a++;
						Console.WriteLine("Sent movement message " + a + " times.");
					}
					break;
			}
		}
	}
}

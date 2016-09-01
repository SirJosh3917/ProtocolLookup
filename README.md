# ProtocolLookup
Read from the protocol directly, and add reverse compatibility to your bot.

# How it works
ProtocolLookup sends a web request to the EE Protocol, and reads directly from the protocol to give you the updated information you need.

# Download
Nuget:

`PM> Install-Package ProtocolLookup`

`https://www.nuget.org/packages/ProtocolLookup/1.0.0`

Mediafire:

`http://www.mediafire.com/download/z18sbdes5rn8gbq/ProtocolLookup.zip`

# How do I use it?
Using it is very simple.

ProtocolLookup downloads the EE Protocol, then saves it within itself, and then disposes the file it downloaded. You can redownload the protocol, or "initiaite" ProtocolLookup by doing the line of code below. However, you dont have to initialize it, the program will initialize itself when you grab a message.

`ProtocolLookup.GetProtocol()`

This returns a string[] of the protocol, and saves it within itself for quicker referencing when you want to get a message.

## What is a "ProtcolMessage"?
A ProtocolMessage is a class object containing the:
Message Id, ( MessageId - String )
Message Name, ( MessageName - String )
Message Type, ( MessageType - String )
Message Description, ( MessageDescription - String )
If the message is "Required" or "Optional" ( RequiredOrOptional - String )

And all of these can form one line of a message on the EE Protocol, such as this line:

| Id  | Type      | Name      | Description
| --- | ----      | ----      | -----------
| `0` | `Integer` | Player Id | The player's id.

### Getting Messages
You don't have to initialize it at all, but when you run one of these functions, it'll initialize itself. So these are the two commands to get a message:

Get a 'Recieve Message', replacing `init` with whatever `List<ProtocolMessage>` you want to recieve:

```
bool BooleanMessageFound;
string StringMessageDescription;
List<ProtocolMessage> Messages = ProtocolReader.GetRecievedMessage("init", out BooleanMessageFound, out StringMessageDescription);
```

Get a 'Send Message', replacing `m` with whatever `List<ProtocolMessage>` you want to recieve:

```
bool BooleanMessageFound;
string StringMessageDescription;
List<ProtocolMessage> Messages = ProtocolReader.GetSendMessage("m", out BooleanMessageFound, out StringMessageDescription);
```

And heres an example of using the `BooleanMessageFound` and `StringMessageDescription`:

```
//Check if the message has been found
if(BooleanMessageFound)
{
  //Display the message description
  Console.WriteLine(StringMessageDescription);
}
```

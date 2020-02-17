#include "network.h"
#include "RakNet/RakNetTypes.h"
#include "DirectionEvent.h"
#include "SpeedEvent.h"
#include "MessageEvent.h"
#include "ColorEvent.h"
#include "RakNet/GetTime.h"

#include "RakNet/NativeFeatureIncludes.h"
#include "RakNet/PacketizedTCP.h"
#include "RakNet/NativeTypes.h"
#include "RakNet/BitStream.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/RakAlloca.h"

Network::Network() {
	peer = RakNet::RakPeerInterface::GetInstance();
}

Network::~Network()
{

}

// Start client
// Attempt to connect with server
// username <=15
int Network::initClient(uString IP, int port, uString username)
{
	RakNet::SocketDescriptor sd;
	peer->Startup(1, &sd, 1);
	isServer = false;
	MAX_CLIENTS = 1;
	SERVER_PORT = port;

	strcpy_s(serverName, username);
	peer->Connect(IP, port, 0, 0);
	//peer->Connect("127.0.0.1:60000", 60000, 0, 0);

	eventMan = new EventManager();

	return TRUE;
}


// Start Server.
int Network::initServer(int port, uString username, int maxClients)
{
	MAX_CLIENTS = maxClients;
	RakNet::SocketDescriptor sd(port, 0);
	peer->Startup(MAX_CLIENTS, &sd, 1);
	isServer = true;
	peer->SetMaximumIncomingConnections(MAX_CLIENTS);
	strcpy_s(serverName, username);
	return TRUE;
}

int Network::cleanup() {
	RakNet::RakPeerInterface::DestroyInstance(peer);
	delete eventMan;
	return TRUE;
}

int Network::GetConnectionState() {
	return peer->GetConnectionState(peer->GetSystemAddressFromGuid(peer->GetMyGUID()));
}

int Network::readMessages()
{
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		switch (packet->data[0]) {
		case ID_REMOTE_DISCONNECTION_NOTIFICATION:
			::fprintf(stderr, "Another client has disconnected.\n");
			break;

		case ID_REMOTE_CONNECTION_LOST:
			::fprintf(stderr, "Another client has lost the connection.\n");
			break;

		case ID_REMOTE_NEW_INCOMING_CONNECTION:
			::fprintf(stderr, "Another client has connected.\n");
			break;

		case ID_CONNECTION_REQUEST_ACCEPTED:
		{
			::fprintf(stderr, "Our connection request has been accepted.\n");
		}
		if (!isServer)
		{
			//isClient do client stuff
			//send username message
		}

		case USER_SEND_USERNAME:
			break;

		case SERVER_RETURN_ACKNOWLEDGE:
			break;

		case USER_SEND_MESSAGE:
			break;

		case RECIEVE_CHAT_MESSAGE:
			break;

		case GAME_START:
			break;

		case GAME_END:
			break;

		case ID_NEW_INCOMING_CONNECTION:
			::fprintf(stderr, "A connection is incoming.\n");
			break;

		case ID_NO_FREE_INCOMING_CONNECTIONS:
			::fprintf(stderr, "The server is full.\n");
			break;

		case ID_DISCONNECTION_NOTIFICATION:
			if (isServer) {
				::fprintf(stderr, "A client has disconnected.\n");
			}
			else {
				::fprintf(stderr, "We have been disconnected.\n");
			}
			break;

		case ID_CONNECTION_LOST:
			if (isServer) {
				::fprintf(stderr, "A client lost the connection.\n");
			}
			else {
				::fprintf(stderr, "Connection lost.\n");
			}
			break;
		default:
			::fprintf(stderr, (char*)packet->data);
		}

	}
	return 0;
}

int Network::sendMessage(char* message)
{
	// peer->Send(message, sizeof(message)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetSystemAddressFromGuid(peer->GetMyGUID()), true);
	peer->Send(message, sizeof(message)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetSystemAddressFromIndex(0), true);
	return 1;
}

int Network::readMessage(char* message, int bufferSize)
{
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		if (packet) {
			strcpy_s(message, bufferSize, (char*)packet->data);

			//read in message and add to event manager
			//decode back into event

			if (isServer)
			{
				peer->Send(message, sizeof(message)*10, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
			}
			else
			{
				if (packet->data[0] > ID_USER_PACKET_ENUM + 1)
				{
					Event* nEvent = (Event*)packet->data;
					eventMan->addEvent(nEvent);
				}
	
			}
		}
	}

	return 0;
}

uString Network::getClient(int index)
{
	char* string = new char['asdf'];
	return string;
}

int Network::nSendColorEvent(float r, float g, float b)
{
	ColorEvent colorEvent = ColorEvent(r, g, b);
	colorEvent.setTime(RakNet::GetTime());
	//send message

	return peer->Send(reinterpret_cast<char*>(&colorEvent), sizeof(colorEvent)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
}

int Network::nSendDirectionEvent(int x)
{
	DirectionEvent dirEvent = DirectionEvent(x);
	dirEvent.setTime(RakNet::GetTime());
	//send message
	return peer->Send(reinterpret_cast<char*>(&dirEvent), sizeof(dirEvent), HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
}

int Network::nSendMessageEvent(char* message, int bufferSize)
{
	MessageEvent messEvent = MessageEvent(message, bufferSize);
	messEvent.setTime(RakNet::GetTime());
	//send message

	return peer->Send(reinterpret_cast<char*>(&messEvent), sizeof(messEvent)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
}

int Network::nSpeedEvent(float speed)
{
	//	Event* ev = &SpeedEvent(speed);
	SpeedEvent spdEvent = SpeedEvent(speed);
	spdEvent.setTime(RakNet::GetTime());
	//send message

	return peer->Send(reinterpret_cast<char*>(&spdEvent), sizeof(spdEvent)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
}

int Network::getEventList()
{
	return eventMan->getListLength();
}

int Network::executeEvent(char* message, int bufferSize)
{
	eventMan->executeEvent(message, bufferSize);
	return 0;
}
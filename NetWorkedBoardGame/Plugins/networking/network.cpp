#include "network.h"
#include "RakNet/RakNetTypes.h"
#include "EventManager.h"
#include "MessageEvent.h"
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
	return TRUE;
}

int Network::GetConnectionState() {
	return peer->GetConnectionState(peer->GetSystemAddressFromGuid(peer->GetMyGUID()));
}


//server read messages / send
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

//client send message
int Network::sendMessage(char* message)
{
	peer->Send(message, sizeof(message)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetSystemAddressFromGuid(peer->GetMyGUID()), true);
	return 1;
}

//client read message
int Network::readMessage(char* message, int bufferSize)
{
	if (isServer)
	{
		//recieve the message 
		packet = peer->Receive();
		if (packet) {
			strcpy_s(message, bufferSize, (char*)packet->data);

			//resend message to other client
			return 1;
		}
	}
	else
	{
		packet = peer->Receive();
		if (packet) {
			strcpy_s(message, bufferSize, (char*)packet->data);

			//read in message and add to event manager
			Event* nEvent;
			
			nEvent = new MessageEvent(message);

			if (nEvent != nullptr)
			{
				EventManager::getInstance()->addEvent(nEvent);
			}

			return 1;
		}
	}
	return 0;
}

void  Network::kickPlayer(int userID)
{

}

int Network::getClientListLength()
{
	return 1;
}

uString Network::getClient(int index)
{
	char* string = new char['asdf'];
	return string;
}

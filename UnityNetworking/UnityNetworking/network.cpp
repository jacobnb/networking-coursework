#include "network.h"

Network::Network() {
	peer = RakNet::RakPeerInterface::GetInstance();
}

Network::~Network()
{

}

// Start client
// Attempt to connect with server
// username <=15
int Network::initClient(uString IP, unsigned short port, uString username)
{
	MAX_CLIENTS = 1;
	SERVER_PORT = port;
	strcpy(serverName, username);
	peer->Connect(IP, port, 0, 0);
	return 0;
}


 // Start Server.
int Network::initServer(uString port, uString username, int maxClients)
{
	return 0;
}

int Network::cleanup() {
	RakNet::RakPeerInterface::DestroyInstance(peer);
	return TRUE;
}


int Network::sendMessage()
{
	return 0;
}


int Network::readMessages()
{
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		switch (packet->data[0]) {
		case ID_REMOTE_DISCONNECTION_NOTIFICATION:
			printf("Another client has disconnected.\n");
			break;

		case ID_REMOTE_CONNECTION_LOST:
			printf("Another client has lost the connection.\n");
			break;

		case ID_REMOTE_NEW_INCOMING_CONNECTION:
			printf("Another client has connected.\n");
			break;

		case ID_CONNECTION_REQUEST_ACCEPTED:
		{
			printf("Our connection request has been accepted.\n");
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
			printf("A connection is incoming.\n");
			break;

		case ID_NO_FREE_INCOMING_CONNECTIONS:
			printf("The server is full.\n");
			break;

		case ID_DISCONNECTION_NOTIFICATION: 
			if (isServer) {
				printf("A client has disconnected.\n");
			}
			else {
				printf("We have been disconnected.\n");
			}
			break;

		case ID_CONNECTION_LOST:
			if (isServer) {
				printf("A client lost the connection.\n");
			}
			else {
				printf("Connection lost.\n");
			}
			break;
		}
	}
	return 0;
}

void  Network::kickPlayer(int userID)
{

}

int Network::getClientListLength()
{

}

uString Network::getClient(int index)
{

}

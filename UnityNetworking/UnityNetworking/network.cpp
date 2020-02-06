#include "network.h"

Network::Network() {
	peer = RakNet::RakPeerInterface::GetInstance();
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
			// TODO
			case 
		}
	}
	return 0;
}

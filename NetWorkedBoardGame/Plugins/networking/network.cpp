#include "network.h"
#include "RakNet/RakNetTypes.h"

#include "RakNet/NativeFeatureIncludes.h"
#include "RakNet/PacketizedTCP.h"
#include "RakNet/NativeTypes.h"
#include "RakNet/BitStream.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/RakAlloca.h"
#include "RakNet/GetTime.h"

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
// TODO: move these anywhere but here.
RakNet::MessageID useTimeStamp;
RakNet::Time timeStamp;
RakNet::MessageID typeId;
int Network::sendBoidMessage(data* boids, int length) {
	RakNet::BitStream* bs = new RakNet::BitStream();
	char* arr;
	arr = (char*)malloc(sizeof(data) * length);
	arr = (char*)boids;
	::fprintf(stderr, "%f, %f, %f\n", boids[0].position.x, boids[0].position.y, boids[0].position.z);
	memcpy(arr, boids, sizeof(data) * length);
	useTimeStamp = ID_TIMESTAMP;
	timeStamp = RakNet::GetTime();
	typeId = ID_USER_PACKET_ENUM;
	bs->Write(useTimeStamp);
	bs->Write(timeStamp);
	bs->Write(typeId);
	bs->Write(arr, sizeof(data) * length);
	peer->Send(bs, HIGH_PRIORITY, RELIABLE_ORDERED, (char)0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
	//free(arr); the memory is still handled Unity side.
	return 1;
}

// any excess memory in boids will be left as is.
int Network::readBoidMessage(data* boids, int length)
{
	::fprintf(stderr, "Reading Message\n");
	packet = peer->Receive();
	if (packet) {
		RakNet::BitStream bs(packet->data, packet->length, false);
		bs.Read(useTimeStamp);
		bs.Read(timeStamp);
		bs.Read(typeId);
		const unsigned int len = min(sizeof(data) * length, packet->bitSize);
		bs.Read((char*)boids, len);
		//memcpy(boids, packet->data, len);
		::fprintf(stderr, "%f, %f, %f\n", boids[0].position.x, boids[0].position.y, boids[0].position.z);
		peer->DeallocatePacket(packet);
		return 1;
	}
	return 0;
}


int Network::sendMessage(char* message)
{
	// peer->Send(message, sizeof(message)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetSystemAddressFromGuid(peer->GetMyGUID()), true);
	peer->Send(message, sizeof(message)*3, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
	return 1;
}

int Network::readMessage(char* message, int bufferSize)
{
	packet = peer->Receive();
	if (packet) {
		strcpy_s(message, bufferSize, (char*)packet->data);
		return 1;
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

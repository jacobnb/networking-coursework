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
	serverGuidSet = false;
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

// just pass along messages to other peer.
int Network::serverMessages()
{
	::fprintf(stderr, "Routing Message ");
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		::fprintf(stderr, "got a message\n");
		
		switch (packet->data[0]) {
		case ID_TIMESTAMP: {
			RakNet::BitStream bs(packet->data, packet->length, false);
			bs.Read(useTimeStamp);
			bs.Read(timeStamp);
			bs.Read(typeId);
			::fprintf(stderr, "%i\n", typeId);
			if (typeId == BOID) {
				::fprintf(stderr, "Routing Boid Message\n");
				peer->Send(&bs, HIGH_PRIORITY, RELIABLE_ORDERED, (char)0, packet->guid, true);
			}
			else if (typeId == GAME_MESSAGE) {
				peer->Send(&bs, HIGH_PRIORITY, RELIABLE_ORDERED, (char)0, packet->guid, true);
			}
			/*break;
		case GAME_MESSAGE:*/

		//break;
	/*default:
		::fprintf(stderr, "Game Message: %i\n", typeId);
	}*/
		}
			break;
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
			//::fprintf(stderr, "Our connection request has been accepted.\n");
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
	return TRUE;
}

int Network::readMessages()
{
	::fprintf(stderr, "Reading Messages ");
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		::fprintf(stderr, "ID = %i", packet->data[0]);
		switch (packet->data[0]) {
		case ID_TIMESTAMP: {
			RakNet::BitStream bs(packet->data, packet->length, false);
			bs.Read(useTimeStamp);
			bs.Read(timeStamp);
			bs.Read(typeId);
			//switch (typeId) {
			if (typeId == BOID) {
				::fprintf(stderr, "Reading in boid message\n");
				int len;
				bs.Read(len);
				data* boids = (data*)malloc(len);
				bs.Read((char*)boids, len);
				boidMessages.push(boids);
				char* message = (char*)malloc(sizeof(int) * 2);
				sprintf(message, "0%d", len);
				gameMessages.push(GameMessage(message, len));
			}
			else if (typeId == GAME_MESSAGE) {
				char* gmessage = (char*)malloc(packet->bitSize);
				bs.Read(gmessage, packet->bitSize);
				gameMessages.push(GameMessage(gmessage, packet->bitSize));
			}
			else {
				::fprintf(stderr, "Unkown ID: %i", typeId);
			}
			/*break;
		case GAME_MESSAGE:*/

		//break;
	/*default:
		::fprintf(stderr, "Game Message: %i\n", typeId);
	}*/
		}
			break;
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
			serverGuid.rakNetGuid = packet->guid;
			serverGuidSet = true;
			::fprintf(stderr, "Our connection request has been accepted.\n");
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
	return 1;
}

int Network::sendBoidMessage(data* boids, int length) {
	::fprintf(stderr, "Sending Message\n");
	RakNet::BitStream* bs = new RakNet::BitStream();
	int len = sizeof(data) * length;
	char* arr;
	arr = (char*)malloc(len);
	arr = (char*)boids;
	//::fprintf(stderr, "%f, %f, %f\n", boids[0].position.x, boids[0].position.y, boids[0].position.z);
	memcpy(arr, boids, len);
	useTimeStamp = ID_TIMESTAMP;
	timeStamp = RakNet::GetTime();
	typeId = BOID;
	bs->Write(useTimeStamp);
	bs->Write(timeStamp);
	bs->Write(typeId);
	bs->Write(len);
	bs->Write(arr, len);
	if (!serverGuidSet && !isServer) {
		peer->Send(bs, HIGH_PRIORITY, RELIABLE_ORDERED, (char)0, serverGuid, false);
	}
	else {
		peer->Send(bs, HIGH_PRIORITY, RELIABLE_ORDERED, (char)0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
		::fprintf(stderr, "Server GUID Not Defined");
	}
	//free(arr); the memory is still handled Unity side.
	return 1;
}

int Network::readBoidMessage(data* boids, int length)
{
	int len = sizeof(data) * length;
	::fprintf(stderr, "Reading Boid Message");
	if (boidMessages.empty()) {
		return 0;
	}
	//boids = boidMessages.front();
	memcpy(boids, boidMessages.front(), len);
	boidMessages.pop();
	::fprintf(stderr, " %f\n", boids[0].position.y);
	return 1;
}


int Network::sendMessage(char* message)
{
	RakNet::BitStream* bs = new RakNet::BitStream();
	useTimeStamp = ID_TIMESTAMP;
	timeStamp = RakNet::GetTime();
	typeId = GAME_MESSAGE;
	bs->Write(useTimeStamp);
	bs->Write(timeStamp);
	bs->Write(typeId);
	bs->Write(message);
	peer->Send(bs, HIGH_PRIORITY, RELIABLE_ORDERED, 0, peer->GetGuidFromSystemAddress(RakNet::UNASSIGNED_SYSTEM_ADDRESS), true);
	return 1;
}

int Network::readMessage(char* message, int bufferSize)
{
	if (gameMessages.empty()) {
		return 0;
	}
	strcpy_s(message, bufferSize, gameMessages.front().message);
	gameMessages.pop();
	return 1;
}


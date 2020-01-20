#include "networking.h"

Networking::Networking()
{
	peer = RakNet::RakPeerInterface::GetInstance();
	MAX_CLIENTS = 10;
	SERVER_PORT = 60000;
}

Networking::~Networking()
{
	if (peer != nullptr) {
		RakNet::RakPeerInterface::DestroyInstance(peer);
	}
}

bool Networking::init()
{
	printf("Set MAX_CLIENTS (default 10)\n");
	fgets(str, 512, stdin);
	int numInput = std::atoi(str);
	numInput == 0 ? 1 : MAX_CLIENTS = numInput;

	printf("Set SERVER_PORT (default 60000)\n");
	fgets(str, 512, stdin);
	int clients = std::atoi(str);
	numInput == 0 ? 1 : SERVER_PORT = numInput;

	printf("(C) or (S)erver?\n");
	fgets(str, 512, stdin);
	if ((str[0] == 'c') || (str[0] == 'C'))
	{
		RakNet::SocketDescriptor sd;
		peer->Startup(1, &sd, 1);
		isServer = false;
	}
	else {
		RakNet::SocketDescriptor sd(SERVER_PORT, 0);
		peer->Startup(MAX_CLIENTS, &sd, 1);
		isServer = true;
	}

	if (isServer)
	{
		printf("Starting the server.\n");
		// We need to let the server accept incoming connections from the clients
		peer->SetMaximumIncomingConnections(MAX_CLIENTS);
	}
	else {
		printf("Enter server IP or hit enter for 127.0.0.1\n");
		fgets(str, 512, stdin);
		if (str[0] == 0) {
			strcpy(str, "127.0.0.1:60000");
		}
		printf("Starting the client.\n");
		peer->Connect(str, SERVER_PORT, 0, 0);

	}

	return true;
}

void Networking::update()
{
	// get packet; if packet; get packet;
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		switch (packet->data[0])
		{
		case ID_CLIENT_TO_SERVER:
		{
			//recieve and print message
			printf("Message recieved from client \n");
			messageData message = *(messageData*)packet->data;
			printf("%s\n", message.mes);

			//request message and send to client with server to client id
			printf("What's your message?\n");
			fgets(str, 512, stdin);
			messageData msOut(ID_SERVER_TO_CLIENT, str);
			// https://www.leadwerks.com/community/topic/4222-raknet-send-and-receive-struct/
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
		}
		break;
		case ID_SERVER_TO_CLIENT:
		{
			//recieve and print message
			printf("Message recieved from server\n");
			messageData message = *(messageData*)packet->data;
			printf("%s\n", message.mes);

			//request message and send to server with client to server id
			printf("What's your message?\n");
			fgets(str, 512, stdin);
			messageData msOut(ID_CLIENT_TO_SERVER, str);
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
		}
		break;
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

			// Use a BitStream to write a custom user message
			// Bitstreams are easier to use than sending casted structures, and handle endian swapping automatically
			RakNet::BitStream bsOut;
			bsOut.Write((RakNet::MessageID)ID_GAME_MESSAGE_1);
			bsOut.Write("Hello world");
			peer->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
		}
		// request for message, then it will send message to server using client to server id
		// http://www.raknet.net/raknet/manual/creatingpackets.html
		if (!isServer) {
			printf("What's your message?\n");
			fgets(str, 512, stdin);
			messageData msOut(ID_CLIENT_TO_SERVER, str);
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);

		}
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
		case ID_GAME_MESSAGE_1:
		{

			RakNet::RakString rs;
			RakNet::BitStream bsIn(packet->data, packet->length, false);
			bsIn.IgnoreBytes(sizeof(RakNet::MessageID));
			bsIn.Read(rs);
			printf("%s\n", rs.C_String());

		}
		break;
		default:
			printf("Message with identifier %i has arrived.\n", packet->data[0]);
			break;
		}
	}
}

void Networking::cleanup()
{
	RakNet::RakPeerInterface::DestroyInstance(peer);
	peer = nullptr;
}

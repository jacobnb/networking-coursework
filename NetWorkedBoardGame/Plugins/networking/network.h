#pragma once
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#include <vector>
#include <string>
#include "NetworkPackageStructs.h"
#include <queue>
#define uString char* //Whatever we use to transfer string data from Unity



class Network {
private:
	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;
	std::queue<data *> boidMessages;
	std::queue<GameMessage> gameMessages;
	RakNet::MessageID useTimeStamp;
	RakNet::Time timeStamp;
	RakNet::MessageID typeId;
	bool isServer;
	RakNet::AddressOrGUID serverGuid;
	bool serverGuidSet;
	RakNet::Packet* packet;

	//client data
	RakNet::SystemAddress serverAddress;
	int clientID;

	//server data
	std::vector<clientData> clientList;
	char serverName[16];

	int playerOneID; //if the id is -1 then it is server
	int playerTwoID;

public:
	RakNet::RakPeerInterface* peer;

	Network();
	~Network();
	int initClient(uString IP, int port, uString username);
	int initServer(int port, uString username, int maxClients = 10);
	int cleanup();
	// http://www.jenkinssoftware.com/raknet/manual/Doxygen/namespaceRakNet.html#84a0fb005391f71130dd341f77f62138
	int GetConnectionState();
	int readMessages();
	int sendMessage(char* message);
	int sendBoidMessage(data* boids, int length);
	int readBoidMessage(data* boids, int length);
	int readMessage(char* message, int bufferSize);
	int serverMessages();
};
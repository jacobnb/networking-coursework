#pragma once
#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#include <vector>
#include <string>
#define RackNet RakNet

#define uString char* //Whatever we use to transfer string data from Unity
enum GameMessages
{
	TURN_MOVE= ID_USER_PACKET_ENUM +1,
};

struct clientData {
	std::string username;
	int ID;
};
class Network {
private:
	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;

	RakNet::RakPeerInterface* peer;
	bool isServer;

	RakNet::Packet* packet;

	//client data
	RakNet::SystemAddress serverAddress;

	//server data
	std::vector<clientData> clientList;
	char serverName[16];
public:
	Network();
	~Network();
	int initClient(uString IP, unsigned short port, uString username);
	int initServer(uString port, uString username, int maxClients = 10);
	int cleanup();
	int sendMessage();
	int readMessages();
	int getClientListLength();
	uString getClient(int index);
};
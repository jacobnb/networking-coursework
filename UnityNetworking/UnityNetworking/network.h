#pragma once
#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#include <vector>
#include <string>
#include "NetworkPackageStructs.h"
#define RackNet RakNet

#define uString char* //Whatever we use to transfer string data from Unity
enum GameMessages
{
	TURN_MOVE= ID_USER_PACKET_ENUM +1,
	USER_SEND_USERNAME,
	SERVER_RETURN_ACKNOWLEDGE,
	USER_SEND_MESSAGE,
	RECIEVE_CHAT_MESSAGE,
	KICK_USER,
	GAME_START,
	GAME_END
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
	int clientID;

	//server data
	std::vector<clientData> clientList;
	char serverName[16];

	int playerOneID; //if the id is -1 then it is server
	int playerTwoID; 

public:
	Network();
	~Network();
	int initClient(uString IP, unsigned short port, uString username);
	int initServer(uString port, uString username, int maxClients = 10);
	int cleanup();
	int sendMessage();
	int readMessages();

	//server functions
	void kickPlayer(int userID);
	int getClientListLength();
	uString getClient(int index);
};
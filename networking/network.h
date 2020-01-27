#pragma once
#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#include "MessageData.h"
#include "clientData.h"
#include <vector>
#define RackNet RakNet

enum GameMessages
{
	ID_SEND_MESSAGE = ID_USER_PACKET_ENUM + 1,
	ID_SEND_USERNAME,
	ID_CLIENT_TO_SERVER,
	CLIENT_SEND_MESSAGE
};



class Network
{
private:

	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;


	RakNet::RakPeerInterface* peer;
	bool isServer;
	RakNet::Packet* packet;
	char str[MESSAGE_LENGTH];


	char curMsg[MESSAGE_LENGTH];
	int cursor; //current location in message;

	//client data
	RakNet::SystemAddress serverAddress;

	//server data
	std::vector<clientData> clientList;
	char serverName[16];
	void dispCurrentMessage();
	void clearAsyncKeyBuffers();

public:
	Network();
	~Network();
	int init();
	int cleanup();
	void update();
	char checkKeyboardState();

	//server functions
	clientData getClient(RakNet::SystemAddress userAddress);
	clientData getClient(char userName[16]);
	void sendPublicMessage(messageData msgData, RakNet::SystemAddress originClient);
	void sendPublicServerMessage(char msg[MESSAGE_LENGTH]);
};
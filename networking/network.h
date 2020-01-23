#pragma once
#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#define RackNet RakNet

enum GameMessages
{
	ID_SEND_MESSAGE = ID_USER_PACKET_ENUM + 1,
	ID_SEND_USERNAME,
	ID_CLIENT_TO_SERVER,
	ID_SERVER_TO_CLIENT
};


class Network
{
private:

	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;

	RakNet::RakPeerInterface* peer;
	bool isServer;
	RakNet::Packet* packet;
	char str[511];

	//TODO: client list

public:
	Network();
	~Network();
	int init();
	int cleanup();
	void update();
	char checkKeyboardState();
};
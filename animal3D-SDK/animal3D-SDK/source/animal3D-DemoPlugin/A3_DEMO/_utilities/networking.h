#pragma once

#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include <string.h>
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#define RackNet RakNet

enum GameMessages
{
	ID_GAME_MESSAGE_1 = ID_USER_PACKET_ENUM + 1,
	ID_CLIENT_TO_SERVER,
	ID_SERVER_TO_CLIENT
};

#pragma pack(push, 1)
struct messageData
{
	/*unsigned char useTimeStamp;
	RakNet::Time timeStamp;*/
	unsigned char typeId; // Your type here
	// message data string
	char mes[512];

	messageData(unsigned char id, char inMess[]) :typeId(id) { strcpy(mes, inMess); }
};
#pragma pack(pop)

class Networking
{
private:
	unsigned int MAX_CLIENTS;
	unsigned short SERVER_PORT;
	char str[512];
	RakNet::RakPeerInterface* peer;
	bool isServer;
	RakNet::Packet* packet;
public:
	Networking();
	~Networking();
	bool init();
	void update();
	void cleanup();
};

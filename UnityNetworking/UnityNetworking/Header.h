#pragma once
#include <stdio.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/BitStream.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#include <vector>
#define RackNet RakNet


enum GameMessages
{
	SELECT_MOVE,
};
class Network
{
private:
	bool isServer;
	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;

	RakNet::RakPeerInterface* peer;

};
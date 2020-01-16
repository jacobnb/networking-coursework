// networking.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <stdio.h>
#include "RakNet/RakPeerInterface.h"

#define RackNet RakNet

int main(void)
{
	// TODO: make these user defined.
	unsigned int MAX_CLIENTS = 10;
	unsigned short SERVER_PORT = 60000;


	char str[512];
	RakNet::RakPeerInterface* peer = RakNet::RakPeerInterface::GetInstance();
	bool isServer;

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


	// TODO - Add code body here

	RakNet::RakPeerInterface::DestroyInstance(peer);

	return 0;
}
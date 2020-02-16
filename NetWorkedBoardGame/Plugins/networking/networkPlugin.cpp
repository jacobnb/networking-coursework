#include "pch.h"
#include "networkPlugin.h"
#include "Event.h"
#include "DirectionEvent.h"
#include "SpeedEvent.h"
#include "MessageEvent.h"
#include "ColorEvent.h"
#include <memory>
#include <vector>

//BEATER-MAN NOTE:  netID is used for when multiple instances of the same thing

// I'm setting this up to have multiple instances for local testing.
Network* instances[5];
int length; //active length

// @param: Number of instances planned to be initialized.
int initNetwork(int numNetworkInstances) {
	length = 0;
	return 1337;
}

// @return: ID to access network instance.
netID getNetworkInstance() {
	instances[length] = new Network();
	return length++;
}

int initClient(uString IP, int port, uString username, netID ID) {
	if (instances[ID]) {
		instances[ID]->initClient(IP, port, username);
		::fprintf(stderr, "init client\n");

		//init event manager
		

		return TRUE;
	}
	return FALSE;
}

int initServer(int port, uString username, int maxClients, netID ID) {
	if (instances[ID]) {
		instances[ID]->initServer(port, username, maxClients);
		::fprintf(stderr, "init server\n");
		return TRUE;
	}
	return FALSE;
}

int cleanup(netID ID) {
	if (instances[ID]) {
		instances[ID]->cleanup();
		delete instances[ID];
		instances[ID] = 0;
		return TRUE;
	}
	return FALSE;
}

int sendMessage(char* message, netID ID) {
	if (instances[ID]) {
		instances[ID]->sendMessage(message);
		return TRUE;
	}
	return FALSE;
}

int readMessage(char* message, int bufferSize, netID ID) {
	if (instances[ID]) {
		return instances[ID]->readMessage(message, bufferSize);
	}
	return -1;
}

int checkConnection(netID ID)
{
	if (instances[ID]){
		return instances[ID]->GetConnectionState();
	}
	return -1;
}

bool server;
bool client = 0;
int doEverything(bool isServer)
{
	if (isServer ) {
		if (!server) {
			::fprintf(stderr, "init server\n");
			initNetwork(5);
			//server = new Network();
			//server->initServer(60000, (char*)"jacob", 3);
			getNetworkInstance();
			initServer(60000, (char*)"Jacob");
			server = 1;
		}
		else {
			instances[0]->readMessages();
		}
	}
	if(!isServer) {
		if (!client) {
			::fprintf(stderr, "init client\n");
			getNetworkInstance();
			initClient((char*)"127.0.0.1", 60000, (char*)"bob", 1);
			//client = new Network();
			//client->initClient((char*)"127.0.0.1", 60000, (char*)"jacob");
			client = 1;
		} else
		{
			instances[1]->sendMessage((char*)"Howdy from client");
			instances[1]->readMessages();
			//client->readMessages();
			//client->readMessages();
		}
	}
	return 0;
}

int sendColorEvent(float r, float g, float b, netID ID = 0)
{
	instances[ID]->nSendColorEvent(r,g,b);
	//send message
}

int sendDirectionEvent(float x, float y, float z, netID ID = 0)
{
  //create event
	instances[ID]->nSendDirectionEvent(x,y,z);
	//send message
}

int sendMessageEvent(char* message, netID ID = 0)
{
	//create event
	instances[ID]->nSendMessageEvent(message);
	//send message
}

int sendSpeedEvent(float speed, netID ID = 0)
{
	//create event

	instances[ID]->nSpeedEvent(speed);
	//send message
}

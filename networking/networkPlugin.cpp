#include "pch.h"
#include "networkPlugin.h"
#include <memory>
#include <vector>

#define dogs std::vector<std::unique_ptr<Network>> // Hannah wanted the datatype to be dogs

// I'm setting this up to have multiple instances for local testing.
dogs instances;
int length; //active length

int testInt(int num) {
	return 42;
}
int testString(char* str) {
	return 42;
}
char* testString2(char* str) {
	return new char['h'];
}
char* testString3() {
	return new char['h'];
}

// @param: Number of instances planned to be initialized.
int initNetwork(int numNetworkInstances) {
	instances.resize(numNetworkInstances);
	length = 0;
	return 1337;
}

// @return: ID to access network instance.
netID getNetworkInstance() {
	instances.push_back(std::make_unique<Network>());
	return length++;
}

int initClient(netID ID, uString IP, int port, uString username) {
	if (instances[ID]) {
		instances[ID]->initClient(IP, port, username);
		// return TRUE;
	}
	// return FALSE;
	return 42;
}

int initServer(netID ID, uString port, uString username, int maxClients) {
	if (instances[ID]) {
		instances[ID]->initServer(port, username, maxClients);
		return TRUE;
	}
	return FALSE;
}

int cleanup(netID ID) {
	if (instances[ID]) {
		instances[ID]->cleanup();
		instances[ID].release();
		return TRUE;
	}
	return FALSE;
}


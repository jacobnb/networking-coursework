#ifndef  clientData_H
#define clientData_H
#include <string.h>
#include "RakNet/RakPeerInterface.h"
#include "RakNet/MessageIdentifiers.h"
#include "RakNet/RakNetTypes.h"  // MessageID
#define RackNet RakNet

struct clientData
{
	char userName[16];
	RakNet::SystemAddress clientAddress;

	clientData() {
		userName[0] = -1;
	}
	clientData(char newUser[], RakNet::SystemAddress newAddress) : clientAddress(newAddress) { strcpy(userName, newUser); }

};

#endif // ! clientData_H
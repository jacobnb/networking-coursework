#pragma once
//#ifndef MessageData_H
//#define MessageData_H
#include <string.h>

//message has no direct destination
const char MD_NO_DESTINATION[16] = "000000000000000";
const int MESSAGE_LENGTH = 510;
const int USERNAME_LENGTH = 16;
#pragma pack(push, 1)
struct messageData
{
	/*unsigned char useTimeStamp;
	RakNet::Time timeStamp;*/
	unsigned char typeId; // Your type here
	// message data string, should be in form of 2
	char mes[MESSAGE_LENGTH];

	//destination username or send username if being recieved as client
	char userName[USERNAME_LENGTH];

	bool privateMessage;
	messageData(unsigned char id, char inMess[], bool isPrivate, const char userN[] = MD_NO_DESTINATION) :typeId(id), privateMessage(isPrivate) { strcpy(mes, inMess); strcpy(userName, userN); }
};
#pragma pack(pop)


//#endif // MessageData_H

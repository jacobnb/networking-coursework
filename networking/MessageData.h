#ifndef MessageData_H
#define MessageData_H
#include <string.h>

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


#endif // MessageData_H

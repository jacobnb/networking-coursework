#ifndef EVENTMANANAGER_H
#define EVENTMANAGER_H

#include "Event.h"
#include "EventList.h"
class EventManager
{
public:
	EventManager();
	~EventManager();

	int executeEvent(char* message, int bufferSize);
	bool addEvent(Event* newEvent);
	int getListLength();
private:

	
	ListNode* mHead;
	int nodeCount;
};



#endif // !EVENTMANA

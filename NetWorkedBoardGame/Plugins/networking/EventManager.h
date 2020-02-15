#ifndef EVENTMANANAGER_H
#define EVENTMANAGER_H

#include "Event.h"
#include "EventList.h"
class EventManager
{
public:

	~EventManager();

	static EventManager* getInstance();

	char* executeEvent();
	bool addEvent(Event* newEvent);
	int getListLength();
private:
	EventManager();
	
	static EventManager* instance;

	ListNode* mHead;
	int nodeCount;
};



#endif // !EVENTMANA

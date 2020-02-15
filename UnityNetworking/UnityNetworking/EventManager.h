#ifndef EVENTMANANAGER_H
#define EVENTMANAGER_H

#include "Event.h"
#include "EventList.h"
class EventManager
{
public:
	EventManager();
	~EventManager();


	bool addEvent(eventType eventID, Event* newEvent);
private:
	ListNode* head;
	int nodeCount;
};

EventManager::EventManager()
{
}

EventManager::~EventManager()
{
}

#endif // !EVENTMANA

#ifndef EVENTMANANAGER_H
#define EVENTMANAGER_H

#include "Event.h"
class EventManager
{
public:
	EventManager();
	~EventManager();


	bool addEvent(eventType eventID, Event* newEvent);
private:
	
};

EventManager::EventManager()
{
}

EventManager::~EventManager()
{
}

#endif // !EVENTMANA

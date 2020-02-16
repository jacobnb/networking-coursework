#ifndef MESSAGEEVENT_H
#define MESSAGEEVENT_H

#include "Event.h"

class MessageEvent : public Event
{
	public:
	MessageEvent(char* message);
	void executeOrder(char* message) ;
	private:

	char* mMessage;
};

#endif // !MESSAGEEVENT_H

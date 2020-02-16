#ifndef MESSAGEEVENT_H
#define MESSAGEEVENT_H

#include "Event.h"

class MessageEvent : public Event
{
	public:
	MessageEvent(char* message, int bufferSize);
	void executeOrder(char* message, int bufferSize) ;
	private:

	char* mMessage;
};

#endif // !MESSAGEEVENT_H

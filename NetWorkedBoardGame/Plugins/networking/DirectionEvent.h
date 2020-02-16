#ifndef DIRECTIONEVENT_H
#define DIRECTIONEVENT_H

#include "Event.h"

class DirectionEvent : public Event
{
	public:
	DirectionEvent(int ix);
	void executeOrder(char* message, int bufferSize);
	private:

	int x;
};
#endif // !DIRECTIONEVENT_H

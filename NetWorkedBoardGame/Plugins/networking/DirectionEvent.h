#ifndef DIRECTIONEVENT_H
#define DIRECTIONEVENT_H

#include "Event.h"

class DirectionEvent : public Event
{
	public:
	DirectionEvent(float ix, float iy, float iz);
	void executeOrder(char* message);
	private:

	float x;
	float y;
	float z;

};
#endif // !DIRECTIONEVENT_H

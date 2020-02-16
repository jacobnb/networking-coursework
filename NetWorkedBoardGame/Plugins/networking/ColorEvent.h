#ifndef COLOREVENT_H
#define COLOREVENT_H

#include "Event.h"

class ColorEvent : public Event
{
	public:
	ColorEvent(float ir, float ig, float ib);
	virtual void executeOrder(char* message, int bufferSize);

	private:
	
	float r;
	float g;
	float b;
};

#endif // !COLOREVENT_H

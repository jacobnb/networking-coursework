#ifndef SPEEDEVENT_H
#define SPEEDEVENT_H

#include "Event.h"

class SpeedEvent : public Event
{
	public: 
	
	SpeedEvent(float iSpeed);
	void executeOrder(char* message);

	private:
	float speed;
};

#endif // !SPEEDEVENT_H

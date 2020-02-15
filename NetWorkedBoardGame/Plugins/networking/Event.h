#ifndef EVENT_H
#define EVENT_H

enum eventType
{
	COLOR_EVENT,
	DIRECTION_EVENT,
	SPEED_EVENT,
	MESSAGE_EVENT
};

class Event
{
public:
	Event();
	~Event();

	float timeStamp;
private:

	virtual void executeOrder() = 0;
};

#endif // !EVENT_H
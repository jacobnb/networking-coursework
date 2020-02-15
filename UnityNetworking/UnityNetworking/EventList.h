#ifndef EVENTLIST_H
#define EVENTLIST_H

#include "Event.h"

class ListNode
{
public:
	ListNode();
	~ListNode();

	Event* getEvent();
private:
	ListNode* next;	

	Event* mEvent;
};


#endif // !EVENTLIST_H

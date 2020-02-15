#ifndef EVENTLIST_H
#define EVENTLIST_H

#include "Event.h"

class ListNode
{
public:
	ListNode();
	~ListNode();

	Event* getEvent();
	ListNode* getNext();
private:
	ListNode* next;	

	Event* mEvent;
};


#endif // !EVENTLIST_H

#ifndef EVENTLIST_H
#define EVENTLIST_H

#include "Event.h"

class ListNode
{
public:
	ListNode(Event* newEvent);
	~ListNode();

	Event* getEvent();
	ListNode* getNext();
	void setNext(ListNode* newNext);
private:
	ListNode* next;

	Event* mEvent;
};


#endif // !EVENTLIST_H

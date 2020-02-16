#include "EventList.h"

ListNode::ListNode(Event* newEvent)
{
	mEvent = newEvent;
}

ListNode::~ListNode()
{
	delete mEvent;
}

Event* ListNode::getEvent()
{
	return mEvent;
}

ListNode* ListNode::getNext()
{
	return next;
}

void ListNode::setNext(ListNode* newNext)
{
	next = newNext;
}
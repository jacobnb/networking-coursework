#include "EventManager.h"

EventManager::EventManager()
{
	nodeCount = 0;
}

EventManager::~EventManager()
{
}

int EventManager::executeEvent(char* message, int bufferSize)
{

	//find current
	ListNode* current = mHead;
	
	if (current == nullptr)
	{
		return -1;
	}

	//reset head to the next one
	mHead = mHead->getNext();

	//get event
	current->getEvent()->executeOrder(message, bufferSize);
	//delete event and current
	delete current->getEvent();

	delete current;

	return 0;
}

bool EventManager::addEvent(Event* newEvent)
{
	//iterate through eventlist, then check time stamp,
	//if the next node's time stamp is greater than it then insert there
	bool inserted = false;
	ListNode* current = mHead;

	//if the list is empty create new head
	if (mHead == nullptr)
	{
		inserted = true;
		mHead = new ListNode(newEvent);
	}

	//if the head is higher than the new event, create new head
	if (mHead->getEvent()->getTime() >= newEvent->getTime())
	{
		inserted = true;
		ListNode* newHead = new ListNode(newEvent);	
		newHead->setNext(mHead);
		mHead = newHead;
	}

	//iterate until inserted
	while (!inserted)
	{
		//if at the end of the list
		if (current->getNext() == nullptr)
		{
			//current is last in line
			inserted = true;
			current->setNext(new ListNode(newEvent));
		}
		//if the next node has greater time stamp than current event
		else if(current->getNext()->getEvent()->getTime() >= newEvent->getTime())
		{
			//insert and relink the nodes
			inserted = true;
			ListNode* newNode = new ListNode(newEvent);
			newNode->setNext(current->getNext());
			current->setNext(newNode);
		}
		else
		{
			//iterate next
			current = current->getNext();
		}
	}

	return true;
}

int EventManager::getListLength()
{
	ListNode* currentNode = mHead;
	int count = 1;

	if (mHead == nullptr)
	{
		return 0;
	}

	while (currentNode->getNext() != nullptr)
	{
		currentNode = currentNode->getNext();
		count++;
	}

	return count;
}
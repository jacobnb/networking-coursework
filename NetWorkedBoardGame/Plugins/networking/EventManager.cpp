#include "EventManager.h"

EventManager* EventManager::getInstance()
{
	if (instance == nullptr)
	{
		instance = new EventManager();
	}

	return instance;
}

EventManager::EventManager()
{
	nodeCount = 0;
}

EventManager::~EventManager()
{
	delete instance;
}

char* EventManager::executeEvent()
{
	//return strign event
}

bool EventManager::addEvent(Event* newEvent)
{
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
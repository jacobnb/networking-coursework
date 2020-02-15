#include "MessageEvent.h"

MessageEvent::MessageEvent(char* message)
{
	mMessage = nullptr;
}

void MessageEvent::executeOrder(char* message)
{
	message = mMessage;
}
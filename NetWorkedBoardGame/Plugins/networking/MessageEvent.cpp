#include "MessageEvent.h"

MessageEvent::MessageEvent(char* message)
{
	mMessage = message;
}

void MessageEvent::executeOrder(char* message)
{
	message = mMessage;
}
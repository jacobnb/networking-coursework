#include "MessageEvent.h"
#include <string>
MessageEvent::MessageEvent(char* message, int bufferSize)
{
	//mMessage is a char*
	strcpy_s(mMessage, bufferSize, message);
}

void MessageEvent::executeOrder(char* message, int bufferSize)
{
	strcpy_s(message, bufferSize, mMessage);
}
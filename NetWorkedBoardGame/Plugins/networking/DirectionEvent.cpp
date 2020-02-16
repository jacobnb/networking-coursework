#include "DirectionEvent.h"
#include <string>
DirectionEvent::DirectionEvent(int ix)
{
	x = ix;

}

void DirectionEvent::executeOrder(char* message, int bufferSize)
{
	strcpy_s(message,bufferSize, std::to_string(x).c_str());
}
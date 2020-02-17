#include "SpeedEvent.h"
#include <string>
SpeedEvent::SpeedEvent(float iSpeed)
{
	speed = iSpeed;
}

void SpeedEvent::executeOrder(char* message, int bufferSize)
{
	strcpy_s(message, bufferSize, (std::to_string(speed)).c_str());
}
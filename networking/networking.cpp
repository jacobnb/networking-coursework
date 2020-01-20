#include "network.h"

int main(void)
{
	Network* newNet = new Network();
	newNet->init();
	while (1) {
		newNet->update();
	}
	newNet->cleanup();
	delete newNet;
}
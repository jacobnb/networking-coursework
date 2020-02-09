#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H


#define MYUNITYPLUGIN_EXPORT

#define netID int
#include "Lib.h"
#include "network.h"

/*
* expose C functions for Unity.
* Use only primitive types b/c mapping is hard with structs / strings.
*
*
*
*
*/
#define __cplusplus
#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif // __cplusplus

MYUNITYPLUGIN_SYMBOL int initNetwork(int numNetworkInstances);
MYUNITYPLUGIN_SYMBOL netID getNetworkInstance();
MYUNITYPLUGIN_SYMBOL int initClient(netID ID, uString IP, unsigned short port, uString username);
MYUNITYPLUGIN_SYMBOL int initServer(netID ID, uString port, uString username, int maxClients = 10);
MYUNITYPLUGIN_SYMBOL int cleanup(netID ID);
__declspec(dllexport) int testInt(int num);
MYUNITYPLUGIN_SYMBOL int testString(char* str);
MYUNITYPLUGIN_SYMBOL char* testString2(char* str);
MYUNITYPLUGIN_SYMBOL char* testString3();

#ifdef __cplusplus
} // extern "C"
#else // !__cplusplus
#endif // __cplusplus


#endif // MYUNITYPLUGIN_H
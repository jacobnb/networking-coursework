#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "Lib.h"

/*
* expose C functions for Unity.
* Use only primitive types b/c mapping is hard with structs / strings.
*
*
*
*
*/

#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif // __cplusplus

MYUNITYPLUGIN_SYMBOL int initFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int doFoo(int bar);
MYUNITYPLUGIN_SYMBOL int termFoo();

#ifdef __cplusplus
} // extern "C"
#else // !__cplusplus
#endif // __cplusplus


#endif // MYUNITYPLUGIN_H
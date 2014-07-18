#include <stdio.h>
#include <string.h>
#include <malloc.h>

#ifdef __cplusplus    // If used by C++ code, 
extern "C" {          // we need to export the C interface
#endif	
	
#ifdef UNIX
#define EXPORT extern
#elif (defined (_WINDOWS))
#define EXPORT extern __declspec( dllexport )
#endif

EXPORT int sum(int a, int b)
{
	return a + b;
}

#ifdef __cplusplus
}
#endif
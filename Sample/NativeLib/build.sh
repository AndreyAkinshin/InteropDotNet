#!/bin/sh
gcc -c -Wall -Werror -fpic ./source.c -DUNIX
gcc -shared -o libNativeLib.so source.o
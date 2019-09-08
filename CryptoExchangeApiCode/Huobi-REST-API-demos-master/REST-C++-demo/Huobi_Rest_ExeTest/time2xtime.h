#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <atlstr.h>
#include <windows.h>//sleep的
#include < math.h>//幂运算
#include <iostream>//cin cout的
#include <string>//cin cout的
#include <io.h>//获取文件大小_filelengthi64
using namespace std;//cin cout的

#include"stdafx.h"
#include<windows.h>
#include<wininet.h>
#include<iostream>
#include <time.h>//时间
#include <conio.h>//时间


/*
** SYSTEMTIME转time_t
*/
time_t systime_to_timet(const SYSTEMTIME& st);
/*
**time_t转SYSTEMTIME
*/
SYSTEMTIME Time_tToSystemTime(time_t t);
/*
**time_t转SYSTEMTIME
*/
SYSTEMTIME TimetToSystemTime(time_t t);
/*
**SYSTEMTIME转time_t
*/
time_t SystemTimeToTimet(SYSTEMTIME st);
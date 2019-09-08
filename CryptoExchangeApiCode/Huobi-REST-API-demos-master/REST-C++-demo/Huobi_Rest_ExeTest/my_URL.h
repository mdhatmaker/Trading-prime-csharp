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
char dec2hexChar(short int n);
short int hexChar2dec(char c);
string escapeURL(const string &URL);
string deescapeURL(const string &URL);
string string_To_UTF8(const std::string & str);


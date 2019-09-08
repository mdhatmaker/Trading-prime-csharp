#pragma once
#include "stdafx.h"
#include <stdio.h>  
#include <iostream>  
#include <fstream>  
//#include "json.h"  
#include "boost/asio.hpp"  
#include <boost/lexical_cast.hpp>   
#include <boost/regex.hpp>

#include"stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <atlstr.h>
#include <windows.h>//sleep的
#include < math.h>//幂运算
#include <iostream>//cin cout的
#include <string>//cin cout的
#include <io.h>//获取文件大小_filelengthi64

#include<windows.h>
#include<wininet.h>
#include<iostream>
#include <time.h>//时间
#include <conio.h>//时间

#include <metahost.h>
#include <fstream>

#include "hmac-sha256.h"
#include "base64.h" 
#include "my_URL.h"
#include "time2xtime.h"
#pragma comment(lib,"wininet.lib")
using namespace std;




/// POST请求  
CString PostRequest(CString CShost, CString path, CString form);
/// GET请求  
CString GetRequest(CString sendmsg);//通过地址GET网页


// Huobi_Rest_ExeTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

//#include <afx.h>
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

#include <metahost.h>
#include <fstream>

#include "hmac-sha256.h"
#include "base64.h" 
#include "my_URL.h"
#include "time2xtime.h"
#include "huobiAPI.h"
#pragma comment(lib,"wininet.lib")
//#include "HttpConnect.h"
//typedef map<CString, CString> UDT_MAP_CSTRING_CSTRING;
//#using "..\Debug\ConsoleApplication.dll"
//using namespace Huobi_Market_WebSocketAPI;
//using namespace std;
//CString Secret_Key;//API 秘密密钥(Secret Key) 
//CString Access_Key;//API 密钥(Access Key)

CString GetIpByDomainName(char *szHost, char* szIp)//取IP，可用
{
	WSADATA        wsaData;

	HOSTENT   *pHostEnt;
	int             nAdapter = 0;
	struct       sockaddr_in   sAddr;
	if (WSAStartup(0x0101, &wsaData))
	{
		printf(" gethostbyname error for host:\n");
		return "err";
	}

	pHostEnt = gethostbyname(szHost);
	if (pHostEnt)
	{
		if (pHostEnt->h_addr_list[nAdapter])
		{
			memcpy(&sAddr.sin_addr.s_addr, pHostEnt->h_addr_list[nAdapter], pHostEnt->h_length);
			sprintf(szIp, "%s", inet_ntoa(sAddr.sin_addr));
		}
	}
	else
	{
		//      DWORD  dwError = GetLastError();
		//      CString  csError;
		//      csError.Format("%d", dwError);
	}
	WSACleanup();
	CString str1(szIp);
	return str1;
}


BOOL ExecDosCmd(char *get_websocket)
{
	SECURITY_ATTRIBUTES   sa;
	HANDLE   hRead, hWrite;

	sa.nLength = sizeof(SECURITY_ATTRIBUTES);
	sa.lpSecurityDescriptor = NULL;
	sa.bInheritHandle = TRUE;
	if (!CreatePipe(&hRead, &hWrite, &sa, 0))
	{
		return   FALSE;
	}

	STARTUPINFO   si;
	PROCESS_INFORMATION   pi;
	si.cb = sizeof(STARTUPINFO);
	GetStartupInfo(&si);
	si.hStdError = hWrite;
	si.hStdOutput = hWrite;
	si.wShowWindow = SW_HIDE;
	si.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;
	//关键步骤，CreateProcess函数参数意义请查阅MSDN   
	if (!CreateProcess("D:\\vs2013cpp\\Huobi_Rest_ExeTest\\Huobi_Rest_ExeTest\\ConsoleApplication.exe", get_websocket
		, NULL, NULL, TRUE, NULL, NULL, NULL, &si, &pi))
	{
		return   FALSE;
	}
	CloseHandle(hWrite);

	char   buffer[4096] = { 0 };
	DWORD   bytesRead;
	ofstream outfile("log.txt");

	while (true)
	{
		if (ReadFile(hRead, buffer, 1, &bytesRead, NULL) == NULL)
			break;
		//buffer中就是执行的结果，可以保存到文本，也可以直接输出   
		//printf(buffer);   
		outfile << buffer /*<< endl*/;
		if (buffer=="}")
		{
			outfile << endl;
		}
		//Sleep(200);
	}

	outfile.close();

	return   TRUE;
}
DWORD WINAPI ThreadProc(LPVOID lpParam)
{
	SYSTEMTIME st = { 0 };
	//GetSystemTime(&st);//UTC时间
	st.wYear = 2017;
	st.wMonth = 12;
	st.wDay = 13;
	st.wHour = 0;
	st.wMinute = 0;
	st.wSecond = 0;

	time_t seconds;
	//time(&seconds);

	seconds= systime_to_timet(st);

	char *get_websocket = " market.btcusdt.kline.1day id1 1513094400 1514649600";
	ExecDosCmd(get_websocket);
	return TRUE;
}
int main()
{
//	SYSTEMTIME st = { 0 };
//	//GetSystemTime(&st);//UTC时间
//	st.wYear = 2017;
//	st.wMonth = 12;
//	st.wDay = 13;
//	st.wHour = 0;
//	st.wMinute = 0;
//	st.wSecond = 0;
//	//st.wMilliseconds = 0;
//	SYSTEMTIME st1 = { 0 };
//	//GetSystemTime(&st);//UTC时间
//	st1.wYear = 2017;
//	st1.wMonth = 12;
//	st1.wDay = 31;
//	st1.wHour = 0;
//	st1.wMinute = 0;
//	st1.wSecond = 0;
//	//st1.wMilliseconds = 0;
//	time_t seconds;
//	time_t seconds1;
//	//time(&seconds);
//
//	seconds = systime_to_timet(st);//秒级  *1000+ms 就是毫秒级了
//	seconds1 = systime_to_timet(st1);
//	//websocket取消息
//
//	LPVOID lpParam;
//	DWORD dwThreadId[3];
//	HANDLE hThread[3];
//	hThread[0]=CreateThread(NULL,0, ThreadProc,lpParam,0, &dwThreadId[0]);
//	BOOL a = CloseHandle(hThread[0]);//销毁内核对象 但不停止线程 失败返回0
//	//ExitThread(dwThreadId[0]);//安全的
//	//BOOL b=TerminateThread(hThread,0);//外部停止、销毁线程(不推荐 不安全) 失败返回0
//	HANDLE handle = GetStdHandle(STD_OUTPUT_HANDLE);
//	/*   0 = 黑色      8 = 灰色
//	   1 = 蓝色      9 = 淡蓝色
//	   2 = 绿色      A = 淡绿色
//	   3 = 浅绿色    B = 淡浅绿色
//	   4 = 红色      C = 淡红色
//	   5 = 紫色      D = 淡紫色
//	   6 = 黄色      E = 淡黄色
//	   7 = 白色      F = 亮白色*/
//	SetConsoleTextAttribute(handle,	0x0A);
//	while (1)
//	{
//		cout<<"yes" << endl;
//		Sleep(1000);
//	}
	//char szHost[512] = "api.huobi.pro";
	//char* szIp=new char[125];
	//GetIpByDomainName(szHost, szIp);
	
	huobiAPI god;
	//god.Get_order_orders("hsrbtc", "5", "0.01", "api", "sell-limit");
	god.Secret_Key = "xxxxxxxx-xxxxxxxx-xxxxxxxx-xxxxx";//API 秘密密钥(Secret Key) 
	god.Access_Key = "xxxxxxxx-xxxxxxxx-xxxxxxxx-xxxxx";//API Access Key

	CString instr[6] = { "symbol", "=btcusdt","currency","=usdt", "states", "=canceled" };//查询一种states可以 查询多种用","分割返回签名错误
	god.Get_路径无参数通用("/v1/margin/loan-orders",instr, 6);
	god.Get_id();//后面很多操作用到 必须先获取
	CString tmp = god.Post_place("hsrbtc", "5", "0.01", "api", "sell-limit");
	//功能重复 弃用god.Get_ordermsg(tmp);//查询订单详情
	god.Post_transfer_inout_apply("/v1/dw/transfer-in/margin", "hsrusdt", "hsr", "1");//转入转出或申请
	god.Get_margin_balance("hsrusdt");//查询借贷账户资产情况
	god.Post_submitcancel(tmp);//撤单
	int j=god.balanceMap.size();//持有币种//ID键 account_id =CS值
	god.Get_balance_matchresults_order("余额","");//持有币种填充balanceMap  查询订单明细或详情需要参数2填写订单号
	god.Get_currencys();//可交易币种填充currencysMap
	god.currencysMap;//可交易币种
	god.Get_symbols();//所有交易对及精度填充symbolsMap索引的SYMBOLS结构体
	god.symbolsMap;//交易对及精度

	int i = 0;
	CString order_id[50] = { "0" };
	for (;i<40;i++)
	{
		order_id[i]=god.Post_place("hsrbtc", "5", "0.01", "api", "sell-limit");//最快一秒3单 略慢了 可考虑主线程写Map多线程读取Map
	}
	god.Post_batchcancel(order_id, 40);//批量撤单
	god.Get_ordermsg("594548831");//查询订单详情
	god.Post_submitcancel("594548831");//撤单
	
	god.Get_market_history("btcusdt", 200);
	god.Get_market_depth("btcusdt", "step0",50,50);//市场挂单量 买1-150 卖1-150



	////输入参数得到发送串
	//SYSTEMTIME st = { 0 };
	//GetSystemTime(&st);//UTC时间
	//CString Timestamp[2];
	//Timestamp[0] = "Timestamp";
	//Timestamp[1].Format(_T("=%d-%02d-%02dT%02d%%3A%02d%%3A%02d"),
	//	st.wYear,
	//	st.wMonth,
	//	st.wDay,
	//	st.wHour,
	//	st.wMinute,
	//	st.wSecond);
	//CString Q[8] = { "AccessKeyId", "="+ god.Access_Key, "SignatureMethod", "=HmacSHA256","SignatureVersion", "=2",
	//	Timestamp[0],Timestamp[1] };
	//CString sendmsg = Get_Sendmsg(Q, 8,"/v1/account/accounts");
	//DWORD dwTime = clock();//毫秒计时
	//CString msg = Get_Httpmsg(sendmsg);

	//DWORD dwTime1 = clock() - dwTime;//毫秒计时
	//if (msg!= "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!")//发送成功
	//{
	//	cout <<   msg<< endl;
	//}
	//发送
    return 0;
}


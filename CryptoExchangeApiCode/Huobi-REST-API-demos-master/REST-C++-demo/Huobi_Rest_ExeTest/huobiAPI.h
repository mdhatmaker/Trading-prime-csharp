#pragma once

//#include <afx.h>
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
using namespace std;//cin cout的
typedef map<CString, CString> UDT_MAP_CSTRING_CSTRING;
typedef map<int, CString> UDT_MAP_INT_CSTRING;
typedef struct SYMBOLS//所有交易对及精度 全部存为CS 需要计算某个时再转换 可降低代价
{
	CString base_currency;//基础币种
	CString quote_currency;//计价币种
	CString price_precision;	//价格精度（0为个位）
	CString amount_precision;	//数量精度（0为个位）
	CString symbol_partition;//交易区：main主区，innovation创新区，bifurcation分叉区
};
typedef map<CString, SYMBOLS> UDT_MAP_CSTRING_SYMBOLS;
typedef struct MARKET_DEPTH//市场挂单价格 数量
{
	 double price;
	 double vol;
};
typedef map<int, MARKET_DEPTH> UDT_MAP_INT_MARKET_DEPTH;
typedef struct MARKET_HISTORY//market/history/trade 历史成交记录
{
	CString amount;//成交量
	CString price;//成交价格
	CString direction;//主动方向
	CString ts;//时间 单位ms
};
typedef map<int, MARKET_HISTORY> UDT_MAP_INT_MARKET_HISTORY;


typedef map<CString, UDT_MAP_CSTRING_CSTRING> UDT_MAP_CSTRING_MAPCSCS;
class huobiAPI
{
public:
	huobiAPI();
	~huobiAPI();
	CString Secret_Key;//API 秘密密钥(Secret Key) 
	CString Access_Key;//API 密钥(Access Key)
	UDT_MAP_CSTRING_CSTRING balanceMap;//币币账户余额及ID：币币 account_id	
	UDT_MAP_CSTRING_CSTRING currencysMap;//市场支持币种
	UDT_MAP_CSTRING_SYMBOLS symbolsMap;//所有交易对及精度
	UDT_MAP_INT_MARKET_HISTORY historyMap;//市场最近成交量，价，方向，时间
	UDT_MAP_CSTRING_MAPCSCS marginMap;//借贷账户 交易对 margin结构体
	/*******************************************************************************/
	/* 买卖挂单Map 只有最新有效 所以只存一个币种，单IP情况下不足以获得全部币种行情 */
	/*******************************************************************************/
	UDT_MAP_INT_MARKET_DEPTH buyvpMap;//保存买盘挂单
	UDT_MAP_INT_MARKET_DEPTH sellvpMap;//保存卖盘挂单
public:
	/************************************************************************/
	/* 					GET								                    */
	/************************************************************************/
	CString Get_id();//获取id
	CString Get_balance_matchresults_order(CString sub, CString order_id);//获得账户余额 或订单详情 或订单成交明细
	BOOL Get_currencys();//查询系统所有支持的币种
	BOOL Get_symbols();//所有交易对及精度
	BOOL Get_market_depth(CString symbol, CString type, int buy_depth, int sell_depth);//市场挂单量 要查看的买盘深度buyvpMap 和卖盘深度sellvpMap
	BOOL Get_market_history(CString symbol, int mun);//市场最近成交量，价，方向，时间
	//CString Get_ordermsg(CString order_id);//order-id查询订单详情  ,   order-id+"/matchresults" 查询某个订单的成交明细  返回原始数据
	BOOL Get_margin_balance(CString symbol);//借贷账户详情 参数交易对xxxusdt

	CString Get_路径无参数通用(CString phat, CString *instr, int mun);
	/************************************************************************/
	/* 					POST							                    */
	/************************************************************************/
	CString Post_place(CString symbol, CString price, CString mun, CString source, CString type);//挂单（ 币种 价格 数量 借贷？ 类型）//返回订单号
	CString Post_transfer_inout_apply(CString phat, CString symbol, CString currency, CString start);//现货与借贷账号的转入转出
	BOOL Post_submitcancel(CString order_id);//撤单
	BOOL Post_batchcancel(CString *order_id, int mun);//批量撤单，参数1订单号数组 参数2数组元素个数
	BOOL Post_margin_repay(CString order_id, CString amount);//归还借贷
	CString Post_自写路径通用(CString phat, CString *instr, int mun);
private:
	CString Get_Sendmsg(CString* intmp, int mun, CString Sub, CString GetOfPost);//排序 计算秘钥 返回拼接后的地址
	CString Get_Times();//获取火币格式的本地UTC时间
	CString find_str(CString inCStr, const char* exptxt);//正则表达式搜索并返回第一个
	void writeLog(CString msg, CString fxname);
	
};
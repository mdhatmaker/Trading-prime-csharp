// API.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"


#include "..\net\http\curl\okcoinapi.h"


int _tmain(int argc, _TCHAR* argv[])
{

	
	string cn_apiKey = "";							//请到www.okcoin.cn申请。
	string cn_secretKey	= "";						//请到www.okcoin.cn申请。
	OKCoinApiCn cnapi(cn_apiKey,cn_secretKey);		//国内站
	string symbolcn = "btc_cny";
	cnapi.GetTicker(symbolcn);
	cnapi.DoUserinfo();
	

	//string com_apiKey		= "";					//请到www.okcoin.com申请。
	//string com_secretKey	= "";					//请到www.okcoin.com申请。
	//OKCoinApiCom comapi(com_apiKey,com_secretKey);	//国际站
	//string symbolcom = "btc_usd";
	//comapi.GetTicker(symbolcom);
	//comapi.DoUserinfo();

	
	{
	//现货下单测试
	string type		= "buy";
	string price	= "1";
	string amount	= "0.01";
	string symbol = "btc_cny";
	cnapi.DoTrade(symbol, type, price, amount);								//POST /api/v1/trade						下单交易
	//{"order_id":88861705,"result":true}
	string since = "88861705";
	string order_id	= "88861705";
	cnapi.DoCancel_Order(symbol,order_id);										//POST /api/v1/order_info					获取用户的订单信息
	}
	


	//{
	//期货下单测试
	//string symbolcom		=		"ltc_usd";
	//string contract_type	=		"this_week";
	//string price			=		"1";
	//string amount			=		"1";
	//string type				=		"1";
	//string match_price		=		"0";
	//string lever_rate		=		"10";

	//comapi.DoFuture_Trade(symbolcom,contract_type,price,amount,type,match_price,lever_rate);		//POST /api/v1/future_trade				期货下单										//POST /api/v1/future_batch_trade		批量下单
	//{"order_id":862348954,"result":true}
	//string order_id	= "862348954";
	//comapi.DoFuture_Cancel(symbolcom,order_id,contract_type);	

	//}


	return 0;
}


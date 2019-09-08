#include "stdafx.h"
#include "huobiAPI.h"
//#include "HttpConnect.h"
#include <boost/lexical_cast.hpp>   
#include <boost/regex.hpp>
#include <boost/asio.hpp>//post用
using namespace std;//cin cout的
using namespace boost;

#include "NetworkRequest.h"  

huobiAPI::huobiAPI()
{

}

huobiAPI::~huobiAPI()
{
	balanceMap.clear();
}
CString huobiAPI::Get_Times()
{
	SYSTEMTIME st = { 0 };
	GetSystemTime(&st);//UTC时间
	CString Timestamp;
	Timestamp.Format(_T("%d-%02d-%02dT%02d%%3A%02d%%3A%02d"),
		st.wYear,
		st.wMonth,
		st.wDay,
		st.wHour,
		st.wMinute,
		st.wSecond);
	return Timestamp;
}
//返回完整带签名get用地址 post用需要左删除到"/v1"
//参数1参数数组{ "AccessKeyId","=xxxxx-xxxx-xxxxx-xx","SignatureMethod","=HmacSHA256"..... }，参数2个数,参数3API地址
CString huobiAPI::Get_Sendmsg(CString* intmp, int mun, CString Sub,CString GetOfPost)
{
	//GetOfPost = "GET"/"POST";
	CString Url = "api.huobi.pro";
	CString *tmp = new CString[mun + 2];
	for (int i = 0; i < mun; i++)
	{
		tmp[i] = intmp[i];
	}
	for (int i = 0; i < mun; i += 2)
	{
		if (tmp[i] > tmp[i + 2])
		{
			tmp[mun] = tmp[i];
			tmp[mun + 1] = tmp[i + 1];
			tmp[i] = tmp[i + 2];
			tmp[i + 1] = tmp[i + 3];
			tmp[i + 2] = tmp[mun];
			tmp[i + 3] = tmp[mun + 1];
		}
	}
	CString Q = GetOfPost + "\n" + Url + "\n" + Sub + "\n";
	Q += tmp[0] + tmp[1];
	for (int i = 2; i < mun; i += 2)
	{
		Q += "&";
		Q += tmp[i];
		Q += tmp[i + 1];
	}
	unsigned char* ucVar = (unsigned char*)(char*)(LPCTSTR)Q;
	unsigned char* ucVar1 = (unsigned char*)(char*)(LPCTSTR)Secret_Key;
	hmac_sha256 hmac;
	hmac_sha256_initialize(&hmac, ucVar1, 32);
	hmac_sha256_update(&hmac, ucVar, Q.GetLength());
	hmac_sha256_finalize(&hmac, NULL, 0);

	CString str;
	string str1 = "";
	for (int i = 0; i < 32; ++i) {
		str.Format("%02lx", (unsigned long)hmac.digest[i]);
		str1 += str;
	}
	CString  sendmsg = "https://";
	sendmsg = sendmsg + Url + Sub + "?";//地址+方法+?
	CString Q1 = intmp[0] + intmp[1];
	for (int i = 2; i < mun; i += 2)
	{
		Q1 += "&";
		Q1 += intmp[i];
		Q1 += intmp[i + 1];
	}
	sendmsg += Q1;//参数
	sendmsg += "&Signature=";
	//string encoded = base64_encode(hmac.digest, 32);//base64编码
	sendmsg += escapeURL(base64_encode(hmac.digest, 32)/*base64编码*/)/*URL编码*/.c_str();
	return sendmsg;
}
CString huobiAPI::find_str(CString inCStr, const char* exptxt)
{
	string str(inCStr.GetBuffer(inCStr.GetLength()));
	regex expression(exptxt);
	smatch what;
	string::const_iterator start = str.begin();
	string::const_iterator end = str.end();
	if (boost::regex_search(start, end, what, expression))
	{
		//cout << what[0];
		str = what[0];
	}
	else {
		return "-1";
	}
	return str.c_str();
}

void huobiAPI::writeLog(CString msg, CString fxname)
{
	ofstream outfile;
	outfile.open("err_log.txt", ios::app);
	if (!outfile) //检查文件是否正常打开//不是用于检查文件是否存在
	{
		MessageBox(NULL, msg, "err_log.txt打开失败！", MB_OK);
		abort(); //打开失败，结束程序
	}
	else
	{
		outfile << Get_Times() << "\t" << "huobiAPI::" << fxname << "\t" << msg << endl;
		outfile.close();
	}
}

/************************************************************************/
/* 获取币币交易账户ID存入balanceMap										*/
/************************************************************************/
CString huobiAPI::Get_id()
{
	CString Q[8] = { "AccessKeyId", "=" + Access_Key, 
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString sendmsg = Get_Sendmsg(Q, 8, "/v1/account/accounts", "GET");
	CString msg = GetRequest(sendmsg);
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1==msg.Find("error"))//发送成功
	{
		CString sstr = find_str(msg, "[0-9]+");
		balanceMap.insert(make_pair("account_id", sstr));
		return sstr;
	}
	writeLog(msg, "Get_id()");
	return msg;
}
/************************************************************************/
/*查询账户余额存入balanceMap											*/
/*balanceMap<币种_trade><数量> //数量使用CString 无精度损失			    */
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
CString huobiAPI::Get_balance_matchresults_order(CString sub,CString order_id)
{
	CString phat;
	if (sub=="余额")
	{
		phat = "/v1/account/accounts/" + balanceMap.find("account_id")->second + "/balance";
	}
	else if(sub == "订单成交明显"){
		phat = "/v1/order/orders/"+ order_id +"/matchresults";
	}
	else if (sub == "订单详情") {
		phat = "/v1/order/orders/" + order_id;
	}
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString sendmsg = Get_Sendmsg(Q, 8, phat, "GET");
	CString msg = GetRequest(sendmsg);
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		if (sub == "balance")
		{
		msg.Delete(0, msg.Find("[", 0) + 1);			//连 { （大括号！）一起删掉
		//msg.Delete(msg.Find("]", 0)+1, msg.GetLength());	//留 ] （中括号！）删掉后面
		CString  balance, temp;
		int i = 0;
		int munstar = 0;
		int munend = 0;
		//DWORD dwTime = clock();//毫秒计时
		msg.Replace("\"", "");
		msg.Replace("{currency:", "");
		msg.Replace(",type:", "_");
		msg.Replace("balance:", "");
		msg.Replace("}}", "");
		while(1)
		{
			i = msg.Find("}", 0);
			if (i>=0)
			{
				munstar=msg.Find(",", 0);
				temp = msg.Left(munstar);
				msg.Delete(0, munstar+1);
				munend = msg.Find("}", 0);
				balanceMap.insert(make_pair(temp, msg.Left(munend)));
				msg.Delete(0, munend +2);
			}
			else
			{
				break;
			}
		}
		}
		//DWORD dwTime1 = clock() - dwTime;//毫秒计时
		return msg;
	}
	writeLog(msg,"Get_balance(sub:"+ sub+",order_id:"+ order_id+")");
	return msg;
}
/************************************************************************/
/*查询所有系统支持交易的币种存入currencysMap	                        */
/*currencysMap<币种><币种_trade> //_trade可作为余额balanceMap的索引键   */
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
BOOL huobiAPI::Get_currencys()
{
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString sendmsg = Get_Sendmsg(Q, 8, "/v1/common/currencys", "GET");
	CString msg = GetRequest(sendmsg);
	CString rmsg = msg;
	CString rrmsg = msg;
	int i = 0;
		int tmpi=0;
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		msg.Delete(0, msg.Find("[", 0) + 1);			//连 { （大括号！）一起删掉
		//DWORD dwTime = clock();//毫秒计时
		while (1)
		{
			i = msg.Find(",", 0);
			if (i > 0)
			{
				msg.Delete(0, i + 1);
			}
			else
			{
				break;
			}
		}
		CString temp;
		rmsg.Replace("{\"status\":\"ok\",\"data\":[\"", "");
		rmsg.Replace("\"]}", "");
		rmsg.Replace("\",\"", ",");
		while (1)
		{
			i = rmsg.Find(",", 0);
			if (i > 0)
			{
				currencysMap.insert(make_pair(rmsg.Left(i),rmsg.Left(i)+"_trade"));
				rmsg.Delete(0, i + 1);
			}
			else
			{
				break;
			}
			//tmpi++;
		}
		//DWORD dwTime1 = clock() - dwTime;//毫秒计时
		return TRUE;
	}
	writeLog(rmsg, "Get_currencys()");
	return FALSE;
}
/**********************************************************************************/
/*获取所有交易对及精度填充symbolsMap索引的SYMBOLS结构体					          */
/*symbolsMap<交易对><SYMBOLS结构体>												  */
/*(symbolsMap.find(symbol)->second).find("price_precision")->second				  */
/*SYMBOLS结构			  												          */
/*CString base_currency;//基础币种										          */
/*CString quote_currency;//计价币种										          */
/*CString price_precision;	//价格精度（0为个位）						          */
/*CString amount_precision;	//数量精度（0为个位）						          */
/*CString symbol_partition;//交易区：main主区，innovation创新区，bifurcation分叉区*/
/*返回值：成功TRUE 失败FALSE并写入err_log.txt								      */
/**********************************************************************************/
BOOL huobiAPI::Get_symbols()
{
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString sendmsg = Get_Sendmsg(Q, 8, "/v1/common/symbols", "GET");
	CString msg = GetRequest(sendmsg);
	int i = 0;
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		msg.Delete(0, msg.Find("[", 0) + 2);			//连 { （大括号！）一起删掉
		msg.Delete(msg.ReverseFind(']'), msg.GetLength());	//连 ] （中括号！）一起删掉
		CString  balance, temp;
		int i = 0;
		//DWORD dwTime = clock();//毫秒计时
		msg.Replace("\"", "");
		msg.Replace("base-currency:", "");
		msg.Replace("quote-currency:", "");
		msg.Replace("price-precision:", "");
		msg.Replace("amount-precision:", "");
		msg.Replace("symbol-partition:", "");
		msg.Replace(",{", "");
		msg.Replace("}", ",");

			string str(msg.GetBuffer(msg.GetLength()));
			boost::regex expression("[0-9]+");
			boost::smatch what;
			std::string::const_iterator start = str.begin();
			std::string::const_iterator end = str.end();
			string tmpstr;
		while (1)
		{
			i = msg.Find(",", 0);
			SYMBOLS tmp;
			int mun;
			if (i > 0)
			{
				mun = msg.Find(",", 0);
				tmp.base_currency = msg.Left(mun);
				msg.Delete(0, mun + 1);

				mun = msg.Find(",", 0);
				tmp.quote_currency = msg.Left(mun);
				msg.Delete(0, mun + 1);

				mun = msg.Find(",", 0);
				tmp.price_precision = msg.Left(mun);
				msg.Delete(0, mun + 1);
				mun = msg.Find(",", 0);
				tmp.amount_precision = msg.Left(mun);
				msg.Delete(0, mun + 1);
				mun = msg.Find(",", 0);
				tmp.symbol_partition = msg.Left(mun);
				msg.Delete(0, mun + 1);
				symbolsMap.insert(make_pair(tmp.base_currency+ tmp.quote_currency, tmp));
			}
			else
			{
				break;
			}
		}
		return TRUE;
	}
	writeLog(msg, "Get_symbols()");

	return FALSE;
}
/*************************************************************************************/
/*获得买盘深度存入buyvpMap 卖盘深度存入sellvpMap                                     */
/*参数1：交易对                                                                      */
/*参数2：step0, step1, step2, step3, step4, step5（合并深度0-5）；step0时，不合并深度*/
/*参数3：要获取的买盘数量 从现价向下buy_depth个                                      */
/*参数4：要获取的卖盘数量 从现价向下sell_depth个                                     */
/*返回值：成功TRUE 失败FALSE并写入err_log.txt										 */
/*************************************************************************************/
BOOL huobiAPI::Get_market_depth(CString symbol,CString type,int buy_depth,int sell_depth)
{
	CString Q[12] = { "AccessKeyId", "=" + Access_Key,
		"symbol","="+symbol,"type","="+ type,//参数行
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times()  };
	CString sendmsg = Get_Sendmsg(Q, 12, "/market/depth", "GET");
	CString msg = GetRequest(sendmsg);
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		msg.Delete(0, msg.Find("[", 0) + 2);			//连 { （大括号！）一起删掉
		msg.Replace("[", "");
		msg.Replace("]", "");
		CString bids, asks;
		int tmpmun=msg.Find("\"asks\"", 0);
		bids = msg.Left(tmpmun);
		msg.Delete(0, tmpmun + 7);
		msg.Delete(msg.Find("\"ts\":", 0), msg.GetLength());
		asks = msg;
		MARKET_DEPTH tmp;
		int mun = 0;
		for (int i=0;i<buy_depth*2;i+=2)
		{
			tmpmun = bids.Find(",", 0);
			tmp.price= atof(bids.Left(tmpmun));
			bids.Delete(0, tmpmun + 1);
			tmpmun = bids.Find(",", 0);
			tmp.vol = atof(bids.Left(tmpmun));
			bids.Delete(0, tmpmun + 1);
			buyvpMap.insert(make_pair(mun, tmp));
			mun++;
		}
		mun = 0;
		for (int i = 0; i < sell_depth * 2; i += 2)
		{
			tmpmun = asks.Find(",", 0);
			tmp.price = atof(asks.Left(tmpmun));
			asks.Delete(0, tmpmun + 1);
			tmpmun = asks.Find(",", 0);
			tmp.vol = atof(asks.Left(tmpmun));
			asks.Delete(0, tmpmun + 1);
			sellvpMap.insert(make_pair(mun, tmp));
			mun++;
		}
		return TRUE;
	}
	CString strtemp;
	strtemp.Format("%d", buy_depth);
	CString strtemp1;
	strtemp1.Format("%d", sell_depth);
	writeLog(msg, "Get_market_depth(symbol:"+ symbol+",type:"+  type + ",buy_depth:" + strtemp + ",sell_depth:" + strtemp1+")");
	return FALSE;
}
/************************************************************************/
/*获得市场最近成交信息存入historyMap                                    */
/*参数1：交易对                                                         */
/*参数2：要获得的信息数量                                               */
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
BOOL huobiAPI::Get_market_history(CString symbol, int mun)
{
	CString strtemp;
	strtemp.Format("%d", mun);
	CString Q[12] = { "AccessKeyId", "=" + Access_Key,
		"symbol","=" + symbol,"size","=" + strtemp,//参数行
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString sendmsg = Get_Sendmsg(Q, 12, "/market/history/trade", "GET");
	CString msg = GetRequest(sendmsg);
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		UDT_MAP_INT_MARKET_HISTORY historyMaptmp;
		MARKET_HISTORY market_history_tmp;
		string str(msg.GetBuffer(msg.GetLength()));
		boost::regex expression("[0-9]+[/.]+[0-9]+");
		boost::smatch what;
		std::string::const_iterator start = str.begin();
		std::string::const_iterator end = str.end();
		string strtmp;
		int i = 0;
		while (boost::regex_search(start, end, what, expression))
		{
			if (i==mun)
			{
				break;
			}
			//↓成交量
			strtmp=what[0];
			market_history_tmp.amount = strtmp.c_str();
			start = what[0].second;
			//↓成交价
			boost::regex_search(start, end, what, expression);
			strtmp = what[0];
			market_history_tmp.price = strtmp.c_str();
			start = what[0].second;
			//↓成交方向
			expression = "\"direction\":\"[a-z]+\"";
			boost::regex_search(start, end, what, expression);
			strtmp = what[0];
			market_history_tmp.direction = strtmp.c_str();
			start = what[0].second;
			//↓成交时间
			expression = "[0-9]+";
			boost::regex_search(start, end, what, expression);
			strtmp = what[0];
			market_history_tmp.ts = strtmp.c_str();
			start = what[0].second;

			expression = "[0-9]+[/.]+[0-9]+";
			historyMaptmp.insert(make_pair(i, market_history_tmp));
			i++;
		}
		historyMap = historyMaptmp;
		return TRUE;
	}

	CString strtemp1;
	strtemp1.Format("%d", mun);
	writeLog(msg, "Get_market_history(symbol:" + symbol + ",mun:" + strtemp1 + ")");
	return FALSE;
}
/************************************************************************/
/*下订单													            */
/*参数1：交易对                                                         */
/*参数2：单价                                                           */
/*参数3：数量                                                           */
/*参数4：api为币币交易账户 margin-api为借贷账户	                        */
/*参数5：buy-market：市价买,                                            */
/*       sell-market：市价卖,                                           */
/*       buy-limit：限价买,                                             */
/*       sell-limit：限价卖                                             */
/*返回值：成功返回订单号 失败返回服务器给出的原始信息并写入err_log.txt	*/
/************************************************************************/
CString huobiAPI::Post_place(CString symbol,CString price, CString mun,CString source,CString type)
{
	//↓↓↓↓↓↓↓↓↓↓↓↓↓↓精度调整↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
	if (0==(symbolsMap.size()))//没读取过精度
	{
		Get_symbols();
	}
	int priceD = price.Find(".", 0);
	int munD = mun.Find(".", 0);
	int wantlen;
	if (priceD>=0)//有小数部分
	{
		wantlen= _ttoi((symbolsMap.find(symbol)->second).price_precision)+1;/*获取价格精度*/
		if (price.GetLength()>priceD + wantlen)
		{
			price=price.Left(priceD + wantlen);
		}
	}
	if (munD >= 0)//有小数部分
	{
		wantlen = _ttoi((symbolsMap.find(symbol)->second).amount_precision)+1;/*获取价格精度*/
		if (mun.GetLength() > munD + wantlen)
		{
			mun=mun.Left(munD + wantlen);
		}
	}
	//↑↑↑↑↑↑↑↑↑↑↑根据精度删除多余长度↑↑↑↑↑↑↑↑↑↑
	CString sendmsg;
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	CString send_data;
	CString account_id;
	if (source=="margin-api")
	{
		account_id = balanceMap.find("margin_id")->second;
	}
	else {
		account_id = balanceMap.find("account_id")->second;
	}
	if (type=="buy-limit"||type=="sell-limit")//定价
	{
		send_data = "{\"account-id\":" + balanceMap.find("account_id")->second + ",\
						\"amount\":" + mun + ",\
						\"symbol\":\"" + symbol + "\",\
						\"type\":\"" + type + "\",\
						\"source\":\"" + source + "\",\
						\"price\":" + price + "}";
	}else if (type == "buy-market" || type == "sell-market")//市价
	{
		send_data = "{\"account-id\":" + balanceMap.find("account_id")->second + ",\
						\"amount\":" + mun + ",\
						\"symbol\":\"" + symbol + "\",\
						\"type\":\"" + type + "\",\
						\"source\":\"" + source + "\"}";
	}
	sendmsg = Get_Sendmsg(Q, 8, "/v1/order/orders/place", "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0) );
	CString msg = PostRequest("api.huobi.pro", sendmsg, send_data);
	if (-1 == msg.Find("error"))//发送成功
	{
		msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");
		return sstr;
	}
	writeLog(msg, "Post_place(symbol:" +symbol+",price:" + price+",mun:" + mun+",source:"+ source+",type:"+ type+")");
	return msg;
}
/************************************************************************/
/*post 撤单																*/
/*参数1：单号															*/
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
BOOL huobiAPI::Post_submitcancel(CString order_id)
{
	CString sendmsg;
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	sendmsg = Get_Sendmsg(Q, 8, "/v1/order/orders/"+ order_id +"/submitcancel", "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0));
	CString msg = PostRequest("api.huobi.pro", sendmsg, "");
	if (-1 == msg.Find("error"))//发送成功
	{
		/*msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");*/
		return TRUE;
	}
	writeLog(msg, "Post_submitcancel(order_id:" + order_id+ ")");
	return FALSE;
}
/************************************************************************/
/*订单详情合并至：Get_balance_matchresults_order(sub,order_id)			*/
/*参数1：单号															*/
/*返回值：成功/失败 返回原始数据 失败写入err_log.txt                    */
/************************************************************************/
//CString huobiAPI::Get_ordermsg(CString order_id)
//{
//	CString sendmsg;
//	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
//		"SignatureMethod", "=HmacSHA256",
//		"SignatureVersion", "=2",
//		"Timestamp","=" + Get_Times() };
//	sendmsg = Get_Sendmsg(Q, 8, "/v1/order/orders/" + order_id , "GET");
//	CString msg = GetRequest(sendmsg);
//	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
//	{
//		/*msg.Delete(0, msg.Find("data", 0));
//		CString sstr = find_str(msg, "[0-9]+");*/
//		return msg;
//	}
//	writeLog(msg, "Get_ordermsg(order_id:" + order_id + ")");
//	return "-1";
//}
/************************************************************************/
/*post 批量撤单                                                         */
/*参数1：单号数组                                                       */
/*参数2：订单个数                                                       */
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
BOOL huobiAPI::Post_batchcancel(CString *order_id, int mun)
{
	CString sendmsg;
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	sendmsg = Get_Sendmsg(Q, 8, "/v1/order/orders/batchcancel", "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0));
	/*string postmsg = sendmsg.GetBuffer(sendmsg.GetLength());
	char *buf = new char[strlen(postmsg.c_str()) + 1];
	strcpy(buf, postmsg.c_str());*/
	CString senddata = "{\"order-ids\":[\"";
	for (int i=0;i<mun;i++)
	{
		senddata += order_id[i];
		if (i!=mun-1)
		{
			senddata += "\",\"";
		}
		else {
			senddata += "\"]}";
		}
	}
	string reponse_data = PostRequest("api.huobi.pro", sendmsg, senddata);
	CString msg = reponse_data.c_str();
	if (-1 == msg.Find("error"))//发送成功
	{
		/*msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");*/
		return TRUE;
	}
	CString str; 
	str.Format("%d", mun);
	writeLog(reponse_data.c_str(), "Post_batchcancel(order_id:x,mun:" + str + ")");
	return FALSE;
}
/************************************************************************/
/*获取借贷账户ID  及资产情况	API手册叫：借贷账户详情                 */
/*参数1	交易对		 													*/
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/*具体查看方法：														*/
/*(marginMap.find(symbol)->second).find("margin_id")->second			*/
/************************************************************************/
BOOL huobiAPI::Get_margin_balance(CString symbol)
{
		CString Q[10] = { "AccessKeyId", "=" + Access_Key,
			"SignatureMethod", "=HmacSHA256",
			"SignatureVersion", "=2",
			"Timestamp","=" + Get_Times(),
			"symbol","="+ symbol };
		CString sendmsg = Get_Sendmsg(Q, 10, "/v1/margin/accounts/balance", "GET");
		CString msg = GetRequest(sendmsg);
		if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
		{
			UDT_MAP_CSTRING_CSTRING tmp;
			//tmp.margin_id = find_str(msg, "[0-9]+");
			tmp.insert(make_pair("margin_id", find_str(msg, "[0-9]+")));
			CString tmpstr= find_str(msg, "fl-price\":\"[0-9]+[/.]*[0-9]*");
			//tmp.fl_price = find_str(tmpstr, "[0-9]+[/.]*[0-9]*");
			tmp.insert(make_pair("fl_price", find_str(tmpstr, "[0-9]+[/.]*[0-9]*")));
			msg.Delete(0, msg.Find("risk-rate", 0));
			string str(msg.GetBuffer(msg.GetLength()));
			boost::smatch what;
			std::string::const_iterator start = str.begin();
			std::string::const_iterator end = str.end();
			string strtmp,inttmp;
			//风险率
			boost::regex expression("[0-9]+[/.]+[0-9]+");
			boost::regex_search(start, end, what, expression);
			strtmp = what[0];
			//tmp.risk_rate = strtmp.c_str();
			tmp.insert(make_pair("risk_rate", strtmp.c_str()));
			msg.Delete(0, msg.Find("[", 0) + 1);
			msg.Replace("\"", "");
			msg.Replace("{currency:", "");
			msg.Replace(",type:", "-");
			msg.Replace(",balance:", "");
			str = msg.GetBuffer(msg.GetLength());
			start = str.begin();
			end = str.end();
			for (int i=0;i<12;i++)
			{
				expression = "[a-z]+\-[a-z]+([\-]*[a-z]*)*";
				boost::regex_search(start, end, what, expression);
				strtmp = what[0];
				//tmp.xxx_trade = strtmp.c_str();
				expression = "[0-9]+[/.]+[0-9]+";
				boost::regex_search(start, end, what, expression);
				inttmp = what[0];
				start = what[0].second;
				tmp.insert(make_pair(strtmp.c_str(), inttmp.c_str()));

			}
			marginMap.insert(make_pair(symbol, tmp));
			// xxx_trade;//可用币
			// xxx_frozen;//借贷账户币冻结
			// xxx_loan;//已借贷币
			// xxx_interest;//币待还利息
			// xxx_transfer_out_available;//可转币
			// xxx_loan_available;//可借币
			// usdt_trade;//可用usdt
			// usdt_frozen;//借贷账户usdt冻结
			// usdt_loan;//已借贷usdt
			// usdt_interest;//usdt待还利息
			// usdt_transfer_out_available;//可转usdt
			// usdt_loan_available;//可借usdt
			//CString tid = (marginMap.find(symbol)->second).find("margin_id")->second;
			return TRUE;
		}
		writeLog(msg, "Get_margin_balance(symbol"+ symbol +")");
		return FALSE;
	
}
/************************************************************************/
/*现货与借贷账号的转入转出或申请借贷                                    */
/*phat控制转入转出借贷账户或申请借贷					                */
/*转入：phat="/v1/dw/transfer-in/margin"								*/
/*转出：phat="/v1/dw/transfer-out/margin"								*/
/*申请：phat="/v1/margin/orders"										*/
/*symbol	true	string	交易对	                                    */
/*currency	true	string	币种	                                    */
/*amount	true	string	金额	                                    */
/*返回值：成功返回划转ID 或订单号 失败返回原始信息写入err_log.txt       */
/************************************************************************/
CString huobiAPI::Post_transfer_inout_apply(CString phat, CString symbol, CString currency, CString amount)//借贷订单
{
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times()};
	CString sendmsg = Get_Sendmsg(Q, 8, phat, "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0));
	CString senddata = "{\"symbol\":\""+ symbol +"\",\"currency\":\""+ currency +"\",\"amount\":\""+amount+"\"}";
	string reponse_data = PostRequest("api.huobi.pro", sendmsg, senddata);
	CString msg = reponse_data.c_str();
	if (-1 == msg.Find("error"))//发送成功
	{
		msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");
		return sstr;
	}
	writeLog(msg, "Post_transfer_inout(symbol:" + symbol + ",currency:" + currency + ",amount:" + amount + ")");
	return msg;
}
/************************************************************************/
/*归还借贷																*/
/*参数1：单号															*/
/*参数2：还款量															*/
/*返回值：成功TRUE 失败FALSE并写入err_log.txt                           */
/************************************************************************/
BOOL huobiAPI::Post_margin_repay(CString order_id,CString amount)
{
	CString sendmsg;
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	sendmsg = Get_Sendmsg(Q, 8, "/v1/margin/orders/" + order_id + "/repay", "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0));
	CString msg = PostRequest("api.huobi.pro", sendmsg, "{\"amount\":\""+ amount +"\"}");
	if (-1 == msg.Find("error"))//发送成功
	{
		/*msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");*/
		return TRUE;
	}
	writeLog(msg, "Post_submitcancel(order_id:" + order_id + ")");
	return FALSE;
}
//GET /v1/order/orders 查询当前委托、历史委托
/*请求参数:

参数名称	是否必须	类型	描述								取值范围
symbol		true		string	交易对								btcusdt, bccbtc, rcneth ...
types		false		string	查询的订单类型组合，使用','分割		buy-market：市价买, sell-market：市价卖, buy-limit：限价买, sell-limit：限价卖
start-date	false		string	查询开始日期, 日期格式yyyy-mm-dd
end-date	false		string	查询结束日期, 日期格式yyyy-mm-dd
states		true		string	查询的订单状态组合，使用','分割		pre-submitted 准备提交, submitted 已提交, partial-filled 部分成交, partial-canceled 部分成交撤销, filled 完全成交, canceled 已撤销
from		false		string	查询起始 ID
direct		false		string	查询方向							prev 向前，next 向后
size		false		string	查询记录大小		*/
//GET /v1/margin/loan-orders 借贷订单
/*请求参数

参数名称	是否必须	类型	描述								取值范围
symbol		true		string	交易对
currency	true		string	币种
start-date	false		string	查询开始日期, 日期格式yyyy-mm-dd
end-date	false		string	查询结束日期, 日期格式yyyy-mm-dd
states		true		string	状态
from		false		string	查询起始 ID
direct		false		string	查询方向							prev 向前，next 向后
size		false		string	查询记录大小		*/
//GET /v1/order/matchresults 查询当前成交、历史成交
/*请求参数:

参数名称	是否必须	类型	描述								取值范围
symbol		true		string	交易对								btcusdt, bccbtc, rcneth ...
types		false		string	查询的订单类型组合，使用','分割		buy-market：市价买, sell-market：市价卖, buy-limit：限价买, sell-limit：限价卖
start-date	false		string	查询开始日期, 日期格式yyyy-mm-dd
end-date	false		string	查询结束日期, 日期格式yyyy-mm-dd
from		false		string	查询起始 ID
direct		false		string	查询方向							prev 向前，next 向后
size		false		string	查询记录大小		*/
//GET /market/history/kline 获取K线数据
/*请求参数:

参数名称	是否必须	类型	描述		取值范围
symbol		true		string	交易对		btcusdt, bccbtc, rcneth ...
period		true		string	K线类型		1min, 5min, 15min, 30min, 60min, 1day, 1mon, 1week, 1year
size		false		integer	获取数量	150	[1,2000]*/
//GET /market/detail/merged 获取聚合行情(Ticker)
/*请求参数:

参数名称	是否必须	类型	描述		取值范围
symbol		true		string	交易对		btcusdt, bccbtc, rcneth ...*/
//GET /market/detail 获取 Market Detail 24小时成交量数据
/*请求参数:

参数名称	是否必须	类型	描述		取值范围
symbol		true		string	交易对		btcusdt, bccbtc, rcneth ...*/
//GET /v1/common/timestamp 查询系统当前时间 参数2输入任意指针 参数3位0


/************************************************************************/
/*币币交易和借贷交易的历史订单查询 K线查询 聚合行情	24小时成交量	    */
/*参数1phat不同 查询项不同 值见上方注释块，使用例子在main函数			*/
/*参数2数组元素值不同 具体参考上面块注释                                */
/*返回值：成功/失败 返回原始数据 失败写入err_log.txt                    */
/************************************************************************/
CString huobiAPI::Get_路径无参数通用(CString phat, CString *instr, int mun)
{
	int j = 8;
	CString Q[64] = {"0"};
	CString Q1[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	for (int i=0;i<8;i++)
	{
		Q[i] = Q1[i];
	}
	for (int i = 0; i < mun; i++)
	{
		Q[j] = instr[i];
		j++;
	}
	CString sendmsg = Get_Sendmsg(Q, mun+8, phat, "GET");
	CString msg = GetRequest(sendmsg);
	if (msg != "Can't InternetOpen!"&&msg != "Can't InternetOpenUrl!"&&msg != "Can't InternetReadFile!"&&-1 == msg.Find("error"))//发送成功
	{
		/*msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");*/
		return msg;
	}
	CString str;
	str.Format("%d", mun);
	writeLog(msg, "Get_路径无参数通用(phat:"+ phat+", *instr:x,mun" + str + ")");
	return msg;

}
//POST /v1/dw/withdraw/api/create 申请提现虚拟币
/*请求参数:

参数名称	是否必须	类型	描述						取值范围
address		true		string	提现地址
amount		true		string	提币数量
currency	true		string	资产类型					btc, ltc, bcc, eth, etc ...(火币Pro支持的币种)
fee			false		string	转账手续费
addr-tag	false		string	虚拟币共享地址tag，XRP特有	格式, "123"类的整数字符串*/
//POST /v1/dw/withdraw-virtual/{withdraw-id}/cancel 申请取消提现虚拟币
/*请求参数:

参数名称	是否必须	类型	描述	
withdraw-id	true		long	提现ID，填在path中	*/
/************************************************************************/
/* 数据格式为：{"x":"x","x":"x","x":"x"} 的都可以用                     */
/************************************************************************/
CString huobiAPI::Post_自写路径通用(CString phat, CString *instr, int mun)
{
	CString sendmsg;
	CString Q[8] = { "AccessKeyId", "=" + Access_Key,
		"SignatureMethod", "=HmacSHA256",
		"SignatureVersion", "=2",
		"Timestamp","=" + Get_Times() };
	sendmsg = Get_Sendmsg(Q, 8, "/v1/order/orders/batchcancel", "POST");
	sendmsg.Delete(0, sendmsg.Find("/v1", 0));
	CString senddata = "{\"";
	for (int i = 0; i<mun; i+=2)
	{
		senddata += instr[i];
		senddata += "\":\"";
		senddata += instr[i+1];
		if (i != mun - 2)
		{
			senddata += "\",\"";
		}
		else {
			senddata += "\"}";
		}
	}
	CString msg = PostRequest("api.huobi.pro", sendmsg, "");
	if (-1 == msg.Find("error"))//发送成功
	{
		/*msg.Delete(0, msg.Find("data", 0));
		CString sstr = find_str(msg, "[0-9]+");*/
		return msg;
	}
	CString str;
	str.Format("%d", mun);
	writeLog(msg, "Post_自写路径通用(phat:" + phat + ", *instr:x,mun" + str + ")");
	return msg;
}
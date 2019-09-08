#ifndef __OKCOINAPI_H__
#define __OKCOINAPI_H__

#include <string>
#include <map>
#include <vector>
#include <list>
#include <algorithm>
using namespace std;
#include "curlprotocol.h"

class OKCoinApi	//国内站
{
protected:
	OKCoinApi(){};
	void SetKey(string api_key,string secret_key);

public:

	virtual ~OKCoinApi(){};
	CUrlProtocol urlprotocol;
	string m_api_key;			//用户申请的apiKey
	string m_secret_key;		//请求参数签名的私钥

	
	//现货行情 API
	//获取OKCoin最新市场现货行情数据

	string GetTicker(string &symbol);																								//Get /api/v1/ticker						获取OKCoin行情
	string GetDepth(string &symbol,string &size,string &merge);																		//Get /api/v1/depth							获取OKCoin市场深度
	string GetTrades(string &symbol,string &since);							//Get /api/v1/trades						获取OKCoin最近600交易信息
	string GetKline(string &symbol,string &type,string &size,string &since);														//Get /api/v1/kline							获取比特币或莱特币的K线数据




	//现货交易 API
	//用于OKCoin快速进行现货交易

	string DoUserinfo();																								//POST /api/v1/userinfo						获取用户信息
	string DoTrade(string &symbol,string &type,string &price,string &amount);														//POST /api/v1/trade						下单交易
	string DoTrade_History(string &symbol,string &since);																			//POST /api/v1/trade_history				获取OKCoin历史交易信息
	string DoBatch_Trade(string &symbol,string &type,string &orders_data);															//POST /api/v1/batch_trade					批量下单
	string DoCancel_Order(string &symbol,string &order_id);																			//POST /api/v1/cancel_order					撤销订单
	string DoOrder_Info(string &symbol,string &order_id);																			//POST /api/v1/order_info					获取用户的订单信息
	string DoOrders_Info(string &type,string &symbol,string &order_id);																//POST /api/v1/orders_info					批量获取用户订单
	string DoOrder_History(string &symbol,string &status,string &current_page,string &page_length);									//POST /api/v1/order_history				获取历史订单信息，只返回最近七天的信息
	string DoWithdraw(string &symbol,string &chargefee,string &trade_pwd,string &withdraw_address,string &withdraw_amount);			//POST /api/v1/withdraw						提币BTC/LTC
	string DoCancel_Withdraw(string &symbol,string &withdraw_id);																	//POST /api/v1/cancel_withdraw				取消提币BTC/LTC
	string DoOrder_Fee(string &symbol,string &order_id);																			//POST /api/v1/order_fee					查询手续费
	string DoLend_Depth(string &symbol);																							//POST /api/v1/lend_depth					获取放款深度前10
	string DoBorrows_Info(string &symbol);																							//POST /api/v1/borrows_info					查询用户借款信息
	string DoBorrow_Money(string &symbol,string &days,string &amount,string &rate);													//POST /api/v1/borrow_money					申请借款
	string DoCancel_Borrow(string &symbol,string &borrow_id);																		//POST /api/v1/cancel_borrow				取消借款申请
	string DoBorrow_Order_info(string &borrow_id);																					//POST /api/v1/borrow_order_info			获取借款订单记录
	string DoRepayment(string &borrow_id);																							//POST /api/v1/repayment					用户还全款
	string DoUnrepayments_Info(string &symbol,string &current_page,string &page_length);											//POST /api/v1/unrepayments_info			未还款列表
	string DoAccount_Records(string &symbol,string &type,string &current_page,string &page_length);									//POST /api/v1/account_records				获取用户提现/充值记录

};


class OKCoinApiCn:public OKCoinApi	//国内站
{
public:
	OKCoinApiCn(string api_key,string secret_key){
		SetKey(api_key,secret_key);
		urlprotocol.InitApi(HTTP_SERVER_TYPE_CN);		//国内站
	};
	~OKCoinApiCn(){};
	
};


class OKCoinApiCom:public OKCoinApi	//国际站
{
public:
	OKCoinApiCom(string api_key,string secret_key){
		SetKey(api_key,secret_key);
		urlprotocol.InitApi(HTTP_SERVER_TYPE_COM);		//国际站
	};
	~OKCoinApiCom(){};
	

	//期货行情 API
	//获取OKCoin期货行情数据

	string DoFuture_Ticker(string &symbol,string &contract_type);																	//GET /api/v1/future_ticker				获取OKCoin期货行情
	string DoFuture_Depth(string &symbol,string &contract_type,string &size,string &merge);											//GET /api/v1/future_depth				获取OKCoin期货深度信息
	string DoFuture_Trades(string &symbol,string &contract_type);																	//GET /api/v1/future_trades				获取OKCoin期货交易记录信息
	string DoFuture_Index(string &symbol);																							//GET /api/v1/future_index				获取OKCoin期货指数信息
	string DoExchange_Rate();																										//GET /api/v1/exchange_rate				获取美元人民币汇率
	string DoFuture_Estimated_Price(string &symbol);																				//GET /api/v1/future_estimated_price	获取交割预估价
	string DoFuture_Kline(string &symbol,string &type,string &contract_type,string &size,string &since);											//GET /api/v1/future_kline				获取期货合约的K线数据
	string DoFuture_Hold_amount(string &symbol,string &contract_type);																//GET /api/v1/future_hold_amount		获取当前可用合约总持仓量



	//期货交易 API
	//用于OKCoin快速进行期货交易
	string DoFuture_Userinfo();																															//POST /api/v1/future_userinfo			获取OKCoin期货账户信息 （全仓）
	string DoFuture_Position(string &symbol,string &contract_type);																						//POST /api/v1/future_position			获取用户持仓获取OKCoin期货账户信息 （全仓）
	string DoFuture_Trade(string &symbol,string &contract_type,string &price,string &amount,string &type,string &match_price,string &lever_rate);		//POST /api/v1/future_trade				期货下单
	string DoFuture_Trades_history(string &symbol,string &date,string &since);																			//POST /api/v1/future_trades_history	获取OKCoin期货交易历史
	string DoFuture_Batch_trade(string &symbol,string &contract_type,string &orders_data,string &lever_rate);											//POST /api/v1/future_batch_trade		批量下单
	string DoFuture_Cancel(string &symbol,string &order_id,string &contract_type);																		//POST /api/v1/future_cancel			取消期货订单
	string DoFuture_Order_info(string &symbol,string &contract_type,string &status,string &order_id,string &current_page,string &page_length);			//POST /api/v1/future_order_info		获取期货订单信息
	string DoFuture_Orders_info(string &symbol,string &contract_type,string &order_id);																	//POST /api/v1/future_orders_info		批量获取期货订单信息
	string DoFuture_Userinfo_4fix();																													//POST /api/v1/future_userinfo_4fix		获取逐仓期货账户信息	
	string DoFuture_Position_4fix(string &symbol,string &contract_type,string &type);																	//POST /api/v1/future_position_4fix		逐仓用户持仓查询
	string DoFuture_Explosive(string &symbol,string &contract_type,string &status,string &current_page,string &page_length);							//POST /api/v1/future_explosive			获取期货爆仓单
	
};


#endif /* __OKCOINAPI_H__ */

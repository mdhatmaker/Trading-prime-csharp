
#ifndef __CURL_PROTOCOL_H__
#define __CURL_PROTOCOL_H__

#include <map>
using namespace std;
#include "uri.h"

const enum HTTP_SERVER_TYPE
{
	HTTP_SERVER_TYPE_CN,		//国内站
	HTTP_SERVER_TYPE_COM,		//国际站
};

const enum HTTP_PROTOCL_TYPE
{
	HTTP_PROTOCL_TYPE_UNKONWN		= 0	,
	
	//现货行情 API
	//获取OKCoin最新市场现货行情数据

	HTTP_API_TYPE_TICKER					,	//Get /api/v1/ticker				获取OKCoin行情
	HTTP_API_TYPE_DEPTH						,	//Get /api/v1/depth					获取OKCoin市场深度
	HTTP_API_TYPE_TRADES					,	//Get /api/v1/trades				获取OKCoin最近600交易信息
	HTTP_API_TYPE_KLINE						,	//Get /api/v1/kline					获取比特币或莱特币的K线数据
											
										
	//现货交易 API						
	//用于OKCoin快速进行现货交易		
											
	HTTP_API_TYPE_USERINFO					,	//POST /api/v1/userinfo				获取用户信息
	HTTP_API_TYPE_TRADE						,	//POST /api/v1/trade				下单交易
	HTTP_API_TYPE_TRADE_HISTORY				,	//POST /api/v1/trade_history		获取OKCoin历史交易信息
	HTTP_API_TYPE_BATCH_TRADE				,	//POST /api/v1/batch_trade			批量下单
	HTTP_API_TYPE_CANCEL_ORDER				,	//POST /api/v1/cancel_order			撤销订单
	HTTP_API_TYPE_ORDER_INFO				,	//POST /api/v1/order_info			获取用户的订单信息
	HTTP_API_TYPE_ORDERS_INFO				,	//POST /api/v1/orders_info			批量获取用户订单
	HTTP_API_TYPE_ORDER_HISTORY				,	//POST /api/v1/order_history		获取历史订单信息，只返回最近七天的信息
	HTTP_API_TYPE_WITHDRAW					,	//POST /api/v1/withdraw				提币BTC/LTC
	HTTP_API_TYPE_CANCEL_WITHDRAW			,	//POST /api/v1/cancel_withdraw		取消提币BTC/LTC
	HTTP_API_TYPE_ORDER_FEE					,	//POST /api/v1/order_fee			查询手续费
	HTTP_API_TYPE_LEND_DEPTH				,	//POST /api/v1/lend_depth			获取放款深度前10
	HTTP_API_TYPE_BORROWS_INFO				,	//POST /api/v1/borrows_info			查询用户借款信息
	HTTP_API_TYPE_BORROW_MONEY				,	//POST /api/v1/borrow_money			申请借款
	HTTP_API_TYPE_CANCEL_BORROW				,	//POST /api/v1/cancel_borrow		取消借款申请
	HTTP_API_TYPE_BORROW_ORDER_INFO			,	//POST /api/v1/borrow_order_info	获取借款订单记录
	HTTP_API_TYPE_REPAYMENT					,	//POST /api/v1/repayment			用户还全款
	HTTP_API_TYPE_UNREPAYMENTS_INFO			,	//POST /api/v1/unrepayments_info	未还款列表
	HTTP_API_TYPE_ACCOUNT_RECORDS			,	//POST /api/v1/account_records		获取用户提现/充值记录
										
	//期货行情 API						
	//获取OKCoin期货行情数据			
										
	HTTP_API_TYPE_FUTURE_TICKER				,	//GET /api/v1/future_ticker				获取OKCoin期货行情
	HTTP_API_TYPE_FUTURE_DEPTH				,	//GET /api/v1/future_depth				获取OKCoin期货深度信息
	HTTP_API_TYPE_FUTURE_TRADES				,	//GET /api/v1/future_trades				获取OKCoin期货交易记录信息
	HTTP_API_TYPE_FUTURE_INDEX				,	//GET /api/v1/future_index				获取OKCoin期货指数信息
	HTTP_API_TYPE_EXCHANGE_RATE				,	//GET /api/v1/exchange_rate				获取美元人民币汇率
	HTTP_API_TYPE_FUTURE_ESTIMATED_PRICE	,	//GET /api/v1/future_estimated_price	获取交割预估价
	HTTP_API_TYPE_FUTURE_KLINE				,	//GET /api/v1/future_kline				获取期货合约的K线数据
	HTTP_API_TYPE_FUTURE_HOLD_AMOUNT		,	//GET /api/v1/future_hold_amount		获取当前可用合约总持仓量
										
	//期货交易 API						
	//用于OKCoin快速进行期货交易		
										
	HTTP_API_TYPE_FUTURE_USERINFO			,	//POST /api/v1/future_userinfo			获取OKCoin期货账户信息 （全仓）
	HTTP_API_TYPE_FUTURE_POSITION			,	//POST /api/v1/future_position			获取用户持仓获取OKCoin期货账户信息 （全仓）
	HTTP_API_TYPE_FUTURE_TRADE				,	//POST /api/v1/future_trade				期货下单
	HTTP_API_TYPE_FUTURE_TRADES_HISTORY		,	//POST /api/v1/future_trades_history	获取OKCoin期货交易历史
	HTTP_API_TYPE_FUTURE_BATCH_TRADE		,	//POST /api/v1/future_batch_trade		批量下单
	HTTP_API_TYPE_FUTURE_CANCEL				,	//POST /api/v1/future_cancel			取消期货订单
	HTTP_API_TYPE_FUTURE_ORDER_INFO			,	//POST /api/v1/future_order_info		获取期货订单信息
	HTTP_API_TYPE_FUTURE_ORDERS_INFO		,	//POST /api/v1/future_orders_info		批量获取期货订单信息
	HTTP_API_TYPE_FUTURE_USERINFO_4FIX		,	//POST /api/v1/future_userinfo_4fix		获取逐仓期货账户信息
	HTTP_API_TYPE_FUTURE_POSITION_4FIX		,	//POST /api/v1/future_position_4fix		逐仓用户持仓查询
	HTTP_API_TYPE_FUTURE_EXPLOSIVE				//POST /api/v1/future_explosive			获取期货爆仓单

};

#define URL_PROTOCOL_HTTPS					"https://"
#define URL_PROTOCOL_HTTP					"http://"


#define URL_MAIN_CN							"https://www.okcoin.cn"
#define URL_PROTOCOL_CN						"https://"
#define URL_DOMAIN_CN						"www.okcoin.cn"


#define URL_MAIN_COM						"https://www.okcoin.com"
#define URL_PROTOCOL_COM					"https://"
#define URL_DOMAIN_COM						"www.okcoin.com"


//现货行情 API
//获取OKCoin最新市场现货行情数据

#define HTTP_API_ticker							"/api/v1/ticker.do"					//Get /api/v1/ticker					获取OKCoin行情
#define HTTP_API_depth							"/api/v1/depth.do"						//Get /api/v1/depth					获取OKCoin市场深度
#define HTTP_API_trades							"/api/v1/trades.do"					//Get /api/v1/trades					获取OKCoin最近600交易信息
#define HTTP_API_kline							"/api/v1/kline.do"						//Get /api/v1/kline					获取比特币或莱特币的K线数据


//现货交易 API
//用于OKCoin快速进行现货交易

#define HTTP_API_USERINFO						"/api/v1/userinfo.do"					//POST /api/v1/userinfo				获取用户信息
#define HTTP_API_TRADE							"/api/v1/trade.do"						//POST /api/v1/trade				下单交易
#define HTTP_API_TRADE_HISTORY					"/api/v1/trade_history.do"				//POST /api/v1/trade_history		获取OKCoin历史交易信息
#define HTTP_API_BATCH_TRADE					"/api/v1/batch_trade.do"				//POST /api/v1/batch_trade			批量下单
#define HTTP_API_CANCEL_ORDER					"/api/v1/cancel_order.do"				//POST /api/v1/cancel_order			撤销订单
#define HTTP_API_ORDER_INFO						"/api/v1/order_info.do"					//POST /api/v1/order_info			获取用户的订单信息
#define HTTP_API_ORDERS_INFO					"/api/v1/orders_info.do"				//POST /api/v1/orders_info			批量获取用户订单
#define HTTP_API_ORDER_HISTORY					"/api/v1/order_history.do"				//POST /api/v1/order_history		获取历史订单信息，只返回最近七天的信息
#define HTTP_API_WITHDRAW						"/api/v1/withdraw.do"					//POST /api/v1/withdraw				提币BTC/LTC
#define HTTP_API_CANCEL_WITHDRAW				"/api/v1/cancel_withdraw.do"			//POST /api/v1/cancel_withdraw		取消提币BTC/LTC
#define HTTP_API_ORDER_FEE						"/api/v1/order_fee.do"					//POST /api/v1/order_fee			查询手续费
#define HTTP_API_LEND_DEPTH						"/api/v1/lend_depth.do"					//POST /api/v1/lend_depth			获取放款深度前10
#define HTTP_API_BORROWS_INFO					"/api/v1/borrows_info.do"				//POST /api/v1/borrows_info			查询用户借款信息
#define HTTP_API_BORROW_MONEY					"/api/v1/borrow_money.do"				//POST /api/v1/borrow_money			申请借款
#define HTTP_API_CANCEL_BORROW					"/api/v1/cancel_borrow.do"				//POST /api/v1/cancel_borrow		取消借款申请
#define HTTP_API_BORROW_ORDER_INFO				"/api/v1/borrow_order_info.do"			//POST /api/v1/borrow_order_info	获取借款订单记录
#define HTTP_API_REPAYMENT						"/api/v1/repayment.do"					//POST /api/v1/repayment			用户还全款
#define HTTP_API_UNREPAYMENTS_INFO				"/api/v1/unrepayments_info.do"			//POST /api/v1/unrepayments_info	未还款列表
#define HTTP_API_ACCOUNT_RECORDS				"/api/v1/account_records.do"			//POST /api/v1/account_records		获取用户提现/充值记录

//期货行情 API
//获取OKCoin期货行情数据
//接口	描述
#define HTTP_API_FUTURE_TICKER					"/api/v1/future_ticker.do"					//GET /api/v1/future_ticker				获取OKCoin期货行情
#define HTTP_API_FUTURE_DEPTH					"/api/v1/future_depth.do"					//GET /api/v1/future_depth				获取OKCoin期货深度信息
#define HTTP_API_FUTURE_TRADES					"/api/v1/future_trades.do"					//GET /api/v1/future_trades				获取OKCoin期货交易记录信息
#define HTTP_API_FUTURE_INDEX					"/api/v1/future_index.do"					//GET /api/v1/future_index				获取OKCoin期货指数信息
#define HTTP_API_EXCHANGE_RATE					"/api/v1/exchange_rate.do"					//GET /api/v1/exchange_rate				获取美元人民币汇率
#define HTTP_API_FUTURE_ESTIMATED_PRICE			"/api/v1/future_estimated_price.do"			//GET /api/v1/future_estimated_price	获取交割预估价
#define HTTP_API_FUTURE_KLINE					"/api/v1/future_kline.do"					//GET /api/v1/future_kline				获取期货合约的K线数据
#define HTTP_API_FUTURE_HOLD_AMOUNT				"/api/v1/future_hold_amount.do"				//GET /api/v1/future_hold_amount		获取当前可用合约总持仓量

//期货交易 API
//用于OKCoin快速进行期货交易
//接口	描述
#define HTTP_API_FUTURE_USERINFO				"/api/v1/future_userinfo.do"				//POST /api/v1/future_userinfo			获取OKCoin期货账户信息 （全仓）
#define HTTP_API_FUTURE_POSITION				"/api/v1/future_position.do"				//POST /api/v1/future_position			获取用户持仓获取OKCoin期货账户信息 （全仓）
#define HTTP_API_FUTURE_TRADE					"/api/v1/future_trade.do"					//POST /api/v1/future_trade				期货下单
#define HTTP_API_FUTURE_TRADES_HISTORY			"/api/v1/future_trades_history.do"			//POST /api/v1/future_trades_history	获取OKCoin期货交易历史
#define HTTP_API_FUTURE_BATCH_TRADE				"/api/v1/future_batch_trade.do"				//POST /api/v1/future_batch_trade		批量下单
#define HTTP_API_FUTURE_CANCEL					"/api/v1/future_cancel.do"					//POST /api/v1/future_cancel			取消期货订单
#define HTTP_API_FUTURE_ORDER_INFO				"/api/v1/future_order_info.do"				//POST /api/v1/future_order_info		获取期货订单信息
#define HTTP_API_FUTURE_ORDERS_INFO				"/api/v1/future_orders_info.do"				//POST /api/v1/future_orders_info		批量获取期货订单信息
#define HTTP_API_FUTURE_USERINFO_4FIX			"/api/v1/future_userinfo_4fix.do"			//POST /api/v1/future_userinfo_4fix		获取逐仓期货账户信息
#define HTTP_API_FUTURE_POSITION_4FIX			"/api/v1/future_position_4fix.do"			//POST /api/v1/future_position_4fix		逐仓用户持仓查询
#define HTTP_API_FUTURE_EXPLOSIVE				"/api/v1/future_explosive.do"				//POST /api/v1/future_explosive			获取期货爆仓单


class CUrlProtocol
{
public:
	CUrlProtocol();
	virtual ~CUrlProtocol();

	int InitApi(HTTP_SERVER_TYPE type);

	void GetUrl(Uri &uri,UINT http_protocl_type);

	map<UINT,Uri> m_urllist;
	char *domain;
	int AddApi(UINT http_protocl_type,HTTP_PROTOCOL http_protocol,char *api,HTTP_METHOD http_method);

};

#endif /* __CURL_PROTOCOL_H__ */
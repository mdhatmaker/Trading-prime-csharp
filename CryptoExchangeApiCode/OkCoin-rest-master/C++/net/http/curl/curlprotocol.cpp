#include "curlprotocol.h"


CUrlProtocol::CUrlProtocol()
{
	domain = URL_DOMAIN_CN;
}

CUrlProtocol::~CUrlProtocol()
{
}

int CUrlProtocol::InitApi(HTTP_SERVER_TYPE type)
{

	if (type == HTTP_SERVER_TYPE_CN )
	{
		domain = URL_DOMAIN_CN;
	}
	if (type == HTTP_SERVER_TYPE_COM )
	{
		domain = URL_DOMAIN_COM;
	}
	

	//现货行情 API
	//获取OKCoin最新市场现货行情数据


	AddApi(HTTP_API_TYPE_TICKER	,HTTP_PROTOCOL_HTTPS,HTTP_API_ticker	,METHOD_GET);						//Get /api/v1/ticker				获取OKCoin行情
	AddApi(HTTP_API_TYPE_DEPTH	,HTTP_PROTOCOL_HTTPS,HTTP_API_depth		,METHOD_GET);						//Get /api/v1/depth					获取OKCoin市场深度
	AddApi(HTTP_API_TYPE_TRADES	,HTTP_PROTOCOL_HTTPS,HTTP_API_trades	,METHOD_GET);						//Get /api/v1/trades				获取OKCoin最近600交易信息
	AddApi(HTTP_API_TYPE_KLINE	,HTTP_PROTOCOL_HTTPS,HTTP_API_kline		,METHOD_GET);						//Get /api/v1/kline					获取比特币或莱特币的K线数据
								

	//现货交易 API
	//用于OKCoin快速进行现货交易

	AddApi(HTTP_API_TYPE_USERINFO			,HTTP_PROTOCOL_HTTPS,HTTP_API_USERINFO			,METHOD_POST);			//POST /api/v1/userinfo				获取用户信息
	AddApi(HTTP_API_TYPE_TRADE				,HTTP_PROTOCOL_HTTPS,HTTP_API_TRADE				,METHOD_POST);			//POST /api/v1/trade				下单交易
	AddApi(HTTP_API_TYPE_TRADE_HISTORY		,HTTP_PROTOCOL_HTTPS,HTTP_API_TRADE_HISTORY		,METHOD_POST);			//POST /api/v1/trade_history		获取OKCoin历史交易信息
	AddApi(HTTP_API_TYPE_BATCH_TRADE		,HTTP_PROTOCOL_HTTPS,HTTP_API_BATCH_TRADE		,METHOD_POST);			//POST /api/v1/batch_trade			批量下单
	AddApi(HTTP_API_TYPE_CANCEL_ORDER		,HTTP_PROTOCOL_HTTPS,HTTP_API_CANCEL_ORDER		,METHOD_POST);			//POST /api/v1/cancel_order			撤销订单
	AddApi(HTTP_API_TYPE_ORDER_INFO			,HTTP_PROTOCOL_HTTPS,HTTP_API_ORDER_INFO		,METHOD_POST);			//POST /api/v1/order_info			获取用户的订单信息
	AddApi(HTTP_API_TYPE_ORDERS_INFO		,HTTP_PROTOCOL_HTTPS,HTTP_API_ORDERS_INFO		,METHOD_POST);			//POST /api/v1/orders_info			批量获取用户订单
	AddApi(HTTP_API_TYPE_ORDER_HISTORY		,HTTP_PROTOCOL_HTTPS,HTTP_API_ORDER_HISTORY		,METHOD_POST);			//POST /api/v1/order_history		获取历史订单信息，只返回最近七天的信息
	AddApi(HTTP_API_TYPE_WITHDRAW			,HTTP_PROTOCOL_HTTPS,HTTP_API_WITHDRAW			,METHOD_POST);			//POST /api/v1/withdraw				提币BTC/LTC
	AddApi(HTTP_API_TYPE_CANCEL_WITHDRAW	,HTTP_PROTOCOL_HTTPS,HTTP_API_CANCEL_WITHDRAW	,METHOD_POST);			//POST /api/v1/cancel_withdraw		取消提币BTC/LTC
	AddApi(HTTP_API_TYPE_ORDER_FEE			,HTTP_PROTOCOL_HTTPS,HTTP_API_ORDER_FEE			,METHOD_POST);			//POST /api/v1/order_fee			查询手续费
	AddApi(HTTP_API_TYPE_LEND_DEPTH			,HTTP_PROTOCOL_HTTPS,HTTP_API_LEND_DEPTH		,METHOD_POST);			//POST /api/v1/lend_depth			获取放款深度前10
	AddApi(HTTP_API_TYPE_BORROWS_INFO		,HTTP_PROTOCOL_HTTPS,HTTP_API_BORROWS_INFO		,METHOD_POST);			//POST /api/v1/borrows_info			查询用户借款信息
	AddApi(HTTP_API_TYPE_BORROW_MONEY		,HTTP_PROTOCOL_HTTPS,HTTP_API_BORROW_MONEY		,METHOD_POST);			//POST /api/v1/borrow_money			申请借款
	AddApi(HTTP_API_TYPE_CANCEL_BORROW		,HTTP_PROTOCOL_HTTPS,HTTP_API_CANCEL_BORROW		,METHOD_POST);			//POST /api/v1/cancel_borrow		取消借款申请
	AddApi(HTTP_API_TYPE_BORROW_ORDER_INFO	,HTTP_PROTOCOL_HTTPS,HTTP_API_BORROW_ORDER_INFO	,METHOD_POST);			//POST /api/v1/borrow_order_info	获取借款订单记录
	AddApi(HTTP_API_TYPE_REPAYMENT			,HTTP_PROTOCOL_HTTPS,HTTP_API_REPAYMENT			,METHOD_POST);			//POST /api/v1/repayment			用户还全款
	AddApi(HTTP_API_TYPE_UNREPAYMENTS_INFO	,HTTP_PROTOCOL_HTTPS,HTTP_API_UNREPAYMENTS_INFO	,METHOD_POST);			//POST /api/v1/unrepayments_info	未还款列表
	AddApi(HTTP_API_TYPE_ACCOUNT_RECORDS	,HTTP_PROTOCOL_HTTPS,HTTP_API_ACCOUNT_RECORDS	,METHOD_POST);			//POST /api/v1/account_records		获取用户提现/充值记录


	if( type == HTTP_SERVER_TYPE_COM)
	{
		//期货行情 API
		//获取OKCoin期货行情数据

		AddApi(HTTP_API_TYPE_FUTURE_TICKER			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_TICKER				,METHOD_GET);		//GET /api/v1/future_ticker			获取OKCoin期货行情
		AddApi(HTTP_API_TYPE_FUTURE_DEPTH			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_DEPTH				,METHOD_GET);		//GET /api/v1/future_depth			获取OKCoin期货深度信息
		AddApi(HTTP_API_TYPE_FUTURE_TRADES			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_TRADES				,METHOD_GET);		//GET /api/v1/future_trades			获取OKCoin期货交易记录信息
		AddApi(HTTP_API_TYPE_FUTURE_INDEX			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_INDEX				,METHOD_GET);		//GET /api/v1/future_index			获取OKCoin期货指数信息
		AddApi(HTTP_API_TYPE_EXCHANGE_RATE			,HTTP_PROTOCOL_HTTPS,HTTP_API_EXCHANGE_RATE				,METHOD_GET);		//GET /api/v1/exchange_rate			获取美元人民币汇率
		AddApi(HTTP_API_TYPE_FUTURE_ESTIMATED_PRICE	,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_ESTIMATED_PRICE	,METHOD_GET);		//GET /api/v1/future_estimated_price	获取交割预估价
		AddApi(HTTP_API_TYPE_FUTURE_KLINE			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_KLINE				,METHOD_GET);		//GET /api/v1/future_kline			获取期货合约的K线数据
		AddApi(HTTP_API_TYPE_FUTURE_HOLD_AMOUNT		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_HOLD_AMOUNT		,METHOD_GET);		//GET /api/v1/future_hold_amount		获取当前可用合约总持仓量

		//期货交易 API
		//用于OKCoin快速进行期货交易

		AddApi(HTTP_API_TYPE_FUTURE_USERINFO		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_USERINFO		,METHOD_POST);		//POST /api/v1/future_userinfo		获取OKCoin期货账户信息 （全仓）
		AddApi(HTTP_API_TYPE_FUTURE_POSITION		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_POSITION		,METHOD_POST);		//POST /api/v1/future_position		获取用户持仓获取OKCoin期货账户信息 （全仓）
		AddApi(HTTP_API_TYPE_FUTURE_TRADE			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_TRADE			,METHOD_POST);		//POST /api/v1/future_trade			期货下单
		AddApi(HTTP_API_TYPE_FUTURE_TRADES_HISTORY	,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_TRADES_HISTORY	,METHOD_POST);		//POST /api/v1/future_trades_history	获取OKCoin期货交易历史
		AddApi(HTTP_API_TYPE_FUTURE_BATCH_TRADE		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_BATCH_TRADE	,METHOD_POST);		//POST /api/v1/future_batch_trade		批量下单
		AddApi(HTTP_API_TYPE_FUTURE_CANCEL			,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_CANCEL			,METHOD_POST);		//POST /api/v1/future_cancel			取消期货订单
		AddApi(HTTP_API_TYPE_FUTURE_ORDER_INFO		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_ORDER_INFO		,METHOD_POST);		//POST /api/v1/future_order_info		获取期货订单信息
		AddApi(HTTP_API_TYPE_FUTURE_ORDERS_INFO		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_ORDERS_INFO	,METHOD_POST);		//POST /api/v1/future_orders_info		批量获取期货订单信息
		AddApi(HTTP_API_TYPE_FUTURE_USERINFO_4FIX	,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_USERINFO_4FIX	,METHOD_POST);		//POST /api/v1/future_userinfo_4fix	获取逐仓期货账户信息
		AddApi(HTTP_API_TYPE_FUTURE_POSITION_4FIX	,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_POSITION_4FIX	,METHOD_POST);		//POST /api/v1/future_position_4fix	逐仓用户持仓查询
		AddApi(HTTP_API_TYPE_FUTURE_EXPLOSIVE		,HTTP_PROTOCOL_HTTPS,HTTP_API_FUTURE_EXPLOSIVE		,METHOD_POST);		//POST /api/v1/future_explosive		获取期货爆仓单
	}
	return 0;
};

void CUrlProtocol::GetUrl(Uri &uri,UINT http_protocl_type)
{
	if(m_urllist.find(http_protocl_type) != m_urllist.end())
	{
		uri = m_urllist[http_protocl_type];
		uri.ClearParam();
		uri.isinit = true;
	}
	return ;
}

int CUrlProtocol::AddApi(UINT http_protocl_type,HTTP_PROTOCOL http_protocol,char *api,HTTP_METHOD http_method)
{
	
	Uri uri;
	uri.type = http_protocl_type;
	uri.protocol = http_protocol;
	if (http_protocol == HTTP_PROTOCOL_HTTP)
	{
		uri.protoclstr = URL_PROTOCOL_HTTP;
	}
	else if (http_protocol == HTTP_PROTOCOL_HTTPS)
	{
		uri.protoclstr = URL_PROTOCOL_HTTPS;
	}
	uri.domain = domain;
	uri.api += api;
	uri.method = http_method;
	m_urllist[http_protocl_type] = uri;

	return 0;
};
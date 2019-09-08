package services

import (
	"encoding/json"
	"fmt"
	"strconv"

	"github.com/MsloveDl/HuobiProAPI/config"
	"github.com/MsloveDl/HuobiProAPI/models"
	"github.com/MsloveDl/HuobiProAPI/untils"
)

// 批量操作的API下个版本再封装

//------------------------------------------------------------------------------------------
// 交易API

// 获取K线数据
// strSymbol: 交易对, btcusdt, bccbtc......
// strPeriod: K线类型, 1min, 5min, 15min......
// nSize: 获取数量, [1-2000]
// return: KLineReturn 对象
func GetKLine(strSymbol, strPeriod string, nSize int) models.KLineReturn {
	kLineReturn := models.KLineReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol
	mapParams["period"] = strPeriod
	mapParams["size"] = strconv.Itoa(nSize)

	strRequestUrl := "/market/history/kline"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonKLineReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonKLineReturn), &kLineReturn)

	return kLineReturn
}

// 获取聚合行情
// strSymbol: 交易对, btcusdt, bccbtc......
// return: TickReturn对象
func GetTicker(strSymbol string) models.TickerReturn {
	tickerReturn := models.TickerReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol

	strRequestUrl := "/market/detail/merged"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonTickReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonTickReturn), &tickerReturn)

	return tickerReturn
}

// 获取交易深度信息
// strSymbol: 交易对, btcusdt, bccbtc......
// strType: Depth类型, step0、step1......stpe5 (合并深度0-5, 0时不合并)
// return: MarketDepthReturn对象
func GetMarketDepth(strSymbol, strType string) models.MarketDepthReturn {
	marketDepthReturn := models.MarketDepthReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol
	mapParams["type"] = strType

	strRequestUrl := "/market/depth"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonMarketDepthReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonMarketDepthReturn), &marketDepthReturn)

	return marketDepthReturn
}

// 获取交易细节信息
// strSymbol: 交易对, btcusdt, bccbtc......
// return: TradeDetailReturn对象
func GetTradeDetail(strSymbol string) models.TradeDetailReturn {
	tradeDetailReturn := models.TradeDetailReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol

	strRequestUrl := "/market/trade"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonTradeDetailReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonTradeDetailReturn), &tradeDetailReturn)

	return tradeDetailReturn
}

// 批量获取最近的交易记录
// strSymbol: 交易对, btcusdt, bccbtc......
// nSize: 获取交易记录的数量, 范围1-2000
// return: TradeReturn对象
func GetTrade(strSymbol string, nSize int) models.TradeReturn {
	tradeReturn := models.TradeReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol
	mapParams["size"] = strconv.Itoa(nSize)

	strRequestUrl := "/market/history/trade"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonTradeReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonTradeReturn), &tradeReturn)

	return tradeReturn
}

// 获取Market Detail 24小时成交量数据
// strSymbol: 交易对, btcusdt, bccbtc......
// return: MarketDetailReturn对象
func GetMarketDetail(strSymbol string) models.MarketDetailReturn {
	marketDetailReturn := models.MarketDetailReturn{}

	mapParams := make(map[string]string)
	mapParams["symbol"] = strSymbol

	strRequestUrl := "/market/detail"
	strUrl := config.MARKET_URL + strRequestUrl

	jsonMarketDetailReturn := untils.HttpGetRequest(strUrl, mapParams)
	json.Unmarshal([]byte(jsonMarketDetailReturn), &marketDetailReturn)

	return marketDetailReturn
}

//------------------------------------------------------------------------------------------
// 公共API

// 查询系统支持的所有交易及精度
// return: SymbolsReturn对象
func GetSymbols() models.SymbolsReturn {
	symbolsReturn := models.SymbolsReturn{}

	strRequestUrl := "/v1/common/symbols"
	strUrl := config.TRADE_URL + strRequestUrl

	jsonSymbolsReturn := untils.HttpGetRequest(strUrl, nil)
	json.Unmarshal([]byte(jsonSymbolsReturn), &symbolsReturn)

	return symbolsReturn
}

// 查询系统支持的所有币种
// return: CurrencysReturn对象
func GetCurrencys() models.CurrencysReturn {
	currencysReturn := models.CurrencysReturn{}

	strRequestUrl := "/v1/common/currencys"
	strUrl := config.TRADE_URL + strRequestUrl

	jsonCurrencysReturn := untils.HttpGetRequest(strUrl, nil)
	json.Unmarshal([]byte(jsonCurrencysReturn), &currencysReturn)

	return currencysReturn
}

// 查询系统当前时间戳
// return: TimestampReturn对象
func GetTimestamp() models.TimestampReturn {
	timestampReturn := models.TimestampReturn{}

	strRequest := "/v1/common/timestamp"
	strUrl := config.TRADE_URL + strRequest

	jsonTimestampReturn := untils.HttpGetRequest(strUrl, nil)
	json.Unmarshal([]byte(jsonTimestampReturn), &timestampReturn)

	return timestampReturn
}

//------------------------------------------------------------------------------------------
// 用户资产API

// 查询当前用户的所有账户, 根据包含的私钥查询
// return: AccountsReturn对象
func GetAccounts() models.AccountsReturn {
	accountsReturn := models.AccountsReturn{}

	strRequest := "/v1/account/accounts"
	jsonAccountsReturn := untils.ApiKeyGet(make(map[string]string), strRequest)
	json.Unmarshal([]byte(jsonAccountsReturn), &accountsReturn)

	return accountsReturn
}

// 根据账户ID查询账户余额
// nAccountID: 账户ID, 不知道的话可以通过GetAccounts()获取, 可以只现货账户, C2C账户, 期货账户
// return: BalanceReturn对象
func GetAccountBalance(strAccountID string) models.BalanceReturn {
	balanceReturn := models.BalanceReturn{}

	strRequest := fmt.Sprintf("/v1/account/accounts/%s/balance", strAccountID)
	jsonBanlanceReturn := untils.ApiKeyGet(make(map[string]string), strRequest)
	json.Unmarshal([]byte(jsonBanlanceReturn), &balanceReturn)

	return balanceReturn
}

//------------------------------------------------------------------------------------------
// 交易API

// 下单
// placeRequestParams: 下单信息
// return: PlaceReturn对象
func Place(placeRequestParams models.PlaceRequestParams) models.PlaceReturn {
	placeReturn := models.PlaceReturn{}

	mapParams := make(map[string]string)
	mapParams["account-id"] = placeRequestParams.AccountID
	mapParams["amount"] = placeRequestParams.Amount
	if 0 < len(placeRequestParams.Price) {
		mapParams["price"] = placeRequestParams.Price
	}
	if 0 < len(placeRequestParams.Source) {
		mapParams["source"] = placeRequestParams.Source
	}
	mapParams["symbol"] = placeRequestParams.Symbol
	mapParams["type"] = placeRequestParams.Type

	strRequest := "/v1/order/orders/place"
	jsonPlaceReturn := untils.ApiKeyPost(mapParams, strRequest)
	json.Unmarshal([]byte(jsonPlaceReturn), &placeReturn)

	return placeReturn
}

// 申请撤销一个订单请求
// strOrderID: 订单ID
// return: PlaceReturn对象
func SubmitCancel(strOrderID string) models.PlaceReturn {
	placeReturn := models.PlaceReturn{}

	strRequest := fmt.Sprintf("/v1/order/orders/%s/submitcancel", strOrderID)
	jsonPlaceReturn := untils.ApiKeyPost(make(map[string]string), strRequest)
	json.Unmarshal([]byte(jsonPlaceReturn), &placeReturn)

	return placeReturn
}


# HitBtcApi


## ApiResponse

Converting JSON to Models


## Categories.MarketData

Market data RESTful API


### M:HitBtcApi.GetOrderbook(symbol)

returns a list of open orders for specified currency symbol: their prices and sizes. /api/1/public/:symbol/orderbook

| Name | Description |
| ---- | ----------- |
| symbol | *HitBtcApi.Model.Symbol*<br>is a currency symbol traded on HitBTC exchange  |


#### Returns




### M:HitBtcApi.GetPublickTickers

returns the actual data on exchange rates for all traded cryptocurrencies - all tickers. /api/1/public/ticker


#### Returns




### M:HitBtcApi.GetRecentTrades(symbol, max_results)

returns recent trades for the specified currency symbol. /api/1/public/:symbol/trades/recent

| Name | Description |
| ---- | ----------- |
| symbol | *HitBtcApi.Model.Symbol*<br>is a currency symbol traded on HitBTC exchange |
| max_results | *System.Int32*<br>Maximum quantity of returned results, at most 1000 |


#### Returns




### M:HitBtcApi.GetSymbols

Simbols returns the actual list of currency symbols traded on HitBTC exchange with their characteristics /api/1/public/symbols


#### Returns




### M:HitBtcApi.GetSymbolTicker(symbol)

returns the actual data on exchange rates of the specified cryptocurrency. /api/1/public/:symbol/ticker

| Name | Description |
| ---- | ----------- |
| symbol | *HitBtcApi.Model.Symbol*<br>is a currency symbol traded on HitBTC exchange |


#### Returns




### M:HitBtcApi.GetTimestamp

returns the server time in UNIX timestamp format /api/1/public/time


#### Returns




## Categories.Payment

Payment RESTful API


### M:HitBtcApi.CreateAddress(currency)

returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. /api/1/payment/address/ (GET)

| Name | Description |
| ---- | ----------- |
| currency | *System.String*<br> |


#### Returns




### M:HitBtcApi.GetAddress(currency)

returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. /api/1/payment/address/ (GET)

| Name | Description |
| ---- | ----------- |
| currency | *System.String*<br> |


#### Returns




### M:HitBtcApi.GetMultiCurrencyBalance

returns multi-currency balance of the main account. /api/1/payment/balance


#### Returns




### M:HitBtcApi.GetPyout(amount, currency_code, address, id)

withdraws money and creates an outgoing crypotocurrency transaction; returns a transaction ID on the exchange or an error. /api/1/payment/payout

| Name | Description |
| ---- | ----------- |
| amount | *System.Decimal*<br>Funds amount to withdraw, Required |
| currency_code | *System.String*<br>Currency symbol, e.g.BTC, Required |
| address | *System.String*<br>BTC/LTC address to withdraw to, Required |
| id | *System.String*<br>payment id for cryptonote |


#### Returns




### M:HitBtcApi.GetTransactions(offset, limit, dir)

returns a list of payment transactions and their statuses(array of transactions). /api/1/payment/transactions

| Name | Description |
| ---- | ----------- |
| offset | *System.Int32*<br>Start index for the query, default = 0 |
| limit | *System.String*<br>Maximum results for the query, Required |
| dir | *System.Int32*<br>Transactions are sorted ascending (ask) or descending (desc) (default) |


#### Returns




### M:HitBtcApi.GetTransactions(id)

returns payment transaction and its status. /api/1/payment/transactions/:id

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>Transaction Id, Required |


#### Returns




### M:HitBtcApi.TransferToMain(amount, currency_code)

transfers funds between main and trading accounts; returns a transaction ID or an error. /api/1/payment/transfer_to_main

| Name | Description |
| ---- | ----------- |
| amount | *System.Decimal*<br>Funds amount to transfer, Required |
| currency_code | *System.String*<br>Currency symbol, e.g. BTC, Required |


#### Returns




### M:HitBtcApi.TransferToTrading(amount, currency_code)

transfers funds between main and trading accounts; returns a transaction ID or an error. /api/1/payment/transfer_to_trading

| Name | Description |
| ---- | ----------- |
| amount | *System.Decimal*<br>Funds amount to transfer, Required |
| currency_code | *System.String*<br>Currency symbol, e.g. BTC, Required |


#### Returns




## Categories.Trading

Trading RESTful API


### M:HitBtcApi.GetActiveOrders(symbols, clientOrderId)

returns all orders in status new or partiallyFilled. /api/1/trading/orders/active

| Name | Description |
| ---- | ----------- |
| symbols | *System.String*<br>Comma-separated list of symbols. Default - all symbols |
| clientOrderId | *System.String*<br>Unique order ID |


#### Returns




### M:HitBtcApi.GeTradingBalance

returns trading balance. /api/1/trading/balance


#### Returns




### M:HitBtcApi.GetRecentOrders(start_index, max_results, sort, symbols, statuses)

returns an array of user’s recent orders (order objects) for last 24 hours, sorted by order update time. /api/1/trading/orders/recent

| Name | Description |
| ---- | ----------- |
| start_index | *System.Int32*<br>Zero-based index, 0 by default |
| max_results | *System.Int32*<br>Maximum quantity of returned items, at most 1000, Required |
| sort | *System.String*<br>Orders are sorted ascending (default) or descending |
| symbols | *System.String*<br>Comma-separated list of currency symbols |
| statuses | *System.String*<br>Comma-separated list of order statuses: new, partiallyFilled, filled, canceled, expired,rejected |


#### Returns




### M:HitBtcApi.HitBtcApi.Authorize(apiKey, secretKey)

Method for authorization

| Name | Description |
| ---- | ----------- |
| apiKey | *System.String*<br>API key from the Settings page. |
| secretKey | *System.String*<br>Secret key from the Settings page. |

### M:HitBtcApi.HitBtcApi.Execute(request, requireAuthentication)

Method that allow to execute a request to api

| Name | Description |
| ---- | ----------- |
| request | *RestSharp.RestRequest*<br> |
| requireAuthentication | *System.Boolean*<br> |


#### Returns




### .HitBtcApi.IsAuthorized

Flag shows that user is authorized


### .Model.Address.address

BTC/LTC address to withdraw to


### .Model.Balance.balance

Funds amount


### .Model.Balance.currency_code

Currency symbol, e.g. BTC


### .Model.Order.avgPrice

Average price


### .Model.Order.clientOrderId

Unique client-generated ID


### .Model.Order.cumQuantity

Cumulative quantity


### .Model.Order.execQuantity

Last executed quantity, in lots


### .Model.Order.lastTimestamp

UTC timestamp of the last change, in milliseconds


### .Model.Order.orderId

Order ID on the exchange


### .Model.Order.orderPrice

Order price


### .Model.Order.orderQuantity

Order quantity, in lots


### .Model.Order.orderStatus

Order status new, partiallyFilled, filled, canceled, expired, rejected


### .Model.Order.quantityLeaves

Remaining quantity, in lots


### .Model.Order.side

Side of a trade


### .Model.Order.symbo

Currency symbol


### .Model.Order.timeInForce

Time in force GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day


### .Model.Order.type

Type of an order


### .Model.Symbol.commodity

Second value of this symbol


### .Model.Symbol.currency

Value of this symbol


### .Model.Symbol.lot

Lot size parameter


### .Model.Symbol.provideLiquidityRate

Liquidity provider rebate


### .Model.Symbol.step

Price step parameter


### .Model.Symbol.symbol

Symbol name


### .Model.Symbol.takeLiquidityRate

Liquidity taker fee


### .Model.Ticker.ask

Lowest sell order


### .Model.Ticker.bid

Highest buy order


### .Model.Ticker.high

Highest trade price per last 24h + last incomplete minute


### .Model.Ticker.last

Last price


### .Model.Ticker.low

Lowest trade price per last 24h + last incomplete minute


### .Model.Ticker.open

Price in which instrument open


### .Model.Ticker.timestamp

Server time in UNIX timestamp format


### .Model.Ticker.volume

Volume per last 24h + last incomplete minute


### .Model.Ticker.volume_quote

Volume in second currency per last 24h + last incomplete minute


### .Model.Timestamp.timestamp

time in UNIX timestamp format


### .Model.Trade.clientOrderId

Unique order ID generated by client. From 8 to 32 characters


### .Model.Trade.execPrice

Trade price


### .Model.Trade.execQuantity

Trade size, in lots


### .Model.Trade.fee

Fee for the trade, negative value means rebate


### .Model.Trade.originalOrderId

Order ID on the exchange


### .Model.Trade.side

Side of a trade


### .Model.Trade.symbol

Currency symbol


### .Model.Trade.timestamp

Timestamp, in milliseconds


### .Model.Trade.tradeId

Trade ID on the exchange


### .Model.TradingBalance.cash




### .Model.TradingBalance.currency_code

Currency symbol, e.g. BTC


### .Model.TradingBalance.reserved




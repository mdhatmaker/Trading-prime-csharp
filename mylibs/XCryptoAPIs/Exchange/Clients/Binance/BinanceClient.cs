using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.Binance.Websocket;

namespace CryptoAPIs.Exchange.Clients.Binance
{
    #region ------------------------------------ BINANCE CLIENT ENUMS ---------------------------------------------------------------------
    /// <summary>
    /// HTTPMethods to be used by the API.
    /// </summary>
    public enum ApiMethod
    {
        POST,
        GET,
        PUT,
        DELETE
    }

    public enum DepositStatus
    {
        Pending = 0,
        Success = 1
    }

    /// <summary>
    /// Different sides of an order.
    /// </summary>
    public enum OrderSide
    {
        BUY,
        SELL
    }

    /// <summary>
    /// Different types of an order.
    /// </summary>
    public enum OrderType
    {
        LIMIT,
        MARKET
    }

    /// <summary>
    /// Different Time in force of an order.
    /// </summary>
    public enum TimeInForce
    {
        GTC,
        IOC
    }

    /// <summary>
    /// Time interval for the candlestick.
    /// </summary>
    public enum TimeInterval
    {
        [Description("1m")]
        Minutes_1,
        [Description("3m")]
        Minutes_3,
        [Description("5m")]
        Minutes_5,
        [Description("15m")]
        Minutes_15,
        [Description("30m")]
        Minutes_30,
        [Description("1h")]
        Hours_1,
        [Description("2h")]
        Hours_2,
        [Description("4h")]
        Hours_4,
        [Description("6h")]
        Hours_6,
        [Description("8h")]
        Hours_8,
        [Description("12h")]
        Hours_12,
        [Description("1d")]
        Days_1,
        [Description("3d")]
        Days_3,
        [Description("1w")]
        Weeks_1,
        [Description("1M")]
        Months_1
    }

    public enum WithdrawStatus
    {
        EmailSent = 0,
        Cancelled = 1,
        AwaitingApproval = 2,
        Rejected = 3,
        Processing = 4,
        Failure = 5,
        Completed = 6
    }
    #endregion ----------------------------------------------------------------------------------------------------------------------------


    public class ApiClient : ApiClientAbstract, IApiClient
    {

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="apiKey">Key used to authenticate within the API.</param>
        /// <param name="apiSecret">API secret used to signed API calls.</param>
        /// <param name="apiUrl">API base url.</param>
        public ApiClient(string apiKey, string apiSecret, string apiUrl = @"https://www.binance.com", string webSocketEndpoint = @"wss://stream.binance.com:9443/ws/", bool addDefaultHeaders = true) : base(apiKey, apiSecret, apiUrl, webSocketEndpoint, addDefaultHeaders)
        {
        }

        /// <summary>
        /// Calls API Methods.
        /// </summary>
        /// <typeparam name="T">Type to which the response content will be converted.</typeparam>
        /// <param name="method">HTTPMethod (POST-GET-PUT-DELETE)</param>
        /// <param name="endpoint">Url endpoing.</param>
        /// <param name="isSigned">Specifies if the request needs a signature.</param>
        /// <param name="parameters">Request parameters.</param>
        /// <returns></returns>
        public async Task<T> CallAsync<T>(ApiMethod method, string endpoint, bool isSigned = false, string parameters = null)
        {
            var finalEndpoint = endpoint + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (isSigned)
            {
                // Joining provided parameters
                parameters += (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + Utilities.GenerateTimeStamp(DateTime.Now.ToUniversalTime());

                // Creating request signature
                var signature = Utilities.GenerateSignature(_apiSecret, parameters);
                finalEndpoint = $"{endpoint}?{parameters}&signature={signature}";
            }

            var request = new HttpRequestMessage(Utilities.CreateHttpMethod(method.ToString()), finalEndpoint);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                // Api return is OK
                response.EnsureSuccessStatusCode();

                // Get the result
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // Serialize and return result
                return JsonConvert.DeserializeObject<T>(result);
            }

            // We received an error
            if (response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                //throw new Exception("Api Request Timeout.");
                Console.WriteLine("\n***BinanceClient::CallAsync<T>=> Api Request Timeout\n");
                return default(T);
            }

            // Get te error code and message
            var e = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Error Values
            var eCode = 0;
            string eMsg = "";
            if (e.IsValidJson())
            {
                try
                {
                    var i = JObject.Parse(e);

                    eCode = i["code"]?.Value<int>() ?? 0;
                    eMsg = i["msg"]?.Value<string>();
                }
                catch { }
            }

            //throw new Exception(string.Format("Api Error Code: {0} Message: {1}", eCode, eMsg));
            ErrorMessage(string.Format("***BinanceClient::CallAsync<T>=> Api Error Code: {0} Message: {1}", eCode, eMsg));
            return default(T);  // Task.FromResult(default(T));
        }

        /// <summary>
        /// Connects to a Websocket endpoint.
        /// </summary>
        /// <typeparam name="T">Type used to parsed the response message.</typeparam>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="messageDelegate">Deletage to callback after receive a message.</param>
        /// <param name="useCustomParser">Specifies if needs to use a custom parser for the response message.</param>
        public void ConnectToWebSocket<T>(string parameters, MessageHandler<T> messageHandler, bool useCustomParser = false)
        {
            var finalEndpoint = _webSocketEndpoint + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += (sender, e) =>
            {
                dynamic eventData;

                if (useCustomParser)
                {
                    var customParser = new CustomParser();
                    eventData = customParser.GetParsedDepthMessage(JsonConvert.DeserializeObject<dynamic>(e.Data));
                }
                else
                {
                    eventData = JsonConvert.DeserializeObject<T>(e.Data);
                }

                messageHandler(eventData);
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }

        /// <summary>
        /// Connects to a UserData Websocket endpoint.
        /// </summary>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="accountHandler">Deletage to callback after receive a account info message.</param>
        /// <param name="tradeHandler">Deletage to callback after receive a trade message.</param>
        /// <param name="orderHandler">Deletage to callback after receive a order message.</param>
        public void ConnectToUserDataWebSocket(string parameters, MessageHandler<AccountUpdatedMessage> accountHandler, MessageHandler<OrderOrTradeUpdatedMessage> tradeHandler, MessageHandler<OrderOrTradeUpdatedMessage> orderHandler)
        {
            var finalEndpoint = _webSocketEndpoint + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += (sender, e) =>
            {
                var eventData = JsonConvert.DeserializeObject<dynamic>(e.Data);

                switch (eventData.e)
                {
                    case "outboundAccountInfo":
                        accountHandler(JsonConvert.DeserializeObject<AccountUpdatedMessage>(e.Data));
                        break;
                    case "executionReport":
                        var isTrade = ((string)eventData.x).ToLower() == "trade";

                        if (isTrade)
                        {
                            tradeHandler(JsonConvert.DeserializeObject<OrderOrTradeUpdatedMessage>(e.Data));
                        }
                        else
                        {
                            orderHandler(JsonConvert.DeserializeObject<OrderOrTradeUpdatedMessage>(e.Data));
                        }
                        break;
                }
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }
    } // end of class ApiClient



    public class BinanceClient : BinanceClientAbstract, IBinanceClient
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="apiClient">API client to be used for API calls.</param>
        /// <param name="loadTradingRules">Optional parameter to skip loading trading rules.</param>
        public BinanceClient(IApiClient apiClient, bool loadTradingRules = false) : base(apiClient)
        {
            if (loadTradingRules)
            {
                LoadTradingRules();
            }
        }

        #region Private Methods
        /// <summary>
        /// Validates that a new order is valid before posting it.
        /// </summary>
        /// <param name="orderType">Order type (LIMIT-MARKET).</param>
        /// <param name="symbolInfo">Object with the information of the ticker.</param>
        /// <param name="unitPrice">Price of the transaction.</param>
        /// <param name="quantity">Quantity to transaction.</param>
        /// <param name="stopPrice">Price for stop orders.</param>
        private void ValidateOrderValue(string symbol, OrderType orderType, decimal unitPrice, decimal quantity, decimal icebergQty)
        {
            // Validating parameters values.
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Invalid symbol. ", "symbol");
            }
            if (quantity <= 0m)
            {
                throw new ArgumentException("Quantity must be greater than zero.", "quantity");
            }
            if (orderType == OrderType.LIMIT)
            {
                if (unitPrice <= 0m)
                {
                    throw new ArgumentException("Price must be greater than zero.", "price");
                }
            }

            // Validating Trading Rules
            if (_tradingRules != null)
            {
                var symbolInfo = _tradingRules.Symbols.Where(r => r.SymbolName.ToUpper() == symbol.ToUpper()).FirstOrDefault();
                var priceFilter = symbolInfo.Filters.Where(r => r.FilterType == "PRICE_FILTER").FirstOrDefault();
                var sizeFilter = symbolInfo.Filters.Where(r => r.FilterType == "LOT_SIZE").FirstOrDefault();

                if (symbolInfo == null)
                {
                    throw new ArgumentException("Invalid symbol. ", "symbol");
                }
                if (quantity < sizeFilter.MinQty)
                {
                    throw new ArgumentException($"Quantity for this symbol is lower than allowed! Quantity must be greater than: {sizeFilter.MinQty}", "quantity");
                }
                if (icebergQty > 0m && !symbolInfo.IcebergAllowed)
                {
                    throw new Exception($"Iceberg orders not allowed for this symbol.");
                }

                if (orderType == OrderType.LIMIT)
                {
                    if (unitPrice < priceFilter.MinPrice)
                    {
                        throw new ArgumentException($"Price for this symbol is lower than allowed! Price must be greater than: {priceFilter.MinPrice}", "price");
                    }
                }
            }
        }

        private void LoadTradingRules()
        {
            var apiClient = new ApiClient("", "", EndPoints.TradingRules, addDefaultHeaders: false);
            _tradingRules = apiClient.CallAsync<TradingRules>(ApiMethod.GET, "").Result;
        }
        #endregion

        #region General
        /// Test connectivity to the Rest API.
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> TestConnectivity()
        {
            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.GET, EndPoints.TestConnectivity, false);

            return result;
        }
        /// <summary>
        /// Test connectivity to the Rest API and get the current server time.
        /// </summary>
        /// <returns></returns>
        public async Task<ServerInfo> GetServerTime()
        {
            var result = await _apiClient.CallAsync<ServerInfo>(ApiMethod.GET, EndPoints.CheckServerTime, false);

            return result;
        }
        #endregion

        #region Market Data
        /// <summary>
        /// Get order book for a particular symbol.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="limit">Limit of records to retrieve.</param>
        /// <returns></returns>
        public async Task<OrderBook> GetOrderBook(string symbol, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.GET, EndPoints.OrderBook, false, $"symbol={symbol.ToUpper()}&limit={limit}");

            var parser = new CustomParser();
            var parsedResult = parser.GetParsedOrderBook(result);

            return parsedResult;
        }

        /// <summary>
        /// Get compressed, aggregate trades. Trades that fill at the time, from the same order, with the same price will have the quantity aggregated.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="limit">Limit of records to retrieve.</param>
        /// <returns></returns>
        public async Task<IEnumerable<AggregateTrade>> GetAggregateTrades(string symbol, int limit = 500)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var result = await _apiClient.CallAsync<IEnumerable<AggregateTrade>>(ApiMethod.GET, EndPoints.AggregateTrades, false, $"symbol={symbol.ToUpper()}&limit={limit}");

            return result;
        }

        /// <summary>
        /// Kline/candlestick bars for a symbol. Klines are uniquely identified by their open time.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="interval">Time interval to retreive.</param>
        /// <param name="limit">Limit of records to retrieve.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Candlestick>> GetCandleSticks(string symbol, TimeInterval interval, DateTime? startTime = null, DateTime? endTime = null, int limit = 500)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var args = $"symbol={symbol.ToUpper()}&interval={interval.GetDescription()}"
                + (startTime.HasValue ? $"&startTime={startTime.Value.GetUnixTimeStamp()}" : "")
                + (endTime.HasValue ? $"&endTime={endTime.Value.GetUnixTimeStamp()}" : "")
                + $"&limit={limit}";

            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.GET, EndPoints.Candlesticks, false, args);

            var parser = new CustomParser();
            var parsedResult = parser.GetParsedCandlestick(result);

            return parsedResult;
        }

        /// <summary>
        /// 24 hour price change statistics.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PriceChangeInfo>> GetPriceChange24H(string symbol = "")
        {
            var args = string.IsNullOrWhiteSpace(symbol) ? "" : $"symbol={symbol.ToUpper()}";

            var result = new List<PriceChangeInfo>();

            if (!string.IsNullOrEmpty(symbol))
            {
                var data = await _apiClient.CallAsync<PriceChangeInfo>(ApiMethod.GET, EndPoints.TickerPriceChange24H, false, args);
                result.Add(data);
            }
            else
            {
                result = await _apiClient.CallAsync<List<PriceChangeInfo>>(ApiMethod.GET, EndPoints.TickerPriceChange24H, false, args);
            }

            return result;
        }

        /// <summary>
        /// Latest price for all symbols.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SymbolPrice>> GetAllPrices()
        {
            var result = await _apiClient.CallAsync<IEnumerable<SymbolPrice>>(ApiMethod.GET, EndPoints.AllPrices, false);

            return result;
        }

        /// <summary>
        /// Best price/qty on the order book for all symbols.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OrderBookTicker>> GetOrderBookTicker()
        {
            var result = await _apiClient.CallAsync<IEnumerable<OrderBookTicker>>(ApiMethod.GET, EndPoints.OrderBookTicker, false);

            return result;
        }
        #endregion

        #region Account Information
        /// <summary>
        /// Send in a new order.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="quantity">Quantity to transaction.</param>
        /// <param name="price">Price of the transaction.</param>
        /// <param name="orderType">Order type (LIMIT-MARKET).</param>
        /// <param name="side">Order side (BUY-SELL).</param>
        /// <param name="timeInForce">Indicates how long an order will remain active before it is executed or expires.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<NewOrder> PostNewOrder(string symbol, decimal quantity, decimal price, OrderSide side, OrderType orderType = OrderType.LIMIT, TimeInForce timeInForce = TimeInForce.GTC, decimal icebergQty = 0m, long recvWindow = 5000)
        {
            //Validates that the order is valid.
            ValidateOrderValue(symbol, orderType, price, quantity, icebergQty);

            var args = $"symbol={symbol.ToUpper()}&side={side}&type={orderType}&quantity={quantity}"
                + (orderType == OrderType.LIMIT ? $"&timeInForce={timeInForce}" : "")
                + (orderType == OrderType.LIMIT ? $"&price={price}" : "")
                + (icebergQty > 0m ? $"&icebergQty={icebergQty}" : "")
                + $"&recvWindow={recvWindow}";
            var result = await _apiClient.CallAsync<NewOrder>(ApiMethod.POST, EndPoints.NewOrder, true, args);
                      
            return result;
        }

        /// <summary>
        /// Test new order creation and signature/recvWindow long. Creates and validates a new order but does not send it into the matching engine.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="quantity">Quantity to transaction.</param>
        /// <param name="price">Price of the transaction.</param>
        /// <param name="orderType">Order type (LIMIT-MARKET).</param>
        /// <param name="side">Order side (BUY-SELL).</param>
        /// <param name="timeInForce">Indicates how long an order will remain active before it is executed or expires.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<dynamic> PostNewOrderTest(string symbol, decimal quantity, decimal price, OrderSide side, OrderType orderType = OrderType.LIMIT, TimeInForce timeInForce = TimeInForce.GTC, decimal icebergQty = 0m, long recvWindow = 5000)
        {
            //Validates that the order is valid.
            ValidateOrderValue(symbol, orderType, price, quantity, icebergQty);

            var args = $"symbol={symbol.ToUpper()}&side={side}&type={orderType}&quantity={quantity}"
                + (orderType == OrderType.LIMIT ? $"&timeInForce={timeInForce}" : "")
                + (orderType == OrderType.LIMIT ? $"&price={price}" : "")
                + (icebergQty > 0m ? $"&icebergQty={icebergQty}" : "")
                + $"&recvWindow={recvWindow}";
            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.POST, EndPoints.NewOrderTest, true, args);

            return result;
        }

        /// <summary>
        /// Check an order's status.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="orderId">Id of the order to retrieve.</param>
        /// <param name="origClientOrderId">origClientOrderId of the order to retrieve.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<Order> GetOrder(string symbol, long? orderId = null, string origClientOrderId = null, long recvWindow = 5000)
        {
            var args = $"symbol={symbol.ToUpper()}&recvWindow={recvWindow}";

            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            if (orderId.HasValue)
            {
                args += $"&orderId={orderId.Value}";
            }
            else if (!string.IsNullOrWhiteSpace(origClientOrderId))
            {
                args += $"&origClientOrderId={origClientOrderId}";
            }
            else
            {
                throw new ArgumentException("Either orderId or origClientOrderId must be sent.");
            }

            var result = await _apiClient.CallAsync<Order>(ApiMethod.GET, EndPoints.QueryOrder, true, args);

            return result;
        }

        /// <summary>
        /// Cancel an active order.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="orderId">Id of the order to cancel.</param>
        /// <param name="origClientOrderId">origClientOrderId of the order to cancel.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<CanceledOrder> CancelOrder(string symbol, long? orderId = null, string origClientOrderId = null, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var args = $"symbol={symbol.ToUpper()}&recvWindow={recvWindow}";

            if (orderId.HasValue)
            {
                args += $"&orderId={orderId.Value}";
            }
            else if (string.IsNullOrWhiteSpace(origClientOrderId))
            {
                args += $"&origClientOrderId={origClientOrderId}";
            }
            else
            {
                throw new ArgumentException("Either orderId or origClientOrderId must be sent.");
            }

            var result = await _apiClient.CallAsync<CanceledOrder>(ApiMethod.DELETE, EndPoints.CancelOrder, true, args);

            return result;
        }

        /// <summary>
        /// Get all open orders on a symbol.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetCurrentOpenOrders(string symbol, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var result = await _apiClient.CallAsync<IEnumerable<Order>>(ApiMethod.GET, EndPoints.CurrentOpenOrders, true, $"symbol={symbol.ToUpper()}&recvWindow={recvWindow}");

            return result;
        }

        /// <summary>
        /// Get all account orders; active, canceled, or filled.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="orderId">If is set, it will get orders >= that orderId. Otherwise most recent orders are returned.</param>
        /// <param name="limit">Limit of records to retrieve.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetAllOrders(string symbol, long? orderId = null, int limit = 500, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var result = await _apiClient.CallAsync<IEnumerable<Order>>(ApiMethod.GET, EndPoints.AllOrders, true, $"symbol={symbol.ToUpper()}&limit={limit}&recvWindow={recvWindow}" + (orderId.HasValue ? $"&orderId={orderId.Value}" : ""));

            return result;
        }

        /// <summary>
        /// Get current account information.
        /// </summary>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountInfo(long recvWindow = 5000)
        {
            var result = await _apiClient.CallAsync<AccountInfo>(ApiMethod.GET, EndPoints.AccountInformation, true, $"recvWindow={recvWindow}");

            return result;
        }

        /// <summary>
        /// Get trades for a specific account and symbol.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Trade>> GetTradeList(string symbol, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var result = await _apiClient.CallAsync<IEnumerable<Trade>>(ApiMethod.GET, EndPoints.TradeList, true, $"symbol={symbol.ToUpper()}&recvWindow={recvWindow}");

            return result;
        }

        /// <summary>
        /// Submit a withdraw request.
        /// </summary>
        /// <param name="asset">Asset to withdraw.</param>
        /// <param name="amount">Amount to withdraw.</param>
        /// <param name="address">Address where the asset will be deposited.</param>
        /// <param name="addressName">Address name.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<WithdrawResponse> Withdraw(string asset, decimal amount, string address, string addressName = "", long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(asset))
            {
                throw new ArgumentException("asset cannot be empty. ", "asset");
            }
            if (amount <= 0m)
            {
                throw new ArgumentException("amount must be greater than zero.", "amount");
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("address cannot be empty. ", "address");
            }

            var args = $"asset={asset.ToUpper()}&amount={amount}&address={address}"
              + (!string.IsNullOrWhiteSpace(addressName) ? $"&name={addressName}" : "")
              + $"&recvWindow={recvWindow}";

            var result = await _apiClient.CallAsync<WithdrawResponse>(ApiMethod.POST, EndPoints.Withdraw, true, args);

            return result;
        }

        /// <summary>
        /// Fetch deposit history.
        /// </summary>
        /// <param name="asset">Asset you want to see the information for.</param>
        /// <param name="status">Deposit status.</param>
        /// <param name="startTime">Start time. </param>
        /// <param name="endTime">End time.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<DepositHistory> GetDepositHistory(string asset, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(asset))
            {
                throw new ArgumentException("asset cannot be empty. ", "asset");
            }

            var args = $"asset={asset.ToUpper()}"
              + (status.HasValue ? $"&status={(int)status}" : "")
              + (startTime.HasValue ? $"&startTime={startTime.Value.GetUnixTimeStamp()}" : "")
              + (endTime.HasValue ? $"&endTime={endTime.Value.GetUnixTimeStamp()}" : "")
              + $"&recvWindow={recvWindow}";

            var result = await _apiClient.CallAsync<DepositHistory>(ApiMethod.POST, EndPoints.DepositHistory, true, args);

            return result;
        }

        /// <summary>
        /// Fetch withdraw history.
        /// </summary>
        /// <param name="asset">Asset you want to see the information for.</param>
        /// <param name="status">Withdraw status.</param>
        /// <param name="startTime">Start time. </param>
        /// <param name="endTime">End time.</param>
        /// <param name="recvWindow">Specific number of milliseconds the request is valid for.</param>
        /// <returns></returns>
        public async Task<WithdrawHistory> GetWithdrawHistory(string asset, WithdrawStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, long recvWindow = 5000)
        {
            if (string.IsNullOrWhiteSpace(asset))
            {
                throw new ArgumentException("asset cannot be empty. ", "asset");
            }

            var args = $"asset={asset.ToUpper()}"
              + (status.HasValue ? $"&status={(int)status}" : "")
              + (startTime.HasValue ? $"&startTime={Utilities.GenerateTimeStamp(startTime.Value)}" : "")
              + (endTime.HasValue ? $"&endTime={Utilities.GenerateTimeStamp(endTime.Value)}" : "")
              + $"&recvWindow={recvWindow}";

            var result = await _apiClient.CallAsync<WithdrawHistory>(ApiMethod.POST, EndPoints.WithdrawHistory, true, args);

            return result;
        }
        #endregion

        #region User Stream
        /// <summary>
        /// Start a new user data stream.
        /// </summary>
        /// <returns></returns>
        public async Task<UserStreamInfo> StartUserStream()
        {
            var result = await _apiClient.CallAsync<UserStreamInfo>(ApiMethod.POST, EndPoints.StartUserStream, false);

            return result;
        }

        /// <summary>
        /// PING a user data stream to prevent a time out.
        /// </summary>
        /// <param name="listenKey">Listenkey of the user stream to keep alive.</param>
        /// <returns></returns>
        public async Task<dynamic> KeepAliveUserStream(string listenKey)
        {
            if (string.IsNullOrWhiteSpace(listenKey))
            {
                throw new ArgumentException("listenKey cannot be empty. ", "listenKey");
            }

            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.PUT, EndPoints.KeepAliveUserStream, false, $"listenKey={listenKey}");

            return result;
        }

        /// <summary>
        /// Close out a user data stream.
        /// </summary>
        /// <param name="listenKey">Listenkey of the user stream to close.</param>
        /// <returns></returns>
        public async Task<dynamic> CloseUserStream(string listenKey)
        {
            if (string.IsNullOrWhiteSpace(listenKey))
            {
                throw new ArgumentException("listenKey cannot be empty. ", "listenKey");
            }

            var result = await _apiClient.CallAsync<dynamic>(ApiMethod.DELETE, EndPoints.CloseUserStream, false, $"listenKey={listenKey}");

            return result;
        }
        #endregion

        #region Web Socket Client
        /// <summary>
        /// Listen to the Depth endpoint.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="depthHandler">Handler to be used when a message is received.</param>
        public void ListenDepthEndpoint(string symbol, ApiClientAbstract.MessageHandler<DepthMessage> depthHandler)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var param = symbol + "@depth";
            _apiClient.ConnectToWebSocket(param, depthHandler, true);
        }

        /// <summary>
        /// Listen to the Kline endpoint.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="interval">Time interval to retreive.</param>
        /// <param name="klineHandler">Handler to be used when a message is received.</param>
        public void ListenKlineEndpoint(string symbol, TimeInterval interval, ApiClientAbstract.MessageHandler<KlineMessage> klineHandler)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var param = symbol + $"@kline_{interval.GetDescription()}";
            _apiClient.ConnectToWebSocket(param, klineHandler);
        }

        /// <summary>
        /// Listen to the Trades endpoint.
        /// </summary>
        /// <param name="symbol">Ticker symbol.</param>
        /// <param name="tradeHandler">Handler to be used when a message is received.</param>
        public void ListenTradeEndpoint(string symbol, ApiClientAbstract.MessageHandler<AggregateTradeMessage> tradeHandler)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("symbol cannot be empty. ", "symbol");
            }

            var param = symbol + "@aggTrade";
            _apiClient.ConnectToWebSocket(param, tradeHandler);
        }

        /// <summary>
        /// Listen to the User Data endpoint.
        /// </summary>
        /// <param name="accountInfoHandler">Handler to be used when a account message is received.</param>
        /// <param name="tradesHandler">Handler to be used when a trade message is received.</param>
        /// <param name="ordersHandler">Handler to be used when a order message is received.</param>
        /// <returns></returns>
        public string ListenUserDataEndpoint(ApiClientAbstract.MessageHandler<AccountUpdatedMessage> accountInfoHandler, ApiClientAbstract.MessageHandler<OrderOrTradeUpdatedMessage> tradesHandler, ApiClientAbstract.MessageHandler<OrderOrTradeUpdatedMessage> ordersHandler)
        {
            var listenKey = StartUserStream().Result.ListenKey;

            _apiClient.ConnectToUserDataWebSocket(listenKey, accountInfoHandler, tradesHandler, ordersHandler);

            return listenKey;
        }
        #endregion
    } // end of class BinanceClient

} // end of namespace

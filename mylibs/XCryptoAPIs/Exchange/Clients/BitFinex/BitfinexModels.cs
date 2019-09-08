using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange.Clients.BitFinex
{
    public class ActiveOrdersRequest : GenericRequest
    {
        public ActiveOrdersRequest(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/orders";
        }
    }

    public class ActiveOrdersResponse
    {
        public List<OrderStatusResponse> orders;

        public static ActiveOrdersResponse FromJSON(string response)
        {
            List<OrderStatusResponse> orders = JsonConvert.DeserializeObject<List<OrderStatusResponse>>(response);
            return new ActiveOrdersResponse(orders);
        }
        private ActiveOrdersResponse(List<OrderStatusResponse> orders)
        {
            this.orders = orders;
        }
    }

    public class ActivePositionsRequest : GenericRequest
    {
        public ActivePositionsRequest(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/positions";
        }
    }

    public class ActivePositionItem
    {
        public string id;
        public string symbol;
        public string status;
        public string _base;//base reserved word
        public string amount;
        public string timestamp;
        public string swap;
        public string pl;
    }
    public class ActivePositionsResponse
    {
        public List<ActivePositionItem> positions;
        public static ActivePositionsResponse FromJSON(string response)
        {
            response = response.Replace("base", "_base");
            List<ActivePositionItem> items = JsonConvert.DeserializeObject<List<ActivePositionItem>>(response);
            return new ActivePositionsResponse(items);
        }
        private ActivePositionsResponse(List<ActivePositionItem> positions)
        {
            this.positions = positions;
        }
    }

    public class BalancesRequest : GenericRequest
    {
        public BalancesRequest(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/balances";
        }
    }

    public class BalanceResponseItem
    {
        public string type;
        public string currency;
        public string amount;
        public string available;
    }
    public class BalancesResponse
    {
        public Balance trading;
        public Balance deposit;
        public Balance exchange;
        public decimal totalUSD = 0;
        public decimal totalAvailableUSD = 0;
        public decimal totalBTC = 0;
        public decimal totalAvailableBTC = 0;

        public static BalancesResponse FromJSON(string response)
        {
            if (response == null) return null;
            List<BalanceResponseItem> items = JsonConvert.DeserializeObject<List<BalanceResponseItem>>(response);
            return new BalancesResponse(items);
        }
        private BalancesResponse(List<BalanceResponseItem> items)
        {
            trading = new Balance();
            deposit = new Balance();
            exchange = new Balance();

            Balance cur = null;
            decimal tmp;
            foreach (BalanceResponseItem item in items)
            {

                switch (item.type)
                {
                    case "trading":
                        cur = trading;
                        break;
                    case "deposit":
                        cur = deposit;
                        break;
                    case "exchange":
                        cur = exchange;
                        break;
                }
                switch (item.currency)
                {
                    case "usd":
                        tmp = decimal.Parse(item.available, CultureInfo.InvariantCulture);
                        cur.availableUSD = tmp;
                        totalAvailableUSD += tmp;
                        tmp = decimal.Parse(item.amount, CultureInfo.InvariantCulture);
                        cur.USD = tmp;
                        totalUSD += tmp;
                        break;
                    case "btc":
                        tmp = decimal.Parse(item.available, CultureInfo.InvariantCulture);
                        cur.availableBTC = tmp;
                        totalAvailableBTC += tmp;
                        tmp = decimal.Parse(item.amount, CultureInfo.InvariantCulture);
                        cur.BTC = tmp;
                        totalBTC += tmp;
                        break;

                }
            }
        }
    }

    public class Balance
    {
        public decimal USD = 0;
        public decimal BTC = 0;
        public decimal availableUSD = 0;
        public decimal availableBTC = 0;
    }

    public class BitfinexException : WebException
    {

        public BitfinexException(WebException ex, string bitfinexMessage) :
            base(bitfinexMessage, ex)
        {
        }
    }

    public class CancelAllOrdersRequest : GenericRequest
    {
        public CancelAllOrdersRequest(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/order/cancel/all";
        }
    }

    public class CancelAllOrdersResponse
    {
        public string message;
        public CancelAllOrdersResponse(string message)
        {
            this.message = message;
        }
    }

    public class CancelOrderRequest : GenericRequest
    {
        public int order_id;
        public CancelOrderRequest(string nonce, int order_id)
        {
            this.nonce = nonce;
            this.order_id = order_id;
            this.request = "/v1/order/cancel";
        }
    }

    public class CancelOrderResponse : OrderStatusResponse
    {
        public static CancelOrderResponse FromJSON(string response)
        {
            return JsonConvert.DeserializeObject<CancelOrderResponse>(response);
        }
    }

    public class GenericRequest
    {
        public string request;
        public string nonce;
        public ArrayList options = new ArrayList();
    }

    public enum OrderType
    {
        MarginMarket,
        MarginLimit,
        MarginStop,
        MarginTrailingStop
    }
    public enum OrderSide
    {
        Buy,
        Sell
    }
    public enum OrderExchange
    {
        Bitfinex,
        Bitstamp,
        All
    }
    public enum OrderSymbol
    {
        BTCUSD,
        LTCUSD,
        LTCBTC,
        ETHUSD,
        ETCUSD,
        ETCBTC,
        BFXUSD,
        BFXBTC,
        RRTUSD,
        RRTBTC,
        ZECUSD,
        ZECBTC
    }
    public class NewOrderRequest : GenericRequest
    {
        public string symbol;
        public string amount;
        public string price;
        public string exchange;
        public string side;
        public string type;
        //public bool is_hidden=false;
        public NewOrderRequest(string nonce, OrderSymbol symbol, decimal amount, decimal price, OrderExchange exchange, OrderSide side, OrderType type)
        {
            this.symbol = EnumHelper.EnumToStr(symbol);
            this.amount = amount.ToString(CultureInfo.InvariantCulture);
            this.price = price.ToString(CultureInfo.InvariantCulture);
            this.exchange = EnumHelper.EnumToStr(exchange);
            this.side = EnumHelper.EnumToStr(side);
            this.type = EnumHelper.EnumToStr(type);
            this.nonce = nonce;
            this.request = "/v1/order/new";
        }
    }
    public class EnumHelper
    {
        private static Dictionary<object, string> enumStr = null;
        private static Dictionary<object, string> Get()
        {
            if (enumStr == null)
            {
                enumStr = new Dictionary<object, string>();
                enumStr.Add(OrderSymbol.BTCUSD, "btcusd");
                enumStr.Add(OrderSymbol.LTCUSD, "ltcusd");
                enumStr.Add(OrderSymbol.LTCBTC, "ltcbtc");
                enumStr.Add(OrderSymbol.ETHUSD, "ethusd");
                enumStr.Add(OrderSymbol.ETCUSD, "etcusd");
                enumStr.Add(OrderSymbol.ETCBTC, "etcbtc");
                enumStr.Add(OrderSymbol.BFXUSD, "bfxusd");
                enumStr.Add(OrderSymbol.BFXBTC, "bfxbtc");
                enumStr.Add(OrderSymbol.RRTUSD, "rrtusd");
                enumStr.Add(OrderSymbol.RRTBTC, "btcusd");
                enumStr.Add(OrderSymbol.BTCUSD, "btcusd");
                enumStr.Add(OrderSymbol.BTCUSD, "btcusd");

                enumStr.Add(OrderExchange.All, "all");
                enumStr.Add(OrderExchange.Bitfinex, "bitfinex");
                enumStr.Add(OrderExchange.Bitstamp, "bitstamp");

                enumStr.Add(OrderSide.Buy, "buy");
                enumStr.Add(OrderSide.Sell, "sell");

                enumStr.Add(OrderType.MarginLimit, "limit");
                enumStr.Add(OrderType.MarginMarket, "market");
                enumStr.Add(OrderType.MarginStop, "stop");
                enumStr.Add(OrderType.MarginTrailingStop, "trailing-stop");
            }
            return enumStr;
        }

        public static string EnumToStr(object enumItem)
        {
            return Get()[enumItem];
        }
    }

    public class NewOrderResponse : OrderStatusResponse
    {
        public string order_id;

        public static NewOrderResponse FromJSON(string response)
        {
            NewOrderResponse resp = JsonConvert.DeserializeObject<NewOrderResponse>(response);
            return resp;
        }
    }

    public class OrderStatusRequest : GenericRequest
    {
        public int order_id;
        public OrderStatusRequest(string nonce, int order_id)
        {
            this.nonce = nonce;
            this.order_id = order_id;
            this.request = "/v1/order/status";
        }
    }

    public class OrderStatusResponse
    {
        public string id;
        public string symbol;
        public string exchange;
        public string price;
        public string avg_execution_price;
        public string type;
        public string timestamp;
        public string is_live;
        public string is_cancelled;
        public string was_forced;
        public string executed_amount;
        public string remaining_amount;
        public string original_amount;

        public static OrderStatusResponse FromJSON(string response)
        {
            return JsonConvert.DeserializeObject<OrderStatusResponse>(response);
        }
    }



} // end of namespace

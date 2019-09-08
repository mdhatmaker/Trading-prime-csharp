using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeSharp;
using CryptoTools;
using CryptoTools.Models;

namespace CryptoApis
{
    public static class ApiHelper
    {
      
        // where symbol like "ETH-USD"
        // where side is OrderSide.Buy/OrderSide.Sell
        // where price like 390.19M
        // where amount like 0.1M
        // where postOnly is true/false (true means DO NOT allow "taker" -- "maker" only)
        public static ExchangeOrderResult GdaxOrder(ExchangeGdaxAPI api, string symbol, OrderSide side, decimal price, decimal amount, bool postOnly = true)
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = symbol;
            order.Price = price;
            order.Amount = amount;
            order.IsBuy = side == OrderSide.Buy;
            //order.ShouldRoundAmount = true;
            //order.RoundAmount();
            //var parameters = order.ExtraParameters;
            order.ExtraParameters["post_only"] = postOnly;

            //var api = new ExchangeGdaxAPI();
            //api.LoadAPIKeysUnsecure(m_creds["GDAX"].Key, m_creds["GDAX"].Secret, m_creds["GDAX"].Passphrase);
            //var api = m_apiMap["GDAX"];
            return api.PlaceOrder(order);
        }

        // Join the inside bid/offer
        public static ExchangeOrderResult GdaxJoinInside(ExchangeGdaxAPI api, string symbol, OrderSide side, decimal amount)
        {
            var t = api.GetTicker(symbol);
            var order = new ExchangeOrderRequest();
            var price = (side == OrderSide.Buy) ? t.Bid : t.Ask;
            return GdaxOrder(api, symbol, side, price, amount, postOnly: true);
        }

        // TODO: change iceberg orders to require TOTAL amount (and split that amount based on repeatCount)

        // ICEBERG: Join the inside bid/offer
        public static void GdaxIcebergJoinInside(ExchangeGdaxAPI api, string symbol, OrderSide side, decimal amount, int repeatCount)
        {
			decimal amountPer = amount / repeatCount;
            int count = 0;
            while (count < repeatCount)
            {
                // Submit new order
                var or = GdaxJoinInside(api, symbol, side, amountPer);
                Console.WriteLine(or.ToStr());
                var eor = api.GetOrderDetails(or.OrderId);

                // Wait for fill
                while (eor.AmountFilled < eor.Amount)
                {
                    Thread.Sleep(1000);
                    eor = api.GetOrderDetails(or.OrderId);
                }
                count++;
            }
        }

        // ICEBERG: Join the inside bid/offer
        public static void GdaxIcebergJoinInside(ExchangeGdaxAPI api, string symbol, OrderSide side, decimal amount, int repeatCount, TimeSpan delayBetweenTrades)
        {
			decimal amountPer = amount / repeatCount;
            int count = 0;
            while (count < repeatCount)
            {
                // Submit new order
                var or = GdaxJoinInside(api, symbol, side, amountPer);
                Console.WriteLine("{0}/{1}: {2}", count, repeatCount, or.ToStr());
                var eor = api.GetOrderDetails(or.OrderId);
                count++;
                Thread.Sleep((int) delayBetweenTrades.TotalMilliseconds);
            }
        }



		// where symbol like "ETHUSDT"
        // where side is OrderSide.Buy/OrderSide.Sell
        // where price like 390.19M
        // where amount like 0.1M
        public static ExchangeOrderResult PlaceOrder(IExchangeAPI api, string symbol, OrderSide side, decimal price, decimal amount)
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = symbol;
            order.Price = price;
            order.Amount = amount;
            order.IsBuy = side == OrderSide.Buy;
            order.ShouldRoundAmount = true;
            //order.RoundAmount();
            //var parameters = order.ExtraParameters;
            //order.ExtraParameters["post_only"] = postOnly;

            return api.PlaceOrder(order);
        }

        // where symbol like "ETHUSDT"
        // where side is OrderSide.Buy/OrderSide.Sell
        // where price like 390.19M
        // where amount like 0.1M
        public static async Task<ExchangeOrderResult> PlaceOrderAsync(IExchangeAPI api, string symbol, OrderSide side, decimal price, decimal amount)
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = symbol;
            order.Price = price;
            order.Amount = amount;
            order.IsBuy = side == OrderSide.Buy;
            order.ShouldRoundAmount = true;
            //order.RoundAmount();
            //var parameters = order.ExtraParameters;
            //order.ExtraParameters["post_only"] = postOnly;

            return await api.PlaceOrderAsync(order);
        }

        /*public static void CancelOrder(IExchangeAPI api, string orderId)
		{
			api.CancelOrder(orderId);
		}*/

        // Given a Global symbol ("BTC-ADA", "BTC-XMR", "ETH-DOGE", ...)
        // Return the Base currency ("BTC", "BTC", "ETH", ...)
        public static string GetQuoteCurrency(string globalSymbol)
        {
            var split = globalSymbol.Split('-');
            return split[0];
        }

        // Given a Global symbol ("BTC-ADA", "BTC-XMR", "ETH-DOGE", ...)
        // Return the Quote currency ("ADA", "XMR", "DOGE", ...)
        public static string GetBaseCurrency(string globalSymbol)
        {
            var split = globalSymbol.Split('-');
            return split[1];
        }

        #region ---------- EXTENSION METHODS --------------------------------------------------------------------------
        // Extension method to calculate ExchangeTicker mid price (bid/ask average price--not weighted)
        public static decimal MidPrice(this ExchangeTicker t)
        {
            return (t.Bid + t.Ask) / 2.0M;
        }

        // Extension method to calculate ExchangeTicker bid/ask spread
        public static decimal BidAskSpread(this ExchangeTicker t)
        {
            return (t.Ask - t.Bid);
        }

        // Get currency value in BTC for the given amount
        // where currency like "ETH", "NEO", "TRX", ...
        public static decimal BtcValue(this IExchangeAPI api, string currency, decimal amount)
        {
            var globalSymbol = currency + "-BTC";
            var symbol = api.GlobalSymbolToExchangeSymbol(globalSymbol);
            var t = api.GetTicker(symbol);
            var btcAmount = t.MidPrice() * amount;
            Console.WriteLine("{0,-8} {1,-6} {2,13:0.00000000}        btc:{3,13:0.00000000}", api.Name, currency, amount, btcAmount);
            return btcAmount;
        }
        
        // Helper method for BUY orders
        public static ExchangeOrderResult Buy(this IExchangeAPI api, string symbol, decimal price, decimal amount)
        {
            return ApiHelper.PlaceOrder(api, symbol, OrderSide.Buy, price, amount);
        }

        // Helper method for SELL orders
        public static ExchangeOrderResult Sell(this IExchangeAPI api, string symbol, decimal price, decimal amount)
        {
            return ApiHelper.PlaceOrder(api, symbol, OrderSide.Sell, price, amount);
        }

        // Helper method for BUY orders (ASYNC)
        public static async Task<ExchangeOrderResult> BuyAsync(this IExchangeAPI api, string symbol, decimal price, decimal amount)
        {
            return await ApiHelper.PlaceOrderAsync(api, symbol, OrderSide.Buy, price, amount);
        }

        // Helper method for SELL orders (ASYNC)
        public static async Task<ExchangeOrderResult> SellAsync(this IExchangeAPI api, string symbol, decimal price, decimal amount)
        {
            return await ApiHelper.PlaceOrderAsync(api, symbol, OrderSide.Sell, price, amount);
        }

        // Formatted string representation of an order
        public static string ToStr(this ExchangeOrderResult or)
        {
            string buySell = or.IsBuy ? "BUY " : "SELL";
            var eor = or.Result;
			return string.Format("{0} [oid:{1,9}] {2,-15} {3,-9} {4,4} {5:0.00000000}  {6,5}    filled:{7,5}  avg_price:{8:0.00000000}  fees:{9} ({10}) '{11}'", or.OrderDate, or.OrderId, or.Result.ToString(), or.Symbol, buySell, or.Price, or.Amount, or.AmountFilled, or.AveragePrice, or.Fees, or.FeesCurrency, or.Message);
        }

        // Abbreviated string representation of an order (good for sending via Prowl notifications)
		public static string ToMsgStr(this ExchangeOrderResult or)
        {
            string buySell = or.IsBuy ? "BUY " : "SELL";
            var eor = or.Result;
			var shortTimeString = or.OrderDate.ToLocalTime().ToString("MMM-dd HH:mm:ss");
			return string.Format("{0} [oid:{1}] {2} {3} {4:0.00000000}  qty:{5}   {6}", shortTimeString, or.OrderId, or.Symbol, buySell, or.Price, or.Amount, or.Result.ToString());
        }

        public static OrderSide Side(this ExchangeOrderResult or)
        {
            return or.IsBuy ? OrderSide.Buy : OrderSide.Sell;
        }

        public static void DisplaySymbols(this IExchangeAPI api)
        {
            Console.WriteLine("\n{0}", new string('-', 60));
            int count = 0;
            var symbols = api.GetSymbols();
            foreach (var s in symbols)
            {
                Console.WriteLine("{0,4}  {1}  {2}", ++count, api.Name, s);
            }
        }

        public static void Print<T>(this IEnumerable<T> collection, string title = null)
        {
            if (title != null)
                Console.WriteLine(title);
            collection.ToList().ForEach(x => Console.WriteLine(x));
        }

        public static IEnumerable<XCandle> ToXCandles(this IEnumerable<MarketCandle> candles)
        {
            return candles.Select(x => x.ToXCandle());
        }

        public static XCandle ToXCandle(this MarketCandle candle)
        {
            return new XCandle(candle.ExchangeName, candle.Name, candle.Timestamp, candle.PeriodSeconds, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, (decimal)candle.BaseVolume, (decimal)candle.ConvertedVolume, candle.WeightedAverage);
        }
        #endregion ----------------------------------------------------------------------------------------------------

        public static XOrder CreateXOrder(string exchange, ExchangeOrderResult eor, string strategyId)
        {
            var xo = new XOrder(exchange, strategyId);
            //m_api = api;

            //xo.API = api;
            xo.Amount = eor.Amount;
            xo.AmountFilled = eor.AmountFilled;
            xo.AveragePrice = eor.AveragePrice;
            xo.Fees = eor.Fees;
            xo.FeesCurrency = eor.FeesCurrency;
            xo.IsBuy = eor.IsBuy;
            xo.Message = eor.Message;
            xo.OrderDate = eor.OrderDate;
            xo.OrderId = eor.OrderId;
            xo.Price = eor.Price;
            //xo.Result = MapToResult(bo.Status);
            xo.Symbol = eor.Symbol;

            return xo;
        }


    } // end of class ApiHelper


} // end of namespace

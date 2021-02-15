using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis.Models;
using ES = ExchangeSharp;
using Binance.Net;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.SpotData;
using CryptoTools;
using CryptoTools.Net;
using static CryptoTools.Global;
using CryptoTools.Models;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot.MarketData;
using Binance.Net.Objects.Spot;

namespace CryptoApis.RestApi
{
    // https://www.nuget.org/packages/Binance.Net/
    // https://github.com/JKorf/Binance.Net

    public class BinanceRestApi : ICryptoRestApi
    {
        private CryptoTools.SymbolManager m_symbolManager;

        private BinanceClient m_client;

        public BinanceRestApi(string apiKey, string apiSecret)
        {
            var options = new BinanceClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);            
            var limit = new BinanceRateLimit();
            limit.Interval = RateLimitInterval.Minute;
            limit.Limit = 1200;
            //options.RateLimiters.Add(limit);
            m_client = new BinanceClient(options);  
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public static void Test()
        {
            // API test code goes here
        }

        public async Task<XTickerMap> GetTickers()
        {
            var res = await m_client.Spot.Market.GetAllBookPricesAsync();
            return new XTickerMap(res.Data);
        }

        public async Task<BinancePlacedOrder> Sell(string symbolId, decimal qty, decimal price)
        {
            string symbol = GetSymbol(symbolId);
            var side = Binance.Net.Enums.OrderSide.Sell;
            var type = OrderType.Limit;
            string oid = null;
            var tif = TimeInForce.GoodTillCancel;
            decimal quoteOrderQty = qty;
            var res = await m_client.Spot.Order.PlaceOrderAsync(symbol, side, type, qty, quoteOrderQty, oid, price, tif);
            if (res.Error != null) Console.WriteLine("Binance::Sell ERROR: {0} {1}", res.Error.Code, res.Error.Message);
            return res.Data;
        }

        public async Task<BinanceExchangeInfo> GetExchangeInfo()
        {
            var res = await m_client.Spot.System.GetExchangeInfoAsync();
            if (res.Error != null) Console.WriteLine("Binance::GetExchangeInfo ERROR: {0} {1}", res.Error.Code, res.Error.Message);
            return res.Data;
        }

        public XSymbol GetXSymbol(string symbolId)
        {
            if (m_symbolManager == null) m_symbolManager = new CryptoTools.SymbolManager();
            var symbol = m_symbolManager.GetXSymbol(Exchange, symbolId);
            if (symbol != null)
                return symbol;
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public IEnumerable<XOrder> GetOpenOrders(string symbol = null)
        {
            var result = new List<XOrder>();
            var res = m_client.Spot.Order.GetOpenOrders(symbol);
            // TODO: check for errors here
            foreach (var bo in res.Data)
            {
                var xo = CreateXOrder(bo); //new XOrder(null, bo, "None");
                result.Add(xo);
            }
            return result.AsEnumerable<XOrder>().OrderBy(o => o.OrderDate);
        }

        public IEnumerable<XOrder> GetAllOrders(string symbol)
        {
            var result = new List<XOrder>();
            var res = m_client.Spot.Order.GetAllOrders(symbol);
            // TODO: check for errors here
            foreach (var bo in res.Data)
            {
                var xo = CreateXOrder(bo);  // new XOrder(null, bo, "None");
                result.Add(xo);
            }
            return result.AsEnumerable<XOrder>().OrderBy(o => o.OrderDate);
        }

        public void GetMyTrades()
        {

        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "BINANCE"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var res = m_client.Spot.Market.GetAllPricesAsync();
            res.Wait();
            foreach (var p in res.Result.Data)
            {
                result.Add(p.Symbol);
            }
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            var xs = GetXSymbol(symbolId);
            if (xs != null)
                return xs.Symbol;
            else
                return null;
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            string symbol = GetSymbol(symbolId);
            var res = await m_client.Spot.Market.GetBookPriceAsync(symbol);
            return new XTicker(res.Data);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            var res = await m_client.General.GetAccountInfoAsync();
            return new XBalanceMap(res.Data);
        }
        #endregion ----------------------------------------------------------------------------------

        public XOrder CreateXOrder(BinanceOrder bo, string strategyId = "")
        {
            var xo = new XOrder("BINANCE", strategyId);

            //xo.API = api;
            xo.Amount = bo.Quantity;                //.OriginalQuantity;
            xo.AmountFilled = bo.QuantityFilled;    //.ExecutedQuantity;
            xo.AveragePrice = bo.Price;
            xo.Fees = 0;
            xo.FeesCurrency = "";
            xo.IsBuy = bo.Side == Binance.Net.Enums.OrderSide.Buy;
            xo.Message = "";
            xo.OrderDate = bo.CreateTime;           //.Time;
            xo.OrderId = bo.OrderId.ToString();
            xo.Price = bo.Price;
            xo.Result = MapToResult(bo.Status);
            xo.Symbol = bo.Symbol;

            return xo;
        }

        /*public enum ExchangeAPIOrderResult
        {
            Unknown,
            Filled,
            FilledPartially,
            Pending,
            Error,
            Canceled,
            PendingCancel
        }*/
        /*private ES.ExchangeAPIOrderResult MapToResult(Binance.Net.Objects.OrderStatus status)
        {
            var d = new Dictionary<OrderStatus, ES.ExchangeAPIOrderResult>() {
                { OrderStatus.New, ES.ExchangeAPIOrderResult.Pending },
                { OrderStatus.PartiallyFilled, ES.ExchangeAPIOrderResult.FilledPartially },
                { OrderStatus.Filled, ES.ExchangeAPIOrderResult.Filled },
                { OrderStatus.Canceled, ES.ExchangeAPIOrderResult.Canceled },
                { OrderStatus.PendingCancel, ES.ExchangeAPIOrderResult.PendingCancel },
                { OrderStatus.Rejected, ES.ExchangeAPIOrderResult.Error },
                { OrderStatus.Expired, ES.ExchangeAPIOrderResult.Canceled }    // Expired becomes Canceled
            };
            return d[status];
        }*/

        private OrderResult MapToResult(Binance.Net.Enums.OrderStatus status)
        {
            var d = new Dictionary<OrderStatus, OrderResult>() {
                { OrderStatus.New, OrderResult.Pending },
                { OrderStatus.PartiallyFilled, OrderResult.FilledPartially },
                { OrderStatus.Filled, OrderResult.Filled },
                { OrderStatus.Canceled, OrderResult.Canceled },
                { OrderStatus.PendingCancel, OrderResult.PendingCancel },
                { OrderStatus.Rejected, OrderResult.Error },
                { OrderStatus.Expired, OrderResult.Canceled }    // Expired becomes Canceled
            };
            return d[status];
        }

    } // end of class BinanceRestApi
} // end of namespace


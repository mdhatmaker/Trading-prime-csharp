using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis.SharedModels;
using CryptoTools.Net;

namespace Aggregator
{
    public static class BinanceModels
    {
        public static BinanceExchangeInfo GetExchangeInfo()
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://api.binance.com/api/v1/exchangeInfo");
                var exchangeInfo = JsonConvert.DeserializeObject<BinanceExchangeInfo>(json);
                return exchangeInfo;
            }
        }
    } // end of class BinanceModels

    public class BinanceRateLimit
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int limit { get; set; }
    } // end of class BinanceRateLimit

    public class BinanceExchangeFilter
    {
    } // end of class BinanceExchangeFilter

    // Combine the fields for ALL types of filters into one object (filterType field will determine which 
    public class BinanceFilter
    {
        public string filterType { get; set; }      // "PRICE_FILTER", "LOT_SIZE", "MIN_NOTIONAL"
        public decimal minPrice { get; set; }       // PRICE_FILTER
        public decimal maxPrice { get; set; }       // PRICE_FILTER
        public decimal tickSize { get; set; }       // PRICE_FILTER
        public decimal minQty { get; set; }         // LOT_SIZE
        public decimal maxQty { get; set; }         // LOT_SIZE
        public decimal stepSize { get; set; }       // LOT_SIZE
        public decimal minNotional { get; set; }    // MIN_NOTIONAL
    } // end of class BinanceFilter

    public class BinanceSymbolInfo
    {
        public string symbol { get; set; }
        public string status { get; set; }
        public string baseAsset { get; set; }
        public int baseAssetPrecision { get; set; }
        public string quoteAsset { get; set; }
        public int quotePrecision { get; set; }
        public List<string> orderTypes { get; set; }
        public bool icebergAllowed { get; set; }
        public List<BinanceFilter> filters { get; set; }

        private Dictionary<string, BinanceFilter> m_filters;
        private Dictionary<string, BinanceFilter> Filters
        {
            get
            {
                if (m_filters == null)
                {
                    m_filters = new Dictionary<string, BinanceFilter>();
                    m_filters["PRICE_FILTER"] = GetFilter("PRICE_FILTER");
                    m_filters["LOT_SIZE"] = GetFilter("LOT_SIZE");
                    m_filters["MIN_NOTIONAL"] = GetFilter("MIN_NOTIONAL");
                }
                return m_filters;
            }
        }

        // TODO: These methods need to handle null values
        // Convenience methods to get at the fields of the various filters
        public decimal MinPrice { get { return Filters["PRICE_FILTER"].minPrice; } }
        public decimal MaxPrice { get { return Filters["PRICE_FILTER"].maxPrice; } }
        public decimal TickSize { get { return Filters["PRICE_FILTER"].tickSize; } }
        public decimal MinQty { get { return Filters["LOT_SIZE"].minQty; } }
        public decimal MaxQty { get { return Filters["LOT_SIZE"].maxQty; } }
        public decimal StepSize { get { return Filters["LOT_SIZE"].stepSize; } }
        public decimal MinNotional { get { return Filters["MIN_NOTIONAL"].minNotional; } }

        public BinanceFilter GetFilter(string filterType)
        {
            BinanceFilter result = null;
            for (int i = 0; i < filters.Count; ++i)
            {
                if (filters[i].filterType == filterType)
                {
                    result = filters[i];
                    break;
                }
            }
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}: minPrice={1} maxPrice={2} tickSize={3} minQty={4} maxQty={5} stepSize={6} minNotional={7}", symbol, MinPrice, MaxPrice, TickSize, MinQty, MaxQty, StepSize, MinNotional);
        }
    } // end of class BinanceSymbolInfo

    public class BinanceExchangeInfo
    {
        public string timezone { get; set; }
        public long serverTime { get; set; }
        public List<BinanceRateLimit> rateLimits { get; set; }
        public List<BinanceExchangeFilter> exchangeFilters { get; set; }
        public List<BinanceSymbolInfo> symbols { get; set; }
    }

    // {"symbol":"LTCBTC","orderId":38591510,"clientOrderId":"nYJTtWs7BMmMkTdLjdfPIE","transactTime":1523301882071,"price":"0.01000000","origQty":"1.00000000","executedQty":"0.00000000","status":"NEW","timeInForce":"GTC","type":"LIMIT","side":"BUY"}
    public class BinanceOrder : NullableObject
    {
        public string symbol { get; set; }
        public long orderId { get; set; }
        public string clientOrderId { get; set; }
        public long transactTime { get; set; }
        public decimal price { get; set; }
        public decimal origQty { get; set; }
        public decimal executedQty { get; set; }
        public string status { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public decimal stopPrice { get; set; }
        public decimal icebergQty { get; set; }
        public long time { get; set; }
        public bool isWorking { get; set; }

        public bool IsNull { get { return clientOrderId == null; } }
    }

    public class BinanceOrderList : List<BinanceOrder>, NullableObject
    {
        public bool IsNull { get { return false; } }    // TODO: implement IsNull property
    }

    public class BinanceStartUserDataStream : NullableObject
    {
        public string listenKey { get; set; }

        public bool IsNull { get { return listenKey == null; } }
    }

    public class BinanceAccountBalance : AccountBalance
    {
        public string asset { get; set; }
        public decimal free { get; set; }
        public decimal locked { get; set; }

        public override string Asset { get { return asset; } }
        public override decimal Free { get { return free; } }
        public override decimal Locked { get { return locked; } }
    }

    public class BinanceBalance
    {
        public string a { get; set; }       // Asset
        public decimal f { get; set; }      // Free amount
        public decimal l { get; set; }      // Locked amount
    }

    public class BinanceOutboundAccountInfo : NullableObject
    {
        public string e { get; set; }               // Event type
        public long E { get; set; }                 // Event time
        public int m { get; set; }                  // Maker commission rate (bips)
        public int t { get; set; }                  // Taker commission rate (bips)
        public int b { get; set; }                  // Buyer commission rate (bips)
        public int s { get; set; }                  // Seller commission rate (bips)
        public bool T { get; set; }                 // Can trade?
        public bool W { get; set; }                 // Can withdraw?
        public bool D { get; set; }                 // Can deposit?
        public long u { get; set; }                 // Time of last account update
        public List<BinanceBalance> B { get; set; } // Balances array

        // Convenience properties (reference the single-char properties above)
        public string eventType => e;
        public long eventTime => E;
        public int makerCommissionRateBips => m;
        public int takerCommissionRateBips => t;
        public int buyerCommissionRateBips => b;
        public int sellerCommissionRateBips => s;
        public bool canTrade => T;
        public bool canWithdraw => W;
        public bool canDeposit => D;
        public long updateTime => u;
        public List<BinanceBalance> balances => B;

        public bool IsNull { get { return e == null; } }

        public override string ToString() { return string.Format("e:{0} E:{1} m:{2} t:{3} b:{4} s:{5} T:{6} W:{7} D:{8} u:{9}", e, E, m, t, b, s, T, W, D, u); }
    }

    public class BinanceExecutionReport : NullableObject
    {
        public string e { get; set; }       // Event type
        public long E { get; set; }         // Event time
        public string s { get; set; }       // Symbol
        public string c { get; set; }       // Client order ID
        public string S { get; set; }       // Side
        public string o { get; set; }       // Order type
        public string f { get; set; }       // Time in force
        public decimal q { get; set; }      // Order quantity
        public decimal p { get; set; }      // Order price
        public decimal P { get; set; }      // Stop price
        public decimal F { get; set; }      // Iceberg quantity
        public int g { get; set; }          // (ignore)
        public string C { get; set; }       // Original client order ID; this is the ID of the order being cancelled
        public string x { get; set; }       // Current execution type ("NEW"/"CANCELED"/"REPLACED"/"REJECTED"/"TRADE"/"EXPIRED")
        public string X { get; set; }       // Current order status (same as execution types?)
        public string r { get; set; }       // Order reject reason; will be an error code (or "NONE")
        public long i { get; set; }         // Order ID
        public decimal l { get; set; }      // Last executed quantity
        public decimal z { get; set; }      // Cumulative filled quantity
        public decimal L { get; set; }      // Last executed price
        public decimal n { get; set; }      // Commission amount
        public string N { get; set; }       // Commission asset
        public long T { get; set; }         // Transaction time
        public long t { get; set; }         // Trade ID
        public int I { get; set; }          // (ignore)
        public bool w { get; set; }         // Is the order working?
        public bool m { get; set; }         // Is this trade the maker side?
        public bool M { get; set; }         // (ignore)

        // Convenience properties (reference the single-char properties above)
        public string eventType { get { return e; } }
        public long eventTime { get { return E; } }
        public string symbol { get { return s; } }
        public string clientOrderId { get { return c; } }
        public string side { get { return S; } }
        public string type { get { return o; } }
        public string timeInForce { get { return f; } }
        public decimal origQty { get { return q; } }
        public decimal price { get { return p; } }
        public decimal stopPrice { get { return P; } }
        public decimal icebergQty { get { return F; } }
        public string origClientOrderId { get { return C; } }
        public string executionType { get { return x; } }
        public string status { get { return X; } }
        public string rejectReason { get { return r; } }
        public long orderId { get { return i; } }
        public decimal lastExecutedQty { get { return l; } }
        public decimal executedQty { get { return z; } }
        public decimal lastExecutedPrice { get { return L; } }
        public decimal commissionAmount { get { return n; } }
        public string commissionAsset { get { return N; } }
        public long transactTime { get { return T; } }
        public long tradeId { get { return t; } }
        public bool isWorking { get { return w; } }
        public bool isMakerSide { get { return m; } }

        public bool IsNull { get { return e == null; } }

        public override string ToString() { return string.Format("e:{0} E:{1} s:{2} c:{3} S:{4} o:{5} f:{6} q:{7} p:{8} P:{9} F:{10} c:{11} x:{12} X:{13} r:{14} i:{15} l:{16} z:{17} L:{18} n:{19} N:{20} T:{21} t:{22} w:{23} m:{24}", e, E, s, c, S, o, f, q, p, P, F, c, x, X, r, i, l, z, L, n, N, T, t, w, m); }
    }

    public class BinanceAccountInfo : NullableObject
    {
        public int makerCommissionRateBips { get; set; }
        public int takerCommissionRateBips { get; set; }
        public int buyerCommissionRateBips { get; set; }
        public int sellerCommissionRateBips { get; set; }
        public bool canTrade { get; set; }
        public bool canWithdraw { get; set; }
        public bool canDeposit { get; set; }
        public long updateTime { get; set; }
        public List<BinanceAccountBalance> balances { get; set; }

        public bool IsNull { get { return false; } }    // TODO: implement IsNull property

        public BinanceAccountBalance GetBalance(string asset)
        {
            foreach (var bab in balances)
            {
                if (bab.asset == asset)
                {
                    return bab;
                }
            }
            return null;
        }

        public IDictionary<string, BinanceAccountBalance> GetNonZeroBalances()
        {
            var result = new SortedDictionary<string, BinanceAccountBalance>();
            foreach (var bab in balances)
            {
                if (bab.free != 0 || bab.locked != 0)
                {
                    result[bab.asset] = bab;
                }
            }
            return result;
        }
    } // end of class BinanceAccountInfo

    public class BinanceOrderBookTicker : NullableObject
    {
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal bidQty { get; set; }
        public decimal askPrice { get; set; }
        public decimal askQty { get; set; }

        public decimal MidPrice { get { return (bidPrice + askPrice) / 2.0M; } }
        public decimal BidAskSpread { get { return askPrice - bidPrice; } }

        public bool IsNull { get { return symbol == null; } }
    }

    public class BinanceOrderBookTickerList : List<BinanceOrderBookTicker>, NullableObject
    {
        public bool IsNull { get { return this.Count <= 0 || this[0].symbol == null; } }
    }

    public class BinanceEmpty : NullableObject
    {
        public bool IsNull { get { return false; } }
    }

    public class BinanceServerTime : NullableObject
    {
        public long serverTime { get; set; }

        public bool IsNull { get { return false; } }
    }

    public class BinanceError
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

} // end of namespace

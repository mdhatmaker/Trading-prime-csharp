using System;
using System.Collections.Generic;
using CryptoTools.Net;

namespace CryptoApis.Exchange
{
    public class CoinExchangeResponse : NullableObject
    {
        public int success { get; set; }                // "1"
        public string request { get; set; }             // "/api/v1/public/getmarkets"
        public string message { get; set; }             // ""

        public bool IsNull => request == null;
    }

    public class CoinExchangeMarket
    {
        public int MarketID { get; set; }               // "1"
        public string MarketAssetName { get; set; }     // "Megacoin"
        public string MarketAssetCode { get; set; }     // "MEC"
        public int MarketAssetID { get; set; }          // "3"
        public string MarketAssetType { get; set; }     // "currency"
        public string BaseCurrency { get; set; }        // "Bitcoin"
        public string BaseCurrencyCode { get; set; }    // "BTC"
        public int BaseCurrencyID { get; set; }         // "1"
        public bool Active { get; set; }                // true
    }

    public class CoinExchangeMarketList : CoinExchangeResponse
    {
        public List<CoinExchangeMarket> result { get; set; }
    }

    public class CoinExchangeTicker : NullableObject
    {
        public int MarketID { get; set; }               // "1"
        public decimal LastPrice { get; set; }          // "0.00902321"
        public decimal Change { get; set; }             // "2.01"
        public decimal HighPrice { get; set; }          // "0.00961681"
        public decimal LowPrice { get; set; }           // "0.00853751"
        public decimal Volume { get; set; }             // "3043.78746852"
        public decimal BTCVolume { get; set; }          // "3043.78746852"
        public int TradeCount { get; set; }             // "1332"
        public decimal BidPrice { get; set; }           // "0.00902321"
        public decimal AskPrice { get; set; }           // "0.00928729"
        public int BuyOrderCount { get; set; }          // "7796"
        public int SellOrderCount { get; set; }         // "7671"

        public bool IsNull => MarketID <= 0;
    }

    public class CoinExchangeTickerList : CoinExchangeResponse
    {
        public List<CoinExchangeTicker> result { get; set; }
    }

    public class CoinExchangeBookEntry
    {
        public string Type { get; set; }                // "sell"
        public decimal Price { get; set; }              // "0.00928729"
        public DateTime OrderTime { get; set; }         // "2016-02-12 03:43:53"
        public decimal Quantity { get; set; }           // "37.04860800"
    }

    public class CoinExchangeOrderBook : CoinExchangeResponse
    {
        public List<CoinExchangeBookEntry> SellOrders { get; set; }
        public List<CoinExchangeBookEntry> BuyOrders { get; set; }
    }

    public class CoinExchangeCurrency
    {
        public int CurrencyID { get; set; }             // "1"
        public string Name { get; set; }                // "Bitcoin"
        public string TickerCode { get; set; }          // "BTC"
        public string WalletStatus { get; set; }        // "online"
        public string Type { get; set; }                // "currency"
    }

    public class CoinExchangeCurrencyResult : CoinExchangeResponse
    {
        public CoinExchangeCurrency result { get; set; }
    }

    public class CoinExchangeCurrencyList : CoinExchangeResponse
    {
        public List<CoinExchangeCurrency> result { get; set; }
    }

} // end of namespace

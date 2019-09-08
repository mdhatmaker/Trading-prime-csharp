using System;
using System.Collections.Generic;
using CryptoApis.SharedModels;
using CryptoTools.Net;

namespace Aggregator
{
    
    public class BittrexMarket
    {
        public string MarketCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public decimal MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public bool IsActive { get; set; }
        public string Created { get; set; }     // "2014-02-13T00:00:00"
        public string Notice { get; set; }      // null
        public bool? IsSponsored { get; set; }
        public string LogoUrl { get; set; }     // "https://bittrexblobstorage.blob.core.windows.net/public/6defbc41-582d-47a6-bb2e-d0fa88663524.png"

        public bool IsNull { get { return MarketCurrency == null; } }

        public override string ToString() { return string.Format("{0}    {1} {2}   {3} {4}    min_trade_size:{5}", MarketName, MarketCurrency, MarketCurrencyLong, BaseCurrency, BaseCurrencyLong, MinTradeSize); }
    }

    public class BittrexObject : NullableObject
    {
        public bool success { get; set; }
        public string message { get; set; }

        public bool IsNull { get { return message == null; } }
    }

    public class BittrexGetMarketsResponse : BittrexObject
    {
        public List<BittrexMarket> result { get; set; }
    }

    public class BittrexOrder
    {
        public string uuid { get; set; }
    }

    public class BittexOrderResponse : BittrexObject
    {
        public BittrexOrder result { get; set; }
    }

    public class BittrexMarketSummaryResponse : BittrexObject
    {
        //"result":[{"MarketName":"BTC-LTC","High":0.01797499,"Low":0.01697001,"Volume":34808.80065622,"Last":0.01762501,
        //"BaseVolume":608.35342619,"TimeStamp":"2018-04-20T07:36:37.3","Bid":0.01762501,"Ask":0.01767183,
        //"OpenBuyOrders":2267,"OpenSellOrders":3619,"PrevDay":0.01717437,"Created":"2014-02-13T00:00:00"}]}
        public List<BittrexMarketSummary> result { get; set; }    
    }

    public class BittrexMarketSummary
    {
        public string MarketName { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal Last { get; set; }
        public decimal BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal OpenBuyOrders { get; set; }
        public decimal OpenSellOrders { get; set; }
        public decimal PrevDay { get; set; }
        public string Created { get; set; }

        public override string ToString() { return string.Format("{0}  b:{1} a:{2} last:{3}   h:{4} l:{5} v:{6} vbase:{7}    {8}", MarketName, Bid, Ask, Last, High, Low, Volume, BaseVolume, TimeStamp); }
    }

    public class BittrexAccountBalanceResponse : BittrexObject
    {
        public List<BittrexAccountBalance> result { get; set; }
    }

    public class BittrexAccountBalance
    {
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal Available { get; set; }
        public decimal Pending { get; set; }
        public string CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public string Uuid { get; set; }

        public override string ToString() { return string.Format("{0}  balance:{1} available:{2} pending:{3}    crypto_address:{4}    requested:{5}    uuid:{6}", Currency, Balance, Available, Pending, CryptoAddress, Requested, Uuid); }
    }

    public class BittrexOrderBookEntry
    {
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }

    public class BittrexOrderBook
    {
        public List<BittrexOrderBookEntry> buy { get; set; }
        public List<BittrexOrderBookEntry> sell { get; set; }
    }

    public class BittrexOrderBookResponse : BittrexObject
    {
        public BittrexOrderBook result { get; set; }   
    }

    public class BittrexBidAskLast
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
    }

    public class BittrexTicker : BittrexObject
    {
        public BittrexBidAskLast result { get; set; }
    }
} // end of namespace

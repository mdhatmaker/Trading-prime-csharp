using System;
using System.Collections.Generic;
using CryptoTools.Net;

namespace CryptoApis.Exchange
{

    public class BiboxError
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

    public class BiboxResultEntry
    {
        public string cmd { get; set; }
        public string result { get; set; }
    }

    public class BiboxResultPair
    {
        public int id { get; set; }
        public string pair { get; set; }
    }

    public class BiboxResultKline
    {
        public long time { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal vol { get; set; }
    }

    public class BiboxDepthEntry
    {
        public decimal price { get; set; }
        public decimal volume { get; set; }
    }

    public class BiboxResultDepth
    {
        public string pair { get; set; }
        public long update_time { get; set; }
        public List<BiboxDepthEntry> bids { get; set; }
        public List<BiboxDepthEntry> asks { get; set; }
    }

    public class BiboxResultTrade
    {
        public string pair { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public long time { get; set; }
        public int side { get; set; }               //transaction side，1-bid，2-ask
    }

    public class BiboxResultTicker
    {
        public decimal last { get; set; }
        public decimal last_usd { get; set; }
        public decimal last_cny { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal buy { get; set; }
        public decimal buy_amount { get; set; }
        public decimal sell { get; set; }
        public decimal sell_amount { get; set; }
        public decimal vol { get; set; }
        public string percent { get; set; }         // "-1.82%"
        public long timestamp { get; set; }
    }

    public class BiboxResultMarket
    {
        public int id { get; set; }
        public string coin_symbol { get; set; }
        public string currency_symbol { get; set; }
        public decimal last { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal change { get; set; }         // "+0.00000715"
        public string percent { get; set; }         // "+35.95%"
        public decimal vol24H { get; set; }         // "641954"
        public decimal amount { get; set; }
        public decimal last_cny { get; set; }
        public decimal high_cny { get; set; }
        public decimal low_cny { get; set; }
        public decimal last_usd { get; set; }
        public decimal high_usd { get; set; }
        public decimal low_usd { get; set; }
    }

    public class BiboxResult<T> : NullableObject
    {
        public BiboxError error { get; set; }
        public T result { get; set; }
        public string cmd { get; set; }

        public bool IsNull => result == null;
    }

} // end of namespace

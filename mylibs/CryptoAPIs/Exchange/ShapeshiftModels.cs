using System;
using System.Collections.Generic;
using CryptoTools.Net;

namespace CryptoApis.Exchange
{
    public class ShapeshiftRate : NullableObject
    {
        public string pair { get; set; }    // "btc_ltc"
        public decimal rate { get; set; }   // "70.1234"

        public bool IsNull => pair == null;
    }

    public class ShapeshiftDepositLimit : NullableObject
    {
        public string pair { get; set; }    // "btc_ltc"
        public decimal limit { get; set; }  // "1.2345"

        public bool IsNull => pair == null;
    }

    public class ShapeshiftMarketInfo : NullableObject
    {
        public string pair { get; set; }        // "btc_ltc"
        public decimal rate { get; set; }       // 130.12345678
        public decimal limit { get; set; }      // 1.2345
        public decimal min { get; set; }        // 0.02621232
        public decimal minerFee { get; set; }   // 0.0001

        public bool IsNull => pair == null;
    }

    public class ShapeshiftTransaction : NullableObject
    {
        public string curIn { get; set; }       // "btc"
        public string curOut { get; set; }      // "ltc"
        public decimal amount { get; set; }     // 1.2345
        public long timestamp { get; set; }     // 

        public bool IsNull => curIn == null;
    }

    // Note: this can still get the normal style error returned. For example if
    // request is made without an address.
    public class ShapeshiftDepositStatus : NullableObject
    {
        public string status { get; set; }          // "no_deposits", "received", "complete", "failed"
        public string address { get; set; }         // address
        public string withdraw { get; set; }        // withdrawal addresss
        public decimal incomingCoin { get; set; }   // amount deposited
        public string incomingType { get; set; }    // coin type of deposit
        public decimal outgoingCoin { get; set; }   // amount sent to withdrawal address
        public string outgoingType { get; set; }    // coin type of withdrawal
        public string transaction { get; set; }     // tansaction id of coin sent to withdrawal address
        public string error { get; set; }           // text describing failure 

        public bool IsNull => status == null;
    }

    public class ShapeshiftTimeRemaining : NullableObject
    {
        public string status { get; set; }          // "pending", "expired"
        public int seconds_remaining { get; set; }  // 600

        public bool IsNull => status == null;
    }

    public class ShapeshiftCoinMap : Dictionary<string, ShapeshiftCoin>, NullableObject
    {
        public bool IsNull => false;
    }

    public class ShapeshiftCoin : NullableObject
    {
        public string name { get; set; }            // Currency formal name
        public string symbol { get; set; }         // 
        public string image { get; set; }           // "https://shapeshift.io/images/coins/coinName.png"
        public string status { get; set; }          // "available", "unavailable"

        public bool IsNull => symbol == null;
    }



} // end of namespace

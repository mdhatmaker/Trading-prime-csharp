using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static CryptoTools.Global;

namespace CryptoApis.Exchange.HitBtc
{
    public static class HitBtcModels
    {
    }

    // A List of HitBtcSymbol objects (that implements NullableObject)
    public class HitBtcSymbolList : List<HitBtcSymbol>, CryptoTools.Net.NullableObject
    {
        public bool IsNull => (this.Count == 0);
    }

    public class HitBtcSymbol : CryptoTools.Net.NullableObject
    {        
        public string id { get; set; }                      // "ETHBTC"
        public string baseCurrency { get; set; }            // "ETH"
        public string quoteCurrency { get; set; }           // "BTC"
        public decimal quantityIncrement { get; set; }      // "0.001"
        public decimal tickSize { get; set; }               // "0.000001"
        public decimal takeLiquidityRate { get; set; }      // "0.001"
        public decimal provideLiquidityRate { get; set; }   // "-0.0001"
        public string feeCurrency { get; set; }             // "BTC"

        public bool IsNull => (id == null);

        public override string ToString()
        {
            //string updateTime = FromTimestampSeconds((int)last_updated).ToString("yyyy-MM-dd HH:mm:ss");
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", id, baseCurrency, quoteCurrency, quantityIncrement, tickSize, takeLiquidityRate, provideLiquidityRate, feeCurrency);
        }
    } // end of class HitBtcSymbol

    // A List of HitBtcTicker objects (that implements NullableObject)
    public class HitBtcTickerList : List<HitBtcTicker>, CryptoTools.Net.NullableObject
    {
        public bool IsNull => (this.Count == 0);
    }

    public class HitBtcTicker : CryptoTools.Net.NullableObject
    {
        public decimal ask { get; set; }
        public decimal bid { get; set; }
        public decimal last { get; set; }
        public decimal open { get; set; }
        public decimal low { get; set; }
        public decimal high { get; set; }
        public decimal volume { get; set; }
        public decimal volumeQuote { get; set; }
        public string timestamp { get; set; }
        public string symbol { get; set; }              // "BCNBTC"

        public bool IsNull => false;

        public static string CsvHeaders => "DateTime,Symbol,Bid,Ask,Last,Open,High,Low,Volume,VolumeQuote";

        public string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", timestamp, symbol, bid, ask, last, open, high, low, volume, volumeQuote);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} b:{2} a:{3} o:{4} h:{5} l:{6} c:{7} vol:{8} qvol:{9}", timestamp, symbol, bid, ask, open, high, low, last, volume, volumeQuote);
        }
    } // end of class HitBtcTicker

    // A List of HitBtcCandle objects (that implements NullableObject)
    public class HitBtcCandleList : List<HitBtcCandle>, CryptoTools.Net.NullableObject
    {
        public bool IsNull => (this.Count == 0);
    }

    public class HitBtcCandle : CryptoTools.Net.NullableObject
    {
        public string timestamp { get; set; }       // "2018-04-30T12:42:00.000Z"
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal min { get; set; }
        public decimal max { get; set; }
        public decimal volume { get; set; }
        public decimal volumeQuote { get; set; }

        public bool IsNull => false;

		public override string ToString()
		{
            return string.Format("{0} o:{1} h:{2} l:{3} c:{4} vol:{5} qvol:{6}", timestamp, open, max, min, close, volume, volumeQuote);
		}
	} // end of class HitBtcCandle

} // end of namespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jayrock.Json;

namespace KrakenClient
{
    /*
     Result:
      h = ["3345.00000","3360.00000"]
      l = ["3250.00000","3166.10000"]
      o = 3310.00000
      p = ["3297.08632","3287.01729"]
      b = ["3342.40000","3","3.000"]
      c = ["3345.00000","0.02250000"]
      t = [6230,21351]
      a = ["3345.00000","28","28.000"]
      v = ["2214.99841208","8128.44084017"]*/

    public class TickerInfo
    {
        public decimal HighToday { get; set; }
        public decimal High24Hours { get; set; }
        public decimal LowToday { get; set; }
        public decimal Low24Hours { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal VwapToday { get; set; }
        public decimal Vwap24Hours { get; set; }
        public decimal BidPrice { get; set; }
        public int BidWholeLotVolume { get; set; }
        public decimal BidLotVolume { get; set; }
        public decimal AskPrice { get; set; }
        public int AskWholeLotVolume { get; set; }
        public decimal AskLotVolume { get; set; }
        public decimal LastTradePrice { get; set; }
        public decimal LastTradeVolume { get; set; }
        public int TradeCountToday { get; set; }
        public int TradeCount24Hours { get; set; }
        public decimal VolumeToday { get; set; }
        public decimal Volume24Hours { get; set; }

        public static Dictionary<string, TickerInfo> Create(JsonObject json)
        {
            var dict = new Dictionary<string, TickerInfo>();

            IList error = json["error"] as IList;
            JsonObject result = json["result"] as JsonObject; // as IDictionary;

            foreach (string name in result.Names)
            {
                var ti = new TickerInfo(result[name] as JsonObject);
                dict[name.ToString()] = ti;
            }

            return dict;
        }

        decimal dec(object obj)
        {
            return decimal.Parse(obj.ToString());
        }

        List<decimal> declist(object obj)
        {
            var result = new List<decimal>();
            IList li = obj as IList;
            foreach (var item in li)
            {
                result.Add(dec(item));
            }
            return result;
        }

        List<int> intlist(object obj)
        {
            var result = new List<int>();
            IList li = obj as IList;
            foreach (var item in li)
            {
                result.Add(int.Parse(item.ToString()));
            }
            return result;
        }

        public TickerInfo(JsonObject json)
        {
            var h = json["h"] as IList;
            HighToday = dec(h[0]);
            High24Hours = dec(h[1]);

            var l = declist(json["l"]);
            LowToday = l[0];
            Low24Hours = l[1];

            OpenPrice = dec(json["o"]);

            var p = declist(json["p"]);
            VwapToday = p[0];
            Vwap24Hours = p[1];

            var b = json["b"] as IList;
            BidPrice = dec(b[0]);
            BidWholeLotVolume = int.Parse(b[1].ToString());
            BidLotVolume = dec(b[2]);

            var a = json["a"] as IList;
            AskPrice = dec(a[0]);
            AskWholeLotVolume = int.Parse(a[1].ToString());
            AskLotVolume = dec(a[2]);

            var c = declist(json["c"]);
            LastTradePrice = c[0];
            LastTradeVolume = c[1];

            var t = intlist(json["t"]);
            TradeCountToday = t[0];
            TradeCount24Hours = t[1];

            var v = declist(json["v"]);
            VolumeToday = v[0];
            Volume24Hours = v[1];
        }

    } // end of CLASS

} // end of NAMESPACE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tools;
using static Tools.G;
using static Tools.GDate;

namespace CryptoAPIs.ExchangeX
{
    // https://www.coindesk.com/api/

    public class Coindesk
    {
        public static void Test()
        {
            var bpi = Coindesk.GetBPI();
            dout(Str(bpi));

            var bpiYesterday = Coindesk.GetBPIHistoricalYesterday();
            dout(Str(bpiYesterday));

            DateTime dt1 = new DateTime(2017, 9, 1);
            DateTime dt2 = new DateTime(2017, 10, 15);
            var bpiHistorical = Coindesk.GetBPIHistorical(dt1, dt2);
            dout(Str(bpiHistorical));
        }

        public static CoindeskBPI GetBPI()
        {
            string json = GetJSON("https://api.coindesk.com/v1/bpi/currentprice.json");
            return DeserializeJson<CoindeskBPI>(json);
        }

        public static CoindeskBPIHistorical GetBPIHistoricalYesterday()
        {
            string json = GetJSON("https://api.coindesk.com/v1/bpi/historical/close.json?for=yesterday");
            return DeserializeJson<CoindeskBPIHistorical>(json);
        }

        // ?start=<VALUE>&end=<VALUE>
        // Allows data to be returned for a specific date range.
        // Must be listed as a pair of start and end parameters, with dates supplied in the YYYY-MM-DD format, e.g. 2013-09-01 for September 1st, 2013.
        public static CoindeskBPIHistorical GetBPIHistorical(DateTime dt1, DateTime dt2)
        {
            string json = GetJSON("https://api.coindesk.com/v1/bpi/historical/close.json?start={0}&end={1}", dt1.ToSortableDate(), dt2.ToSortableDate());
            return DeserializeJson<CoindeskBPIHistorical>(json);
        }

    } // end of class Coindesk

    //----------------------------------------------------------------------------------------------------------------------------------------------------------
    public class CoindeskBPITime
    {
        public string updated { get; set; }
        public string updatedISO { get; set; }
        public string updateduk { get; set; }
    } // end of class CoindeskBPITime

    public class CoindeskBPICurrency
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    } // end of class CoindeskBPICurrency

    public class CoindeskBPI
    {
        public CoindeskBPITime time { get; set; }
        public Dictionary<string, CoindeskBPICurrency> bpi { get; set; }
    } // end of class CoindeskBPI


    public class CoindeskBPIHistorical
    {
        public CoindeskBPITime time { get; set; }
        public Dictionary<string, float> bpi { get; set; }          // dictionary of historical BPI values <string sortableDateYYYY-MM-DD, float bpiValue>
    } // end of class CoindeskBPIHistorical

} // end of namespace

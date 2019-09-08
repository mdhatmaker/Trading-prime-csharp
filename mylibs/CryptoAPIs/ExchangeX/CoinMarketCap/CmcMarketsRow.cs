using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis.ExchangeX.CoinMarketCap
{
    public class CmcMarketsRow
    {
        public string number { get; set; }          // "1"
        public string exchange { get; set; }        // "Cryptopia"
        public string pair { get; set; }            // "GCN/LTC"
        public string volume24h { get; set; }       // "$1,028"
        public string price { get; set; }           // "$0.000011"
        public string volumePct { get; set; }       // "41.32%"
        public string updated { get; set; }         // "Recently"

        public CmcMarketsRow(string rowText)
        {
            var items = new List<string>();
            var split = rowText.Split('\n');
            foreach (var s in split)
            {
                var text = s.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                    items.Add(text);
            }

            number = items[0];
            exchange = items[1];
            pair = items[2];
            volume24h = items[3];
            price = items[4];
            volumePct = items[5];
            updated = items[6];
        }

        public override string ToString()
        {
            return string.Format("{0,-20} {1,-13} vol(24h):{2}  price:{3}  vol(%):{4}  updated:{5}", exchange, pair, volume24h, price, volumePct, updated);
        }
    } // end of class CmcMarketRow

} // end of namespace

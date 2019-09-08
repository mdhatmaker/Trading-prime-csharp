using System;
using System.Collections.Generic;
using System.Text;
using ExchangeSharp;

namespace CryptoApis.SharedModels
{
    public class TickerOutput
    {
        public string exchange { get; private set; }
        public DateTime timestamp { get; private set; }
        public IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers { get; private set; }

        public TickerOutput(string exchange, IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers)
        {
            this.exchange = exchange;
            this.tickers = tickers;
            this.timestamp = DateTime.Now;
        }

        public static string CsvHeaders => "DateTime,Exchange,Symbol,Bid,Ask,Last,BaseVolume,ConvertedVolume,BaseSymbol,ConvertedSymbol";

        public string ToCsv()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in tickers)
            {
                var symbol = kv.Key;
                //var globalSymbol = 
                var t = kv.Value;
                var timeString = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                sb.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", timeString, exchange, symbol, t.Bid, t.Ask, t.Last, t.Volume.BaseVolume, t.Volume.ConvertedVolume, t.Volume.BaseSymbol, t.Volume.ConvertedSymbol));
            }
            return sb.ToString();
        }
    } // end of class TickerOutput

} // end of namespace

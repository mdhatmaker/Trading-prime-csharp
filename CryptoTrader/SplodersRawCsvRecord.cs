using System;
using CryptoTools.Interfaces;
using static CryptoTools.Global;

namespace CryptoTrader
{
	public class SplodersRawCsvRecord : IRawCsvRecord
    {
        public string Timestamp { get; set; }
        public string Symbol { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string PriceUSD { get; set; }
        public string PriceBTC { get; set; }
        public string Volume24h { get; set; }
        public string MarketCapUSD { get; set; }
        public string AvailableSupply { get; set; }
        public string TotalSupply { get; set; }
        public string MaxSupply { get; set; }
        public string PercentChg1h { get; set; }
        public string PercentChg24h { get; set; }
        public string PercentChg7d { get; set; }
        public string Exchanges { get; set; }

        public string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", Timestamp, Symbol, Id, Name, PriceUSD, PriceBTC, Volume24h, MarketCapUSD, AvailableSupply, TotalSupply, MaxSupply, PercentChg1h, PercentChg24h, PercentChg7d, Exchanges);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SplodersRawCsvRecord))
                return false;
            else
            {
                bool b = Timestamp == ((SplodersRawCsvRecord)obj).Timestamp &&
                      Symbol == ((SplodersRawCsvRecord)obj).Symbol &&
                      Id == ((SplodersRawCsvRecord)obj).Id &&
                      Name == ((SplodersRawCsvRecord)obj).Name &&
                      PriceUSD == ((SplodersRawCsvRecord)obj).PriceUSD &&
                      PriceBTC == ((SplodersRawCsvRecord)obj).PriceBTC &&
                      Volume24h == ((SplodersRawCsvRecord)obj).Volume24h &&
                      MarketCapUSD == ((SplodersRawCsvRecord)obj).MarketCapUSD &&
                      AvailableSupply == ((SplodersRawCsvRecord)obj).AvailableSupply &&
                      TotalSupply == ((SplodersRawCsvRecord)obj).TotalSupply &&
                      MaxSupply == ((SplodersRawCsvRecord)obj).MaxSupply &&
                      PercentChg1h == ((SplodersRawCsvRecord)obj).PercentChg1h &&
                      PercentChg24h == ((SplodersRawCsvRecord)obj).PercentChg24h &&
                      PercentChg7d == ((SplodersRawCsvRecord)obj).PercentChg7d &&
                      Exchanges == ((SplodersRawCsvRecord)obj).Exchanges;
                return b;
            }
        }

        public override int GetHashCode()
        {
            return ToTimestampSeconds(System.DateTime.Parse(Timestamp));
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14}", Timestamp, Symbol, Id, Name, PriceUSD, PriceBTC, Volume24h, MarketCapUSD, AvailableSupply, TotalSupply, MaxSupply, PercentChg1h, PercentChg24h, PercentChg7d, Exchanges);
        }
    } // end of class SplodersRawCsv

} // end of namespace

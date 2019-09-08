using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis
{
    public class TickerUpdate
    {
        public string Symbol { get; protected set; }
        public decimal LastTradePrice { get; protected set; }
        public int LastTradeSize { get; protected set; }
        public string LastTradeTime { get; protected set; }
        public decimal Bid { get; protected set; }
        public int BidSize { get; protected set; }
        public decimal Ask { get; protected set; }
        public int AskSize { get; protected set; }
        public int TotalVolume { get; protected set; }
        public decimal OpenPrice { get; protected set; }
        public decimal HighPrice { get; protected set; }
        public decimal LowPrice { get; protected set; }
        public decimal ClosePrice { get; protected set; }

        public decimal Mid { get { return (Bid + Ask) / 2; } }
        public decimal WeightedMid { get { return Bid + (Ask - Bid) * (AskSize / (BidSize + AskSize)); } }     // TODO: check this calculation

        public decimal PercentChange { get { return (decimal.MinValue == LastTradePrice || decimal.MinValue == ClosePrice) ? decimal.MinValue : Math.Round((LastTradePrice - ClosePrice) / ClosePrice * 100.0M, 2); } }

        //public static TickerUpdate Empty { get { return } }

        public TickerUpdate() { }

        // Copy Constructor
        public TickerUpdate(TickerUpdate update)
        {
            Symbol = update.Symbol;
            LastTradePrice = update.LastTradePrice;
            LastTradeSize = update.LastTradeSize;
            LastTradeTime = update.LastTradeTime;
            Bid = update.Bid;
            BidSize = update.BidSize;
            Ask = update.Ask;
            AskSize = update.AskSize;
            TotalVolume = update.TotalVolume;
            OpenPrice = update.OpenPrice;
            HighPrice = update.HighPrice;
            LowPrice = update.LowPrice;
            ClosePrice = update.ClosePrice;
        }

        public void CopyTo(TickerUpdate update)
        {
            update.Symbol = Symbol;
            update.LastTradePrice = LastTradePrice;
            update.LastTradeSize = LastTradeSize;
            update.LastTradeTime = LastTradeTime;
            update.Bid = Bid;
            update.BidSize = BidSize;
            update.Ask = Ask;
            update.AskSize = AskSize;
            update.TotalVolume = TotalVolume;
            update.OpenPrice = OpenPrice;
            update.HighPrice = HighPrice;
            update.LowPrice = LowPrice;
            update.ClosePrice = ClosePrice;
        }


    } // end of class TickerUpdate

} // end of namespace

using System;

namespace CryptoApis.Models
{
    public class XTicker : XModel
    {
        
        public decimal Bid { get; private set; }
        public decimal Ask { get; private set; }
        public decimal BidSize { get; private set; }
        public decimal AskSize { get; private set; }

        public decimal MidPrice { get { return (Bid + Ask) / 2.0M; } }

        // Kraken
        public XTicker(KrakenCore.Models.TickerInfo t)
        {
            // bid/ask array: [price, whole_lot_volume, volume]
            Bid = t.Bid[0];
            Ask = t.Ask[0];
            BidSize = t.Bid[1];
            AskSize = t.Ask[1];
        }

        // GDAX
        public XTicker(GDAXSharp.Services.Products.Types.ProductTicker t)
        {
            Bid = t.Bid;
            Ask = t.Ask;
            BidSize = 0;     // no bid size provided
            AskSize = 0;     // no ask size provided
        }

        /*// Bitfinex
        public XTicker(Bitfinex.Net.Objects.BitfinexMarketOverviewRest mo)
        {
            Bid = mo.Bid;
            Ask = mo.Ask;
            BidSize = mo.BidSize;
            AskSize = mo.AskSize;
        }*/

        // Bitfinex
        public XTicker(Bitfinex.Net.Objects.BitfinexSymbolOverview so)
        {
            Bid = so.Bid;
            Ask = so.Ask;
            BidSize = so.BidSize;
            AskSize = so.AskSize;
        }

        // Binance
        public XTicker(Binance.Net.Objects.Spot.MarketData.BinanceBookPrice bp)
        {
            Bid = bp.BestBidPrice;
            Ask = bp.BestAskPrice;
            BidSize = bp.BestBidQuantity;
            AskSize = bp.BestAskQuantity;
        }

        // Bittrex
        public XTicker(Bittrex.Net.Objects.BittrexPrice p)
        {
            Bid = p.Bid;
            Ask = p.Ask;
            BidSize = 0;
            AskSize = 0;
        }

        // Poloniex
        public XTicker(Poloniex.GetTickersQuery.Ticker t)
        {
            Bid = t.HighestBid;
            Ask = t.LowestAsk;
            BidSize = 0;
            AskSize = 0;
        }

        // HitBTC
        public XTicker(CryptoApis.Exchange.HitBtc.HitBtcTicker t)
        {
            Bid = t.bid;
            Ask = t.ask;
            BidSize = 0;
            AskSize = 0;
        }

        // CoinExchange
        public XTicker(CryptoApis.Exchange.CoinExchangeTicker t)
        {
            Bid = t.BidPrice;
            Ask = t.AskPrice;
            BidSize = 0;
            AskSize = 0;
        }

        public override string ToString()
        {
            return string.Format("b:{0} a:{1}  bqty:{2} aqty:{3}", Bid, Ask, BidSize, AskSize);
        }

        public void Print(string exchange = "")
        {
            Console.WriteLine(string.Format("{0}   {1}", exchange, this.ToString()));
        }

    } // end of class XTicker

} // end of namespace

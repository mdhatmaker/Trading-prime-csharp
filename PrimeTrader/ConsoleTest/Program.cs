using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoAPIs;
using Tools;
using static Tools.G;
using IQFeed;

namespace ConsoleTest
{
    class Program
    {
        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var hist = new HistoryClient();
            //hist.HistoryIntervalTimeframe("VIX.XO", HistoryIntervalType.Time, "3600", "20160401", "20180424");
            //hist.HistoryIntervalTimeframe("VIX.XO", HistoryIntervalType.Time, "3600");
            //hist.HistoryIntervalTimeframe("VIX.XO", HistoryIntervalType.Time, "60", "20180101");            
            //hist.HistoryIntervalTimeframe("VIX.XO", HistoryIntervalType.Time, HistoryInterval.Time1m, "20180101");

            string startDate = "20100101";
            hist.HistoryContractIntervalTimeframe("VIX.XO", HistoryIntervalType.Time, HistoryInterval.Time1h, startDate);
            hist.HistoryFuturesIntervalTimeframe("@VX", HistoryIntervalType.Time, HistoryInterval.Time1h, startDate);
            hist.HistoryFuturesIntervalTimeframe("@ES", HistoryIntervalType.Time, HistoryInterval.Time1h, startDate);

            return;


            Crypto.InitializeExchanges();

            var cb = new CryptoAPIs.ExchangeX.Coinbase(Crypto.ApiKeys["COINBASE"].ApiKey, Crypto.ApiKeys["COINBASE"].ApiSecret);

            cb.GetTime();
            cb.GetSpotPrices();
            cb.GetAccounts();

            return;

            //CreateCurrencyPairFile();

            /*foreach (var exchange in Crypto.Exchanges.Values)
            {
                var symbols = exchange.SymbolList;
                foreach (var s in symbols)
                {
                    cout("{0} {1}", exchange, s);
                }
            }*/

            //Crypto.poloniex.TestWebsocket();
            /*Crypto.poloniex.StartWebSocket(new string[] { "BTC_USD" });
            Thread.Sleep(2000);
            Crypto.poloniex.SubscribeWebSocket();*/

            /*var gemini = CryptoAPIs.Exchange.Gemini.Instance;
            gemini.StartWebSocket(new string[] { "BTC_USD" });
            gemini.SubscribeWebSocket();*/

            //Crypto.bitflyer.TestWebsocket();

            //Crypto.bitstamp.TestWebSocket();

            //Crypto.Test();

            //ZCurrencyPair pair = Crypto.CurrencyPairs["BTC_USD"]
            Crypto.bitfinex.StartWebSocket();
            Crypto.UpdateTickerEvent += Crypto_UpdateTickerEvent;

            ZCurrencyPair pair;

            pair = Crypto.CurrencyPairs["BTC_USD"];
            Crypto.SubscribeTickerUpdates(pair);

            pair = Crypto.CurrencyPairs["NEO_ETH"];
            Crypto.SubscribeTickerUpdates(pair);
            Crypto.SubscribeTradeUpdates(pair);

            /*var exchanges = Crypto.GetExchangesForPair(pairSymbol);
            foreach (var exchange in exchanges)
            {
                cout(exchange.Name);
                exchange.UpdateOrderBookEvent += Exchange_UpdateOrderBookEvent;
                exchange.SubscribeOrderBookUpdates(Crypto.CurrencyPairs[pairSymbol]);
            }*/


            resetEvent.WaitOne();


            //var mq = Crypto.b2c2.GetMarginRequirements("EUR");
            //var ops = Crypto.b2c2.GetOpenPositions();

            string pathname = Folders.misc_path("encrypt_sample.txt");
            Encryption.EncryptTextFile("Now is the time for all good men to come to the aid of their country.\0", pathname, "woody");
            
            string decrypted = Encryption.DecryptTextFile(pathname, "woody");
            cout("Decrypted: '{0}'", decrypted);

            cout("Done.");
        }

        private static void Crypto_UpdateTickerEvent(object sender, TickersUpdateArgs e)
        {
            cout(e.ToString());
        }

        private static ZOrderBook m_orderBook = new ZOrderBook();
        private static Dictionary<BaseExchange, double> bestBids = new Dictionary<BaseExchange, double>();
        private static Dictionary<BaseExchange, double> bestAsks = new Dictionary<BaseExchange, double>();
        private static double prevBestBaSpread, prevWorstBaSpread;
        private static void Exchange_UpdateOrderBookEvent(object sender, OrderBookUpdateArgs e)
        {
            //m_aggregateTimer.Enabled = false;

            double minSize = 1.0;       // minimum size to trade (in BTC)

            var exchange = sender as CryptoAPIs.BaseExchange;

            double bestBid = exchange.OrderBook.Bids.Last().Key;
            bestBids[exchange] = bestBid;
            //double bidSize = bestBid.Value;
            double bestAsk = exchange.OrderBook.Asks.First().Key;
            bestAsks[exchange] = bestAsk;


            //bestBids[e.OrderBook.Bids.Keys.Max()] = exchange;
            /*bestBids[Crypto.gdax.OrderBook.Bids.Keys.Max()] = CryptoExch.GDAX;
            bestBids[Crypto.hitbtc.OrderBook.Bids.Keys.Max()] = CryptoExch.HITBTC;
            bestBids[Crypto.bitfinex.OrderBook.Bids.Keys.Max()] = CryptoExch.BITFINEX;
            bestBids[Crypto.gemini.OrderBook.Bids.Keys.Max()] = CryptoExch.GEMINI;
            bestBids[Crypto.bitflyer.OrderBook.Bids.Keys.Max()] = CryptoExch.BITFLYER;
            bestBids[Crypto.poloniex.OrderBook.Bids.Keys.Max()] = CryptoExch.POLONIEX;*/

            /*bestAsks[Crypto.gdax.OrderBook.Asks.Keys.First()] = CryptoExch.GDAX;
            bestAsks[Crypto.hitbtc.OrderBook.Asks.Keys.First()] = CryptoExch.HITBTC;
            bestAsks[Crypto.bitfinex.OrderBook.Asks.Keys.First()] = CryptoExch.BITFINEX;
            bestAsks[Crypto.gemini.OrderBook.Asks.Keys.First()] = CryptoExch.GEMINI;
            bestAsks[Crypto.bitflyer.OrderBook.Asks.Keys.First()] = CryptoExch.BITFLYER;
            bestAsks[Crypto.poloniex.OrderBook.Asks.Keys.First()] = CryptoExch.POLONIEX;

            double bidPrice = bestBids.Keys.Last();
            double askPrice = bestAsks.Keys.First();*/

            double bestBidPrice = bestBids.Values.Max();
            double bestAskPrice = bestAsks.Values.Min();
            double bestBaSpread = Math.Round(bestAskPrice - bestBidPrice, 2);
            double worstBidPrice = bestBids.Values.Min();
            double worstAskPrice = bestAsks.Values.Max();
            double worstBaSpread = Math.Round(worstAskPrice - worstBidPrice, 2);

            // Only display updates if the one of the B/A spreads has changed
            if (bestBaSpread != prevBestBaSpread || worstBaSpread != prevWorstBaSpread)
            {
                cout("{0}  BestBid: {1:0.00}    BestAsk: {2:0.00}      b/a spread  best: {3:0.00}  worst: {4:0.00}", DateTime.Now.ToString("HH:mm:ss"), bestBidPrice, bestAskPrice, bestBaSpread, worstBaSpread);
                prevBestBaSpread = bestBaSpread;
                prevWorstBaSpread = worstBaSpread;
            }            
            
            //m_aggregateTimer.Enabled = false;
        }

        static void CreateCurrencyPairFile()
        {
            string pathname = Folders.system_path("CURRENCY_PAIRS.TXT");
            using (var f = new StreamWriter(pathname))
            {
                f.WriteLine("Exchange,Symbol,ExchangeSymbol,ExchangeLeft,ExchangeRight");
                foreach (var exchange in Crypto.Exchanges.Values)
                {
                    var symbols = exchange.SymbolList;
                    foreach (var s in symbols)
                    {
                        var exch = Crypto.GetExch(exchange.Name);
                        var zcp = ZCurrencyPair.FromSymbol(s, exch);
                        if (zcp == null) continue;
                        cout("{0},{1}", exch, zcp);
                        f.WriteLine(string.Format("{0},{1}", exch, zcp));
                    }
                    cout("--------------------------------------------------------------------");
                }
                cout("\nCurrency pairs output to file: '{0}'", pathname);
            }
        }

    } // end of class Program
} // end of namespace

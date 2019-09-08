using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange;
using CryptoAPIs.Exchange.BittrexX;

namespace CryptoAPIs
{
    public class OHLC
    { }

    public class BittrexTradeMACD
    {
        Bittrex bittrex;

        private static iMACD m_macd;
        private static List<OHLC> m_bars = new List<OHLC>();
        private static int m_pos = 0;                                       // our trade's current position (-1, 0, +1)
        private static decimal m_tradeUnitSize;                             // order volume for a SINGLE trade unit (ex: 0.001)
        private static decimal m_tradeUnits = 1;                            // how many units to trade (1 unit for first trade, 2 units thereafter)
        private static List<string> m_buyTxIds = new List<string>();        // store all txids from BUY orders
        private static List<string> m_sellTxIds = new List<string>();       // store all txids from SELL orders
        private static List<double[]> m_indicator = new List<double[]>();   // store each generated set of indicator values

        public BittrexTradeMACD(Bittrex exch)
        {
            bittrex = exch;
        }

        void ShowThreadInfo(String s)
        {
            Console.WriteLine("{0} Thread ID: {1}", s, Thread.CurrentThread.ManagedThreadId);
        }

        void BittrexOHLC(string pair, bool display = true)
        {
            // TODO: CONVERT THIS TO USE BINANCE (or other) SOURCE FOR OHLC CANDLESTICK DATA
            var ohlc = bittrex.GetOHLC(pair, 1);    // OHLC 1-MINUTE BARS
            if (ohlc == null) return;                       // if an exception occurs, skip this iteration
            long last = ohlc.Last;
            //ohlc.WriteToFile(Folders.crypto_path(string.Format("Kraken.{0}.DF.csv", pair)));

            double MACD, signal, hist;
            foreach (var bar in ohlc.Pairs.Values.First())
            {
                if (m_bars.Count == 0 || bar.Time > m_bars.Last().Time)     // only add bars whose Time is greater than the last added bar
                {
                    m_bars.Add(bar);
                    decimal o = bar.Open;
                    decimal h = bar.High;
                    decimal l = bar.Low;
                    decimal c = bar.Close;
                    int time = bar.Time;
                    decimal volume = bar.Volume;
                    decimal vwap = bar.Vwap;
                    int count = bar.Count;
                    if (display) cout(bar.ToString());
                    m_macd.ReceiveTick((double)c);
                    if (m_macd.isPrimed())                                  // indicator is "primed" when it has enough data to calculate values
                    {
                        m_macd.Value(out MACD, out signal, out hist);                               // calculate MACD values
                        if (display) cout("---MACD:{0} signal:{1} hist:{2}", MACD, signal, hist);   // display the values
                        m_indicator.Add(new double[] { MACD, signal, hist });                       // add the values to the m_indicator List
                        if (m_pos > 0 && hist < 0)                                                  // CHECK TO SEE IF WE CROSSED pos-to-neg or neg-to-pos
                        {
                            TradeSell(pair);                                                        // long position and histMACD negative = SELL
                        }
                        else if (m_pos < 0 && hist > 0)
                        {
                            TradeBuy(pair);                                                         // short position and histMACD positive = BUY
                        }
                    }
                }
            }
        }

        void BracketInsideMarket(string pair)
        {
            /*int dplaces = 1;
            decimal percent = 0.05M;
            var bid = Math.Round(ticker[xpair].BidPrice - percent * ticker[xpair].BidPrice, dplaces);    // bid - 5%
            var ask = Math.Round(ticker[xpair].AskPrice + percent * ticker[xpair].BidPrice, dplaces);    // ask + 5%*/
        }

        void TradeBuy(string pair)
        {
            //string xpair = ohlc.Pairs.Keys.First();
            var ticker = bittrex.GetTicker(pair);
            cout("\n\n---BUY:");
            ticker.Display("ticker:");
            var t = ticker; // ticker.Values.First();
            cout("{0}  buy {1} unit(s) pay {2} (asksize:{3})          [LONG TRADE: {4}]\n\n", pair, m_tradeUnits, t.AskPrice, t.AskVolume, DateTime.Now.ToShortTimeString());
            m_pos += 2;
            // TODO: append trade to file
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }

        void TradeSell(string pair)
        {
            var ticker = bittrex.GetTicker(pair);
            cout("\n\n---SELL:");
            ticker.Display("ticker:");
            var t = ticker; // ticker.Values.First();
            cout("{0}  sell {1} unit(s) at {2} (bidsize:{3})          [SHORT TRADE: {4}]\n\n", pair, m_tradeUnits, t.BidPrice, t.BidVolume, DateTime.Now.ToShortTimeString());
            m_pos -= 2;
            // TODO: append trade to file
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }

        void IndicatorThread(String pair, int threadSleepSeconds = 30)
        {
            for (; ; )
            {
                //Console.WriteLine("{0} INDICATOR Thread (ID={1})", pair, Thread.CurrentThread.ManagedThreadId);
                BittrexOHLC(pair);
                Thread.Sleep(threadSleepSeconds * 1000);
            }
        }

        void OrdersThread(String pair, int threadSleepSeconds = 60)
        {
            int count = 0;
            for (; ; )
            {
                //Console.WriteLine("{0} ORDER Thread (ID={1})", pair, Thread.CurrentThread.ManagedThreadId);

                // ORDERS, TRADES, POSITIONS
                var openOrders = bittrex.GetOpenOrders();
                if (openOrders != null)
                {
                    if (openOrders.Count() == 0)
                        cout("---open orders: (none)");
                    else
                        openOrders.Display("---open orders:");
                }
                /*if (count % 5 == 0)                // display closed orders every 5th time through
                {
                    var closedOrders = kraken.GetClosedOrders();
                    if (closedOrders != null) closedOrders.Display("---closed orders:");
                }*/
                //var qo = kraken.QueryOrders(txids);
                //var qt = kraken.QueryTrades(txids);
                //var op = kraken.GetOpenPositions(txids);
                // TODO: I probably should be checking the open positions here to see if they match our internal position value

                Thread.Sleep(threadSleepSeconds * 1000);
                ++count;
            }
        }

        void BittrexMarketInformation(string pair)
        {
            // GENERAL MARKET INFORMATION
            /*var rt = kraken.GetRecentTrades(pair);
            long timestamp = rt.Last;
            foreach (var symbol in rt.Trades.Keys)
            {
                var trdlist = rt.Trades[symbol];
                foreach (var trd in trdlist)
                {
                    cout("{0} {1} {2} {3} {4} {5}", trd.Price, trd.Volume, trd.Side, trd.Type, trd.Misc, trd.Time);
                }
            }*/

            /*var rt = bittrex.GetRecentTrades(pair);
            var sd = bittrex.GetRecentInsideMarkets(pair);
            var tv = bittrex.GetTradeVolume(pair);*/
        }

        void BittrexBalances()
        {
            // ACCOUNT AND TRADE BALANCES
            /*var abal = bittrex.GetAccountBalance();
            foreach (var s in abal.Keys)
            {
                cout("acct balance {0}: {1}", s, abal[s]);
            }
            var tbal = bittrex.GetTradeBalance();
            cout(tbal.ToString());*/
        }

        public void StartTrade(string pair, decimal tradeUnitSize)
        {
            m_tradeUnitSize = tradeUnitSize;

            m_macd = new iMACD(10, 26, 9);

            BittrexOHLC(pair, false);                        // get the initial set of OHLC data (and calculate latest MACD values)
            var hist = m_indicator.Last()[2];               // get the most recent "histogram" MACD value (watch this going positive/negative)
            if (hist > 0)                                   // set our initial "position" according to current "histogram" MACD value
                m_pos = 1;
            else
                m_pos = -1;

            /*// ADD ORDER
            var ticker = kraken.GetTicker(pair);
            xpair = ohlc.Pairs.Keys.First();
            ticker.Display("ticker:");
            int dplaces = 1;
            decimal percent = 0.05M;
            var bid = Math.Round(ticker[xpair].BidPrice - percent * ticker[xpair].BidPrice, dplaces);    // bid - 5%
            var ask = Math.Round(ticker[xpair].AskPrice + percent * ticker[xpair].BidPrice, dplaces);    // ask + 5%
            var addorder = kraken.AddOrder(pair, "buy", bid, 0.001M, 2);
            string[] txids;
            if (addorder != null)
            {
                txids = addorder.Txid;
                var descr = addorder.Descr;
                cout(addorder.ToString());
            }
            else
                txids = null;

            // ORDERS, TRADES, POSITIONS
            var openOrders = kraken.GetOpenOrders();
            var closedOrders = kraken.GetClosedOrders();
            openOrders.Display("open orders:");
            closedOrders.Display("closed orders:");
            var qo = kraken.QueryOrders(txids);
            var qt = kraken.QueryTrades(txids);
            var op = kraken.GetOpenPositions(txids);

            bool cancel = true;
            if (cancel && txids != null)
            {
                // CANCEL PREVIOUSLY ADDED ORDER
                foreach (var id in txids)
                {
                    var cancelorder = kraken.CancelOrder(id);
                    cout(cancelorder.ToString());
                }
            }*/

            //ShowThreadInfo("Application");
            var tind = Task.Run(() => IndicatorThread(pair));
            //t.Wait();

            var torders = Task.Run(() => OrdersThread(pair));


            System.Console.WriteLine("Starting BITTREX trading system...");
            //System.Console.ReadLine();
            tind.Wait();
        }

    } // end of class BittrexTradeMACD


}

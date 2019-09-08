using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;
using Tools.Messaging;
using static Tools.G;
using CryptoAPIs.Exchange;
using CryptoAPIs.Exchange.Clients.Poloniex;

namespace CryptoAPIs
{
    public class PoloniexTradeMACD
    {
        Poloniex m_exch;

        //private SortedDictionary<int, ZCandlestick> m_bars = new SortedDictionary<int, ZCandlestick>();     // map timestamp to OHLC bar
        private ZCandlestickMap m_bars = new ZCandlestickMap();

        private iMACD m_macd;                                           // this trade's Indicator
        private iMACD m_macdLive;                                       // version of Indicator that updates intra-bar
        private int m_pos = 0;                                          // our trade's current position (-1, 0, +1)
        private decimal m_tradeUnits = 1;                               // how many units to trade (default is 1 unit for first trade, 2 units thereafter)
        private string m_pair;                                          // pair (symbol) to trade
        private decimal m_tradeUnitSize;                                // order volume for a SINGLE trade unit (ex: 0.001)
        private int m_barIntervalMinutes;                               // determines bar interval of indicator (in minutes)
        private bool m_tradeLive;                                       // live trades submitted only when m_tradeLive is true
        private List<string> m_buyTxIds = new List<string>();           // store all txids from BUY orders
        private List<string> m_sellTxIds = new List<string>();          // store all txids from SELL orders
        private List<double[]> m_indicator = new List<double[]>();      // store each generated set of indicator values
        private HashSet<string> m_txids = new HashSet<string>();        // store valid Txids
        private string m_tradeStartTime;                                // store a "yyMMDDHHmmss" version of the DateTime we start this trade

        private static string m_tradeFile = "POLONIEX.MACD.trades.{0}.{1}.DF.csv";   // format this string with pair ("BTCEUR", etc.) and datetime ("yyyyMMddHHmmss") to get trade filename

        private IMessenger m_msg;

        public PoloniexTradeMACD(Poloniex exch, string pair, IMessenger messenger)
        {
            m_exch = exch;
            m_pair = pair;
            m_msg = messenger;
        }

        public void StartTrade(decimal tradeUnitSize, int barIntervalMinutes, bool tradeLive = false)
        {
            m_tradeUnitSize = tradeUnitSize;
            m_barIntervalMinutes = barIntervalMinutes;
            m_tradeLive = tradeLive;

            m_tradeStartTime = DateTime.Now.ToCompactDateTime();    // time that we started this trade
            InitializeTradeLog();                                   // write the column headers to the trade log file

            m_macd = new iMACD(10, 26, 9);                          // create the MACD indicator
            m_macdLive = new iMACD(10, 26, 9);                      // create intra-bar version of the MACD indicator

            try
            {
                //KrakenOHLC(pair, m_barIntervalMinutes, false, false);   // get the initial set of OHLC data (and calculate latest MACD values)
                GetOHLC(m_barIntervalMinutes, false, false);         // get the initial set of OHLC data (and calculate latest MACD values)
                var hist = m_indicator.Last()[2];                       // get the most recent "histogram" MACD value (watch this going positive/negative)
                cout("{0}>---initial MACD value: {1:0.00000000}", m_pair, hist);     // display the initial indicator value
                if (hist > 0)                                           // set our initial "position" according to current "histogram" MACD value
                    m_pos = 1;
                else
                    m_pos = -1;
            }
            catch (Exception ex)
            {
                cout("{0}> An error occurred initializing the KRAKEN MACD trade: {1}\nRestarting the trade usually solves the issue.", m_pair, ex.Message);
                return;
            }

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
            var tind = Task.Run(() => IndicatorThread());
            var torders = Task.Run(() => OrdersThread());
            var tbalances = Task.Run(() => BalancesThread());

            cout("Starting KRAKEN trading system: {0} ...", m_pair);
            string filename = string.Format(m_tradeFile, m_pair, m_tradeStartTime);
            cout("Trades will be written to file: '{0}'", filename);
        }

        /*void DisplayLatestIndicator(string pair)
        {
            var hist = m_indicator.Last()[2];                       // get the most recent "histogram" MACD value (watch this going positive/negative)
            cout("{0}>---latest MACD value: {1}     {2}", pair, hist, DateTime.Now.ToShortTimeString());     // display the initial indicator value
        }*/

        void GetOHLC(int barIntervalMinutes, bool displayIndicator, bool displayDebug = false)
        {
            //var ohlc = kraken.GetOHLC(pair, 1);     // OHLC 1-MINUTE BARS
            //var ohlc = kraken.GetOHLC(pair, 60);    // OHLC 60-MINUTE BARS
            var barList = m_exch.GetOHLC(m_pair, barIntervalMinutes.ToChartPeriod());
            if (barList == null) return;               // if an exception occurs, skip this iteration
            //long last = ohlcList;
            //ohlc.WriteToFile(Folders.crypto_path(string.Format("Kraken.{0}.DF.csv", pair)));
            //var barList = ohlc; //  .Pairs.Values.First();
            foreach (var bar in barList)
            {
                ProcessBar(bar, displayIndicator, displayDebug);        // check this bar vs the bars we have already processed to see if we need to update Indicator
            }
        }

        void ProcessBar(ZCandlestick bar, bool displayIndicator, bool displayDebug)
        {
            if (m_bars.ContainsKey(bar.time))   // this is a bar that is already in our list (should be an update of the last/latest bar)
            {
                var lastBar = m_bars.Last().Value;
                if (bar.time == lastBar.time)
                {
                    m_bars[bar.time] = bar;
                    if (displayDebug) cout("{0}> update the LAST (latest) bar in our list: {1}", m_pair, bar.ToString());
                    m_macdLive.UpdateLastTick((double)lastBar.close);
                    CheckIndicator(m_macdLive, false, displayIndicator);
                }
            }
            else    // otherwise, add this NEW bar to our list AND update the indicator with the previous last bar in our list
            {
                if (m_bars.Count > 0)
                {
                    var lastBar = m_bars.Last().Value;
                    if (displayDebug) cout("{0}> updating indicator with LAST bar in our list: {1}", m_pair, lastBar.ToString());
                    m_macd.ReceiveTick((double)lastBar.close);
                    CheckIndicator(m_macd, true, displayIndicator);
                }
                m_macdLive.ReceiveTick((double)bar.close);
                CheckIndicator(m_macdLive, false, displayIndicator);
                m_bars.Add(bar.time, bar);
                if (displayDebug) cout("{0}> added to bar list: {1}", m_pair, bar.ToString());
            }
        }

        void CheckIndicator(iMACD macd, bool isCompleteBar, bool displayIndicator = true)
        {
            double MACD, signal, hist;
            if (macd.isPrimed())                                  // indicator is "primed" when it has enough data to calculate values
            {
                macd.Value(out MACD, out signal, out hist);                                 // calculate MACD values
                                                                                            //if (display) cout("---MACD:{0} signal:{1} hist:{2}", MACD, signal, hist);   // display the values
                if (displayIndicator) cout("{0}>---MACD:{1:0.00000000}--- [{2}]", m_pair, hist, DateTime.Now.ToShortTimeString());  // display only the histMACD value
                m_indicator.Add(new double[] { MACD, signal, hist });                       // add the values to the m_indicator List
                if (m_pos > 0 && hist < 0)                                                  // CHECK TO SEE IF WE CROSSED pos-to-neg or neg-to-pos
                {
                    if (isCompleteBar) TradeSell();                                         // long position and histMACD negative = SELL
                }
                else if (m_pos < 0 && hist > 0)
                {
                    if (isCompleteBar) TradeBuy();                                          // short position and histMACD positive = BUY
                }
            }
        }

        void InitializeTradeLog()
        {
            string pathname = Folders.exe_path(string.Format(m_tradeFile, m_pair, m_tradeStartTime));
            using (var writer = new StreamWriter(pathname, true))
            {
                writer.WriteLine("DateTime,Symbol,Side,Units,Size,Price,InsideMarket");
            }
        }

        void LogTrade(string trade)
        {
            string pathname = Folders.exe_path(string.Format(m_tradeFile, m_pair, m_tradeStartTime));
            using (var writer = new StreamWriter(pathname, true))
            {
                writer.WriteLine(trade);
            }
        }

        // Where side is "buy"|"sell"
        OrderNew SubmitOrder(OrderSide side, decimal price, decimal size)
        {
            if (m_tradeLive == false) return null;       // only submit trades when m_tradeLive is true

            var working = m_exch.SubmitLimitOrder(m_pair, side, price, size);
            return working;

            /*string[] txids;
            if (addorder != null)
            {
                txids = addorder.Txid;
                //var descr = addorder.Descr;
                cout("{0}>---SUBMITTED ORDER {1}", m_pair, addorder.ToString());
                m_txids.UnionWith(txids);
                if (side == "buy")
                    m_buyTxIds.AddRange(txids);
                else
                    m_sellTxIds.AddRange(txids);
            }
            else
                txids = null;*/
        }

        OrderCxl CancelOrder(OrderNew wo)
        {
            return m_exch.CancelOrder(wo.Pair, wo.OrderId);
        }

        void TradeBuy()
        {
            //string xpair = ohlc.Pairs.Keys.First();
            var t = m_exch.GetTicker(m_pair);
            cout("\n\n{0}>---BUY:", m_pair);
            t.Display("ticker:");
            string msg = string.Format("{0}  buy {1} unit(s) pay {2} (asksize:{3})    [LONG TRADE: {4}]\n", m_pair, m_tradeUnits, t.Ask, t.AskSize, DateTime.Now.ToShortTimeString());
            cout(msg);
            decimal tradePrice = t.Ask;                        // assume ASK price for BUY
            decimal tradeSize = m_tradeUnits * m_tradeUnitSize;     // trade size is trade_units * unit_size
            SubmitOrder(OrderSide.Buy, tradePrice, tradeSize);
            msg += GetTradeBalances();
            m_msg.Send(msg);
            m_pos += 2;
            LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}x{7}:{8}x{9}", DateTime.Now.ToSortableDateTime(), m_pair, "buy", m_tradeUnits, tradeSize, tradePrice, t.BidSize, t.Bid, t.Ask, t.AskSize));
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }

        void TradeSell()
        {
            var t = m_exch.GetTicker(m_pair);
            cout("\n\n{0}---SELL:", m_pair);
            t.Display("ticker:");
            string msg = string.Format("{0}  sell {1} unit(s) at {2} (bidsize:{3})    [SHORT TRADE: {4}]\n", m_pair, m_tradeUnits, t.Bid, t.BidSize, DateTime.Now.ToShortTimeString());
            cout(msg);
            decimal tradePrice = t.Bid;                        // assume BID price for SELL
            decimal tradeSize = m_tradeUnits * m_tradeUnitSize;     // trade size is trade_units * unit_size
            SubmitOrder(OrderSide.Sell, tradePrice, tradeSize);
            msg += GetTradeBalances();
            m_msg.Send(msg);
            m_pos -= 2;
            LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}x{7}:{8}x{9}", DateTime.Now.ToSortableDateTime(), m_pair, "sell", m_tradeUnits, m_tradeUnits * m_tradeUnitSize, tradePrice, t.BidSize, t.Bid, t.Ask, t.AskSize));
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }

        void IndicatorThread(int threadSleepSeconds = 30)
        {
            for (;;)
            {
                //Console.WriteLine("{0} INDICATOR Thread (ID={1})", pair, Thread.CurrentThread.ManagedThreadId);
                //DisplayLatestIndicator(pair);
                GetOHLC(m_barIntervalMinutes, true, false);
                Thread.Sleep(threadSleepSeconds * 1000);
            }
        }

        void OrdersThread(int threadSleepSeconds = 60)
        {
            int count = 0;
            for (;;)
            {
                //Console.WriteLine("{0} ORDER Thread (ID={1})", pair, Thread.CurrentThread.ManagedThreadId);

                // ORDERS, TRADES, POSITIONS
                /*var openOrders = m_exch.GetOpenOrders();
                if (openOrders != null)
                {
                    if (openOrders.Count == 0)
                        cout("{0}>---open orders: (none)", m_pair);
                    else
                    {
                        openOrders.Display(string.Format("{0}>---open orders:", m_pair));
                        // If there are open orders, send a Telegram message showing these open orders
                        StringBuilder sb = new StringBuilder();
                        foreach (var s in openOrders.Keys)
                        {
                            sb.Append(string.Format("{0}: {1}\n", s, openOrders[s].ToShortString()));
                        }
                        TelegramBot.Instance.SendToAll(sb.ToString());
                    }
                }*/
                /*if (count % 5 == 0)                // display closed orders every 5th time through
                {
                    var closedOrders = kraken.GetClosedOrders();
                    if (closedOrders != null) closedOrders.Display("---closed orders:");
                }*/

                /*if (m_txids.Count > 0)
                {
                    var qo = kraken.QueryOrders(m_txids);
                    qo.Display(pair + ">---query orders:");
                    var qt = kraken.QueryTrades(m_txids);
                    qt.Display(pair + ">---query trades:");
                    var op = kraken.GetOpenPositions(m_txids);
                    op.Display(pair + ">---open positions:");
                }*/
                // TODO: I probably should be checking the open positions here to see if they match our internal position value

                Thread.Sleep(threadSleepSeconds * 1000);
                ++count;
            }
        }

        void BalancesThread(int threadSleepSeconds = 180)
        {
            int count = 0;
            for (;;)
            {
                GetBalances();
                //var op = kraken.GetOpenPositions(txids);
                // TODO: I probably should be checking the open positions here to see if they match our internal position value

                Thread.Sleep(threadSleepSeconds * 1000);
                ++count;
            }
        }




        string GetTradeBalances()
        {
            return "";
            //var tbal = m_exch.GetTradeBalance();
            //return (tbal == null) ? "" : string.Format("---{0}\n", tbal.ToString());
        }

        void GetMarketInformation()
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
            /*var rt = m_exch.GetRecentTrades(m_pair);
            var sd = m_exch.GetRecentInsideMarkets(m_pair);
            var tv = m_exch.GetTradeVolume(m_pair);*/
        }

        void GetBalances()
        {
            // ACCOUNT AND TRADE BALANCES
            /*var abal = m_exch.GetAccountBalance();
            if (abal != null)
            {
                foreach (var s in abal.Keys)
                {
                    cout("---acct balance {0}: {1}", s, abal[s]);
                }
            }
            var tbal = m_exch.GetTradeBalance();
            if (tbal != null)
            {
                cout("---" + tbal.ToString());
            }*/
            //cout("\n");
        }


    } // end of class PoloniexTradeMACD

} // end of namespace

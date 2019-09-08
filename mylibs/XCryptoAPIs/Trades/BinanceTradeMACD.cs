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
using CryptoAPIs.Exchange.Clients.Binance;

namespace CryptoAPIs
{
    public class BinanceTradeMACD
    {
        public bool Debug { get { return m_displayDebug; } set { m_displayDebug = value; } }
        private bool m_displayDebug;

        Binance m_exch;

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
        private string m_tradeLogPathname;                              // pathname to the file in which we log our trades

        private static string m_tradeFile = "BINANCE.MACD.trades.{0}.{1}.{2}.DF.csv";   // format this string with pair ("BTCEUR", etc.), bar interval (minutes), and datetime ("yyyyMMddHHmmss") to get trade filename

        private IMessenger m_msg;

        private string PairId { get { return string.Format("{0}:{1}", m_pair, m_barIntervalMinutes); } }

        public BinanceTradeMACD(Binance exch, string pair, IMessenger messenger)
        {
            m_exch = exch;
            m_pair = pair;
            m_msg = messenger;
        }

        public void StartTrade(decimal tradeUnitSize, int barIntervalMinutes, bool tradeLive = false, bool backTestOnly = false)
        {
            m_tradeUnitSize = tradeUnitSize;
            m_barIntervalMinutes = barIntervalMinutes;
            //m_tradeLive = tradeLive;

            m_tradeStartTime = DateTime.Now.ToCompactDateTime();    // time that we started this trade
            InitializeTradeLog();                                   // write the column headers to the trade log file

            cout("Starting BINANCE MACD trade: '{0}' interval={1} size={2}", m_pair, m_barIntervalMinutes, m_tradeUnitSize);

            m_macd = new iMACD(10, 26, 9);                          // create the MACD indicator
            m_macdLive = new iMACD(10, 26, 9);                      // create intra-bar version of the MACD indicator

            m_tradeLive = false;                                    // turn OFF live trading while we process historical bars
            try
            {
                //KrakenOHLC(pair, m_barIntervalMinutes, false, false);               // get the initial set of OHLC data (and calculate latest MACD values)
                GetOHLC(m_barIntervalMinutes, displayIndicator:false, limit:500);   // get the initial set of OHLC data (and calculate latest MACD values)
                //GetOHLC(m_barIntervalMinutes, false, true, 500);                    // get the initial set of OHLC data (and calculate latest MACD values)
                var hist = m_indicator.Last()[2];                                   // get the most recent "histogram" MACD value (watch this going positive/negative)
                cout("{0}>---initial MACD value: {1:0.00000000}", PairId, hist);    // display the initial indicator value
                m_tradeLive = tradeLive;
                if (hist > 0)                                           // set our initial "position" according to current "histogram" MACD value
                    m_pos = 1;
                else
                    m_pos = -1;
            }
            catch (Exception ex)
            {
                cout("{0}> An error occurred initializing the BINANCE MACD trade: {1}\nRestarting the trade usually solves the issue.", m_pair, ex.Message);
                return;
            }

            // If we are only running a backtest, then exit after running through the initial historical bar data
            if (backTestOnly) return;

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
            //var torders = Task.Run(() => OrdersThread());
            //var tbalances = Task.Run(() => BalancesThread());

            cout("Starting {0} trading system: {1} ...", m_exch.Name, m_pair);
            cout("Trades will be written to file: '{0}'", m_tradeLogPathname);
        }

        /*void DisplayLatestIndicator(string pair)
        {
            var hist = m_indicator.Last()[2];                       // get the most recent "histogram" MACD value (watch this going positive/negative)
            cout("{0}>---latest MACD value: {1}     {2}", pair, hist, DateTime.Now.ToShortTimeString());     // display the initial indicator value
        }*/

        string GetBinanceInterval(int minutes)
        {
            return ToBinanceInterval(minutes).ToString().Substring(1);
        }

        BinanceOHLCInterval ToBinanceInterval(int minutes)
        {
            if (minutes == 1) return BinanceOHLCInterval._1m;
            else if (minutes == 3) return BinanceOHLCInterval._3m;
            else if (minutes == 5) return BinanceOHLCInterval._5m;
            else if (minutes == 15) return BinanceOHLCInterval._15m;
            else if (minutes == 30) return BinanceOHLCInterval._30m;
            else if (minutes == 60) return BinanceOHLCInterval._1h;
            else if (minutes == 120) return BinanceOHLCInterval._2h;
            else if (minutes == 240) return BinanceOHLCInterval._4h;
            else if (minutes == 360) return BinanceOHLCInterval._6h;
            else if (minutes == 480) return BinanceOHLCInterval._8h;
            else if (minutes == 720) return BinanceOHLCInterval._12h;
            else if (minutes == 1440) return BinanceOHLCInterval._1d;
            else
                throw new Exception(string.Format("Can't convert {0} minutes to Binance OHLC Interval", minutes));
        }

        void GetOHLC(int barIntervalMinutes, bool displayIndicator, int limit)
        {
            //var ohlc = kraken.GetOHLC(pair, 1);        // OHLC 1-MINUTE BARS
            //var ohlc = kraken.GetOHLC(pair, 60);       // OHLC 60-MINUTE BARS
            var barList = m_exch.GetOHLC(m_pair, GetBinanceInterval(barIntervalMinutes), limit);
            if (barList == null) return;                // if an exception occurs, skip this iteration
            //long last = ohlcList;
            //ohlc.WriteToFile(Folders.crypto_path(string.Format("Kraken.{0}.DF.csv", pair)));
            //var barList = ohlc; //  .Pairs.Values.First();
            /*foreach (var bar in barList)
            {
                cout("{0} {1} {2} {3} {4} {5} {6}", bar.time.ToDateTimeString(), bar.OpenTime, bar.CloseTime, bar.Open, bar.High, bar.Low, bar.Close);
            }
            return;*/
            foreach (var bar in barList)
            {
                ProcessBar(bar, displayIndicator);      // check this bar vs the bars we have already processed to see if we need to update Indicator
            }
        }

        void ProcessBar(ZCandlestick bar, bool displayIndicator)
        {
            int indicator = 0;
            if (m_bars.ContainsKey(bar.time))   // this is a bar that is already in our list (should be an update of the last/latest bar)
            {
                var lastBar = m_bars.Last().Value;
                if (bar.time == lastBar.time)
                {
                    m_bars[bar.time] = bar;
                    if (m_displayDebug) cout("{0}> update the 'real-time' indicator LAST (latest) bar in our list: {1}", PairId, bar.ToString());
                    m_macdLive.UpdateLastTick((double)lastBar.close);
                    indicator = CheckIndicator(m_macdLive, displayIndicator);
                    // not a completed bar, so do NOT execute a trade
                }
            }
            else    // otherwise, add this NEW bar to our list AND update the indicator with the previous last bar in our list
            {
                if (m_bars.Count > 0)
                {
                    var lastBar = m_bars.Last().Value;
                    if (m_displayDebug) cout("{0}> update the 'bar-complete' indicator with LAST COMPLETE bar in our list: {1}", PairId, lastBar.ToString());
                    m_macd.ReceiveTick((double)lastBar.close);
                    indicator = CheckIndicator(m_macd, displayIndicator);
                    // this IS a complete bar, so make a trade
                    if (indicator == -1)                                // indicator = SHORT
                    {
                        if (m_pos > 0)                                  // CHECK TO SEE IF WE CROSSED positive-to-negative
                            TradeSell(lastBar.time.ToDateTimeString(), lastBar.close);
                        else if (m_pos == 0)                            // if no position, then set our initial pos (DON'T trade)
                            m_pos = -1;
                    }
                    else if (indicator == +1)                           // indicator = LONG
                    {
                        if (m_pos < 0)                                  // CHECK TO SEE IF WE CROSSED positive-to-negative
                            TradeBuy(lastBar.time.ToDateTimeString(), lastBar.close);
                        else if (m_pos == 0)                            // if no position, then set our initial pos (DON'T trade)
                            m_pos = +1;
                    }
                }
                m_macdLive.ReceiveTick((double)bar.close);
                indicator = CheckIndicator(m_macdLive, displayIndicator);
                // not a completed bar, so do NOT execute a trade
                m_bars.Add(bar.time, bar);
                if (m_displayDebug) cout("{0}> added to bar list: {1}", m_pair, bar.ToString());
            }
        }

        // Return +1 (BUY), -1 (SELL), or 0 (NO TRADE)
        int CheckIndicator(iMACD macd, bool displayIndicator = true)
        {
            if (!macd.isPrimed()) return 0;                         // indicator is not primed, so return zero (NO TRADE)

            // indicator is "primed" when it has enough data to calculate values
            double MACD, signal, hist;
           
            macd.Value(out MACD, out signal, out hist);                                 // calculate MACD values
                                                                                        //if (display) cout("---MACD:{0} signal:{1} hist:{2}", MACD, signal, hist);   // display the values
            if (displayIndicator)
            {
                var ticker = m_exch.GetTicker(m_pair);
                cout("{0}>---MACD:{1:0.00000000}--- [{2}]  slow:{3:0.00000000} fast:{4:0.00000000}     {5} x {6} -- {7} x {8}   (b/a={9})  vol={10}", PairId, hist, DateTime.Now.ToShortTimeString(), MACD, signal, ticker.BidSize, ticker.Bid, ticker.Ask, ticker.AskSize, ticker.BidAskSpread, ticker.Volume);  // display only the histMACD value
            }
            m_indicator.Add(new double[] { MACD, signal, hist });                       // add the values to the m_indicator List
            //if (m_pos > 0 && hist < 0)                                                  // CHECK TO SEE IF WE CROSSED pos-to-neg or neg-to-pos
            if (hist < 0)                                                  // CHECK TO SEE IF WE CROSSED pos-to-neg or neg-to-pos
            {
                //if (isCompleteBar) TradeSell();                                         // long position and histMACD negative = SELL
                return -1;
            }
            //else if (m_pos < 0 && hist > 0)
            else if (hist > 0)
            {
                //if (isCompleteBar) TradeBuy();                                          // short position and histMACD positive = BUY
                return +1;
            }
            else
            {
                return 0;
            }
        }

        void InitializeTradeLog()
        {
            Folders.EnsureFolderExists(Folders.exe_path("DATA"));
            m_tradeLogPathname = Folders.exe_path("DATA/" + string.Format(m_tradeFile, m_pair, m_barIntervalMinutes, m_tradeStartTime));
            //cout("Trades logged to file: '{0}'", m_tradeLogPathname);
            using (var writer = new StreamWriter(m_tradeLogPathname, true))
            {
                writer.WriteLine("DateTime,Symbol,Side,Units,Size,Price,Active");  //,InsideMarket");
            }
        }

        void LogTrade(string trade)
        {
            using (var writer = new StreamWriter(m_tradeLogPathname, true))
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

        void TradeBuy(string tradeTime = null, decimal? price = null)
        {
            tradeTime = tradeTime ?? DateTime.Now.ToSortableDateTime();
            //string xpair = ohlc.Pairs.Keys.First();
            var t = m_exch.GetTicker(m_pair);
            string msg = string.Format("{0}  buy {1} unit(s) pay {2} (asksize:{3})    [LONG TRADE: {4}]", PairId, m_tradeUnits, t.Ask, t.AskSize, tradeTime);
            if (m_tradeLive)
            {
                cout("\n\n{0}>---BUY:", PairId);
                t.Display("ticker:");
                cout(msg + "\n");
            }
            decimal tradePrice = t.Ask;                             // assume ASK price for BUY
            decimal tradeSize = m_tradeUnits * m_tradeUnitSize;     // trade size is trade_units * unit_size
            SubmitOrder(OrderSide.Buy, tradePrice, tradeSize);
            msg += GetTradeBalances();
            if (m_tradeLive) m_msg.Send(msg);
            m_pos += 2;
            //LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}:{7}-{8}:{9}", tradeTime, m_pair, "buy", m_tradeUnits, tradeSize, tradePrice, t.BidSize, t.Bid, t.Ask, t._AskSize));
            LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}", tradeTime, m_pair, "buy", m_tradeUnits, tradeSize, price.Value, m_tradeLive ? 1 : 0));
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }

        void TradeSell(string tradeTime = null, decimal? price = null)
        {
            tradeTime = tradeTime ?? DateTime.Now.ToSortableDateTime();
            var t = m_exch.GetTicker(m_pair);
            string msg = string.Format("{0}  sell {1} unit(s) at {2} (bidsize:{3})    [SHORT TRADE: {4}]", PairId, m_tradeUnits, t.Bid, t.BidSize, tradeTime);
            if (m_tradeLive)
            {
                cout("\n\n{0}---SELL:", PairId);
                t.Display("ticker:");
                cout(msg + "\n");
            }
            decimal tradePrice = t.Bid;                             // assume BID price for SELL
            decimal tradeSize = m_tradeUnits * m_tradeUnitSize;     // trade size is trade_units * unit_size
            SubmitOrder(OrderSide.Sell, tradePrice, tradeSize);
            msg += GetTradeBalances();
            if (m_tradeLive) m_msg.Send(msg);
            m_pos -= 2;
            //LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}:{7}-{8}:{9}", tradeTime, m_pair, "sell", m_tradeUnits, m_tradeUnits * m_tradeUnitSize, tradePrice, t.BidSize, t.Bid, t.Ask, t._AskSize));
            LogTrade(string.Format("{0},{1},{2},{3}u,{4},{5},{6}", tradeTime, m_pair, "sell", m_tradeUnits, tradeSize, price.Value, m_tradeLive ? 1 : 0));
            m_tradeUnits = 2;                       // after the first trade, we always buy/sell 2 units
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This thread method displays updated indicator values on a (default) 30-second delay
        void IndicatorThread(int threadSleepSeconds = 30)
        {
            for (;;)
            {
                //Console.WriteLine("{0} INDICATOR Thread (ID={1})", pair, Thread.CurrentThread.ManagedThreadId);
                //DisplayLatestIndicator(pair);
                GetOHLC(m_barIntervalMinutes, true, 25);            // only retrieve 25 most recent bars
                Thread.Sleep(threadSleepSeconds * 1000);
            }
        }

        // This thread method displays open orders on a (default) 60-second delay
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

        // This thread method displays account balances on a (default) 300-second delay
        void BalancesThread(int threadSleepSeconds = 300)
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
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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
            var ai = m_exch.GetAccountInfo();
            cout("BINANCE commissions:   buyer={0}  seller={1}  maker={2}  taker={3}", ai.BuyerCommission, ai.SellerCommission, ai.MakerCommission, ai.TakerCommission);
            cout("BINANCE abilities:   can_deposit={0}  can_trade={1}  can_withdraw={2}", ai.CanDeposit, ai.CanTrade, ai.CanWithdraw);
            var bals = ai.Balances;
            cout("BINANCE BALANCES (showing non-zero only):");
            foreach (var b in bals)
            {
                if (b.Free != 0 || b.Locked != 0)
                    cout("Binance:{0}> free:{1} locked:{2}", b.Currency, b.Free, b.Locked);
            }

            /*var dh = m_exch.GetDepositHistory(m_pair);
            cout("BINANCE DEPOSIT HISTORY:");
            foreach (var d in dh.DepositList)
            {
                cout("{0}> {1}  {2}  {3}", d.Asset, d.Amount, d.InsertTime, d.Status);
            }

            var wh = m_exch.GetWithdrawHistory(m_pair);
            cout("BINANCE WITHDRAW HISTORY:");
            foreach (var w in wh.WithdrawList)
            {
                cout("{0}> {1}  {2}  {3}", w.Asset, w.Amount, w.InsertTime, w.Status);
            }*/



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

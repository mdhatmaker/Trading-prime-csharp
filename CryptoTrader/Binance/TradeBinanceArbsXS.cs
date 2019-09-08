using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CryptoApis.SharedModels;
using CryptoTools;
using CryptoTools.Cryptography;
using ExchangeSharp;
using CryptoApis.RestApi;
using CryptoTools.Messaging;
using System.Collections.Concurrent;
using CryptoApis;

namespace CryptoTrader
{
    // ---- QUANTITIES ----
    // NEO/USDT qty=0.20
    // BNB/USDT qty=1.00
    // BCC/USDT qty=0.014
    // QTUM/USDT qty=0.75
    // LTC/USDT qty = 0.09
    // ADA/USDT qty = 50.0
    // XRP/USDT qty = 20.0
    
    public class TradeBinanceArbsXS
    {
        // https://github.com/binance-exchange/binance-official-api-docs/blob/master/web-socket-streams.md

        // m_tickers[global_symbol][exchange] ==> ConcurrentStack<ExchangeTicker>
        // ex: m_tickers["BTC-USD"]["BINANCE"] ==> ConcurrentStack<ExchangeTicker>
        //private ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>> m_tickers;

        private List<string> m_tradingSymbols = new List<string>();
        private Dictionary<string, ExchangeTicker> m_tickers = new Dictionary<string, ExchangeTicker>();
        private Dictionary<string, decimal> m_lastBuy = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> m_lastSell = new Dictionary<string, decimal>();

        private Dictionary<string, decimal> m_tradeQty = new Dictionary<string, decimal>();
        private Dictionary<string, int> m_position = new Dictionary<string, int>();

        private int m_minPosition, m_maxPosition;

        private ExchangeSharpApi m_api;
        private List<IDisposable> m_sockets;
        private ProwlPub m_prowl;
        private Credentials m_creds;
        private bool m_notify = false;

        public TradeBinanceArbsXS()
        {
            m_api = new ExchangeSharpApi();
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_creds = Credentials.LoadEncryptedCsv(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            //m_tickers = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>>();

            m_tradingSymbols.AddRange(new List<string>() { "ETHUSDT", "BTCUSDT" });
            m_tradingSymbols.AddRange(new List<string>() { "NEOUSDT", "NEOETH", "NEOBTC" });
            m_tradingSymbols.AddRange(new List<string>() { "BNBUSDT", "BNBETH", "BNBBTC" });
            m_tradingSymbols.AddRange(new List<string>() { "QTUMUSDT", "QTUMETH", "QTUMBTC" });
            m_tradingSymbols.AddRange(new List<string>() { "LTCUSDT", "LTCETH", "LTCBTC" });
            m_tradingSymbols.AddRange(new List<string>() { "BCCUSDT", "BCCETH", "BCCBTC" });
            m_tradingSymbols.AddRange(new List<string>() { "ADAUSDT", "ADAETH", "ADABTC" });
            m_tradingSymbols.AddRange(new List<string>() { "XRPUSDT", "XRPETH", "XRPBTC" });

            SetTradeSizes(5.0M);
            InitializeArbPositions();
        }

        public void StartTrading()
        {
            //BuyMinimums(10); return;

            StartWebSockets();
            Console.WriteLine("Press ENTER to shutdown.");
            Console.ReadLine();
            foreach (var s in m_sockets)
            {
                s.Dispose();
            }
        }

        // NOTE: THIS IS A ONE-OFF METHOD TO BE CALLED ONCE TO "LOAD UP" (BUY) THE INITIAL COIN NEEDED TO TRADE THE ARBS
        private void BuyMinimums(decimal multiplier)
        {
            var api = m_api.bina;
            foreach (var kv in m_tradeQty)
            {
                var symbol = kv.Key;
                decimal qty = kv.Value * multiplier;
                Console.Write("{0} ", symbol);
                var t = api.GetTicker(symbol);
                var price = t.Bid;
                Console.WriteLine("   {0}:{1}   b/a spread:{2}       BUY {3} pay {4}", t.Bid, t.Ask, t.BidAskSpread(), qty, price);
                api.Buy(symbol, price, qty);
            }
        }

        /*// NOTE: THIS IS A ONE-OFF METHOD TO BE CALLED ONCE TO "LOAD UP" (BUY) THE INITIAL COIN NEEDED TO TRADE THE ARBS
        private async Task BuyMinimums(decimal multiplier)
        {
            var tasks = new List<Task<ExchangeOrderResult>>();
            var api = m_api.bina;
            var tickers = await api.GetTickersAsync();
            foreach (var kv in m_tradeQty)
            {
                var symbol = kv.Key;
                decimal qty = kv.Value * multiplier;
                Console.Write("{0} ", symbol);
                //var t = await api.GetTickerAsync(symbol);
                var t = tickers.Where(x => x.Key == symbol).First().Value;
                var price = t.Bid;
                Console.WriteLine("   {0}:{1}   b/a spread:{2}       BUY {3} pay {4}", t.Bid, t.Ask, t.BidAskSpread(), qty, price);
                tasks.Add(api.BuyAsync(symbol, price, qty));
            }
            Task.WaitAll(tasks.ToArray());
        }*/

        private void StartWebSockets()
        {
            m_sockets = new List<IDisposable>();
            m_sockets.Add(StartWebsocketsBinance(true));
            //m_sockets.Add(TestWebsocketsBittrex(true));
            //m_sockets.Add(TestWebsocketsPoloniex(true));
            //m_sockets.Add(TestWebsocketsGdax(true));
            //m_sockets.Add(TestWebsocketsBitfinex(true));
        }

        public IDisposable StartWebsocketsBinance(bool display = false)
        {
            Console.WriteLine("\nLAUNCH: Binance\n");
            IExchangeAPI a = new ExchangeBinanceAPI();
            int count = 0;
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                //if (display) Console.WriteLine("BINANCE  {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                if (count++ % 5 == 0) Console.Write(".");
                HandleTickerUpdate(a, tickers);
                //m_outputQ.Enqueue(new TickerOutput("BINANCE", tickers));
            });
            return socket;
        }

        public void HandleTickerUpdate(IExchangeAPI api, IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers)
        {
            var symbolsInUpdate = new HashSet<string>();
            foreach (var kv in tickers)
            {
                string symbol = kv.Key;
                var ticker = kv.Value;
                m_tickers[symbol] = ticker;

                symbolsInUpdate.Add(symbol);
            }

            if (symbolsInUpdate.Intersect(m_tradingSymbols).Count() > 0)
            {
                CheckArbs();
            }
        }

        // For each symbol (currency pair), set the trade qty to be used in our ARBS
        // where multiplier like 1.0M that will adjust the trade quantity for each symbol (currency pair)
        private void SetTradeSizes(decimal multiplier = 1.0M)
        {
            m_tradeQty["NEOUSDT"] = 0.20M;
            m_tradeQty["BNBUSDT"] = 1.00M;
            m_tradeQty["BCCUSDT"] = 0.020M;
            m_tradeQty["QTUMUSDT"] = 0.75M;
            m_tradeQty["LTCUSDT"] = 0.09M;
            m_tradeQty["ADAUSDT"] = 50.0M;
            m_tradeQty["XRPUSDT"] = 20.0M;

            foreach (var k in m_tradeQty.Keys.ToList())
            {
                m_tradeQty[k] = m_tradeQty[k] * multiplier;
            }
        }

        // For each arb (i.e. "neobtc", "neoeth", "qtumbtc", etc.), set the position to zero
        private void InitializeArbPositions()
        {
            m_position["NEOBTC"] = 0;
            m_position["BNBBTC"] = 0;
            m_position["BCCBTC"] = 0;
            m_position["QTUMBTC"] = 0;
            m_position["LTCBTC"] = 0;
            m_position["ADABTC"] = 0;
            m_position["XRPBTC"] = 0;

            m_position["NEOETH"] = 0;
            m_position["BNBETH"] = 0;
            m_position["BCCETH"] = 0;
            m_position["QTUMETH"] = 0;
            m_position["LTCETH"] = 0;
            m_position["ADAETH"] = 0;
            m_position["XRPETH"] = 0;

            // Set minimum and maximum allowable positions for the arbs
            m_minPosition = -3;
            m_maxPosition = +3;
        }


        private void CheckArbs()
        {
            decimal minPercentage = 0.15M;      // minimum percentage that should be available in the arb before it will be executed
            CheckArb1("NEO", minPercentage);
            CheckArb1("BNB", minPercentage);
            CheckArb1("QTUM", minPercentage);
            CheckArb1("LTC", minPercentage);
            CheckArb1("BCC", minPercentage);
            CheckArb1("ADA", minPercentage);
            CheckArb1("XRP", minPercentage);
        }

        private void CheckArb1(string currency, decimal minimum)
        {
            CheckArb1(currency + "USDT", currency + "ETH", "ETHUSDT", minimum);
            CheckArb1(currency + "USDT", currency + "BTC", "BTCUSDT", minimum);
        }

        private void CheckArb1(string symbol1, string symbol2, string symbol3, decimal minimum)
        {
            if (m_tickers.ContainsKey(symbol1) && m_tickers.ContainsKey(symbol2) && m_tickers.ContainsKey(symbol3))
            {
                // BUY xxxusdt
                decimal a1 = m_tickers[symbol1].Ask;
                decimal b2 = m_tickers[symbol2].Bid;
                decimal a3 = m_tickers[symbol3].Ask;
                decimal arb1 = ((b2 * a3) - a1) / a1 * 100.0M;

                // SELL xxxusdt
                decimal b1 = m_tickers[symbol1].Bid;
                decimal a2 = m_tickers[symbol2].Ask;
                decimal b3 = m_tickers[symbol3].Bid;
                decimal arb2 = (b1 - (a2 * b3)) / b1 * 100.0M;

                string dt = DateTime.Now.ToString("HH:mm:ss");

                string signal1, signal2;
                if (arb1 > minimum)
                {
                    signal1 = "BUY ";
                    if ((!m_lastBuy.ContainsKey(symbol2) || m_lastBuy[symbol2] != a1) && m_position[symbol2] > m_minPosition && m_position[symbol2] < m_maxPosition)    // TODO: Update this method to avoid duplicating previous trade
                    {
                        //decimal aq1 = m_tickers[symbol1].AskSize;
                        //decimal bq2 = m_tickers[symbol2].BidSize;
                        //decimal aq3 = m_tickers[symbol3].AskSize;
                        //decimal qty1 = Math.Min(Math.Min(aq1, m_tradeQty[symbol1]), bq2);
                        decimal qty1 = m_tradeQty[symbol1];
                        decimal qty2 = qty1 * b2;
                        qty2 = symbol3.StartsWith("BTC") ? Math.Round(qty2, 6) : Math.Round(qty2, 5);   // round BTC qty to 6 places, ETH qty to 5 places

                        Buy(symbol1, a1, qty1);
                        Sell(symbol2, b2, qty1);
                        Buy(symbol3, a3, qty2);

                        Console.WriteLine("\n{0} {1,8}/{2,7}:    {3}  {4:0.00}%", dt, symbol1, symbol2, signal1, arb1);
                        Console.WriteLine("      BUY {0} {1} @ {2}      SELL {3} {4} @ {5}      BUY {6} {7} @ {8}\n", symbol1, qty1, a1, symbol2, qty1, b2, symbol3, qty2, a3);

                        ++m_position[symbol2];          // add 1 to the stored position for this arb

                        m_lastBuy[symbol2] = a1;
                    }
                }

                if (arb2 > minimum)
                {
                    signal2 = "SELL";
                    if ((!m_lastSell.ContainsKey(symbol2) || m_lastSell[symbol2] != b1) && m_position[symbol2] > m_minPosition && m_position[symbol2] < m_maxPosition)  // TODO: Update this method to avoid duplicating previous trade
                    {
                        //decimal bq1 = m_tickers[symbol1].BidSize;
                        //decimal aq2 = m_tickers[symbol2].AskSize;
                        //decimal bq3 = m_tickers[symbol3].BidSize;
                        //decimal qty1 = Math.Min(Math.Min(bq1, m_tradeQty[symbol1]), aq2);
                        decimal qty1 = m_tradeQty[symbol1];
                        decimal qty2 = qty1 * a2;
                        qty2 = symbol3.StartsWith("BTC") ? Math.Round(qty2, 6) : Math.Round(qty2, 5);   // round BTC qty to 6 places, ETH qty to 5 places

                        Sell(symbol1, b1, qty1);
                        Buy(symbol2, a2, qty1);
                        Sell(symbol3, b3, qty2);

                        Console.WriteLine("\n{0} {1,8}/{2,7}:    {3}  {4:0.00}%", dt, symbol1, symbol2, signal2, arb2);
                        Console.WriteLine("      SELL {0} {1} @ {2}      BUY {3} {4} @ {5}      SELL {6} {7} @ {8}\n", symbol1, qty1, b1, symbol2, qty1, a2, symbol3, qty2, b3);

                        --m_position[symbol2];          // subtract 1 from the stored position for this arb

                        m_lastSell[symbol2] = b1;
                    }
                }
            }
            /*if (!string.IsNullOrWhiteSpace(signal1) || !string.IsNullOrWhiteSpace(signal2))
            {
                Console.WriteLine("{0,8}/{1,7}:   {2}  {3:0.00}       {4}  {5:0.00}", symbol1, symbol2, signal1, arb1, signal2, arb2);
            }*/
        }

        private void Buy(string symbol, decimal price, decimal amount)
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = symbol;
            order.Price = price;
            order.Amount = amount;
            order.IsBuy = true;
            m_api.bina.PlaceOrderAsync(order);
        }

        private void Sell(string symbol, decimal price, decimal amount)
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = symbol;
            order.Price = price;
            order.Amount = amount;
            order.IsBuy = false;
            m_api.bina.PlaceOrderAsync(order);
        }

        /*// For each symbol in the list, retrieve the BinanceSymbolInfo object
        private void GetSymbolInfo(List<string> symbolList)
        {
            foreach (var symbol in symbolList)
            {
                m_symbolInfo[symbol] = GetSymbolInfo(symbol);
            }
        }

        // Return the BinanceSymbolInfo object for the specified symbol
        private BinanceSymbolInfo GetSymbolInfo(string symbol)
        {
            BinanceSymbolInfo symbolInfo = null;
            for (int i = 0; i < m_exchangeInfo.symbols.Count; ++i)
            {
                var si = m_exchangeInfo.symbols[i];
                if (si.symbol.ToLower() == symbol)
                {
                    symbolInfo = si;
                }
            }
            return symbolInfo;
        }

        private void Rebalance()
        {
            var tickers = m_api.GetOrderBookTickers();

            // NEO, QTUM, BNB, LTC, BCC, ADA, XRP => 0.004 BTC
            // ETH => 0.02 BTC
            // BTC => 0.02 BTC
            // USDT => 0.04 BTC

        }

        public void GetBalances(out decimal totalBtc, out decimal totalUsdt, bool display = false)
        {
            totalBtc = 0.0M;
            totalUsdt = 0.0M;

            var tickers = m_api.GetOrderBookTickers();

            m_api.UpdateAccountBalances();

            // NEO, QTUM, BNB, LTC, BCC, ADA, XRP => 0.004 BTC
            // ETH => 0.02 BTC
            // BTC => 0.02 BTC
            // USDT => 0.04 BTC

            List<string> assets = new List<string>() { "ADA", "BCC", "BNB", "LTC", "NEO", "QTUM", "XRP", "BTC", "ETH", "USDT" };

            if (display) Console.WriteLine("");

            decimal btc;
            foreach (var k in m_api.Balances.Keys)
            {
                if (assets.Contains(k))     // only display balances for the selected asset list
                {
                    if (k == "BTC")
                        btc = m_api.Balances[k].Total;
                    else if (k == "USDT")
                        btc = m_api.Balances[k].Total / tickers["BTCUSDT"].MidPrice;
                    else
                    {
                        string symbol = k + "BTC";
                        btc = m_api.Balances[k].Total * tickers[symbol].MidPrice;
                    }
                    totalBtc += btc;

                    if (display)
                    {
                        Console.Write("{0:0.00000000,14} BTC     ", btc);
                        Console.WriteLine(m_api.Balances[k]);
                    }
                }
            }

            totalBtc = Math.Round(totalBtc, 8);
            totalUsdt = Math.Round(totalBtc * tickers["BTCUSDT"].MidPrice, 2);
        }

        public void StartPositionDisplayThread()
        {
            GetBalances(out decimal initTotalBtc, out decimal initTotalUsdt, true);
            Console.WriteLine("\n{0}    {1} BTC   {2} USDT\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), initTotalBtc, initTotalUsdt);

            int n = 0;
            while (true)
            {
                Thread.Sleep(30000);

                ++n;

                decimal totalBtc, totalUsdt;
                if (n % 10 == 0)
                    GetBalances(out totalBtc, out totalUsdt, true);
                else
                    GetBalances(out totalBtc, out totalUsdt);

                Console.WriteLine("{0}    {1} BTC   {2} USDT            change: {3} BTC   {4} USDT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), totalBtc, totalUsdt, totalBtc - initTotalBtc, totalUsdt - initTotalUsdt);
            }
        }*/

    } // end of class TradeBinanceArbsXS

} // end of namespace


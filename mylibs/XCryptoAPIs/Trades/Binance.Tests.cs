using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Timers;
//using PusherClient;
using CryptoAPIs.Exchange;
using CryptoAPIs.ExchangeX;
using Tools;
using static Tools.G;
using Tools.Messaging;
using System.IO;
using CryptoAPIs.Exchange.Clients.Kraken;
using CryptoAPIs.Exchange.Clients.Bittrex;
using CryptoAPIs.Exchange.Clients.Binance;

namespace CryptoAPIs
{
    public static class BinanceTests
    {
        private static Binance binance { get { return Crypto.Exchanges[CryptoExch.BINANCE] as Binance; }}
        private static Bittrex bittrex { get { return Crypto.Exchanges[CryptoExch.BITTREX] as Bittrex; }}

        public static void BacktestBinanceTrades()
        {
            var msg1 = new ConsoleMessenger();

            /*var symbols = new string[] { "NEOBTC", "NEOETH", "TRXBTC", "TRXETH", "XMRBTC", "XMRETH", "XRPBTC", "XRPETH",
                "ZECBTC", "ZECETH", "ZRXBTC", "ZRXETH" , "ASTBTC", "ASTETH", "LTCBNB", "LTCBTC", "LTCETH", "LTCUSDT"
            };*/

            var symbols = new string[] { "ADABTC","ADAETH","ADXBNB","ADXBTC","ADXETH","AEBNB","AEBTC","AEETH","AIONBNB","AIONBTC","AIONETH","AMBBNB","AMBBTC","AMBETH",
            "APPCBNB","APPCBTC","APPCETH","ARKBTC","ARKETH","ARNBTC","ARNETH","ASTBTC","ASTETH","BATBNB","BATBTC","BATETH",
            "BCCBNB","BCCBTC","BCCETH","BCCUSDT","BCDBTC","BCDETH","BCPTBNB","BCPTBTC","BCPTETH","BLZBNB","BLZBTC","BLZETH",
            "BNBBTC","BNBETH","BNBUSDT","BNTBTC","BNTETH","BQXBTC","BQXETH","BRDBNB","BRDBTC","BRDETH","BTCUSDT","BTGBTC","BTGETH","BTSBNB","BTSBTC","BTSETH",
            "CDTBTC","CDTETH","CHATBTC","CHATETH","CMTBNB","CMTBTC","CMTETH","CNDBNB","CNDBTC","CNDETH","CTRBTC","CTRETH","DASHBTC","DASHETH",
            "DGDBTC","DGDETH","DLTBNB","DLTBTC","DLTETH","DNTBTC","DNTETH","EDOBTC","EDOETH","ELFBTC","ELFETH","ENGBTC","ENGETH","ENJBTC","ENJETH",
            "EOSBTC","EOSETH","ETCBTC","ETCETH","ETHBTC","ETHUSDT","EVXBTC","EVXETH","FUELBTC","FUELETH","FUNBTC","FUNETH","GASBTC",
            "GTOBNB","GTOBTC","GTOETH","GVTBTC","GVTETH","GXSBTC","GXSETH","HSRBTC","HSRETH","ICNBTC","ICNETH","ICXBNB","ICXBTC","ICXETH",
            "INSBTC","INSETH","IOSTBTC","IOSTETH","IOTABNB","IOTABTC","IOTAETH","KMDBTC","KMDETH","KNCBTC","KNCETH","LENDBTC","LENDETH",
            "LINKBTC","LINKETH","LRCBTC","LRCETH","LSKBNB","LSKBTC","LSKETH","LTCBNB","LTCBTC","LTCETH","LTCUSDT","LUNBTC","LUNETH",
            "MANABTC","MANAETH","MCOBNB","MCOBTC","MCOETH","MDABTC","MDAETH","MODBTC","MODETH","MTHBTC","MTHETH","MTLBTC","MTLETH",
            "NANOBNB","NANOBTC","NANOETH","NAVBNB","NAVBTC","NAVETH","NEBLBNB","NEBLBTC","NEBLETH","NEOBNB","NEOBTC","NEOETH","NEOUSDT",
            "NULSBNB","NULSBTC","NULSETH","OAXBTC","OAXETH","OMGBTC","OMGETH","OSTBNB","OSTBTC","OSTETH","PIVXBNB","PIVXBTC","PIVXETH",
            "POEBTC","POEETH","POWRBNB","POWRBTC","POWRETH","PPTBTC","PPTETH","QSPBNB","QSPBTC","QSPETH","QTUMBTC","QTUMETH","RCNBNB","RCNBTC","RCNETH",
            "RDNBNB","RDNBTC","RDNETH","REQBTC","REQETH","RLCBNB","RLCBTC","RLCETH","SALTBTC","SALTETH","SNGLSBTC","SNGLSETH","SNMBTC","SNMETH",
            "SNTBTC","SNTETH","STEEMBNB","STEEMBTC","STEEMETH","STORJBTC","STORJETH","STRATBTC","STRATETH","SUBBTC","SUBETH",
            "TNBBTC","TNBETH","TNTBTC","TNTETH","TRIGBNB","TRIGBTC","TRIGETH","TRXBTC","TRXETH","VENBNB","VENBTC","VENETH","VIABNB","VIABTC","VIAETH",
            "VIBBTC","VIBEBTC","VIBEETH","VIBETH","WABIBNB","WABIBTC","WABIETH","WAVESBNB","WAVESBTC","WAVESETH","WINGSBTC","WINGSETH",
            "WTCBNB","WTCBTC","WTCETH","XLMBNB","XLMBTC","XLMETH","XMRBTC","XMRETH","XRPBTC","XRPETH","XVGBTC","XVGETH","XZCBNB","XZCBTC","XZCETH",
            "YOYOBNB","YOYOBTC","YOYOETH","ZECBTC","ZECETH","ZRXBTC","ZRXETH" };

            foreach (string s in symbols)
            {
                var b1 = new BinanceTradeMACD(binance, s, msg1);
                b1.StartTrade(0.1M, 1, tradeLive: false, backTestOnly: true);
                var b5 = new BinanceTradeMACD(binance, s, msg1);
                b5.StartTrade(0.1M, 5, tradeLive: false, backTestOnly: true);
                var b15 = new BinanceTradeMACD(binance, s, msg1);
                b15.StartTrade(0.1M, 15, tradeLive: false, backTestOnly: true);
                var b30 = new BinanceTradeMACD(binance, s, msg1);
                b30.StartTrade(0.1M, 30, tradeLive: false, backTestOnly: true);
                var b60 = new BinanceTradeMACD(binance, s, msg1);
                b60.StartTrade(0.1M, 60, tradeLive: false, backTestOnly: true);
            }
        }

        public static void TestBinanceTrades()
        {
            // Create Prowl messaging client
            var msg1 = new Tools.Messaging.Prowl.ProwlClient(Crypto.ApiKeys["PROWL"].ApiKey, "BINANCE", "MACD");

            var assetInfo = binance.GetAssetInfo();
            //assetInfo.Select(xkv => xkv).OrderBy(xkv => xkv.Key).ToList().ForEach(xai => cout(xai.Value.ToDisplay()));

            // Dictionary of symbols/intervals to trade
            var li = new string[,] {
                {"BCCBTC", "15"},
                {"BCCETH", "1"}, {"BCCETH", "5"}, {"BCCETH", "15"},
                {"BCCUSDT", "1"}, {"BCCUSDT", "5"}, {"BCCUSDT", "15"},
                {"BNBUSDT", "5"},
                {"BTCUSDT", "5"},
                {"DASHETH", "1"}, {"DASHETH", "15"},
                {"DGDETH", "1"}, {"DGDETH", "30"},
                {"ETHUSDT", "1"}, {"ETHUSDT", "5"}, {"ETHUSDT", "15"},
                {"LTCETH", "60"},
                {"LTCUSDT", "1"}, {"LTCUSDT", "5"}, {"LTCUSDT", "15"}, {"LTCUSDT", "30"}, {"LTCUSDT", "60"},
                {"NEOETH", "60"},
                {"NEOUSDT", "15"},
                {"XMRETH", "5"}, {"XMRETH", "60"},
                {"ZECETH", "1"}, {"ZECETH", "30"}, {"ZECETH", "60"}
            };

            // Iterate through each symbol/interval and create a new Trade (and start the trade)
            var trd = new Dictionary<string, BinanceTradeMACD>();
            for (int i = 0; i < li.GetLength(0); ++i)
            {
                cout("===============================================================================================");
                string symbol = li[i, 0];
                int barInterval = int.Parse(li[i, 1]);
                cout("Initiating BINANCE MACD '{0}' trade, {1} minutes", symbol, barInterval);

                var id = string.Format("{0}:{1}", symbol, barInterval);
                trd[id] = new BinanceTradeMACD(binance, symbol, msg1);
                trd[id].Debug = true;
                decimal stepSize = assetInfo[symbol].stepSize;
                trd[id].StartTrade(stepSize, barInterval, tradeLive: false, backTestOnly: false);
                cout("===============================================================================================");
            }

            return;

            /*var b60neo = new BinanceTradeMACD(binance, "NEOETH", msg1);
            b60neo.StartTrade(0.2M, 60, tradeLive: true, backTestOnly: false);
            var b15xmr = new BinanceTradeMACD(binance, "XMRETH", msg1);
            b15xmr.StartTrade(0.15M, 15, tradeLive: true, backTestOnly: false);
            var b60xmr = new BinanceTradeMACD(binance, "XMRETH", msg1);
            b60xmr.StartTrade(0.15M, 60, tradeLive: true, backTestOnly: false);
            var b30zec = new BinanceTradeMACD(binance, "ZECETH", msg1);
            b30zec.StartTrade(0.11M, 30, tradeLive: true, backTestOnly: false);
            var b60zec = new BinanceTradeMACD(binance, "ZECETH", msg1);
            b60zec.StartTrade(0.11M, 60, tradeLive: true, backTestOnly: false);
            var b30ltc = new BinanceTradeMACD(binance, "LTCETH", msg1);
            b30ltc.StartTrade(0.2M, 30, tradeLive: true, backTestOnly: false);
            var b60ltc = new BinanceTradeMACD(binance, "LTCETH", msg1);
            b60ltc.StartTrade(0.2M, 60, tradeLive: true, backTestOnly: false);*/

            /*ApiCredentials creds = m_security.ApiKeys["BINANCE"];
            var apiClient = new CryptoAPIs.Exchange.Clients.Binance.ApiClient(creds.ApiKey, creds.ApiSecret);
            var bn = new CryptoAPIs.Exchange.Clients.Binance.BinanceClient(apiClient);
            var ai = bn.GetAccountInfo().Result;
            Console.WriteLine("{0} {1} {2} {3}", ai.BuyerCommission, ai.SellerCommission, ai.MakerCommission, ai.TakerCommission);

            var bookTicker = bn.GetOrderBookTicker().Result;
            foreach (var obt in bookTicker)
            {
                if (obt.Symbol.Contains("NEO") || obt.Symbol.Contains("XMR"))
                    Console.WriteLine(obt.Symbol);
            }

            var kl = bn.GetCandleSticks("NEOETH", Exchange.Clients.Binance.TimeInterval.Hours_1).Result;
            foreach (var k in kl)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}", k.CloseTime.ToDateTimeString(), k.Open, k.High, k.Low, k.Close);
            }
            //bn.ListenKlineEndpoint("NEOETH", Exchange.BinanceX.TimeInterval.Minutes_1, HandleKline);

            Thread.Sleep(120000);
            return;*/
        }

        // Return a dictionary that maps each Binance currency-pair symbol (string) to a ZCurrencyPair object
        public static Dictionary<string, ZCurrencyPair> GetBinancePairs()
        {
            Dictionary<string, ZCurrencyPair> result = new Dictionary<string, ZCurrencyPair>();
            foreach (var s in binance.SymbolList)
            {
                result.Add(s, ZCurrencyPair.FromSymbol(s, CryptoExch.BINANCE));
            }
            return result;
        }

        // Create ArbZeroArgs for the given quote asset against "BTC", "ETH" and "BNB"
        // Where quote like "BCC" and size like 0.1M
        public static List<ArbZeroArgs> GetArgs(string quoteAsset, decimal size)
        {
            var args = new List<ArbZeroArgs>();
            string baseAsset;
            baseAsset = "BTC";
            args.Add(new ArbZeroArgs(new string[] { quoteAsset + "USDT", quoteAsset + baseAsset, baseAsset + "USDT" }, size));
            baseAsset = "ETH";
            args.Add(new ArbZeroArgs(new string[] { quoteAsset + "USDT", quoteAsset + baseAsset, baseAsset + "USDT" }, size));
            baseAsset = "BNB";
            args.Add(new ArbZeroArgs(new string[] { quoteAsset + "USDT", quoteAsset + baseAsset, baseAsset + "USDT" }, size));
            return args;
        }

        // BINANCE ARBS #0
        public static void TestBinanceArbs0()
        {
            // Create Prowl messaging client
            var msg1 = new Tools.Messaging.Prowl.ProwlClient(Crypto.ApiKeys["PROWL"].ApiKey, "BINANCE", "MACD");

            var assetInfo = binance.GetAssetInfo();
            //assetInfo.Select(xkv => xkv).OrderBy(xkv => xkv.Key).ToList().ForEach(xai => cout(xai.Value.ToDisplay()));

            // Create a BalanceMinMaxMap that we will pass to ArbZero to set our Min/Max currency balance limits
            var balances = binance.GetAccountBalances();
            var limits = BalanceMinMaxMap.CreatePercentageUpDown(balances, 50, 50);     // set limits to up or down 50% from current balances

            var args = new List<ArbZeroArgs>();
            args.AddRange(GetArgs("BCC", 0.01M));
            args.AddRange(GetArgs("LTC", 0.1M));
            args.AddRange(GetArgs("NEO", 0.1M));

            Task.Run(() => ArbZero(args, limits, true, 0.14M, 4001));
        }

        // BINANCE ARBS #1
        public static void TestBinanceArbs1()
        {
            // Create Prowl messaging client
            var msg1 = new Tools.Messaging.Prowl.ProwlClient(Crypto.ApiKeys["PROWL"].ApiKey, "BINANCE", "MACD");

            var assetInfo = binance.GetAssetInfo();
            //assetInfo.Select(xkv => xkv).OrderBy(xkv => xkv.Key).ToList().ForEach(xai => cout(xai.Value.ToDisplay()));

            // Create a BalanceMinMaxMap that we will pass to ArbZero to set our Min/Max currency balance limits
            var balances = binance.GetAccountBalances();
            var limits = BalanceMinMaxMap.CreatePercentageUpDown(balances, 50, 50);     // set limits to up or down 50% from current balances

            var args = new List<ArbZeroArgs>();
            args.AddRange(GetArgs("BCC", 0.01M));
            args.AddRange(GetArgs("LTC", 0.1M));
            args.AddRange(GetArgs("NEO", 0.1M));

            Task.Run(() => ArbOne(args, limits, true, 0.14M, 4001));
        }

        //var symbols = new string[] { "BCCUSDT", "BCCBTC", "BTCUSDT" };
        //decimal size = 0.01M;
        //int? threadSleepMillis = (int?)5000;
        public static void ArbZero(List<ArbZeroArgs> args, BalanceMinMaxMap balanceLimits, bool enableLiveOrders, decimal targetPct = 0.2M, int? threadSleepMillis = null)
        {
            var rnd = new Random();

            int sleepMillis = 0;
            if (threadSleepMillis == null)
                sleepMillis = 5000 + rnd.Next(-2000, 2000);
            else
                sleepMillis = threadSleepMillis.Value;

            cout("\nSLEEP_MILLIS = {0}\n", sleepMillis);

            var pairs = GetBinancePairs();
            var assetInfo = binance.GetAssetInfo();

            binance.EnableLiveOrders = enableLiveOrders;        // turn on/off live orders

            const int MAX_ORDERS = 100;   //100000;
            int norders = 0;

            const int MIN_PRIME_COUNT = 2;
            int nprime = 0;

            // THREAD LOOP STARTS HERE
            //DateTime startTime;
            //double elapsed;
            //List<double> millis = new List<double>();
            ZTicker t1, t2, t3;
            while (true)
            {
                // Get the balances in each currency (we will check these against our balanceLimits Min/Max values)
                var balances = binance.GetAccountBalances();

                // Get all the tickers as a dictionary
                var d = binance.GetAllTickers().Result;

                // Loop through each symbol set that was passed as ArbZeroArgs
                foreach (var aza in args)
                {
                    var symbols = aza.symbols;
                    var size = aza.size;

                    t1 = d[symbols[0]];
                    t2 = d[symbols[1]];
                    t3 = d[symbols[2]];

                    // THIS WHOLE SECTION WAS TO SPEED TEST VARIOUS GET TICKER METHODS
                    /*int calc = 2;
                    if (calc == 1)          // use GetFastTicker
                    {
                        //startTime = DateTime.Now;
                        t1 = binance.GetBookTicker(symbols[0]);
                        t2 = binance.GetBookTicker(symbols[1]);
                        t3 = binance.GetBookTicker(symbols[2]);
                        //elapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                        //millis.Add(elapsed);
                    }
                    else if (calc == 2)     // use GetAllTickers
                    {
                        //startTime = DateTime.Now;
                        var d = binance.GetAllTickers();
                        t1 = d[symbols[0]];
                        t2 = d[symbols[1]];
                        t3 = d[symbols[2]];
                        //elapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                        //millis.Add(elapsed);    
                    }
                    else                    // standard GetTicker
                    {
                        //startTime = DateTime.Now;
                        t1 = binance.GetTicker(symbols[0]);
                        t2 = binance.GetTicker(symbols[1]);
                        t3 = binance.GetTicker(symbols[2]);
                        //elapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                        //millis.Add(elapsed);
                    }*/
                    //cout("elapsed: {0:0.000} ms       avg:{1:0.000}  std:{2:0.000}", elapsed, millis.Average(), GMath.Std(millis));

                    var buy1 = t1.Ask;
                    var buy2 = t2.Bid * t3.Ask;
                    var buypct = (buy2 - buy1) / buy2 * 100;
                    var sell1 = t1.Bid;
                    var sell2 = t2.Ask * t3.Bid;
                    var sellpct = (sell1 - sell2) / sell1 * 100;

                    if (nprime >= MIN_PRIME_COUNT)
                    {
                        // Check tickers for BUY signal
                        if (buypct >= targetPct)
                        {
                            if (norders < MAX_ORDERS)
                            {
                                var pair0 = pairs[symbols[0]];
                                var lbalance0 = balances[pair0.Left];
                                var rbalance0 = balances[pair0.Right];
                                var llimit0 = balanceLimits[pair0.Left];
                                var rlimit0 = balanceLimits[pair0.Right];
                                if (lbalance0.Free < llimit0.Max && rbalance0.Free > rlimit0.Min)
                                {
                                    var pair1 = pairs[symbols[1]];
                                    var lbalance1 = balances[pair1.Left];
                                    var rbalance1 = balances[pair1.Right];
                                    var llimit1 = balanceLimits[pair1.Left];
                                    var rlimit1 = balanceLimits[pair1.Right];
                                    if (lbalance1.Free > llimit1.Min && rbalance1.Free < rlimit1.Max)
                                    {
                                        var pair2 = pairs[symbols[2]];
                                        var lbalance2 = balances[pair2.Left];
                                        var rbalance2 = balances[pair2.Right];
                                        var llimit2 = balanceLimits[pair2.Left];
                                        var rlimit2 = balanceLimits[pair2.Right];
                                        if (lbalance2.Free < llimit2.Max && rbalance2.Free > rlimit2.Min)
                                        {
                                            int places = binance.RoundSize(symbols[2]);
                                            //int places = m_binanceRoundLotSize[assetInfo[symbols[2]].stepSize];
                                            var calculatedSize = Math.Round(size * t2.Bid, places);
                                            binance.SubmitLimitOrder(symbols[0], OrderSide.Buy, t1.Ask, size);
                                            binance.SubmitLimitOrder(symbols[1], OrderSide.Sell, t2.Bid, size);
                                            binance.SubmitLimitOrder(symbols[2], OrderSide.Buy, t3.Ask, calculatedSize);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[0], OrderSide.Buy, t1.Ask, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[1], OrderSide.Sell, t2.Bid, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[2], OrderSide.Buy, t3.Ask, calculatedSize);
                                            // Adjust the balances for these orders/trades (assume they will be filled)
                                            lbalance0.Free += size;
                                            rbalance2.Free -= calculatedSize;
                                        }
                                        else
                                        {
                                            cout("+++Ignoring BUY signal due to {0} limits", symbols[2]);
                                        }
                                    }
                                    else
                                    {
                                        cout("+++Ignoring BUY signal due to {0} limits", symbols[1]);
                                    }
                                }
                                else
                                {
                                    cout("+++Ignoring BUY signal due to {0} limits", symbols[0]);
                                }
                            }
                            ++norders;
                        }

                        // Check tickers for SELL signal
                        if (sellpct >= targetPct)
                        {
                            if (norders < MAX_ORDERS)
                            {
                                var pair0 = pairs[symbols[0]];
                                var lbalance0 = balances[pair0.Left];
                                var rbalance0 = balances[pair0.Right];
                                var llimit0 = balanceLimits[pair0.Left];
                                var rlimit0 = balanceLimits[pair0.Right];
                                if (lbalance0.Free > llimit0.Min && rbalance0.Free < rlimit0.Max)
                                {
                                    var pair1 = pairs[symbols[1]];
                                    var lbalance1 = balances[pair1.Left];
                                    var rbalance1 = balances[pair1.Right];
                                    var llimit1 = balanceLimits[pair1.Left];
                                    var rlimit1 = balanceLimits[pair1.Right];
                                    if (lbalance1.Free < llimit1.Max && rbalance1.Free > rlimit1.Min)
                                    {
                                        var pair2 = pairs[symbols[2]];
                                        var lbalance2 = balances[pair2.Left];
                                        var rbalance2 = balances[pair2.Right];
                                        var llimit2 = balanceLimits[pair2.Left];
                                        var rlimit2 = balanceLimits[pair2.Right];
                                        if (lbalance2.Free > llimit2.Min && rbalance2.Free < rlimit2.Max)
                                        {
                                            int places = binance.RoundSize(symbols[2]);
                                            var calculatedSize = Math.Round(size * t2.Ask, places);
                                            binance.SubmitLimitOrder(symbols[0], OrderSide.Sell, t1.Bid, size);
                                            binance.SubmitLimitOrder(symbols[1], OrderSide.Buy, t2.Ask, size);
                                            binance.SubmitLimitOrder(symbols[2], OrderSide.Sell, t3.Bid, calculatedSize);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[0], OrderSide.Sell, t1.Bid, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[1], OrderSide.Buy, t2.Ask, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[2], OrderSide.Sell, t3.Bid, calculatedSize);
                                            // Adjust the balances for these orders/trades (assume they will be filled)
                                            lbalance0.Free -= size;
                                            rbalance2.Free += calculatedSize;
                                        }
                                        else
                                        {
                                            cout("+++Ignoring SELL signal due to {0} limits", symbols[2]);
                                        }
                                    }
                                    else
                                    {
                                        cout("+++Ignoring SELL signal due to {0} limits", symbols[1]);
                                    }
                                }
                                else
                                {
                                    cout("+++Ignoring SELL signal due to {0} limits", symbols[0]);
                                }
                            }
                            ++norders;
                        }
                    }

                    //cout("{0}\n{1}\n{2}", t1.ToDisplay(), t2.ToDisplay(), t3.ToDisplay());

                    var bsize1 = t1.AskSize;
                    var bsize2 = Math.Min(t2.BidSize, t3.AskSize);
                    string btext = (buy1 < buy2 ? "BUY" : "buy");
                    string bbtext = (buy1 < buy2 ? string.Format("BUY {0:#.000}%", buypct) : "      ");
                    var ssize1 = t1.BidSize;
                    var ssize2 = Math.Min(t2.AskSize, t3.BidSize);
                    string stext = (sell1 > sell2 ? "SELL" : "sell");
                    string sstext = (sell1 > sell2 ? string.Format("SELL {0:#.000}%", sellpct) : "       ");
                    cout("{0}: {1} x {2} < {3} x {4}    {5}: {6} x {7} > {8} x {9}     {10} {11}", btext, bsize1, buy1, buy2, bsize2, stext, ssize1, sell1, sell2, ssize2, bbtext, sstext);
                    /*buy1 = t1.Bid;
                    buy2 = t2.Bid * t3.Ask;
                    bsize1 = t1.BidSize;
                    bsize2 = Math.Min(t2.BidSize, t3.AskSize);
                    btext = (buy1 < buy2 ? "BUY" : "buy");
                    sell1 = t1.Ask;
                    sell2 = t2.Ask * t3.Bid;
                    ssize1 = t1.AskSize;
                    ssize2 = Math.Min(t2.AskSize, t3.BidSize);
                    stext = (sell1 > sell2 ? "SELL" : "sell");
                    cout("{0}: {1} x {2} < {3} x {4}    {5}: {6} x {7} > {8} x {9}\n", btext, bsize1, buy1, buy2, bsize2, stext, ssize1, sell1, sell2, ssize2);
                    */
                }

                ++nprime;

                Thread.Sleep(sleepMillis);
            }
        }

        // BINANCE ARB #1
        public static void ArbOne(List<ArbZeroArgs> args, BalanceMinMaxMap balanceLimits, bool enableLiveOrders, decimal targetPct = 0.2M, int? threadSleepMillis = null)
        {
            var rnd = new Random();

            int sleepMillis = 0;
            if (threadSleepMillis == null)
                sleepMillis = 5000 + rnd.Next(-2000, 2000);
            else
                sleepMillis = threadSleepMillis.Value;

            cout("\nSLEEP_MILLIS = {0}\n", sleepMillis);

            var pairs = GetBinancePairs();
            var assetInfo = binance.GetAssetInfo();

            binance.EnableLiveOrders = enableLiveOrders;        // turn on/off live orders

            const int MAX_ORDERS = 100;   //100000;
            int norders = 0;

            const int MIN_PRIME_COUNT = 2;
            int nprime = 0;

            // THREAD LOOP STARTS HERE
            ZTicker t1, t2, t3;
            while (true)
            {
                // Get the balances in each currency (we will check these against our balanceLimits Min/Max values)
                var balances = binance.GetAccountBalances();

                // Get all the tickers as a dictionary
                var bnd = binance.GetAllTickers().Result;

                var bxd = bittrex.GetAllTickers();

                // Loop through each symbol set that was passed as ArbZeroArgs
                foreach (var aza in args)
                {
                    var symbols = aza.symbols;
                    var size = aza.size;

                    t1 = bnd[symbols[0]];
                    t2 = bnd[symbols[1]];
                    t3 = bnd[symbols[2]];


                    var buy1 = t1.Ask;
                    var buy2 = t2.Bid * t3.Ask;
                    var buypct = (buy2 - buy1) / buy2 * 100;
                    var sell1 = t1.Bid;
                    var sell2 = t2.Ask * t3.Bid;
                    var sellpct = (sell1 - sell2) / sell1 * 100;

                    if (nprime >= MIN_PRIME_COUNT)
                    {
                        // Check tickers for BUY signal
                        if (buypct >= targetPct)
                        {
                            if (norders < MAX_ORDERS)
                            {
                                var pair0 = pairs[symbols[0]];
                                var lbalance0 = balances[pair0.Left];
                                var rbalance0 = balances[pair0.Right];
                                var llimit0 = balanceLimits[pair0.Left];
                                var rlimit0 = balanceLimits[pair0.Right];
                                if (lbalance0.Free < llimit0.Max && rbalance0.Free > rlimit0.Min)
                                {
                                    var pair1 = pairs[symbols[1]];
                                    var lbalance1 = balances[pair1.Left];
                                    var rbalance1 = balances[pair1.Right];
                                    var llimit1 = balanceLimits[pair1.Left];
                                    var rlimit1 = balanceLimits[pair1.Right];
                                    if (lbalance1.Free > llimit1.Min && rbalance1.Free < rlimit1.Max)
                                    {
                                        var pair2 = pairs[symbols[2]];
                                        var lbalance2 = balances[pair2.Left];
                                        var rbalance2 = balances[pair2.Right];
                                        var llimit2 = balanceLimits[pair2.Left];
                                        var rlimit2 = balanceLimits[pair2.Right];
                                        if (lbalance2.Free < llimit2.Max && rbalance2.Free > rlimit2.Min)
                                        {
                                            int places = binance.RoundSize(symbols[2]);
                                            var calculatedSize = Math.Round(size * t2.Bid, places);
                                            binance.SubmitLimitOrder(symbols[0], OrderSide.Buy, t1.Ask, size);
                                            binance.SubmitLimitOrder(symbols[1], OrderSide.Sell, t2.Bid, size);
                                            binance.SubmitLimitOrder(symbols[2], OrderSide.Buy, t3.Ask, calculatedSize);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[0], OrderSide.Buy, t1.Ask, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[1], OrderSide.Sell, t2.Bid, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[2], OrderSide.Buy, t3.Ask, calculatedSize);
                                            // Adjust the balances for these orders/trades (assume they will be filled)
                                            lbalance0.Free += size;
                                            rbalance2.Free -= calculatedSize;
                                        }
                                        else
                                        {
                                            cout("+++Ignoring BUY signal due to {0} limits", symbols[2]);
                                        }
                                    }
                                    else
                                    {
                                        cout("+++Ignoring BUY signal due to {0} limits", symbols[1]);
                                    }
                                }
                                else
                                {
                                    cout("+++Ignoring BUY signal due to {0} limits", symbols[0]);
                                }
                            }
                            ++norders;
                        }

                        // Check tickers for SELL signal
                        if (sellpct >= targetPct)
                        {
                            if (norders < MAX_ORDERS)
                            {
                                var pair0 = pairs[symbols[0]];
                                var lbalance0 = balances[pair0.Left];
                                var rbalance0 = balances[pair0.Right];
                                var llimit0 = balanceLimits[pair0.Left];
                                var rlimit0 = balanceLimits[pair0.Right];
                                if (lbalance0.Free > llimit0.Min && rbalance0.Free < rlimit0.Max)
                                {
                                    var pair1 = pairs[symbols[1]];
                                    var lbalance1 = balances[pair1.Left];
                                    var rbalance1 = balances[pair1.Right];
                                    var llimit1 = balanceLimits[pair1.Left];
                                    var rlimit1 = balanceLimits[pair1.Right];
                                    if (lbalance1.Free < llimit1.Max && rbalance1.Free > rlimit1.Min)
                                    {
                                        var pair2 = pairs[symbols[2]];
                                        var lbalance2 = balances[pair2.Left];
                                        var rbalance2 = balances[pair2.Right];
                                        var llimit2 = balanceLimits[pair2.Left];
                                        var rlimit2 = balanceLimits[pair2.Right];
                                        if (lbalance2.Free > llimit2.Min && rbalance2.Free < rlimit2.Max)
                                        {
                                            int places = binance.RoundSize(symbols[2]);
                                            var calculatedSize = Math.Round(size * t2.Ask, places);
                                            binance.SubmitLimitOrder(symbols[0], OrderSide.Sell, t1.Bid, size);
                                            binance.SubmitLimitOrder(symbols[1], OrderSide.Buy, t2.Ask, size);
                                            binance.SubmitLimitOrder(symbols[2], OrderSide.Sell, t3.Bid, calculatedSize);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[0], OrderSide.Sell, t1.Bid, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[1], OrderSide.Buy, t2.Ask, size);
                                            cout("---Crypto::ArbZero=> '{0}' {1} {2} {3}", symbols[2], OrderSide.Sell, t3.Bid, calculatedSize);
                                            // Adjust the balances for these orders/trades (assume they will be filled)
                                            lbalance0.Free -= size;
                                            rbalance2.Free += calculatedSize;
                                        }
                                        else
                                        {
                                            cout("+++Ignoring SELL signal due to {0} limits", symbols[2]);
                                        }
                                    }
                                    else
                                    {
                                        cout("+++Ignoring SELL signal due to {0} limits", symbols[1]);
                                    }
                                }
                                else
                                {
                                    cout("+++Ignoring SELL signal due to {0} limits", symbols[0]);
                                }
                            }
                            ++norders;
                        }
                    }

                    //cout("{0}\n{1}\n{2}", t1.ToDisplay(), t2.ToDisplay(), t3.ToDisplay());

                    var bsize1 = t1.AskSize;
                    var bsize2 = Math.Min(t2.BidSize, t3.AskSize);
                    string btext = (buy1 < buy2 ? "BUY" : "buy");
                    string bbtext = (buy1 < buy2 ? string.Format("BUY {0:#.000}%", buypct) : "      ");
                    var ssize1 = t1.BidSize;
                    var ssize2 = Math.Min(t2.AskSize, t3.BidSize);
                    string stext = (sell1 > sell2 ? "SELL" : "sell");
                    string sstext = (sell1 > sell2 ? string.Format("SELL {0:#.000}%", sellpct) : "       ");
                    cout("{0}: {1} x {2} < {3} x {4}    {5}: {6} x {7} > {8} x {9}     {10} {11}", btext, bsize1, buy1, buy2, bsize2, stext, ssize1, sell1, sell2, ssize2, bbtext, sstext);
                }

                ++nprime;

                Thread.Sleep(sleepMillis);
            }
        }

        public static void TestCoinbaseBinance(string pair)
        {
            /*KrakenOHLC(pair, false);                        // get the initial set of OHLC data (and calculate latest MACD values)
            var hist = m_indicator.Last()[2];               // get the most recent "histogram" MACD value (watch this going positive/negative)
            if (hist > 0)                                   // set our initial "position" according to current "histogram" MACD value
                m_pos = 1;
            else
                m_pos = -1;*/

            //var creds = Crypto.ApiKeys["COINBASE"];
            //var coin = new Coinbase(creds.ApiKey, creds.ApiSecret);
            //coin.TryMe();


            /*bittrex.GetBalances().Display("BITTREX Account Balances:");
            string currency = "BTC";
            cout(bittrex.GetDepositAddress(currency).ToString());
            cout(bittrex.GetBalance(currency).ToString());

            // MARKETS, CURRENCIES and TICKERS (MarketSummary=>SymbolDetails)
            //var markets = bittrex.GetMarkets();
            //var currencies = bittrex.GetCurrencies();
            var ticker = bittrex.GetTicker(pair);
            var summary = bittrex.GetMarketSummary(pair);
            var summaries = bittrex.GetMarketSummaries();

            var history = bittrex.GetMarketHistory(pair);*/
        }

        private static Binance.BinanceExchangeInfo m_binanceExchInfo;
        private static Dictionary<string, Binance.SymbolDetail> m_binanceSymbolDetail;
        public static void DisplayBinanceExchangeInfo()
        {
            m_binanceExchInfo = binance.GetExchangeInfo();
            var tz = m_binanceExchInfo.timezone;
            var stime = m_binanceExchInfo.serverTime;
            var exchFilters = m_binanceExchInfo.exchangeFilters;
            var rateLimits = m_binanceExchInfo.rateLimits;
            var symbols = m_binanceExchInfo.symbols;
            cout("BINANCE exchangeInfo:      server_time={0} timezone={1}", stime, tz);
            cout("exchFilters:\n");
            cout(exchFilters);
            cout("\nrateLimits:");
            cout(rateLimits);
            cout("\nsymbols ({0}):", symbols.Count);
            //cout(symbols);
            m_binanceSymbolDetail = new Dictionary<string, Binance.SymbolDetail>();
            foreach (var sd in symbols)
            {
                m_binanceSymbolDetail[sd.symbol] = sd;
            }

            cout(m_binanceSymbolDetail["TRXETH"]);
        }

        public static void DisplayBinanceAccountInfo()
        {
            var ai = binance.GetAccountInfo();
            cout("BINANCE commissions:   buyer={0}  seller={1}  maker={2}  taker={3}", ai.BuyerCommission, ai.SellerCommission, ai.MakerCommission, ai.TakerCommission);
            cout("BINANCE abilities:   can_deposit={0}  can_trade={1}  can_withdraw={2}", ai.CanDeposit, ai.CanTrade, ai.CanWithdraw);
            var bals = ai.Balances;
            cout("BINANCE BALANCES (showing non-zero only):");
            foreach (var b in bals)
            {
                if (b.Free != 0 || b.Locked != 0)
                    cout("Binance:{0}> free:{1} locked:{2}", b.Currency, b.Free, b.Locked);
            }

            /*var dh = binance.GetDepositHistory(pair);
            cout("BINANCE DEPOSIT HISTORY:");
            foreach (var d in dh.DepositList)
            {
                cout("{0}> {1}  {2}  {3}", d.Asset, d.Amount, d.InsertTime, d.Status);
            }

            var wh = binance.GetWithdrawHistory(pair);
            cout("BINANCE WITHDRAW HISTORY:");
            foreach (var w in wh.WithdrawList)
            {
                cout("{0}> {1}  {2}  {3}", w.Asset, w.Amount, w.InsertTime, w.Status);
            }*/
        }

        public static void DisplayBinanceOrders(string symbol)
        {
            var ord1 = binance.GetAllOrders(symbol);
            var enBuy1 = ord1.Where(o => o.Status == "FILLED" && o.Side.ToUpper() == "BUY").Select(o => o);
            var enSell1 = ord1.Where(o => o.Status == "FILLED" && o.Side.ToUpper() == "SELL").Select(o => o);
            decimal netBuys = enBuy1.Select(o => o.ExecutedQty).Sum();
            decimal netSells = enSell1.Select(o => o.ExecutedQty).Sum();
            decimal avgBuy = enBuy1.Select(o => o.Price).Average();
            decimal avgSell = enSell1.Select(o => o.Price).Average();

            decimal net = netBuys - netSells;
            string sNet = net.ToString("+0.00000000;-0.00000000;0");

            cout("{0}   net:{1}        net_buys:{2} avg_buy:{3:0.00000000}     net_sells:{4} avg_sell:{5:0.00000000}", symbol, sNet, netBuys, avgBuy, netSells, avgSell);
            foreach (var o in ord1)
            {
                cout("order [id:{0}] {1} {2} {3} {4} {5} {6} {7}", o.ClientOrderId, o.Symbol, o.Side, o.Price, o.OrigQty, o.ExecutedQty, o.Status, o.Time.ToDateTimeString());
            }
            var ticker = binance.GetTicker(symbol);
            cout("{0} current: {1} -- {2}     mid={3:0.00000000}\n", symbol, ticker.Bid, ticker.Ask, (ticker.Bid + ticker.Ask) / 2);
        }

    } // end of class BinanceTests



    public class ArbZeroArgs
    {
        public string[] symbols;
        public decimal size;

        public ArbZeroArgs(string[] symbols, decimal size)
        {
            this.symbols = symbols;
            this.size = size;
        }
    } // end of class ArbZeroArgs


} // end of namespace

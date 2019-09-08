using System;
using System.IO;
using System.Threading;
using CryptoTools.CryptoFile;
using CryptoTools.Messaging;

namespace CryptoTrader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO CRYPTO TRADER ***\n");

            #region ---------- COMMAND LINE ARGUMENTS -----------------------------------------------------------------
            // *** RUN CRYPTOTRADER "MODULE" BASED ON COMMAND LINE ARGUMENT ("1", "2", "3", ...)
            if (args.Length > 0)
            {
                if (args[0] == "1")
				{
					var mmaker = new TraderScalper();
                    mmaker.StartTrading();
				}
				else if (args[0] == "2")
				{
					var mmaker = new TraderScalper();
					mmaker.DisplayTradeSymbolATRs();
				}
                else if (args[0] == "3")
                {
                    var mmaker = new TraderScalper();
                    mmaker.DisplayProfits(1);
                }
                else if (args[0] == "4")
                {
                    var mmaker = new TraderScalper();
                    mmaker.DisplayAllFills();
                }
                else if (args[0] == "5")
                {
                    var mmaker = new TraderScalper();
                    mmaker.DisplayBetaCalcs("BINANCE", true);   // TODO: handle exchanges other than BINANCE
                }
                else if (args[0] == "6")
                {
                    var mmaker = new TraderScalper();
                    var ranks = mmaker.DisplayRanks(maxRank: 150);
                    Console.WriteLine("{0} symbols with specified max CoinMarketCap rank", ranks.Count);
                }
                else if (args[0] == "7")
                {
                    var mmaker = new TraderScalper();
                    var task = mmaker.DisplayBalances(true);
                    task.Wait();
                }
                else if (args[0] == "10")
                {
                    var tbsi = new TraderBuySellIndicator();
                    tbsi.StartBuySellIndicator("BINANCE", "ETHUSDT");
                    //tbsi.StartBuySellIndicator("BINANCE", "BTCUSDT");
                    //tbsi.StartBuySellIndicator("BITTREX", "BTC-DCR");*/
                }
                else if (args[0] == "20")
                {
                    var binanceArb = new TradeBinanceArbs();
                    binanceArb.StartTrading();
                }
                else if (args[0] == "21")
                {
                    //var bittrexArb = new TradeBittrexArbs();
                    //bittrexArb.StartTrading();
                }
                else if (args[0] == "22")
                {
                    var splod = new Sploders();
                    splod.Test();
                }
                else if (args[0] == "23")
                {
                    var txe = new TraderCrossExchange();
                    txe.Start();
                }
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("usage: dotnet CryptoTrader.dll <#module>");
				Console.WriteLine("   ex: dotnet CryptoTrader.dll 1     (MarketMaker)");
				Console.WriteLine("   ex: dotnet CryptoTrader.dll 2     (DisplayATRs)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 3     (DisplayDailyProfit)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 4     (DisplayAllFills)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 5     (DisplayBetaCalcs)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 6     (DisplayMarketCapRank)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 7     (DisplayBalances)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 10    (BuySellIndicator)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 20    (BinanceArbs)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 21    (BittrexArbs)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 22    (Sploders)");
                Console.WriteLine("   ex: dotnet CryptoTrader.dll 23    (TradeCrossExchange)");
                Console.WriteLine();
            }
            #endregion ------------------------------------------------------------------------------------------------

            // *** THIS CODE WILL RUN IF NO COMMAND-LINE ARGUMENTS ARE PASSED - USE FOR TESTING ***
            /*var trader = new TraderBuySellIndicator();
            //trader.Test();
			//trader.StartBuySellIndicator("BITTREX", "BTC-DCR", false);
			//trader.StartBuySellIndicator("BINANCE", "ETHUSDT", false);
			trader.StartBuySellIndicator("BINANCE", "BTCUSDT", false);
			//trader.StartBuySellIndicator("BINANCE", "BNBUSDT", false);
			//trader.StartBuySellIndicator("BINANCE", "ZRXBTC", false);*/

            //var barb = new TradeBinanceArbsXS();
            //barb.StartTrading();

            //var fx = new TraderFX();
            //fx.Test();

            var scalper = new TraderScalper();
            //scalper.DisplayProfits(nDays: 1);
            //scalper.Test();
            scalper.StartTrading(testOnly: true);
            //scalper.DisplayTradeSymbolATRs();
            //var scalperTask = scalper.DisplayBalances(true); scalperTask.Wait();
            //var scalperTask = scalper.CloseAllPositions(sellBtc: true); scalperTask.Wait();

            //var sploders = new Sploders();
            //sploders.Test();

            //var tradeXE = new TraderCrossExchange();
            //tradeXE.Start();

            //var pnp = new PubnubPub();
            //pnp.Test();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }



    } // end of class Program

} // end of namespace

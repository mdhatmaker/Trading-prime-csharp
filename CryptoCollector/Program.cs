//using PureSocketCluster;
//using PureWebSockets;
//using WebSocketSharp;
//using WebSocketX;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis;
using CryptoApis.RestApi;
using CryptoApis.Models;
using CryptoApis.ExchangeX.CoinMarketCap;
using CryptoTools.MathStat;

namespace CryptoCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO CRYPTO COLLECTOR ***\n");

            // *** RUN CRYPTOCOLLECTOR "MODULE" BASED ON COMMAND LINE ARGUMENT ("1", "2", "3", ...)
            if (args.Length > 0)
            {
                if (args[0] == "1")
                {
                    var collector = new CollectorTickers();
                    collector.Start();
                }
                else if (args[0] == "2")
                {
                    //var cmaker = new CandlestickMaker();
                    //cmaker.Test();
                }
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("usage: dotnet CryptoCollector.dll <#module>");
                Console.WriteLine("   ex: dotnet CryptoCollector.dll 1     (CollectorTickers)");
                Console.WriteLine("   ex: dotnet CryptoCollector.dll 2     (CandlestickMaker)");
                Console.WriteLine();
            }

            
            // *** THIS CODE WILL RUN IF NO COMMAND-LINE ARGUMENTS ARE PASSED - USE FOR TESTING ***
            var maker = new CandlestickMaker();
			//maker.Test();
			//maker.CreateCandlesFiles("BINANCE", "ETHUSDT");
			//maker.CreateCandlesFiles("BINANCE", "BTCUSDT");
			//maker.CreateCandlesFiles("BINANCE", "NEOUSDT");
			//maker.RealizedVolTest();
			maker.RangeHeightsTest();


            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

    } // end of class Program

} // end of namespace

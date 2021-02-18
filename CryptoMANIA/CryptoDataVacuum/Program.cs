using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CsvHelper;
using System.Globalization;
using Bittrex.Net.Objects;
using Bittrex.Net;
using CryptoExchange.Net.Authentication;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using Confluent.Kafka;
using CryptoExchange.Net.Sockets;
using System.Diagnostics;

namespace CryptoDataVacuum
{
    class Program
    {
        static string SymbolFolder = "C:\\cryptomania\\";


        static void DisplayWelcomeMessage()
        {
            Console.WriteLine("\n=== WELCOME TO CRYPTO DATA VACUUM ===\n");
            Console.WriteLine("This .NET Core app will subscribe to price updates for crypto");
            Console.WriteLine("symbols on Binance, Bittrex, and Bitfinex.\n");
            Console.WriteLine("Use the 'set-crypto-api-env-vars.ps1' script in 'scripts' folder");
            Console.WriteLine("to set your API keys.\n");
            Console.WriteLine("(See 'README.md' for more information.)\n");
        }

        static void DisplayUsageMessage()
        {
            Console.WriteLine("usage: dotnet CryptoDataVacuum all");
            Console.WriteLine("       dotnet CryptoDataVacuum [binance|bittrex|bitfinex]");
            Console.WriteLine();
        }


        static async Task Main(string[] args)
        {
            DisplayWelcomeMessage();


            // --- FOR DEBUGGING ONLY: CAN SET COMMAND-LINE ARGUMENTS ---
#if DEBUG
            if (Debugger.IsAttached)
            {
                //args = new string[] { "binance" };
                args = new string[] { "all" };
            }
#endif

            if (args.Length == 0)
            {
                DisplayUsageMessage();
            }
            else if (args[0].ToUpper() == "ALL")
            {
                await StartTickerProducers();
                
                Console.WriteLine("\n\nDone...Press ENTER to exit");
                Console.ReadLine();
            }
            else if (args[0].ToUpper() == "BINANCE")
            {

            }
            else if (args[0].ToUpper() == "BINANCE")
            {

            }
            else if (args[0].ToUpper() == "BINANCE")
            {

            }
            else
            {
                DisplayUsageMessage();
            }


            //System.Environment.Exit(0);

        } // end of Main


        static async Task StartTickerProducers(int sleepSeconds = 2)
        {
            // Kafka configuration parameters
            string bootstrapServers = "localhost:9092";
            string topic = "crypto-marketdata-symbols";
            string groupId = "marketdata-consumer-group";

            //var kafkaProducer = new KafkaProducer(bootstrapServers, topic);
            
            var kafkaConsumer = new KafkaConsumer(bootstrapServers, topic, groupId);
            var consumerTask = Task.Factory.StartNew(() => kafkaConsumer.Start());


            Console.WriteLine($"\n--- Running Exchange Demos in {sleepSeconds} second(s) ---");
            Thread.Sleep(sleepSeconds * 1000);

            // BINANCE exchange
            //ICryptoDataVacuum binance = new BinanceExchange(kafkaProducer);
            ICryptoDataVacuum binance = new BinanceExchange(bootstrapServers, topic);
            await binance.DisplaySymbolCount();

            // BITTREX exchange
            //ICryptoDataVacuum bittrex = new BittrexExchange(kafkaProducer);
            ICryptoDataVacuum bittrex = new BittrexExchange(bootstrapServers, topic);
            await bittrex.DisplaySymbolCount();

            // BITFINEX exchange
            //ICryptoDataVacuum bitfinex = new BitfinexExchange(kafkaProducer);
            ICryptoDataVacuum bitfinex = new BitfinexExchange(bootstrapServers, topic);
            await bitfinex.DisplaySymbolCount();

            // subscribe to updates
            await binance.SubscribeAllTickerUpdates();
            await bittrex.SubscribeAllTickerUpdates();
            await bitfinex.SubscribeAllTickerUpdates();

            // wait a while...
            int runSeconds = 600;
            Thread.Sleep(runSeconds * 1000);

            // unsubscribe from updates
            await binance.UnsubscribeAllUpdates();
            await bittrex.UnsubscribeAllUpdates();
            await bitfinex.UnsubscribeAllUpdates();

            //kafkaProducer.Shutdown();
            // TODO: shutdown consumer task?

            return;
        }


        /*static async Task DemoKafka(int sleepSeconds = 2)
        {
            Console.WriteLine($"\n--- Running Kafka Demos in {sleepSeconds} second(s) ---");
            Thread.Sleep(sleepSeconds * 1000);

            // Kafka configuration strings
            string bootstrapServers = "localhost:9092";
            string topic = "crypto-marketdata-symbols";
            string groupId = "marketdata-consumer-group";

            await DemoKafkaProducer(bootstrapServers, topic, payload: "This is a sample payload.");
            await DemoKafkaConsumer(groupId, bootstrapServers, topic);

            return;
        }*/

    } // class

} // namespace


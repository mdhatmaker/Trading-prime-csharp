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
                await DemoExchanges();
                await DemoKafka();

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


        static async Task DemoExchanges(int sleepSeconds = 2)
        {
            Console.WriteLine($"\n--- Running Exchange Demos in {sleepSeconds} second(s) ---");
            Thread.Sleep(sleepSeconds * 1000);

            // BINANCE exchange
            ICryptoDataVacuum binance = new BinanceExchange();
            await binance.DisplaySymbolCount();
            //await binance.DemoSymbolTickerUpdates();
            await binance.SubscribeAllTickerUpdates();

            // BITTREX exchange
            ICryptoDataVacuum bittrex = new BittrexExchange();
            await bittrex.DisplaySymbolCount();
            //await bittrex.DemoSymbolTickerUpdates();
            await bittrex.SubscribeAllTickerUpdates();

            // BITFINEX exchange
            ICryptoDataVacuum bitfinex = new BitfinexExchange();
            await bitfinex.DisplaySymbolCount();
            //await bitfinex.DemoSymbolTickerUpdates();
            await bitfinex.SubscribeAllTickerUpdates();

            int runSeconds = 600;
            Thread.Sleep(runSeconds * 1000);
            await binance.UnsubscribeAllUpdates();
            await bittrex.UnsubscribeAllUpdates();
            await bitfinex.UnsubscribeAllUpdates();

            return;
        }


        #region ========== KAFKA ========================================================
        static async Task DemoKafka(int sleepSeconds = 2)
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
        }

        // where bootstrapServers like "localhost:9092" (CSV list if more than one server)
        // where topic like "crypto-marketdata-symbols"
        // where msg like "This is a sample payload."
        static async Task DemoKafkaProducer(string bootstrapServers, string topic, string payload)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = payload });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
            Console.WriteLine();

            // Note that a server round-trip is slow (3ms at a minimum; actual latency depends
            // on many factors). In highly concurrent scenarios you will achieve high overall
            // throughput out of the producer using the above approach, but there will be a delay
            // on each await call. In stream processing applications, where you would like to process
            // many messages in rapid succession, you would typically use the Produce method instead:
            var conf = new ProducerConfig { BootstrapServers = bootstrapServers };

            Action<DeliveryReport<Null, string>> handler = r =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i = 0; i < 10; ++i)
                {
                    p.Produce(topic, new Message<Null, string> { Value = i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
            Console.WriteLine();
        }

        // where GroupId like "test-consumer-group"
        // where bootstrapServers like "localhost:9092" (CSV list if more than one server)
        // where topic like "crypto-marketdata-symbols"
        static async Task DemoKafkaConsumer(string groupId, string bootstrapServers, string topic)
        {
            // Basic Consumer example
            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
                Console.WriteLine();
            }
        }
        #endregion ======================================================================

    } // class

} // namespace


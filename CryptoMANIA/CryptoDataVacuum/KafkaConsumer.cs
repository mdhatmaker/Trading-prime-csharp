using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace CryptoDataVacuum
{
    public class KafkaConsumer
    {
        public string GroupId { get; private set; }
        public string BootstrapServers { get; private set; }
        public string Topic { get; private set; }

        IConsumer<Ignore, string> _c;

        public KafkaConsumer(string bootstrapServers, string topic, string groupId)
        {
            this.GroupId = groupId;
            this.BootstrapServers = bootstrapServers;
            this.Topic = topic;

            // Basic Consumer
            var conf = new ConsumerConfig
            {
                GroupId = this.GroupId,
                BootstrapServers = this.BootstrapServers,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _c = new ConsumerBuilder<Ignore, string>(conf).Build();
        }

        public void Start()
        {
            _c.Subscribe(Topic);

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
                        var cr = _c.Consume(cts.Token);
                        var msg = cr.Message.Value;
                        var values = msg.Split(',');
                        if (values[1] == "BINANCE")
                            Console.Write("B");
                        else if (values[1] == "BITFINEX")
                            Console.Write("F");
                        else if (values[1] == "BITTREX")
                            Console.Write("t");
                        else
                            Console.WriteLine($"\nUnknown Exchange: {values[1]}");
                        //Console.WriteLine($"Consumed message '{msg}' at: '{cr.TopicPartitionOffset}'.");
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
                _c.Close();
            }
            Console.WriteLine();
        }

        /*// where GroupId like "test-consumer-group"
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
        }*/

    } // class

} // namespace

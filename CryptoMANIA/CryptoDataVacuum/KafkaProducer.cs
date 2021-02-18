using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;

namespace CryptoDataVacuum
{
    public class KafkaProducer
    {
        public string BootstrapServers { get; private set; }
        public string Topic { get; private set; }

        IProducer<Null, string> _p;

        public KafkaProducer(string bootstrapServers, string topic)
        {
            this.BootstrapServers = bootstrapServers;
            this.Topic = topic;

            var config = new ProducerConfig { BootstrapServers = this.BootstrapServers };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            _p = new ProducerBuilder<Null, string>(config).Build();
        }

        Action<DeliveryReport<Null, string>> handler = r =>
                {
                    if (r.Error.IsError)
                        Console.WriteLine($"Delivery Error: {r.Error.Reason}");
                    /*Console.WriteLine(!r.Error.IsError
                        ? $"Delivered message to {r.TopicPartitionOffset}"
                        : $"Delivery Error: {r.Error.Reason}");*/
                };

        public void Produce(string msg)
        {
            _p.Produce(Topic, new Message<Null, string> { Value = msg }, handler);
        }

        public void Shutdown()
        {
            Console.WriteLine("Shutting down Kafka producer (may take up to 10 seconds)...");
            _p.Flush(TimeSpan.FromSeconds(10));
            _p.Dispose();
            _p = null;
            Console.WriteLine("Kafka producer closed.");
        }

        /*// where bootstrapServers like "localhost:9092" (CSV list if more than one server)
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
        }*/

    } // class

} // namespace

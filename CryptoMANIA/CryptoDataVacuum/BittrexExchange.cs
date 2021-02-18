using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using Confluent.Kafka;

namespace CryptoDataVacuum
{
    public class BittrexExchange : ICryptoDataVacuum
    {
        public string ExchName => "BITTREX";
        public string ApiKeyEnvVar => "BITTREX_KEY";

        BittrexClient exch;
        BittrexSocketClient sock;

        UpdateSubscription subscription;

        KafkaProducer _p;

        //public BittrexExchange(KafkaProducer p)
        public BittrexExchange(string bootstrapServers, string topic)
        {
            var evKeys = Environment.GetEnvironmentVariable(ApiKeyEnvVar, EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BittrexClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            this.exch = new BittrexClient(clientOptions);
            //----------
            var socketOptions = new BittrexSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            this.sock = new BittrexSocketClient(socketOptions);

            //_p = p;
            _p = new KafkaProducer(bootstrapServers, topic);
        }


        public async Task DisplaySymbolCount()
        {
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
        }

        private void ProduceToKafka(IEnumerable<BittrexStreamSymbolSummary> ticks)
        {
            //Console.WriteLine($"[{ExchName}]   {ticks.Count()} symbol ticker updates received");
            foreach (var tick in ticks)
            {
                int bidQty = 0, askQty = 0; // Bittrex Tick does not have BidQty/AskQty
                //Console.WriteLine($"{tick.TimeStamp:G} [{ExchName} {tick.Symbol}]  {tick.Last} ({tick.BaseVolume}/{tick.Volume})   B  {bidQty} : {tick.Bid}  x  {tick.Ask} x {askQty}  A");
                string msg = string.Format($"{tick.TimeStamp:G},{ExchName},{tick.Symbol},{tick.Last},{tick.BaseVolume},{tick.Volume},{bidQty},{tick.Bid},{tick.Ask},{askQty}");
                //Console.WriteLine(msg);
                _p.Produce(msg);
            }
        }

        public async Task SubscribeAllTickerUpdates()
        {
            Console.WriteLine($"  --- Starting {ExchName} SymbolTickerUpdates thread ---");
            var crSubSymbolTicker = sock.SubscribeToSymbolSummariesUpdate((ticks) =>
            {
                Task.Factory.StartNew(() => ProduceToKafka(ticks));
                /*//Console.WriteLine($"[{ExchName}]   {ticks.Count()} symbol ticker updates received");
                var tick = ticks.First();
                //Console.WriteLine($"[{ExchName} {tick.Symbol}]   {tick.BidQuantity}x{tick.BidPrice}  {tick.AskPrice}x{tick.AskQuantity}   (example 1st update)");
                int bidQty = 0, askQty = 0; // Bittrex Tick does not have BidQty/AskQty
                //Console.WriteLine($"{tick.TimeStamp:G} [{ExchName} {tick.Symbol}]  {tick.Last} ({tick.BaseVolume}/{tick.Volume})   B  {bidQty} : {tick.Bid}  x  {tick.Ask} x {askQty}  A");
                string msg = string.Format($"{tick.TimeStamp:G},{ExchName},{tick.Symbol},{tick.Last},{tick.BaseVolume},{tick.Volume},{bidQty},{tick.Bid},{tick.Ask},{askQty}");
                //Console.WriteLine(msg);
                _p.Produce(msg);*/
            });
            
            this.subscription = crSubSymbolTicker.Data;
        }

        /*public async Task UnsubscribeSymbolTickerUpdates()
        {
            return; // Bittrex sockets do not offer Unsubscribe functionality
        }*/

        public async Task UnsubscribeAllUpdates()
        {
            await sock.UnsubscribeAll();
        }

        public async Task WriteSymbolsCsv()
        {
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
            Tools.WriteObjectsToCsv(symbols, Tools.SymbolFilepath(ExchName));
        }


    } // class

} // namespace
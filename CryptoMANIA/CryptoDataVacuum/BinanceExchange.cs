using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using Confluent.Kafka;

namespace CryptoDataVacuum
{
    public class BinanceExchange : ICryptoDataVacuum
    {
        public string ExchName => "BINANCE";
        public string ApiKeyEnvVar => "BINANCE_KEY";

        BinanceClient exch;
        BinanceSocketClient sock;

        UpdateSubscription subscription;

        KafkaProducer _p;

        public BinanceExchange(KafkaProducer p)
        {
            var evKeys = Environment.GetEnvironmentVariable(ApiKeyEnvVar, EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BinanceClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            this.exch = new BinanceClient(clientOptions);
            //----------
            var socketOptions = new BinanceSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            this.sock = new BinanceSocketClient(socketOptions);

            _p = p;
        }
        

        public async Task DisplaySymbolCount()
        {
            var eiRes = await exch.Spot.System.GetExchangeInfoAsync();
            var ei = eiRes.Data;
            var symbols = ei.Symbols;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
        }

        private void ProduceToKafka(IEnumerable<IBinanceTick> ticks)
        {
            //Console.WriteLine($"[{ExchName}]   {ticks.Count()} symbol ticker updates received");
            foreach (var tick in ticks)
            {
                //tick.LastQuantity
                //Console.WriteLine($"{tick.CloseTime:G} [{ExchName} {tick.Symbol}]  {tick.LastPrice} ({tick.BaseVolume}/{tick.QuoteVolume})    B {tick.LastPrice}{tick.BidQuantity} : {tick.BidPrice}  x  {tick.AskPrice} : {tick.AskQuantity} A");
                string msg = string.Format($"{tick.CloseTime:G},{ExchName},{tick.Symbol},{tick.LastPrice},{tick.BaseVolume},{tick.QuoteVolume},{tick.LastPrice}{tick.BidQuantity},{tick.BidPrice},{tick.AskPrice},{tick.AskQuantity}");
                //Console.WriteLine(msg);
                _p.Produce(msg);
            }
        }

        public async Task SubscribeAllTickerUpdates()
        {
            Console.WriteLine($"  --- Starting {ExchName} SymbolTickerUpdates thread ---");
            var crSubSymbolTicker = sock.Spot.SubscribeToAllSymbolTickerUpdates((ticks) =>
            {
                Task.Factory.StartNew(() => ProduceToKafka(ticks));
                /*//Console.WriteLine($"[{ExchName}]   {ticks.Count()} symbol ticker updates received");
                var tick = ticks.First();
                //tick.LastQuantity
                //Console.WriteLine($"{tick.CloseTime:G} [{ExchName} {tick.Symbol}]  {tick.LastPrice} ({tick.BaseVolume}/{tick.QuoteVolume})    B {tick.LastPrice}{tick.BidQuantity} : {tick.BidPrice}  x  {tick.AskPrice} : {tick.AskQuantity} A");
                string msg = string.Format($"{tick.CloseTime:G},{ExchName},{tick.Symbol},{tick.LastPrice},{tick.BaseVolume},{tick.QuoteVolume},{tick.LastPrice}{tick.BidQuantity},{tick.BidPrice},{tick.AskPrice},{tick.AskQuantity}");
                //Console.WriteLine(msg);
                _p.Produce(msg);*/
            });

            this.subscription = crSubSymbolTicker.Data;
        }

        /*public async Task UnsubscribeSymbolTickerUpdates()
        {
            await sock.Unsubscribe(subscription);
        }*/

        public async Task UnsubscribeAllUpdates()
        {
            await sock.UnsubscribeAll();
        }

        public async Task WriteSymbolsCsv()
        {
            var eiRes = await exch.Spot.System.GetExchangeInfoAsync();
            var ei = eiRes.Data;
            var symbols = ei.Symbols;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
            Tools.WriteObjectsToCsv(symbols, Tools.SymbolFilepath(ExchName));
        }

        
    } // class

} // namespace

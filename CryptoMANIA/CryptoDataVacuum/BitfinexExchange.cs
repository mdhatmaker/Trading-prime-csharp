using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using Confluent.Kafka;

namespace CryptoDataVacuum
{
    public class BitfinexExchange : ICryptoDataVacuum
    {
        public string ExchName => "BITFINEX";
        public string ApiKeyEnvVar => "BITFINEX_KEY";

        BitfinexClient exch;
        BitfinexSocketClient sock;

        UpdateSubscription subscription;

        KafkaProducer _p;

        public BitfinexExchange(KafkaProducer p)
        {
            var evKeys = Environment.GetEnvironmentVariable(ApiKeyEnvVar, EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BitfinexClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            this.exch = new BitfinexClient(clientOptions);
            //----------
            var socketOptions = new BitfinexSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            this.sock = new BitfinexSocketClient(socketOptions);

            _p = p;
        }


        public async Task DisplaySymbolCount()
        {
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
        }

        public async Task SubscribeAllTickerUpdates()
        {
            Console.WriteLine($"  --- Starting {ExchName} SymbolTickerUpdates thread ---");
            var resSymbols = exch.GetSymbols();
            var symbols = resSymbols.Data.Take(15);  // TODO: *** FOR TESTING ONLY ***
            //var symbols = resSymbols.Data;
            int count = symbols.Count();
            int i = 0;
            foreach (var s in symbols)
            {
                ++i;
                await SubscribeSymbolTickerUpdates(s, i, count);
            }
        }

        public async Task SubscribeSymbolTickerUpdates(string rawSymbol, int i = -1, int count = -1)
        {
            string symbol = "t" + rawSymbol.ToUpper();
            Console.WriteLine($"  --- Subscribing to [{ExchName} {symbol}]    ( {i} / {count} ) ---");
            var crSubSymbolTicker = await sock.SubscribeToTickerUpdatesAsync(symbol, (tick) =>
            {
                //Console.WriteLine($"[{ExchName}]   1 symbol ticker updates received");
                int quoteVolume = 0;
                DateTime dt = DateTime.Now.ToUniversalTime();
                //Console.WriteLine($"{dt:G} [{ExchName} {symbol}]  {tick.LastPrice} ({tick.Volume}/{quoteVolume})    B {tick.BidSize} : {tick.Bid}  x  {tick.Ask} : {tick.AskSize} A");
                string msg = string.Format($"{dt:G},{ExchName},{symbol},{tick.LastPrice},{tick.Volume},{quoteVolume},{tick.BidSize},{tick.Bid},{tick.Ask},{tick.AskSize}");
                //Console.WriteLine(msg);
                _p.Produce(msg);
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
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{ExchName}]   {symbols.Count()} symbols");
            Tools.WriteStringsToCsv(symbols, Tools.SymbolFilepath(ExchName), "Symbol");
        }


    } // class

} // namespace
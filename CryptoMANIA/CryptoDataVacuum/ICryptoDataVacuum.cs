using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CryptoExchange.Net.Sockets;

namespace CryptoDataVacuum
{
    public interface ICryptoDataVacuum
    {
        Task DisplaySymbolCount();
        Task WriteSymbolsCsv();
        Task SubscribeAllTickerUpdates();
        //Task UnsubscribeTickerUpdates();
        Task UnsubscribeAllUpdates();
        Task DemoSymbolTickerUpdates(int sleepSeconds = 20);
    } // interface

} // namespace

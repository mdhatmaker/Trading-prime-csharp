using System;

namespace CryptoApis.Models
{
    public abstract class XModel
    {
        public string Exchange { get; protected set; }      // like "BINANCE", "GDAX", ...
        public string Symbol { get; protected set; }        // EXCHANGE symbol: like "BTC-XMR", "XMRBTC", ...
        public decimal Timestamp { get; protected set; }    // timestamp of retrieved data
    } // end of abstract class XModel

} // end of namespace

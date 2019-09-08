using System;

namespace CryptoTrader
{
    public class TradeSymbolRawCsvRecord
    {
		public int InitialSide { get; set; }            // -1 = SHORT, 1 = LONG, 0 = DON'T TRADE
		public string Exchange { get; set; }            // "BINANCE", "BITTREX", "YOBIT", ...
		public string GlobalSymbol { get; set; }        // "BNB-BTC", "LTC-BTC", ...
		public decimal Amount { get; set; }             // 102, 0.24, 0.1, 2020, ...
    } // end of class TradeSymbolRawCsvRecord

} // end of namespace

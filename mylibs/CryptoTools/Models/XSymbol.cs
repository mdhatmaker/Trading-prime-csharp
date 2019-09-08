using System;
using static CryptoTools.Global;

namespace CryptoTools.Models
{
    public class XSymbol
    {
        public string Exchange { get; set; }            // "BINANCE", "BITSTAMP", ...
        public string SymbolId { get; set; }            // standardized symbol id (ex: "btcusd"
        public string Symbol { get; set; }              // exchange-specific symbol for this symbol id (ex: "USD-BTC"
        public decimal MinPrice { get; set; }           // minimum allowed price
        public decimal MaxPrice { get; set; }           // maximum allowed price
        public decimal TickSize { get; set; }           // minimum change in price (ex: 0.00000100)
        public decimal MinQuantity { get; set; }        // minimum allowed quantity
        public decimal MaxQuantity { get; set; }        // maximum allowed quantity
        public decimal StepSize { get; set; }           // minimum change in quantity (ex: 0.01000000)
        public decimal MinNotional { get; set; }        // minimum notional value of a trade


        public XSymbol()
        {
        }

        public XSymbol(string exchange, string symbol)
        {
            Exchange = exchange;
            Symbol = symbol;
        }

        public bool CheckPriceQty(decimal price, decimal qty, out decimal updatedPrice, out decimal updatedQty)
        {
            bool changed = false;
            // Start by assuming there will be no change to price or qty
            updatedPrice = price;
            updatedQty = qty;
            // Now check to see if the price and qty are valid (based on TickSize and StepSize)
            if (!IsInteger(price / TickSize))
            {
                updatedPrice = Math.Round(price / TickSize) * TickSize;
                changed = true;
            }
            if (!IsInteger(qty / StepSize))
            {
                updatedQty = Math.Round(qty / StepSize) * StepSize;
                changed = true;
            }
            return changed;
        }

        public override string ToString()
        {
            return string.Format("[{0,-8} {1,6}]", Exchange, Symbol);
        }


    } // end of class XSymbol
} // end of namespace

using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis.SharedModels
{
    public class OrderBookEntry
    {
        public string Exchange { get; private set; }
        public decimal Price { get; private set; }
        public decimal Amount { get; private set; }

        public OrderBookEntry(string exchange, decimal price, decimal amount)
        {
            Exchange = exchange;
            Price = price;
            Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("{0,-9}  price:{1:0.00######}  amount:{2:0.0#######}", Exchange, Price, Amount);
        }
    } // end of class OrderBookEntry

} // end of namespace

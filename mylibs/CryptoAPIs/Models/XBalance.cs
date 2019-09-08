using System;

namespace CryptoApis.Models
{
    public class XBalance
    {
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Available { get; private set; }

        public XBalance(string currency, decimal amount, decimal available)
        {
            this.Currency = currency;
            this.Amount = amount;
            this.Available = available;
        }

        public bool IsZero { get { return Amount == 0.0M && Available == 0.0M; } }

        public override string ToString()
        {
            return string.Format("{0,-4}  {1,14:0.00000000}  {2,14:0.00000000}", Currency, Amount, Available);
        }
    } // end of class XBalance

} // end of namespace

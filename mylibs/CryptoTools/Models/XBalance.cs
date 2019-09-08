using System;

namespace CryptoTools.Models
{
    public class XBalance
    {
        public string Asset { get; set; }
        public decimal Free { get; set; }
        public decimal Locked { get; set; }
        public decimal Total { get; set; }

        public XBalance(string asset, decimal free, decimal locked, decimal total)
        {
            Asset = asset;
            Free = free;
            Locked = locked;
            Total = total;
        }

    } // end of class XBalance
} // end of namespace

using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis.SharedModels
{
    public abstract class AccountBalance
    {
        public abstract string Asset { get; }
        public abstract decimal Free { get; }
        public abstract decimal Locked { get; }
        public decimal Total { get { return Free + Locked; } }

        public override string ToString() { return string.Format("{0,-4}    free:{1,13}    locked:{2,13}    total:{3,13}", Asset, Free, Locked, Total); }
    } // end of class AccountBalance

} // end of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CryptoAPIs
{

    public class BalanceUpdateArgs : EventArgs
    {
        IEnumerable<ZAccountBalance> m_balances;
        IEnumerable<ZOrder> m_openOrders;

        public BalanceUpdateArgs(IEnumerable<ZAccountBalance> balances, IEnumerable<ZOrder> openOrders)
        {
            m_balances = balances;
            m_openOrders = openOrders;
        }
    }

    public class ZAccountBalance
    {
        public string Asset;
        public decimal Free;
        public decimal Locked;

        public ZAccountBalance(string asset, decimal free, decimal locked)
        {
            Asset = asset;
            Free = free;
            Locked = locked;
        }

        public ZAccountBalance(string asset, decimal balance)
        {
            Asset = asset;
            Free = balance;
            //Locked = 0;
        }
    } // end of class ZAccountBalance


    /*public class ZAccountBalance
    {
        public string Currency { get { return m_currency; } }
        public decimal Available { get { return m_available; } }
        public decimal Balance { get { return m_balance; } }
        public decimal Reserved { get { return m_reserved; } }

        private string m_currency;
        private decimal m_available, m_balance, m_reserved;

        public ZAccountBalance(string currency, decimal available, decimal balance, decimal reserved)
        {
            m_currency = currency;
            m_available = available;
            m_balance = balance;
            m_reserved = reserved;
        }

        public ZAccountBalance(string currency, string available, string balance, string reserved)
        {
            m_currency = currency;
            m_available = decimal.Parse(available ?? "0.0");
            m_balance = decimal.Parse(balance ?? "0.0");
            m_reserved = decimal.Parse(reserved ?? "0.0");
        }

        public ZAccountBalance(string currency, decimal available, decimal balance)
        {
            m_currency = currency;
            m_available = available;
            m_balance = balance;
            m_reserved = balance - available;
        }

    } // end of class ZAccountBalance*/





} // end of namespace

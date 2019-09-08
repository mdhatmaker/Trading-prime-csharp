using System;
using System.Linq;
using System.Collections.Generic;

namespace CryptoApis.Models
{
    public class XBalanceMap
    {
        public IEnumerable<string> Keys { get { return m_balances.Keys; } }
        public XBalance this[string currency] { get { return m_balances[currency]; } }

        private Dictionary<string, XBalance> m_balances = new Dictionary<string, XBalance>();

        // Kraken
        public XBalanceMap(Dictionary<string, decimal> balances)
        {
            foreach (var kv in balances)
            {
                m_balances[kv.Key.ToLower()] = new XBalance(kv.Key, kv.Value, kv.Value);
            }
        }

        // GDAX
        public XBalanceMap(IEnumerable<GDAXSharp.Services.Accounts.Models.Account> accounts)
        {
            foreach (var acct in accounts)
            {
                string currency = acct.Currency.ToString();
                m_balances[currency.ToLower()] = new XBalance(currency, acct.Balance, acct.Available);
            }
        }

        // Bitfinex
        public XBalanceMap(Bitfinex.Net.Objects.BitfinexWallet[] wallets)
        {
            foreach (var w in wallets)
            {
                m_balances[w.Currency.ToLower()] = new XBalance(w.Currency, w.Balance, w.BalanceAvailable ?? 0.0M);
            }
        }

        // Binance
        public XBalanceMap(Binance.Net.Objects.BinanceAccountInfo ai)
        {
            foreach (var b in ai.Balances)
            {
                m_balances[b.Asset.ToLower()] = new XBalance(b.Asset, b.Total, b.Free);
            }
        }

        // Bittrex
        public XBalanceMap(Bittrex.Net.Objects.BittrexBalance[] balances)
        {
            foreach (var b in balances)
            {
                m_balances[b.Currency.ToLower()] = new XBalance(b.Currency, b.Balance ?? 0.0M, b.Available ?? 0.0M);
                // b.CryptoAddress for wallet address
            }
        }

        // Poloniex
        public XBalanceMap(IList<Poloniex.GetBalancesQuery.AvailableBalance> balances)
        {
            foreach (var b in balances)
            {
                // TODO: Get BOTH available and total balance
                m_balances[b.Symbol.ToLower()] = new XBalance(b.Symbol, b.Available, b.Available);
            }
        }


        public void PrintNonZero(string title = "")
        {
            Console.WriteLine("\n--- {0} ---", title);
            var keys = this.Keys.ToList();
            keys.Sort();
            foreach (var currency in keys)
            {
                var b = this[currency];
                if (b.Amount != 0 || b.Available != 0)
                    Console.WriteLine(b.ToString());
            }
        }

    } // end of class XBalanceMap

} // end of namespace

using System;
using System.Linq;
using System.Collections.Generic;

namespace CryptoAPIs
{
    public static class CryptoExtensionMethods
    {
        #region -------------------------------- KRAKEN -----------------------------------------------------------------------------------
        public static void Display(this Dictionary<string, CryptoAPIs.Exchange.Clients.Kraken.OrderInfo> oi, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            foreach (var s in oi.Keys)
            {
                System.Console.WriteLine("{0} {1}", s, oi[s].ToString());
            }
            System.Console.WriteLine();
        }

        public static void Display(this Dictionary<string, CryptoAPIs.Exchange.Clients.Kraken.TradeInfo> ti, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            foreach (var s in ti.Keys)
            {
                System.Console.WriteLine("{0} {1}", s, ti[s].ToString());
            }
            System.Console.WriteLine();
        }

        public static void Display(this Dictionary<string, CryptoAPIs.Exchange.Clients.Kraken.PositionInfo> pi, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            foreach (var s in pi.Keys)
            {
                System.Console.WriteLine("{0} {1}", s, pi[s].ToString());
            }
            System.Console.WriteLine();
        }

        public static void Display(this Dictionary<string, CryptoAPIs.Exchange.Clients.Kraken.Ticker> t, string title = null)
        {
            // If there is a single Ticker, display it on the same line as the title
            if (t.Count < 2)
            {
                System.Console.WriteLine("{0} {1} {2}", title ?? "", t.First().Key, t.First().Value.ToString());
            }
            else    // otherwise display the list as normal
            {
                if (title != null) System.Console.WriteLine(title);
                foreach (var s in t.Keys)
                {
                    System.Console.WriteLine("{0} {1}", s, t[s].ToString());
                }
            }
            System.Console.WriteLine();
        }

        public static void Display(this CryptoAPIs.Exchange.Clients.Bittrex.AccountBalance[] balances, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            foreach (var bal in balances)
            {
                System.Console.WriteLine(bal.ToString());
            }
            System.Console.WriteLine();
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------


        #region -------------------------------- BITTREX ----------------------------------------------------------------------------------
        public static void Display(this CryptoAPIs.Exchange.Clients.Bittrex.OpenOrder[] orders, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            foreach (var o in orders)
            {
                System.Console.WriteLine("{0}", o.ToString());
            }
            System.Console.WriteLine();
        }

        public static void Display(this CryptoAPIs.Exchange.Clients.Bittrex.Ticker t, string title = null)
        {
            if (title != null) System.Console.WriteLine(title);
            System.Console.WriteLine("{0}", t.ToString());
            System.Console.WriteLine();
        }

        #endregion ------------------------------------------------------------------------------------------------------------------------

    } // end of class CryptoExtensionMethods
} // end of namespace

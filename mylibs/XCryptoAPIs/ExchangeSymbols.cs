using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CryptoAPIs
{
    public static class ExchangeSymbols
    {

        public static ExchangeSymbolsMap GetExchangeSymbols(BaseExchange[] exchanges)
        {
            var results = new ExchangeSymbolsMap();
            foreach (var exch in exchanges)
            {
                results[exch.Name] = exch.SymbolList;
            }
            return results;
        }

        public static ExchangeSymbolsMap GetExchangeSymbolsContain(BaseExchange[] exchanges, string[] searchForText)
        {
            var results = new ExchangeSymbolsMap();
            foreach (var exch in exchanges)
            {
                results[exch.Name] = SymbolsContain(exch.SymbolList, searchForText);
            }
            return results;
        }

        public static ExchangeSymbolsMap GetExchangeSymbolsStartWith(BaseExchange[] exchanges, string[] searchForText)
        {
            var results = new ExchangeSymbolsMap();
            foreach (var exch in exchanges)
            {
                results[exch.Name] = SymbolsStartWith(exch.SymbolList, searchForText);
            }
            return results;
        }

        // This method can be called from the main program to search for symbols (good to use from the command line)
        public static void DisplaySymbols(string symbol = null)
        {
            /*var currencySymbols = new string[] { "BCH", "BTC", "BTG", "DCR", "ETC", "ETH", "LTC", "NEO", "XMR", "ZEC" };
            var es = GetExchangeSymbolsStartWith(ExchangeList, currencySymbols);
            es.WriteToFile(Folders.system_path("OneChain.MAIN.Exchange.Symbols.txt"));*/
            //var exchsymbols = ExchangeSymbols.ReadFromFile(Folders.system_path("OneChain.MAIN.Exchange.Symbols.txt"));
            var exchsymbols = ExchangeSymbolsMap.ReadFromFile(Folders.exe_path("Complete.Exchange.Symbols.txt"));
            Console.WriteLine();
            if (symbol == null)
            {
                exchsymbols.Display();          // display all symbols
                exchsymbols.DisplaySummary("ALL EXCHANGE SYMBOL SUMMARY:");
            }
            else
                exchsymbols.Display(symbol);    // display only symbols containing specified text

            /*var es = GetExchangeSymbols(CompleteExchangeList);
            es.Display();
            es.DisplaySummary();*/
        }


        // Where currencySymbols like "BTC", "ETH", "NEO", "USD", "KRW" ...
        public static List<string> SymbolsContain(List<string> symbolList, params string[] currencySymbols)
        {
            var result = new SortedSet<string>();
            // Default to list of standard cryptocurrencies
            if (currencySymbols.Length == 0)
                currencySymbols = new string[] { "BCH", "BTC", "BTG", "DCR", "ETC", "ETH", "LTC", "NEO", "XMR", "ZEC" };
            if (symbolList != null)
            {
                foreach (var currency in currencySymbols)
                {
                    var match = symbolList.Where(s => s.ToUpper().Contains(currency.ToUpper())).ToList();
                    result.UnionWith(match);
                }
            }
            return result.ToList();
        }

        // Where currencySymbols like "BTC", "ETH", "NEO", "USD", "KRW" ...
        public static List<string> SymbolsStartWith(List<string> symbolList, params string[] currencySymbols)
        {
            var result = new SortedSet<string>();
            // Default to list of standard cryptocurrencies
            if (currencySymbols.Length == 0)
                currencySymbols = new string[] { "BCH", "BTC", "BTG", "DCR", "ETC", "ETH", "LTC", "NEO", "XMR", "ZEC" };
            if (symbolList != null)
            {
                foreach (var currency in currencySymbols)
                {
                    var match = symbolList.Where(s => s.ToUpper().StartsWith(currency.ToUpper())).ToList();
                    result.UnionWith(match);
                }
            }
            return result.ToList();
        }


    } // end of class ExchangeSymbols

} // end of namespace

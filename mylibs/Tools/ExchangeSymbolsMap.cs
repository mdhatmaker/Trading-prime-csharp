using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tools
{
    // Class that implements a dictionary of symbol lists mapped to exchange name
    public class ExchangeSymbolsMap : Dictionary<string, List<string>>
    {
        public void Display()
        {
            foreach (var exch in this.Keys)
            {
                System.Console.WriteLine("---{0} symbols:", exch);
                System.Console.WriteLine(string.Join(",", this[exch]));
                System.Console.WriteLine();
            }
        }

        public void Display(string searchForText)
        {
            foreach (var exch in this.Keys)
            {
                var filtered = this[exch].Where(s => s.ToUpper().Contains(searchForText.ToUpper()));
                if (filtered.Count() > 0)
                {
                    System.Console.WriteLine("---{0} symbols:", exch);
                    System.Console.WriteLine(string.Join(", ", filtered));
                    System.Console.WriteLine();
                }                    
            }
        }

        public void DisplaySummary(string title=null)
        {
            if (title != null) Console.WriteLine(title);
            int i = 0;
            foreach (var exch in this.Keys)
            {
                System.Console.WriteLine("{0}: {1} ({2} symbols)", ++i, exch, this[exch].Count);
            }
        }

        public void WriteToFile(string pathname)
        {
            using (var writer = new StreamWriter(pathname))
            {
                foreach (var exch in this.Keys)
                {
                    writer.WriteLine("{0},{1}", exch, string.Join(",", this[exch]));
                }
            }
        }

        public static ExchangeSymbolsMap ReadFromFile(string pathname)
        {
            var results = new ExchangeSymbolsMap();
            var lines = GFile.ReadTextFileLines(pathname);
            foreach (var line in lines)
            {
                var symbols = line.Split(',').ToList();
                string exch = symbols[0];                   // exchange name is first element
                symbols.RemoveAt(0);                        // exchange symbols are all the remaining elements
                results[exch] = symbols;
            }
            return results;
        }
    } // end of class ExchangeSymbols

} // end of namespace

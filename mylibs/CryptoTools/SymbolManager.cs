using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static CryptoTools.Global;
using CryptoTools.Models;
using CryptoTools.CryptoFile;

namespace CryptoTools
{
    public class SymbolManager
    {
        // m_symbols[exchange][symbolId] like m_symbols["BINANCE"]["btcusdt"]
        private SortedDictionary<string, SortedDictionary<string, XSymbol>> m_symbols = new SortedDictionary<string, SortedDictionary<string, XSymbol>>();

        public SymbolManager()
        {
            ReadSymbolFile();
        }

        private void ReadSymbolFile()
        {
            string folder = GFile.ExePath;
            string filename = "system.symbol_ids.DF.csv";
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(Path.Combine(folder, filename));
                var csv = new CsvHelper.CsvReader(reader);
                csv.Read();
                csv.ReadHeader();
                //csv.ValidateHeader<XSymbol>();
                while (csv.Read())
                {
                    var entry = csv.GetRecord<XSymbol>();
                    if (!m_symbols.ContainsKey(entry.Exchange))
                        m_symbols[entry.Exchange] = new SortedDictionary<string, XSymbol>();
                    m_symbols[entry.Exchange][entry.SymbolId] = entry;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SymbolManager ERROR: {0}", ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public string GetSymbol(string exchange, string symbolId)
        {
            if (m_symbols.ContainsKey(exchange) && m_symbols[exchange].ContainsKey(symbolId))
                return m_symbols[exchange][symbolId].Symbol;
            else
                return null;
        }

        public XSymbol GetXSymbol(string exchange, string symbolId)
        {
            if (m_symbols.ContainsKey(exchange) && m_symbols[exchange].ContainsKey(symbolId))
                return m_symbols[exchange][symbolId];
            else
                return null;
        }


    } // end of class SymbolManager


    /*public class SymbolEntry
    {
        public string Exchange { get; set; }
        public string SymbolId { get; set; }
        public string Symbol { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal TickSize { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public decimal StepSize { get; set; }
        public decimal MinNotional { get; set; }
    }*/

} // end of namespace

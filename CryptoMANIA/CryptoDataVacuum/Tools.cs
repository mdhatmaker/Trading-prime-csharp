using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;

namespace CryptoDataVacuum
{
    public class Tools
    {
        static string RootFolder = "C:\\cryptoZ\\";

        #region ========== FILE HELPER METHODS ==========================================
        public static void WriteObjectsToCsv<T>(IEnumerable<T> data, string filepath, string singleColumnHeader = null)
        {
            using (var writer = new StreamWriter(filepath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }

        public static void WriteStringsToCsv(IEnumerable<string> data, string filepath, string singleColumnHeader)
        {
            using (var writer = new StreamWriter(filepath))
            {
                writer.WriteLine(singleColumnHeader);
                data.ToList().ForEach(s => writer.WriteLine(s));
            }
        }

        // Given an exchange name
        // Return the full filepath of the .csv symbols file
        public static string SymbolFilepath(string exchName)
        {
            var filepath = Path.Join(RootFolder, "symbols", $"symbols.{exchName}.csv");
            return filepath;
        }
        #endregion ======================================================================


    } // class

} // namespace

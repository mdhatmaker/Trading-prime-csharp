using System;
using System.IO;

namespace CryptoAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO CRYPTO ANALYSIS ***\n");

			var analyzer = new Analyzer();
			//var dir = @"D:\Users\mhatmaker\Dropbox\dev\csharp\CryptoCollector\bin\Debug\netcoreapp2.0";
			//var datafile = Path.Combine(dir, "tickers_2018-05-06_013133.DF.csv");
			//analyzer.CrossExchangeSpreadAnalysis(datafile);
			analyzer.AnalyzerTest();

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

    } // end of class Program

} // end of namespace

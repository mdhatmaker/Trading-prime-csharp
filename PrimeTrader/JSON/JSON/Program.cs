using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Tools;
using static Tools.G;
using static Tools.DataFrame;
using CryptoAPIs;

namespace JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cryptomania!!!");

            Crypto.Test();
            return;

            /*
            //Gemini.Instance.StartWebSocket();
            ////Gemini.Instance.SubscribeWebSocket("");

            GDAX.Instance.StartWebSocket();
            GDAX.Instance.SubscribeWebSocket("productIds");

            Console.ReadLine();
            */


            //var book1 = Bitfinex.Instance.GetOrderBook("btcusd");
            //cout(book1.ToString());

            /*cout("Exchange,Symbol,Bid,Ask,Last,High,Low,Volume,Timestamp\n");
            foreach (var exch in Crypto.ExchangeList)
            {
                if (exch.ExchangeName == "BITSQUARE")
                    continue;
                
                cout(exch.ExchangeName);

                try
                {
                    var symbols = exch.GetSymbolList();
                    cout(symbols[0]);
                    CryptoOrderBook book = exch.GetOrderBook(symbols[0]);
                    cout(book.ToString());
                }
                catch (Exception ex)
                {
                    cout("ERROR:" + ex.Message);
                }
                finally
                {
                    cout("\n\n");
                }*/

                /*try
                {
                    var tickers = exch.GetTickers();
                    foreach (var t in tickers.Keys)
                    {
                        cout("{0},{1},{2}", exch.ExchangeName, t, tickers[t]); 
                    }
                    //cout(tickers);
                }
                catch (Exception ex)
                {
                    cout("ERROR:" + ex.Message);
                }
                finally
                {
                    cout("\n\n");
                }
            }*/



            /*var fees = Crypto.BitcoinFees.BitcoinFees.GetTransactionFeesSummary();
            cout(fees);

            var fees2 = Crypto.BitcoinFees.BitcoinFees.GetRecommendedTransactionFees();
            cout(fees2);*/



            /*GDAX.StartWebSocket();
            GDAX.SubscribeWebSocket("\"btc_usd\"");*/

            //Console.WriteLine("Press any key to quit");
            //Console.ReadKey();

            /*
            IQFeed.IQFeedAdapter adapter = new IQFeed.IQFeedAdapter();
            adapter.Connect("IQFEED_DEMO", "1.0", "1.0");
            adapter.MessageReceived += new IQFeed.MessageReceivedDelegate(adapter_MessageReceived);
            adapter.QuoteReceived += new IQFeed.QuoteReceivedDelegate(adapter_QuoteReceived);

            adapter.MonitorSymbol("MSFT");

            Console.WriteLine("Press any key to disconnect");
            Console.ReadKey();

            adapter.Disconnect();

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        

            //var sticks = OkCoin.GetCandlestickData("btc_cny", "1min");
            //cout(x);

            GSystem.PingTest(Bithumb.BaseUrl);
            GSystem.PingTest(Bitfinex.BaseUrl);
            GSystem.PingTest(Bitsquare.BaseUrl);

            var symbols = Bitfinex.GetSymbols();
            //cout(symbols);

            foreach (var s in symbols)
            {
                cout(s);
                //cout(Bitfinex.GetTicker(s));
                cout("\n");
            }

            var pairs = BTER.GetSymbolList();
            cout(pairs);

            foreach (var p in pairs)
            {
                cout(p);
                cout(BTER.GetBTERTicker(p));
                cout('\n');
            }


            var meta = MyQuandl.Q.GetMetaDataTimeSeriesDatabase("BITFINEX");
            cout(meta.Database.DatasetsCount);


            // TODO: THIS IS CURRENTLY FUCKED
            //string filename = MyQuandl.Q.GetEntireTimeSeriesDatabase("BITFINEX", "zippy.zip");
            var files = DataFrame.GetDataFrameFileList(Folders.DropboxFolders.df_folder);
            cout(files);

            var datasetCodeFiles = MyQuandl.GetQuandlDatasetCodes(Folders.DropboxFolders.quandl_folder);
            cout(datasetCodeFiles);

            CryptoStats.MarketCapAnalysis();

            BraveNewCoin.Test();

            BitcoinWatch.Test();

            return;

            Gemini.Test();
            //string url = @"https://api.stackexchange.com/2.2/answers?order=desc&sort=activity&site=stackoverflow";
            //string baseUrl = @"https://api.stackexchange.com/2.2/";
            //string command = @"answers?order=desc&sort=activity&site=stackoverflow";
            //baseUrl = @"https://api.gdax.com/";
            //command = "products";
            //baseUrl = @"https://api.gemini.com/v1/";
            //command = "symbols";
            //Tools.GetJSON(baseUrl, command);


            //var symbols = Tools.DownloadAndDeserializeJsonData<List<string>>(@"https://api.gemini.com/v1/symbols");

            //string str = Tools.GET(url);
            //GDAX.Test();

            var df = ReadDataFrame(@"/Users/michael/Dropbox/alvin/data/SYSTEM/crypto_symbols.DF.csv");
            df.print();

            cout(df["btc_usd", "Gemini"]);

            //CryptoCoinCharts.Test();

            //Poloniex.Poloniex.Test();
            //Poloniex.Client.Test();

            //Cex.Test();

            Console.WriteLine("\n");
            cout(CryptoAPIs.Bitfinex.Bitfinex.GetSymbols());

            //var x = CryptoAPIs.Bitfinex.Base.
            var t = CryptoAPIs.Bitfinex.Bitfinex.GetOrderBook("tBTCUSD");
            cout(t.ToString());


            var v = CryptoAPIs.ItBit.ItBit.GetOrderBook("XBTUSD");
            cout(v.ToString());
            //Crypto.ItBit.ItBit.Test();
            */

        }

        /*static void WriteCryptoSymbolsToFile(string filename = "crypto_symbols.txt")
        {
            List<string> output = new List<string>();
            foreach (ICryptoExchange exch in Crypto.ExchangeList)
            {
                cout(exch.ExchangeName);
                var symbols = exch.GetSymbolList();
                foreach (var s in symbols)
                {
                    output.Add(string.Format("{0},{1}", exch.ExchangeName, s));
                }
            }

            string pathname = Path.Combine(Folders.DropboxFolders.misc_folder, filename);
            cout(pathname);
            //string filename = @"C:\Users\Public\TestFolder\WriteLines2.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathname))
            {
                foreach (string line in output)
                {
                    file.WriteLine(line);
                }
            }
        }*/

        static void adapter_QuoteReceived(IQFeed.IQFeedPriceQuote quote)
        {
            Console.WriteLine("{0} {1} {2}", quote.LastTradeTime, quote.Symbol, quote.Last);
        }

        static void adapter_MessageReceived(string message)
        {
            Console.WriteLine(message);
        }




    } // end of class
} // end of namespace

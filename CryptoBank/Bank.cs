using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ExchangeSharp;
using CryptoTools;
using CryptoTools.MathStat;
using CryptoTools.Messaging;
using System.Threading.Tasks;
using System.Threading;
using CryptoApis.RestApi;
using CryptoTools.Cryptography;
using CryptoTools.CryptoFile;
using CryptoApis;

namespace CryptoBank
{
    public class Bank
    {
		private ExchangeSharpApi m_api;

        private StreamWriter m_fout;

		private decimal m_usd;
        private decimal m_btc;

        public Bank()
        {
			m_api = new ExchangeSharpApi();
			m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);            
        }
        

        public void Test()
		{
            //var or = ApiHelper.GdaxJoinInside(m_api.gdax, "BTC-USD", OrderSide.Buy, 0.01M);   // BUY
            //var or = ApiHelper.GdaxJoinInside(m_api.gdax, "BTC-USD", OrderSide.Sell, 0.03M);  // SELL
            //Console.WriteLine(or.ToStr());
            
            //ApiHelper.GdaxIcebergJoinInside(m_api.gdax, "BTC-USD", OrderSide.Buy, 0.1M, 10);   // BUY
            ApiHelper.GdaxIcebergJoinInside(m_api.gdax, "BTC-USD", OrderSide.Buy, 0.2M, 20, new TimeSpan(0, 1, 0));   // BUY


            //Dictionary<string, decimal> amounts;

            //TestSms();

            //StartOrderWebsockets();
            //Thread.Sleep(2000);

            //TestGdaxOrder();
        }

        // Zero out all global balances (used for calculating balance totals)
        private void ZeroBalances()
        {
            m_usd = 0.00M;
            m_btc = 0.00M;
        }

        // Print currency balances for a subset of the PRIMARY exchanges
        public void PrintPrimaryBalances(bool emailStatement = false)
        {
            var filename = $"statement_{DateTime.Now.ToString("yyyy-MM-dd")}.DF.csv";
            var pathname = Path.Combine(Folder.crypto_folder, filename);

            using (m_fout = new StreamWriter(pathname))
            {
                m_fout.WriteLine("DateTime,Exchange,Currency,Amount,ValueBTC,ValueUSD");

                ZeroBalances();

                PrintAmounts("BINANCE");
                PrintAmounts("BITTREX");
                PrintAmounts("GDAX");
                PrintAmounts("KRAKEN");
                PrintAmounts("POLONIEX");

                PrintBalanceSummary();
            }

            if (emailStatement) SendEmails(pathname);       // (optionally) email statement to recipient(s) in each line of "statement_emails.txt" file
        }

        private void SendEmails(string pathname)
        {
            try
            {
                Console.WriteLine("\nEmailing daily statement...");
                var gmailAddress = m_api.Credentials["GMAIL"].Key;
                var gmailPassword = m_api.Credentials["GMAIL"].Secret;
                var gpub = new GmailPub(gmailAddress, gmailPassword, "Michael Hatmaker");
                var subject = DateTime.Now.ToString("ddd, dd MMM yyyy") + " Crypto Statement";
                var msg = "See attached statement (text file).";
                var attachments = new string[] { pathname };
                var fin = new InputFile<EmailRawCsvRecord>("statement_emails.txt", null, false);
                //string line;
                //while ((line = fin.ReadLine()) != null)
                var emailAddresses = fin.ReadAll();
                foreach (var raw in emailAddresses)
                {
                    //if (string.IsNullOrWhiteSpace(line)) continue;
                    //Console.WriteLine("   to {0}", line.Trim());
                    //gpub.Send(line.Trim(), subject, msg, attachments);
                    Console.WriteLine("   to {0}", raw.EmailAddress.Trim());
                    gpub.Send(raw.EmailAddress.Trim(), subject, msg, attachments);
                }
                Console.WriteLine("Done.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to email statement: {0}", ex.Message);
            }
        }

        // Print currency balances for ALL exchanges
        public void PrintAllBalances()
		{
            ZeroBalances();

			foreach (var exch in m_api.ExchangeIds)
			{
				//if (exch == "BITSTAMP") continue;   // TODO: Fix the "Customer ID" error with Bitstamp API
				PrintAmounts(exch);
			}
            PrintBalanceSummary();
		}

        // Print currency balances for a single exchange
        public void PrintBalance(string exchange)
        {
            ZeroBalances();
            PrintAmounts(exchange);
            PrintBalanceSummary();
        }

        // Get current BTC price on GDAX
        public decimal GetGdaxBtcPrice()
        {
            var t = m_api.gdax.GetTicker("BTC-USD");
            return t.MidPrice();
        }

        // Print end-of-statement summary of USD/BTC
        private void PrintBalanceSummary()
        {
            var btcCurrentPrice = GetGdaxBtcPrice();
            var btcUsdValue = btcCurrentPrice * m_btc;
            Console.WriteLine("BTC     : B{0:#,##0.00000000}    (${1:#,##0.00} with BTC={2:0.00000000})", m_btc, btcUsdValue, btcCurrentPrice);
            Console.WriteLine("USD/USDT: ${0:#,##0.00}", m_usd);
            Console.WriteLine();
            Console.WriteLine("TOTAL   : ${0:#,##0.00}", m_usd + btcUsdValue);

            fout("SUBTOTAL", "BTC", m_btc, 0, btcUsdValue);
            fout("SUBTOTAL", "USD/USDT", Math.Round(m_usd, 2));
            fout("TOTAL", "USD", Math.Round(m_usd + btcUsdValue, 2));
        }

        private void PrintAmounts(string exchange)
		{
			Console.WriteLine("---{0}---", exchange);

            try
            {
                var tickers = m_api[exchange].GetTickers();
                var amounts = m_api[exchange].GetAmounts();
                foreach (var kv in amounts)
                {
                    var currency = kv.Key;
                    var amount = kv.Value;
                    if (currency == "USD" || currency == "USDT")
                    {
                        Console.WriteLine("{0,-8} {1,-6} {2,13:0.00000000}", exchange, currency, amount);
                        fout(exchange, currency, amount);
                        m_usd += amount;
                    }
                    else if (currency == "BTC")
                    {
                        Console.WriteLine("{0,-8} {1,-6} {2,13:0.00000000}", exchange, currency, amount);
                        fout(exchange, currency, amount);
                        m_btc += amount;
                    }
                    else
                    {
                        // Get currency value in BTC
                        //var globalSymbol = currency + "-BTC";
                        var globalSymbol = "BTC-" + currency;
                        var symbol = m_api[exchange].GlobalSymbolToExchangeSymbol(globalSymbol);
                        //var t = m_api[exchange].GetTicker(symbol);
                        var t = tickers.Where(x => x.Key == symbol).First().Value;
                        var btcAmount = t.MidPrice() * amount;
                        Console.WriteLine("{0,-8} {1,-6} {2,13:0.00000000}        btc:{3,13:0.00000000}", exchange, currency, amount, btcAmount);
                        fout(exchange, currency, amount, btcAmount);
                        m_btc += btcAmount;
                    }
                    /*else
                    {
                        try
                        {
                            var address = m_apiMap[exchange].GetDepositAddress(currency);
                            Console.WriteLine("[{0} {1}] {2}", exchange, currency, address);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0}", ex.Message);
                        }
                    }*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}\n", exchange, ex.Message);
            }
            Console.WriteLine();
		}

        // If an outupt file is open, write to it
        private void fout(string exchange, string symbol, decimal amount, decimal btcAmount = 0, decimal usdAmount = 0)
        {
            if (m_fout == null) return;

            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var btcAmountStr = (btcAmount == 0 ? "" : string.Format("{0:0.00000000}", btcAmount));
            var usdAmountStr = (usdAmount == 0 ? "" : string.Format("{0:0.00}", usdAmount));
            m_fout.WriteLine("{0},{1},{2},{3:0.00000000},{4},{5:0.00}", dt, exchange, symbol, amount, btcAmountStr, usdAmountStr);
        }


    } // end of class Balances
} // end of namespace

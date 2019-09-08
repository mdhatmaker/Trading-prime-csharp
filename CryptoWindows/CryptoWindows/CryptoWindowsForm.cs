using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoApis;
using CryptoTools;
using ExchangeSharp;

namespace CryptoWindows
{
    public partial class CryptoWindowsForm : Form
    {
        private ExchangeSharpApi m_api;
        private DataView m_dvBal;
        private DataView m_dvOrd;

        public CryptoWindowsForm()
        {
            InitializeComponent();
            m_api = new ExchangeSharpApi(ExchangeSet.All);
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
        }

        private void CryptoWindowsForm_Load(object sender, EventArgs e)
        {
            //StartBinanceWebsockets(); return;            

            CreateBalancesColumns();
            UpdateBalances("BINANCE");

            CreateOrdersColumns();
            UpdateOrders("BINANCE");
        }


        private IDisposable StartBinanceWebsockets(bool display = true)
        {
            IExchangeAPI a = new ExchangeBinanceAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BINANCE  {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //HandleTickerUpdate(a, tickers);
                //m_outputQ.Enqueue(new TickerOutput("BINANCE", tickers));
            });
            /*var osocket = a.GetCompletedOrderDetailsWebSocket((order) =>
            {
                if (display) Console.WriteLine("BINANCE  {0,4} order details, eor: {1}", 1, order);
                //HandleTickerUpdate(a, tickers);
                //m_outputQ.Enqueue(new TickerOutput("BINANCE", tickers));
            });*/
            var tsocket = a.GetOrderBookWebSocket("BTCUSDT", (book) =>
            {
                if (display) Console.WriteLine("BINANCE  {0,4} {1,4} order book, bid: {2}  ask: {3}", book.Data.Bids.Count, book.Data.Asks.Count, book.Data.Bids[0], book.Data.Asks[0]);
            });   
            return socket;
        }

        private void CreateBalancesColumns()
        {
            // Create a DataView using the DataTable
            DataTable myTable = new DataTable("Balances");
            // Create and populate columns
            myTable.Columns.Add("Currency", typeof(string));
            myTable.Columns.Add("Amount", typeof(decimal));
            myTable.Columns.Add("BTC", typeof(decimal));

            m_dvBal = new DataView(myTable);
            BindToDataView(gridBalances, m_dvBal);
        }

        private void CreateOrdersColumns()
        {
            // Create a DataView using the DataTable
            DataTable myTable = new DataTable("Orders");
            // Create and populate columns
            //myTable.Columns.Add("#", typeof(int));
            myTable.Columns.Add("Timestamp", typeof(string));
            myTable.Columns.Add("Symbol", typeof(string));
            myTable.Columns.Add("Side", typeof(string));
            myTable.Columns.Add("Price", typeof(decimal));
            myTable.Columns.Add("Amount", typeof(decimal));
            myTable.Columns.Add("Filled", typeof(decimal));
            myTable.Columns.Add("AvgPrice", typeof(decimal));
            myTable.Columns.Add("Status", typeof(string));
            m_dvOrd = new DataView(myTable);
            BindToDataView(gridOrders, m_dvOrd);
        }

        private void BindToDataView(DataGridView myGrid, DataView myDataView)
        {
            myGrid.DataSource = myDataView;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //GetTickers();
            /*var rv = m_dvBal.AddNew();
            rv["Currency"] = "ETH";
            rv["Amount"] = 1.245M;
            m_dvBal[0]["Amount"] = 2.25M;
            gridBalances.Refresh();*/

            UpdateOrders("BINANCE");
        }

        private void UpdateBalances(string exchange)
        {
            decimal usd, btcTotal, usdTotal;
            DisplayBalances(exchange, out usd, out btcTotal, out usdTotal);
            lblBtcTotal.Text = string.Format("{0:0.00000000}", btcTotal);
            lblUsdTotal.Text = string.Format("${0:#,##0.00}", usdTotal);
        }

        private void UpdateOrders(string exchange)
        {
            int orderCount;
            DisplayOrders(exchange, out orderCount);
            lblOrderCount.Text = string.Format("Working {0} orders", orderCount);
        }

        private void DisplayOrders(string exchange, out int orderCount)
        {
            orderCount = 0;
            if (m_dvOrd == null) return;
            var orders = m_api[exchange].GetOpenOrderDetails().OrderBy(o => o.Symbol);
            m_dvOrd.Table.Clear();
            foreach (var eor in orders)
            {
                ++orderCount;
                var rv = m_dvOrd.AddNew();
                //rv["#"] = ++count;
                rv["Timestamp"] = eor.OrderDate.ToString("MMM-dd HH:mm:ss");
                rv["Symbol"] = eor.Symbol;
                rv["Side"] = eor.IsBuy ? "BUY" : "SELL";
                rv["Price"] = eor.Price;
                rv["Amount"] = eor.Amount;
                rv["Filled"] = eor.AmountFilled;
                rv["AvgPrice"] = eor.AveragePrice;
                rv["Status"] = eor.Result.ToString();
            }
            gridOrders.Refresh();
        }

        // where exchange like "BINANCE"
        // returns total value of currencies in BTC (btcTotal)
        // returns USD or USDT (usd)
        private void DisplayBalances(string exchange, out decimal usd, out decimal btcTotal, out decimal usdTotal)
        {
            usd = 0;
            btcTotal = 0;
            usdTotal = 0;
            if (m_dvBal == null) return;
            string btcUsdSymbol = "BTC-USD";
            var tickers = m_api[exchange].GetTickers();
            var td = tickers.ToDictionary(x => x.Key, x => x.Value);
            var balances = m_api[exchange].GetAmounts().OrderBy(b => b.Key);
            m_dvBal.Table.Clear();
            foreach (var kv in balances)
            {
                var rv = m_dvBal.AddNew();
                var currency = kv.Key;
                var amount = kv.Value;
                rv["Currency"] = currency;
                rv["Amount"] = amount;
                if (currency == "BTC")
                {
                    rv["BTC"] = amount;
                    btcTotal += amount;
                }
                else if (currency.StartsWith("USD"))
                {
                    //btcUsdSymbol = "BTC-" + currency;
                    btcUsdSymbol = currency + "-BTC";
                    usd += amount;
                    usdTotal += amount;
                }
                else // if (currency != "USDT")
                {
                    //var globalSymbol = currency + "-BTC";
                    var globalSymbol = "BTC-" + currency;
                    var symbol = m_api[exchange].GlobalSymbolToExchangeSymbol(globalSymbol);
                    var btcValue = td[symbol].Last;
                    var btc = amount * btcValue;
                    rv["BTC"] = btc;
                    btcTotal += btc;
                }
            }

            var btcSymbol = m_api[exchange].GlobalSymbolToExchangeSymbol(btcUsdSymbol);
            var usdValue = td[btcSymbol].Last;
            usdTotal += btcTotal * usdValue;

            gridBalances.Refresh();
        }

        private async void GetTickers()
        {
            //var tickers = m_api.yobi.GetTickers();
            //var tickers = await m_api.yobi.GetTickersAsync();
            //var tickers = await m_api.gdax.GetTickersAsync();
            //var tickers = await m_api.cryp.GetTickersAsync();
            var tickers = await m_api.huob.GetTickersAsync();

            int count = 0;
            foreach (var kv in tickers.OrderBy(kv => kv.Key))
            {
                var symbol = kv.Key;
                var ticker = kv.Value;
                rtbOutput.AppendText(string.Format("{0,4} {1}\n", ++count, ticker.ToString()));
            }
        }

        private void GetSymbols()
        {
            //var symbols = m_api.huob.GetSymbols();
            var symbols = m_api.yobi.GetSymbols();

            int count = 0;
            foreach (var s in symbols.OrderBy(s => s))
            {
                rtbOutput.AppendText(string.Format("{0,4} {1}\n", ++count, s));
            }
        }

        private void timerBalances_Tick(object sender, EventArgs e)
        {
            UpdateBalances("BINANCE");
        }

        private void timerOrders_Tick(object sender, EventArgs e)
        {
            UpdateOrders("BINANCE");
        }
    }
} // end of namespace

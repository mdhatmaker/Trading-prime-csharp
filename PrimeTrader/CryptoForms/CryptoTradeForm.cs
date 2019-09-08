using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using IQFeed;
using Tools;
using GuiTools;
using GuiTools.Grid;
using static Tools.G;
using CryptoAPIs;
using CryptoAPIs.Exchange;
using CryptoAPIs.ExchangeX;

namespace CryptoForms
{
    
    public partial class CryptoTradeForm : Form
    {
        List<TickerGridPanel> m_gridPanels = new List<TickerGridPanel>();

        private static string[] m_exchangeNames = { "Bitstamp", "Bitfinex", "Kraken", "ItBit", "GDAX", "Bittrex", "BitFlyer", "Poloniex", "Binance" };
        private static Dictionary<string, CryptoExchangeInfo> m_exchanges = new Dictionary<string, CryptoExchangeInfo>();
        private static Dictionary<string, ZCurrencyPair> m_pairs = new Dictionary<string, ZCurrencyPair>();

        private List<string> m_uniqueLefts, m_uniqueRights;
        private ZCurrencyPair m_selectedPair;
        private int m_updateTickerCount, m_updateBalanceCount, m_updateOrderCount;
        private string m_selectedExchange;
        private string m_prevBtc, m_prevXbt;

        private TextBidAsk m_baBtc;
        private TextBidAsk m_baXbt;

        private TickerGridPanel m_orderGridPanel;
        private TickerGridPanel m_tradeGridPanel;

        private PriceGridForm m_priceGridForm;

        private PriceFeed m_iq;

        public CryptoTradeForm()
        {
            InitializeComponent();

            // Wire up events
            Crypto.UpdateOrdersEvent += Crypto_UpdateOrdersEvent;
            Crypto.UpdateTickerEvent += Crypto_UpdateTickerEvent;

            // Create Bid/Ask displays for @BTC and @XBT futures
            m_baBtc = new TextBidAsk(panelBtc, "@BTC:");
            m_baXbt = new TextBidAsk(panelXbt, "@XBT:");

            Crypto.InitializeExchanges();

            LoadAlgos();
            LoadStudies();

            m_orderGridPanel = new TickerGridPanel(panelCrypto2, "Orders", Color.Gray, ZOrder.Columns);
            m_orderGridPanel.Initialize();
            m_orderGridPanel.Grid.CellClick += OrderGrid_CellClick;
            m_gridPanels.Add(m_orderGridPanel);

            m_tradeGridPanel = new TickerGridPanel(panelCrypto3, "Trades", Color.Gray, ZTrade.Columns);
            m_tradeGridPanel.Initialize();
            m_gridPanels.Add(m_tradeGridPanel);

            m_iq = PriceFeed.Instance;
            m_iq.UpdatePrices += M_iq_UpdatePrices;
            m_iq.SubscribePrices("@BTC#");
            m_iq.SubscribePrices("@XBT#");
            m_iq.SubscribePrices("XBTX.XO");
            m_iq.SubscribePrices("BRTI.X");
            m_iq.SubscribePrices("GXBT.XO");


            Initialize();

            m_updateTickerCount = 0;
            Task.Run(() => UpdateExchangeTickers());
            Task.Run(() => UpdateBalances());
            Task.Run(() => UpdateOrders());
        }

        private void LoadAlgos()
        {
            listAlgo.Items.Add("Kraken MACD Crossover");
            listAlgo.Items.Add("Binance MACD Crossover");
            listAlgo.Items.Add("Modified Iceberg");
        }

        private void LoadStudies()
        {
            listStudy.Items.Add("Weekend vs Weekday Performance");
        }

        private void M_iq_UpdatePrices(PriceUpdateIQ update)
        {
            //Console.WriteLine("{0}  {1}:{2}-{3}:{4}  {5}", update.Symbol, update.BidSize, update.Bid, update.Ask, update.AskSize, update.LastTradeTime);
            if (update.Symbol.StartsWith("@BTC"))
            {
                m_baBtc.Update(this, update);

                if (m_prevBtc != null)
                {
                    if (update.LastTradePrice > decimal.Parse(m_prevBtc))
                        GUi.SetLabelColor(this, lblFutBTC, Color.Green);
                    else if (update.LastTradePrice < decimal.Parse(m_prevBtc))
                        GUi.SetLabelColor(this, lblFutBTC, Color.Red);
                }
                GUi.SetLabelText(this, lblFutBTC, update.LastTradePrice.ToString());
                m_prevBtc = update.LastTradePrice.ToString();
            }
            else if (update.Symbol.StartsWith("@XBT"))
            {
                m_baXbt.Update(this, update);

                if (m_prevXbt != null)
                {
                    if (update.LastTradePrice > decimal.Parse(m_prevXbt))
                        GUi.SetLabelColor(this, lblFutXBT, Color.Green);
                    else if (update.LastTradePrice < decimal.Parse(m_prevXbt))
                        GUi.SetLabelColor(this, lblFutXBT, Color.Red);
                }
                GUi.SetLabelText(this, lblFutXBT, update.LastTradePrice.ToString());
                m_prevXbt = update.LastTradePrice.ToString();
            }
        }

        // Populate the exchanges, symbols on each exchange, and the display grids
        private void Initialize()
        {
            dout("Populating CryptoExchange objects:");
            PopulateExchanges();
            PopulateGrids();
            //unboldAllExchangeButtons();
            //rdoBitstamp.Font = new Font(rdoBitstamp.Font, FontStyle.Bold);
        }

        private int ExchangeCount { get { return m_exchangeNames.Length; } }

        #region ---------- THESE METHODS RUN ON THREADS -----------------------------------------------------------------------------------
        private void UpdateExchangeTickers()
        {
            while (true)
            {
                Thread.Sleep(150);

                if (m_selectedPair == null) continue;   // wait until the user has selected a valid symbol pair

                int row = m_updateTickerCount % ExchangeCount;
                if (row == 1 && row % (ExchangeCount * 5) != 0) { ++m_updateTickerCount; continue; }
                if (row >= ExchangeCount) { ++m_updateTickerCount; continue; }
                string exchname = gridBalances[2, row].Value.ToString();    // TODO: let's store this when we populate the grid instead

                //if (exchname == "Bitfinex") { ++m_updateOrderCount; continue; }

                while (!m_exchanges[exchname].Exchange.HasSymbol(m_selectedPair.Symbol))
                {
                    gridBalances[3, row].Value = "";
                    gridBalances[4, row].Value = "";
                    gridBalances[5, row].Value = "";
                    gridBalances[6, row].Value = "";

                    ++m_updateTickerCount;
                    row = m_updateTickerCount % ExchangeCount;
                    exchname = gridBalances[2, row].Value.ToString();    // TODO: let's store this when we populate the grid instead
                }

                ++m_updateTickerCount;

                ZTicker ticker = null;
                var exchange = m_exchanges[exchname].Exchange;
                switch (row)
                {
                    case 0:
                        ticker = exchange.GetTicker(m_selectedPair.BitstampSymbol);
                        break;
                    case 1:
                        ticker = exchange.GetTicker(m_selectedPair.BitfinexSymbol);
                        break;
                    case 2:
                        ticker = exchange.GetTicker(m_selectedPair.KrakenSymbol);
                        break;
                    case 3:
                        //ticker = exchange.GetTicker(m_selectedPair.ItBitSymbol);
                        break;
                    case 4:
                        ticker = exchange.GetTicker(m_selectedPair.GDAXSymbol);
                        break;
                    case 5:
                        ticker = exchange.GetTicker(m_selectedPair.BittrexSymbol);
                        break;
                    case 6:
                        ticker = exchange.GetTicker(m_selectedPair.BitflyerSymbol);
                        break;
                    case 7:
                        ticker = exchange.GetTicker(m_selectedPair.PoloniexSymbol);
                        break;
                    case 8:
                        ticker = exchange.GetTicker(m_selectedPair.BinanceSymbol);
                        break;
                    default:
                        ticker = null;
                        break;
                }

                if (ticker != null)
                {
                    gridBalances[3, row].Value = ticker.BidSize;
                    gridBalances[4, row].Value = ticker.Bid;
                    gridBalances[5, row].Value = ticker.Ask;
                    gridBalances[6, row].Value = ticker.AskSize;
                }
                else
                {
                    gridBalances[3, row].Value = "";
                    gridBalances[4, row].Value = "";
                    gridBalances[5, row].Value = "";
                    gridBalances[6, row].Value = "";
                }

            }
        }

        private void UpdateBalances()
        {
            while (true)
            {
                Thread.Sleep(250);

                if (m_selectedPair == null) continue;   // wait until the user has selected a valid symbol pair

                int row = m_updateBalanceCount % ExchangeCount;
                if (row == 1 && row % (ExchangeCount * 4) != 0) { ++m_updateBalanceCount; continue; }
                if (row >= ExchangeCount) { ++m_updateBalanceCount; continue; }
                string exchname = gridBalances[2, row].Value.ToString();    // TODO: let's store this when we populate the grid instead

                //if (exchname == "Bitfinex") { ++m_updateOrderCount; continue; }

                if (!m_exchanges[exchname].Exchange.HasSymbol(m_selectedPair.Symbol))
                {
                    gridBalances[0, row].Value = "";
                    gridBalances[1, row].Value = "";
                    ++m_updateBalanceCount;
                    continue;
                }

                ++m_updateBalanceCount;

                IDictionary<string, ZAccountBalance> balances = null;
                ZAccountBalance lbal = null, rbal = null;
                var exchange = m_exchanges[exchname].Exchange as IOrderExchange;

                balances = exchange.GetAccountBalances();
                balances.TryGetValue(m_selectedPair.Left, out lbal);
                balances.TryGetValue(m_selectedPair.Right, out rbal);

                if (lbal != null && rbal != null)
                {
                    gridBalances[0, row].Value = lbal.Free;
                    gridBalances[1, row].Value = rbal.Free;
                }
                else
                {
                    gridBalances[0, row].Value = "";
                    gridBalances[1, row].Value = "";
                }
            }
        }

        private void UpdateOrders()
        {
            while (true)
            {
                Thread.Sleep(200);

                if (m_selectedPair == null) continue;   // wait until the user has selected a valid symbol pair

                int row = m_updateOrderCount % ExchangeCount;
                if (row == 1 && row % (ExchangeCount * 4) != 0) { ++m_updateOrderCount; continue; }
                if (row >= ExchangeCount) { ++m_updateOrderCount; continue; }
                string exchname = gridBalances[2, row].Value.ToString();    // TODO: let's store this when we populate the grid instead

                if (exchname == "Bitfinex") { ++m_updateOrderCount; continue; }

                if (!m_exchanges[exchname].Exchange.HasSymbol(m_selectedPair.Symbol))
                {
                    gridBalances[0, row].Value = "";
                    gridBalances[1, row].Value = "";
                    ++m_updateOrderCount;
                    continue;
                }

                ++m_updateOrderCount;

                IEnumerable<ZOrder> orders = null;
                var exchange = m_exchanges[exchname].Exchange as IOrderExchange;

                orders = exchange.GetWorkingOrders(null);

                Dictionary<string, IDataRow> rows = new Dictionary<string, IDataRow>();
                foreach (var o in orders)
                {
                    rows[o.Key] = o;
                }
                m_gridPanels[0].UpdateRows(rows);
            }
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------

        private List<IDataRow> UpdateBitstampTicker()
        {
            var li = BitstampTicker.GetList();
            List<IDataRow> li2 = new List<IDataRow>();
            foreach (var item in li)
            {
                li2.Add(item as IDataRow);
            }
            return li2;
        }

        private Dictionary<string, IDataRow> UpdateBlockchainInfoTicker()
        {
            var dict = BlockchainInfoTicker.GetDictionary();
            Dictionary<string, IDataRow> d2 = new Dictionary<string, IDataRow>();
            foreach (var k in dict.Keys)
            {
                d2.Add(k, dict[k] as IDataRow);
            }
            return d2;
        }

        private List<IDataRow> UpdateCoinMarketCapTicker()
        {
            var li = CoinMarketCapTicker.GetList();
            List<IDataRow> li2 = new List<IDataRow>();
            foreach (var item in li)
            {
                li2.Add(item as IDataRow);
            }
            return li2;
        }

        private Dictionary<string, IDataRow> UpdateBitcoinChartsWeightedPrices()
        {
            var dict = BitcoinChartsWeightedPrices.GetDictionary();
            Dictionary<string, IDataRow> d2 = new Dictionary<string, IDataRow>();
            foreach (var k in dict.Keys)
            {
                d2.Add(k, dict[k] as IDataRow);
            }
            return d2;
        }

        private void CryptoPricesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private bool IsFiatCurrency(string symbol)
        {
            return symbol == "USD" || symbol == "EUR" || symbol == "JPY";
        }

        // After this method, we should have all the CryptoExchanges stored in m_exchanges dictionary (key by name, ex: "Bitstamp")
        // We should also have m_symbols containing the set of UNIQUE symbols within ALL these exchanges
        private void PopulateExchanges()
        {
            // Get top 20 market caps using CoinMarketCap API
            var caps = CoinMarketCap.Instance.GetTickers(20);
            var top20 = caps.Select(c => c.symbol).ToList();

            m_pairs.Clear();
            foreach (var name in m_exchangeNames)
            {
                //if (name == "BitFlyer") { cout("Skipping exchange {0}", name); continue; }
                dout("Processing exchange {0}", name);
                var cryptoexch = new CryptoExchangeInfo(name);
                var exchange = Crypto.GetExchange(name) as IOrderExchange;
                exchange.EnableLiveOrders = true;
                m_exchanges[name] = cryptoexch;
                foreach (var p in cryptoexch.Pairs.Values)
                {
                    // Let's do a little filter here—for top 20 market cap, for instance (both Left and Right currency have to be in top 20)
                    if ((top20.Contains(p.Left) || IsFiatCurrency(p.Left)) && (top20.Contains(p.Right) || IsFiatCurrency(p.Right)))
                        m_pairs[p.Symbol] = p;
                }
            }

            m_uniqueLefts = m_pairs.Select(p => p.Value.Left).OrderBy(s => s).ToList().Distinct().ToList();
            m_uniqueRights = m_pairs.Select(p => p.Value.Right).OrderBy(s => s).ToList().Distinct().ToList();
            //m_pairs.Select(p => p.Value.Left).OrderBy(s => s).ToList().Distinct().ToList().ForEach(s => cout("{0}", s));           
            //m_pairs.Select(p => p.Value.Right).OrderBy(s => s).ToList().Distinct().ToList().ForEach(s => cout("{0}", s));
            //m_symbols.OrderBy(s => s).ToList().ForEach(s => cout("{0}", s));
        }

        private void PopulateGrids()
        {
            // BALANCES grid
            int N = 9;
            gridBalances.Rows.Clear();
            gridBalances.Rows.Add(N);                           // create (add) 9 rows to the grid
            for (int i = 0; i < N; ++i)
            {
                gridBalances.Rows[i].Cells[0].Value = "";
                gridBalances.Rows[i].Cells[1].Value = "";
                gridBalances.Rows[i].Cells[2].Value = m_exchangeNames[i];
                gridBalances.Rows[i].Cells[3].Value = "";
                gridBalances.Rows[i].Cells[4].Value = "";
                gridBalances.Rows[i].Cells[5].Value = "";
                gridBalances.Rows[i].Cells[6].Value = "";
            }

            // SYMBOLS grid
            int max = Math.Max(m_uniqueLefts.Count, m_uniqueRights.Count);  // add columns to the grid (based on Left/Right currency counts)
            gridSymbols.Columns.Clear();
            for (int i = 0; i < max; ++i)
            {
                int ci = gridSymbols.Columns.Add(i.ToString(), i.ToString());
                gridSymbols.Columns[ci].DefaultCellStyle.BackColor = Color.White;
                gridSymbols.Columns[ci].DefaultCellStyle.ForeColor = Color.Black;
                gridSymbols.Columns[ci].DefaultCellStyle.SelectionBackColor = Color.White;
                gridSymbols.Columns[ci].DefaultCellStyle.SelectionForeColor = Color.Black;
            }
            int n = 4;
            gridSymbols.Rows.Clear();
            gridSymbols.Rows.Add(n);                            // create (add) 4 rows to the grid
            for (int i = 0; i < m_uniqueLefts.Count; ++i)       // add the "Left" currency symbols to the top row
            {
                gridSymbols[i, 0].Value = m_uniqueLefts[i];
            }
            for (int i = 0; i < m_uniqueRights.Count; ++i)      // add the "Right" currency symbols to the bottom row
            {
                gridSymbols[i, 1].Value = m_uniqueRights[i];
            }
        }

        private void unboldAllExchangeButtons()
        {
            //rdoBitstamp.Font = new Font(rdoBitstamp.Font, FontStyle.Regular);
            //rdoBitfinex.Font = new Font(rdoBitfinex.Font, FontStyle.Regular);
        }

        private void rdoBitstamp_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void SelectExchange(string exchangeName)
        {
            m_selectedExchange = exchangeName;                // This has the EXCHANGE NAME (ex: "Binance")               
            lblBuy.Text = m_selectedExchange;
            lblSell.Text = m_selectedExchange;
            btnBuy.Enabled = true;
            btnSell.Enabled = true;
        }

        private void gridBalances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)   // Click an EXCHANGE BUTTON
            {
                SelectExchange(senderGrid[e.ColumnIndex, e.RowIndex].Value as string);
                //Console.WriteLine("{0} {1} {2}", e.RowIndex, e.ColumnIndex, text);
            }
            else if (e.RowIndex >= 0)                                                               // Click any other column (but NOT a header)
            {
                if (e.ColumnIndex == 4 || e.ColumnIndex == 5)                                       // Click either BID or ASK column
                {
                    //txtBuyPrice.Text = senderGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                    numBuyPrice.Value = (decimal) senderGrid[e.ColumnIndex, e.RowIndex].Value;
                    numSellPrice.Value = (decimal) senderGrid[e.ColumnIndex, e.RowIndex].Value;
                    SelectExchange(senderGrid[2, e.RowIndex].Value as string);
                }
                else if (e.ColumnIndex == 3 || e.ColumnIndex == 6)                                  // Click either BIDSIZE or ASKSIZE column
                {
                    //txtBuyQty.Text = senderGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                    numBuyQty.Value = (decimal)senderGrid[e.ColumnIndex, e.RowIndex].Value;
                    numSellQty.Value = (decimal) senderGrid[e.ColumnIndex, e.RowIndex].Value;
                    SelectExchange(senderGrid[2, e.RowIndex].Value as string);
                }
            }
        }

        int[] m_boldColumn = { -1, -1 };
        private void gridSymbols_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)                                                                    // Click the SYMBOLS grid (but NOT a header)
            {
                string text = senderGrid[e.ColumnIndex, e.RowIndex].Value as string;                // This has the EXCHANGE NAME (ex: "Binance")
                Console.WriteLine("{0} {1} {2}", e.RowIndex, e.ColumnIndex, text);
                unboldSymbolsGrid(e.RowIndex);
                if (m_boldColumn[e.RowIndex] >= 0)
                    boldSymbolsGrid(e.RowIndex, m_boldColumn[e.RowIndex], false);
                boldSymbolsGrid(e.RowIndex, e.ColumnIndex, true);                                   // Make the symbol clicked on BOLD
                m_boldColumn[e.RowIndex] = e.ColumnIndex;
            }
            if (m_boldColumn[0] >= 0 && m_boldColumn[1] >= 0)
            {
                var left = senderGrid[m_boldColumn[0], 0].Value.ToString();
                var right = senderGrid[m_boldColumn[1], 1].Value.ToString();
                string selectedSymbol = left + "_" + right;
                if (m_pairs.TryGetValue(selectedSymbol, out m_selectedPair))
                {
                    lblSymbol.Text = m_selectedPair.Symbol;
                    Crypto.SubscribeOrderUpdates(m_selectedPair);
                    Crypto.SubscribeTickerUpdates(m_selectedPair);
                }
                else
                {
                    lblSymbol.Text = "???";
                }
            }
        }

        private void OrderGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int ri = e.RowIndex;
            int ci = e.ColumnIndex;

            // Ignore clicks that are not on button cells. 
            const int IDX_CXLBTN = 9;
            const int IDX_SYMBOL = 1;
            if (ri < 0 || ci != IDX_CXLBTN) return;

            var row = m_orderGridPanel.Grid.Rows[ri];
            string tag = row.Tag as string;
            string symbol = row.Cells[IDX_SYMBOL].Value as string;
            cout("CLICK: {0}", tag);
            var split = tag.Split('_');
            if (split[0] == "BINANCE")
            {
                Crypto.binance.CancelOrder(symbol, split[1]);
            }
            else
            {
                ErrorMessage("CrytpoTradeForm::OrderGrid_CellClick=> Unknown exchange: {0}", split[0]);
            }
            m_gridPanels[0].DeleteRowAtIndex(ri);
        }

        private Dictionary<CryptoExch, ZOrderMap> m_orderMaps = new Dictionary<CryptoExch, ZOrderMap>();
        private void Crypto_UpdateOrdersEvent(object sender, OrdersUpdateArgs e)
        {
            var exch = e.Exch;
            var omap = e.Orders;

            /*if (m_orderMaps.ContainsKey(exch))
            {
                //if (m_orderMaps[exch].Orders().Count != omap.Orders().Count)
                //    cout("YO!");
                foreach (var o in m_orderMaps[exch].Orders())
                {
                    if (!omap.Orders().Contains(o))     // order exists in previous order map but not this updated one
                    {
                        m_gridPanels[0].DeleteRow(o.Key);
                    }
                }
            }*/

            Dictionary<string, IDataRow> rows = new Dictionary<string, IDataRow>();
            foreach (var o in omap.Orders())
            {
                rows[o.Key] = o;
            }
            m_gridPanels[0].UpdateRows(rows);

            m_orderMaps[exch] = omap;
            //cout("{0} {1} {2} M_UpdateOrdersEvent", DateTime.Now.ToString("HH:mm:ss"), e.Exch, omap.ExchangeCount);
        }

        private void Crypto_UpdateTickerEvent(object sender, TickersUpdateArgs e)
        {
            cout("UpdateTickerEvent: {0}", e.ToString());
        }

        private void unboldSymbolsGrid(int row)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font(gridSymbols.Font, FontStyle.Regular);
            gridSymbols.Rows[row].DefaultCellStyle.ApplyStyle(style);
            //gridSymbols.Rows[row].DefaultCellStyle = style;
            /*var fontRegular = new Font(gridSymbols[0, 0].Style.Font, FontStyle.Regular);
            for (int i = 0; i < gridSymbols.Columns.Count; ++i)
            {
                gridSymbols[i, row].Style.Font = fontRegular;
            }*/
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            // *** FOR TESTING ONLY!!! ***
            (new CryptoAlgoForm()).Show();
            return;
            Random rnd = new Random();
            decimal value;
            m_priceGridForm = new PriceGridForm();
            m_priceGridForm.Show();
            m_priceGridForm.HeatMapGrid = new HeatMapGrid(m_priceGridForm.Grid, m_priceGridForm);
            int exchCount = Crypto.Exchanges.Keys.Count;
            var rowHeaders = new string[exchCount];
            var colHeaders = new string[exchCount];
            int i = 0;
            foreach (var exch in Crypto.Exchanges.Keys)
            {
                string name = exch.ToString();
                rowHeaders[i] = name;
                colHeaders[i] = name;
                ++i;
            }
            m_priceGridForm.HeatMapGrid.InitializeRowsAndColumns(rowHeaders, colHeaders, Color.RoyalBlue, Color.LightSkyBlue);
            //m_priceGridForm.HeatMapGrid.InitializeRowsAndColumns(rowHeaders, colHeaders, Color.Red, Color.Green);

            for (int j = 0; j < exchCount; ++j)
            {
                value = rnd.Next(10250, 10350);
                m_priceGridForm.HeatMapGrid.SetRowValue(j, value);
                m_priceGridForm.HeatMapGrid.SetColValue(j, value);
                //cout("{0},{1} = {2}", j, j, value);
            }
            m_priceGridForm.HeatMapGrid.UpdateGridColors();

            Task.Run(() => RandomGridCellValues());

            return;

            var chart = new CryptoChartForm();
            chart.Show();
            chart.BringToFront();
            var candlesticks = Crypto.kraken.GetCandlesticks("BCHUSD", "1h");
            chart.DisplayChart(candlesticks);
        }

        private void RandomGridCellValues()
        {
            Random rnd = new Random();

            while (true)
            {
                int i = rnd.Next(0, m_priceGridForm.Grid.RowCount);
                decimal value = rnd.Next(-10, 10);
                m_priceGridForm.HeatMapGrid.UpdateValueDelta(i, value);

                int millis = rnd.Next(10, 50);
                Thread.Sleep(millis);
            }
        }

        private void btnLaunchStudy_Click(object sender, EventArgs e)
        {

        }

        private void boldSymbolsGrid(int row, int col, bool bold)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            if (bold)
                style.Font = new Font(gridSymbols.Font.FontFamily, gridSymbols.Font.Size, FontStyle.Bold);
            else
                style.Font = new Font(gridSymbols.Font.FontFamily, gridSymbols.Font.Size, FontStyle.Regular);
            gridSymbols[col, row].Style.ApplyStyle(style);
            /*var fontBold = new Font(gridSymbols[0, 0].Style.Font, FontStyle.Bold);
            for (int i = 0; i < gridSymbols.Columns.Count; ++i)
            {
                gridSymbols[i, row].Style.Font = fontBold;
            }*/
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            var exchange = Crypto.GetExchange(m_selectedExchange) as IOrderExchange;
            exchange.SubmitLimitOrder(m_selectedPair.BinanceSymbol, OrderSide.Buy, (decimal) numBuyPrice.Value, (decimal) numBuyQty.Value);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            var exchange = Crypto.GetExchange(m_selectedExchange) as IOrderExchange;
            exchange.SubmitLimitOrder(m_selectedPair.BinanceSymbol, OrderSide.Sell, (decimal) numSellPrice.Value, (decimal)numSellQty.Value);
        }
    } // end of CLASS


    public class CryptoExchangeInfo
    {
        public string Name { get; private set; }
        public CryptoExch Exch { get; private set; }
        public BaseExchange Exchange { get; private set; }
        public SortedDictionary<string, ZCurrencyPair> Pairs { get { return m_pairs; } }

        private SortedDictionary<string, ZCurrencyPair> m_pairs = new SortedDictionary<string, ZCurrencyPair>();
        
        public CryptoExchangeInfo(string name)
        {
            Name = name;
            Exch = Crypto.GetExch(name);
            Exchange = Crypto.Exchanges[Exch];
            var symbols = Exchange.GetSymbolList(false);
            foreach (var s in symbols)
            {
                var pair = ZCurrencyPair.FromSymbol(s, Exch);
                if (pair == null) continue;
                m_pairs[pair.Symbol] = pair;                            // Use the OneChain format symbol (ex: "BTC_USD")
            }
        }

        // Does this Exchange trade the given symbol? (in OneChain format, ex: "NEO_ETH")
        public bool HasPair(string symbol)
        {
            return m_pairs.ContainsKey(symbol);
        }
    } // end of class CryptoExchangeInfo

} // end of NAMESPACE

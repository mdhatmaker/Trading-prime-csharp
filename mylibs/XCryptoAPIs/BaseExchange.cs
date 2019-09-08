using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using RestSharp;
using Tools;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.IO;
using static Tools.G;

namespace CryptoAPIs
{
    public abstract class BaseExchange : BaseApi
    {
        public event OrderBookUpdateHandler UpdateOrderBookEvent;
        public event OrdersUpdateHandler UpdateOrdersEvent;
        public event TickerUpdateHandler UpdateTickerEvent;
        public event TradeUpdateHandler UpdateTradeEvent;
        //public event PositionsUpdateHandler UpdatePositionsEvent;

        public abstract CryptoExch Exch { get; }

        public virtual string WebsocketUrl { get { return null; } }

        public ZCurrencyPairMap CurrencyPairs = new ZCurrencyPairMap();

        public abstract List<string> SymbolList { get; }
        public virtual List<string> GetSymbolList(bool forceUpdate) { return SymbolList; }
        public abstract ZTicker GetTicker(string symbol);
        public abstract Task<Dictionary<string, ZTicker>> GetAllTickers();
        public abstract ZCryptoOrderBook GetOrderBook(string symbol);

        public ZOrderBook OrderBook { get { return m_orderBook; } }

        public bool HasSymbol(string symbol) { return CurrencyPairs != null && CurrencyPairs.ContainsKey(symbol); }
        public bool HasPair(ZCurrencyPair pair) { return CurrencyPairs != null && CurrencyPairs.ContainsValue(pair); }

        protected ZWebSocket m_socket;
        protected ZOrderBook m_orderBook = new ZOrderBook();
        protected ZOrderMap m_orders = new ZOrderMap();
        protected ConcurrentSet<ZCurrencyPair> m_subscribedPairs = new ConcurrentSet<ZCurrencyPair>();

        protected List<string> m_symbolList;
        protected System.Timers.Timer m_tickerTimer;
        protected System.Timers.Timer m_positionTimer;

        private DateTime m_lastUpdateTime = DateTime.Now;
        private int m_minimumDelayMilliseconds = 500;


        public BaseExchange()
        {
             //m_restClient = new RestClient(BaseUrl);
        }

        // Given an updated set of exchange-specific symbols, update the CurrencyPairs map
        protected void UpdateCurrencyPairsFromSymbols()
        {
            //SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
            CurrencyPairs.Clear();
            foreach (var s in m_symbolList)
            {
                var exch = Crypto.GetExch(Name);
                var pair = ZCurrencyPair.FromSymbol(s, exch);
                CurrencyPairs.Add(pair);
            }
        }

        #region ------------------------------- TIMERS ------------------------------------------------------------------------------------
        public void StartTickerTimer(int intervalMilliseconds = 5000)
        {
            // Dispose previous version of timer (if it exists)
            if (m_tickerTimer != null)
            {
                m_tickerTimer.Enabled = false;
                m_tickerTimer.Dispose();
                m_tickerTimer = null;
            }

            // Timer to request new prices (tickers)
            m_tickerTimer = new System.Timers.Timer();
            m_tickerTimer.Interval = intervalMilliseconds;
            m_tickerTimer.Elapsed += tickerTimer_Elapsed;
            m_tickerTimer.Enabled = true;
            //tickerTimer_Elapsed(this, System.EventArgs.Empty as System.Timers.ElapsedEventArgs);
        }

        private async void tickerTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_tickerTimer.Enabled = false;
            var tickers = await GetAllTickers();
            UpdateTickerEvent?.Invoke(this, new TickersUpdateArgs(this.Exch, tickers));
            m_tickerTimer.Enabled = true;
        }

        /*public void StartPositionTimer(int intervalMilliseconds = 5000)
        {
            // Dispose previous version of timer (if it exists)
            if (m_positionTimer != null)
            {
                m_positionTimer.Enabled = false;
                m_positionTimer.Dispose();
                m_positionTimer = null;
            }

            // Timer to request new prices (tickers)
            m_positionTimer = new System.Timers.Timer();
            m_positionTimer.Interval = intervalMilliseconds;
            m_positionTimer.Elapsed += tickerTimer_Elapsed;
            m_positionTimer.Enabled = true;
            //tickerTimer_Elapsed(this, System.EventArgs.Empty as System.Timers.ElapsedEventArgs);
        }

        private void positionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_positionTimer.Enabled = false;
            UpdatePositionsEvent?.Invoke(this, new BalanceUpdateArgs(null, null));
            m_positionTimer.Enabled = true;
        }*/
        #endregion ------------------------------------------------------------------------------------------------------------------------

        #region ---------- EVENT SUBSCRIPTIONS --------------------------------------------------------------------------------------------
        public virtual void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            ErrorMessage("BaseExchange::SubscribeOrderBookUpdates=> Exchange {0} does not support SubscribeOrderBookUpdates", this.Name);
        }

        public virtual void SubscribeOrderUpdates(ZCurrencyPair pair, bool subscribe)
        {
            ErrorMessage("BaseExchange::SubscribeOrderUpdates=> Exchange {0} does not support SubscribeOrderBookUpdates", this.Name);
        }

        public virtual void SubscribeTickerUpdates(ZCurrencyPair pair, bool subscribe)
        {
            ErrorMessage("BaseExchange::SubscribeTickerUpdates=> Exchange {0} does not support SubscribeTickerUpdates", this.Name);
        }

        public virtual void SubscribeTradeUpdates(ZCurrencyPair pair, bool subscribe)
        {
            ErrorMessage("BaseExchange::SubscribeTradeUpdates=> Exchange {0} does not support SubscribeTradeUpdates", this.Name);
        }

        
        protected void FireOrderBookUpdate()
        {
            if (DateTime.Now.Subtract(m_lastUpdateTime).Milliseconds >= m_minimumDelayMilliseconds)
            {
                UpdateOrderBookEvent?.Invoke(this, new OrderBookUpdateArgs(m_orderBook.GetRows()));
                m_lastUpdateTime = DateTime.Now;
            }
        }

        protected void FireOrdersUpdate()
        {
            UpdateOrdersEvent?.Invoke(this, new OrdersUpdateArgs(this.Exch, m_orders));
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------


        // Given a populated CurrencyPairs map, update the exchange-specific symbols
        protected void UpdateSymbolsFromCurrencyPairs()
        {
            m_symbolList = new List<string>();
            foreach (var symbol in CurrencyPairs.Symbols)
            {
                m_symbolList.Add(CurrencyPairs[symbol].ExchangeSpecificSymbol);
            }
            m_symbolList.Sort();
        }

        public void Dispose()
        {
        }


    } // end of abstract class BaseExchange

} // end of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.G;
using static Tools.GDate;
using ZS = ZeroSumAPI;
using TT = TradingTechnologies.TTAPI;
using TradingTechnologies.TTAPI.Tradebook;
using TradingTechnologies.TTAPI.WinFormsHelpers;
using TradingTechnologies.TTAPI.CustomerDefaults;


namespace ZeroSumAPI
{
    /*// Define the product (see TTProduct struct) AND a specific contract (ex: "Dec17")
    public struct TTInstrument
    {
        public string MarketName { get { return this.InstrumentKey.MarketKey.Name; } }
        public TT.ProductType ProductType { get { return this.InstrumentKey.ProductKey.Type; } }
        public string ProductName { get { return this.InstrumentKey.ProductKey.Name; } }
        public string SeriesKey { get { return this.InstrumentKey.SeriesKey; } }
        public TT.InstrumentKey InstrumentKey { get; private set; }
        public TT.ProductKey ProductKey { get { return this.InstrumentKey.ProductKey; } }
        public TT.Instrument Instrument { get { return m_instrument; } }
        public List<TT.OrderFeed> OrderFeeds { get { return m_orderFeeds; } }
        public TT.OrderFeed DefaultOrderFeed { get; set; }

        private TT.Instrument m_instrument;
        private List<TT.OrderFeed> m_orderFeeds;

        public TTInstrument(TT.ProductKey productKey, string seriesKey)
        {
            this.InstrumentKey = new TT.InstrumentKey(productKey, seriesKey);
            m_instrument = null;
            m_orderFeeds = null;
            this.DefaultOrderFeed = null;
        }

        public void FoundInstrument(TT.Instrument instrument)
        {
            m_instrument = instrument;
            m_orderFeeds = new List<TT.OrderFeed>();
            foreach (TT.OrderFeed orderFeed in instrument.GetValidOrderFeeds())
            {
                m_orderFeeds.Add(orderFeed);
            }
            if (m_orderFeeds.Count > 0)
                this.DefaultOrderFeed = m_orderFeeds[0];
        }
    } // end of STRUCT*/



    public class TengineTT2 : TradingEngine
    {        
        // ****** ADD NEW PRODUCTS HERE!!! ******
        // Translate IQFeed symbol root into TT ProductKey
        static Dictionary<string, TT.ProductKey> TTProductKeyTranslate = new Dictionary<string, TT.ProductKey>()
        {
            { "@ES", new TT.ProductKey("CME", TT.ProductType.Future, "ES") },
            { "@VX", new TT.ProductKey("CME", TT.ProductType.Future, "VX") }
        };
        // ****** OR HERE IF NOT FUTURES!!! ******
        // Translate IQFeed symbol into TTInstrument struct
        static Dictionary<string, TTInstrument> TTInstrumentTranslate = new Dictionary<string, TTInstrument>()
        {
            { "M.CU3=LX", new TTInstrument(new TT.ProductKey("LME", TT.ProductType.Future, "CA"), "3M") }
        };

        private Dictionary<uint, TTInstrument> _instruments = new Dictionary<uint, TTInstrument>();

        #region Declare the TTAPI objects
        private TT.XTraderModeTTAPI m_TTAPI = null;
        private CustomerDefaultsSubscription m_customerDefaultsSubscription = null;
        private InstrumentTradeSubscription m_instrumentTradeSubscription = null;
        private TT.PriceSubscription m_priceSubscription = null;
        private List<TT.CustomerDefaults.CustomerDefaultEntry> customers = new List<CustomerDefaultEntry>();
        private TT.CustomerDefaults.CustomerDefaultEntry m_defaultCustomer;
        #endregion

        public TengineTT2()
        {
            // Add translations from IQFeed symbols to definition fields of a TT Instrument
            //_translateSymbol.Add("@ESZ17", new TTInstrument("CME", ProductType.Future, "ES", "Dec17"));
        }

        #region TradingEngine abstract overrides
        public override void Startup()
        {
        }

        public override void Shutdown()
        {
            shutdownTTAPI();
        }

        // Expect 'symbol' as IQFeed Symbol ("@ESJ16", "@VXK16", "@VXQ17")
        public override uint CreateInstrument(uint iid, string symbol)
        {
            instruments[iid] = new ZInstrument(iid, symbol);
            _instruments.Add(iid, TranslateInstrument(symbol));
            return iid;
        }

        public override void Subscribe(uint iid)
        {
            SubscribeInstrument(_instruments[iid]);
        }

        public override void Unsubscribe(uint iid)
        {
            UnsubscribeInstrument(_instruments[iid]);
        }

        public override uint CreateOrder(uint iid, ZOrderSide side, int price, uint qty, ZOrderType type, ZOrderTimeInForce tif)
        {
            uint oid = GenerateOrderId();
            var o = new ZOrder(oid, iid, side, price, qty, type, tif);
            orders[oid] = o;

            //_orders[o.Oid] = new OME.EquityOrder(_instruments[o.Iid], OME.Order.OrderTypes.GoodUntilCancelled, GetSide(o), o.Price, o.Qty);

            return oid;
        }

        public override void SubmitOrder(uint oid)
        {
            ZOrder o = orders[oid];
            //var order = _orders[oid];
            //GetBook(o).Insert(order);
            var ttinstr = _instruments[o.Iid];
            SendOrder(ttinstr, o);
            o.SetState(ZOrderState.Working);
        }

        public override void DeleteOrder(uint oid)
        {
            ZOrder o = orders[oid];
            //var order = _orders[oid];
            //GetBook(o).Remove(order);
            o.SetState(ZOrderState.Cancelled);
        }

        public override void ModifyOrder(uint oid, int price, uint qty)
        {
            orders[oid].SetPrice(price);
            orders[oid].SetQty(qty);
        }

        public override void ModifyOrder(uint oid, int price)
        {
            orders[oid].SetPrice(price);
        }
        public override void ModifyOrder(uint oid, uint qty)
        {
            orders[oid].SetQty(qty);
        }

        public override ZOrder GetOrder(uint oid)
        {
            return orders[oid];
        }
        #endregion

        #region Helper Functions
        // Given an IQFeed Future Symbol (that ends in 'mYY' monthcode and 2-digit year)
        // Return (as out parameters) both the corresponding TT Symbol and the TT Month/Year (both as strings)
        // Return True if both Symbol and Month/Year were translated successfully; otherwise return False
        public static TTInstrument TranslateInstrument(string iqSymbol)
        {
            string mYY = GetmYY(iqSymbol);
            string symbol = iqSymbol;
            // symbol will either be the "root" (if it's a futures contract) OR the whole iqSymbol (if not)
            if (mYY != null)
            {
                symbol = iqSymbol.Substring(0, iqSymbol.Length - 3);
                TT.ProductKey ttProductKey = TTProductKeyTranslate[symbol];
                string ttMonthYear = MonthYear(mYY);
                return new TTInstrument(ttProductKey, ttMonthYear);
            }
            else
                return TTInstrumentTranslate[symbol];
        }

        // Given a ZeroSumAPI order side
        // Return a TT.BuySell order side
        public static TT.BuySell TranslateSide(ZS.ZOrderSide side)
        {
            if (side == ZOrderSide.Buy)
                return TT.BuySell.Buy;
            else if (side == ZOrderSide.Sell)
                return TT.BuySell.Sell;
            else
                return TT.BuySell.Unknown;
        }

        private string LimitOrMarketPrice(TT.OrderProfile orderProfile)
        {
            return orderProfile.OrderType == TT.OrderType.Limit ? orderProfile.LimitPrice.ToString() : "Market Price";
        }

        private string LimitOrMarketPrice(TT.Order order)
        {
            return order.OrderType == TT.OrderType.Limit ? order.LimitPrice.ToString() : "Market Price";
        }

        public void SubscribeInstrument(TTInstrument ttinstr)
        {
            cout("Subscribing to Instrument...");

            // Find out instrument based on the previously created key
            TT.InstrumentLookupSubscription instrRequest = new TT.InstrumentLookupSubscription(this.m_TTAPI.Session, TT.Dispatcher.Current, ttinstr.InstrumentKey);
            instrRequest.Update += instrRequest_Completed;
            instrRequest.Start();
        }

        public void UnsubscribeInstrument(TTInstrument ttinstr)
        {
            cout("Unsubscribing from Instrument...");
        }
        #endregion

        #region TTAPI Startup and Shutdown
        /// <summary>
        /// Init and start TT API.
        /// </summary>
        /// <param name="instance">XTraderModeTTAPI instance</param>
        /// <param name="ex">Any exception generated from the ApiCreationException</param>
        public void ttApiInitHandler(TT.TTAPI api, TT.ApiCreationException ex)
        {
            if (ex == null)
            {
                m_TTAPI = (TT.XTraderModeTTAPI)api;
                m_TTAPI.ConnectionStatusUpdate += new EventHandler<TT.ConnectionStatusUpdateEventArgs>(ttapiInstance_ConnectionStatusUpdate);
                m_TTAPI.Start();
                m_customerDefaultsSubscription = new CustomerDefaultsSubscription(m_TTAPI.Session, TT.Dispatcher.Current);
                m_customerDefaultsSubscription.CustomerDefaultsChanged += new EventHandler(m_customerDefaultsSubscription_CustomerDefaultsChanged);
                m_customerDefaultsSubscription.Start();
            }
            else if (ex.IsRecoverable)
            {
                cout("TTAPI Initialization: {0}", ex.Message);
            }
            else
            {
                ErrorMessage("TTAPI Initialization Failed: {0}", ex.Message);
            }
        }

        /// <summary>
        /// ConnectionStatusUpdate callback.
        /// Give feedback to the user that there was an issue starting up and connecting to XT.
        /// </summary>
        void ttapiInstance_ConnectionStatusUpdate(object sender, TT.ConnectionStatusUpdateEventArgs e)
        {
            if (e.Status.IsSuccess)
            {
                cout("TTAPI Connected!");
                //this.Enabled = true;
            }
            else
            {
                cout("TTAPI ConnectionStatusUpdate: {0}", e.Status.StatusMessage);
            }
        }

        /// <summary>
        /// Dispose of all the TT API objects and shutdown the TT API 
        /// </summary>
        public void shutdownTTAPI()
        {
            if (!m_shutdownInProcess)
            {
                // Dispose of all request objects
                if (m_priceSubscription != null)
                {
                    m_priceSubscription.FieldsUpdated -= priceSubscription_FieldsUpdated;
                    m_priceSubscription.Dispose();
                    m_priceSubscription = null;
                }

                if (m_customerDefaultsSubscription != null)
                {
                    m_customerDefaultsSubscription.CustomerDefaultsChanged -= m_customerDefaultsSubscription_CustomerDefaultsChanged;
                    m_customerDefaultsSubscription.Dispose();
                    m_customerDefaultsSubscription = null;
                }

                if (m_instrumentTradeSubscription != null)
                {
                    m_instrumentTradeSubscription.OrderAdded -= m_instrumentTradeSubscription_OrderAdded;
                    m_instrumentTradeSubscription.OrderRejected -= m_instrumentTradeSubscription_OrderRejected;
                    m_instrumentTradeSubscription.Dispose();
                    m_instrumentTradeSubscription = null;
                }

                TT.TTAPI.ShutdownCompleted += new EventHandler(TTAPI_ShutdownCompleted);
                TT.TTAPI.Shutdown();
            }

            // only run shutdown once
            m_shutdownInProcess = true;
        }

        /// <summary>
        /// Event fired when the TT API has been successfully shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TTAPI_ShutdownCompleted(object sender, EventArgs e)
        {
            m_isShutdown = true;
            //this.Close();
        }
        #endregion

        #region TradeSubscription events
        /// <summary>
        /// OrderRejected InstrumentTradeSubscription callback.
        /// </summary>
        /// <param name="sender">Sender (InstrumentTradeSubscription)</param>
        /// <param name="e">OrderRejectedEventArgs</param>
        void m_instrumentTradeSubscription_OrderRejected(object sender, TT.OrderRejectedEventArgs e)
        {
            cout("TT Order Rejected: {0} {1}", e.Order.SiteOrderKey, e.Message);
            /*txtOrderBook.Text += String.Format("Rejected {0} {1}{2}",
                e.Order.SiteOrderKey,
                e.Message,
                System.Environment.NewLine);*/
        }

        /// <summary>
        /// OrderAdded InstrumentTradeSubscription callback.
        /// </summary>
        /// <param name="sender">Sender (InstrumentTradeSubscription)</param>
        /// <param name="e">OrderAddedEventArgs</param>
        void m_instrumentTradeSubscription_OrderAdded(object sender, TT.OrderAddedEventArgs e)
        {
            cout("TT Order Added: {0} {1}|{2}@{3}", e.Order.SiteOrderKey, e.Order.BuySell.ToString(), LimitOrMarketPrice(e.Order));
            /*txtOrderBook.Text += String.Format("Added {0} {1}|{2}@{3}{4}",
                e.Order.SiteOrderKey,
                e.Order.BuySell.ToString(),
                e.Order.OrderQuantity.ToString(),
                e.Order.OrderType == OrderType.Limit ? e.Order.LimitPrice.ToString() : "Market Price",
                System.Environment.NewLine);*/
        }
        #endregion

        #region Customer Defaults events
        /// <summary>
        /// CustomerDefaultsChanged subscription callback.
        /// Update the customers List (and select first list item as DefaultCustomer, if any list items exist).
        /// </summary>
        void m_customerDefaultsSubscription_CustomerDefaultsChanged(object sender, EventArgs e)
        {
            customers.Clear();
            CustomerDefaultsSubscription cds = sender as CustomerDefaultsSubscription;
            foreach (CustomerDefaultEntry entry in cds.CustomerDefaults)
            {
                customers.Add(entry);
            }
            m_defaultCustomer = (customers.Count > 0 ? customers[0] : null);
        }
        #endregion

        #region FindInstrument
        /// <summary>
        /// Function to find a list of InstrumentKeys.
        /// </summary>
        /// <param name="keys">List of InstrumentKeys.</param>
        public void FindInstrument(IList<TT.InstrumentKey> keys)
        {
            foreach (TT.InstrumentKey key in keys)
            {
                // Update the Status Bar text.
                cout("TT API FindInstrument {0}", key.ToString());

                TT.InstrumentLookupSubscription instrRequest = new TT.InstrumentLookupSubscription(m_TTAPI.Session, TT.Dispatcher.Current, key);
                instrRequest.Update += instrRequest_Completed;
                instrRequest.Tag = key.ToString();
                instrRequest.Start();
            }
        }

        public void FindInstrument(TT.InstrumentKey key)
        {
            IList<TT.InstrumentKey> keys = new List<TT.InstrumentKey> { key };
            FindInstrument(keys);
        }

        /// <summary>
        /// Instrument request completed event.
        /// </summary>
        void instrRequest_Completed(object sender, TT.InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.IsFinal && e.Instrument != null)
            {
                try
                {
                    cout("TT API FindInstrument {0}", e.Instrument.Name);

                    TTInstrument ttinstr = _instruments.Values.First(tti => tti.InstrumentKey == e.Instrument.Key);
                    ttinstr.FoundInstrument(e.Instrument);

                    // subscribe for price updates
                    m_priceSubscription = new TT.PriceSubscription(e.Instrument, TT.Dispatcher.Current);
                    m_priceSubscription.Settings = new TT.PriceSubscriptionSettings(TT.PriceSubscriptionType.InsideMarket);
                    m_priceSubscription.FieldsUpdated += new TT.FieldsUpdatedEventHandler(priceSubscription_FieldsUpdated);
                    m_priceSubscription.Start();

                    // subscribe for trade updates
                    m_instrumentTradeSubscription = new InstrumentTradeSubscription(m_TTAPI.Session, TT.Dispatcher.Current, e.Instrument);
                    m_instrumentTradeSubscription.OrderAdded += new EventHandler<TT.OrderAddedEventArgs>(m_instrumentTradeSubscription_OrderAdded);
                    m_instrumentTradeSubscription.OrderRejected += new EventHandler<TT.OrderRejectedEventArgs>(m_instrumentTradeSubscription_OrderRejected);
                    m_instrumentTradeSubscription.Start();
                }
                catch (Exception err)
                {
                    ErrorMessage("TT API FindInstrument Exception: {0}", err.Message);
                }
            }
            else if (e.IsFinal)
            {
                cout("TT API FindInstrument Instrument Not Found: {0}", e.Error);
            }
            else
            {
                cout("TT API FindInstrument Instrument Not Found: (Still Searching) {0}", e.Error);
            }
        }
        #endregion

        /// <summary>
        /// Event to notify the application there has been a change in the price feed
        /// Here we pull the values and publish them to the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void priceSubscription_FieldsUpdated(object sender, TT.FieldsUpdatedEventArgs e)
        {
            if (e.Error != null)
            {
                ErrorMessage("TT API FieldsUpdated Error: {0}", e.Error);
                return;
            }

            // Extract the values we want as Typed fields
            cout("TT PRICE UDPATE: " + e.Fields.Instrument.ToString());
            /*this.txtBidPrice.Text = e.Fields.GetDirectBidPriceField().FormattedValue;
            this.txtBidQty.Text = e.Fields.GetDirectBidQuantityField().FormattedValue;
            this.txtAskPrice.Text = e.Fields.GetDirectAskPriceField().FormattedValue;
            this.txtAskQty.Text = e.Fields.GetDirectAskQuantityField().FormattedValue;
            this.txtLastPrice.Text = e.Fields.GetLastTradedPriceField().FormattedValue;
            this.txtLastQty.Text = e.Fields.GetLastTradedQuantityField().FormattedValue;*/
        }

        #region SendOrder
        /// <summary>
        /// This function sets up the OrderProfile and submits
        /// the order using the InstrumentTradeSubscription SendOrder method.
        /// </summary>
        /// <param name="buySell">The side of the market to place the order on.</param>
        private void SendOrder(TTInstrument ttinstr, ZS.ZOrder o)
        {
            var orderSide = o.Side;
            var qtyStr = o.Qty.ToString();
            var priceStr = o.Price.ToString();
            var orderType = o.Type;
            var stopPriceStr = o.StopPrice.ToString();                 // TODO: WE NEED TO DEAL WITH STOP ORDERS!!!

            try
            {
                TT.OrderProfile orderProfile = new TT.OrderProfile(ttinstr.DefaultOrderFeed, ttinstr.Instrument, m_defaultCustomer.Customer);
                
                orderProfile.BuySell = TranslateSide(orderSide);                                        // Set for Buy or Sell.
                orderProfile.QuantityToWork = TT.Quantity.FromString(ttinstr.Instrument, qtyStr);       // Set the quantity.

                if (orderType == ZS.ZOrderType.Market)         // Market Order
                {
                    orderProfile.OrderType = TT.OrderType.Market;

                }
                else if (orderType == ZS.ZOrderType.Limit)     // Limit Order
                {
                    // Set the limit order price.
                    orderProfile.LimitPrice = TT.Price.FromString(ttinstr.Instrument, priceStr);
                }
                else if (orderType == ZS.ZOrderType.StopMarket)     // Stop Market Order
                {
                    orderProfile.OrderType = TT.OrderType.Market;                                       // Set the order type to "Market" for a market order.                    
                    orderProfile.Modifiers = TT.OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.                    
                    orderProfile.StopPrice = TT.Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }
                else if (orderType == ZS.ZOrderType.StopLimit)      // Stop Limit Order
                {                    
                    orderProfile.OrderType = TT.OrderType.Limit;                                        // Set the order type to "Limit" for a limit order.
                    orderProfile.Modifiers = TT.OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.
                    orderProfile.LimitPrice = TT.Price.FromString(ttinstr.Instrument, priceStr);        // Set the limit order price.
                    orderProfile.StopPrice = TT.Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }

                m_instrumentTradeSubscription.SendOrder(orderProfile);  // Send the order.

                cout("TT Order Send {0} {1}|{2}@{3}", orderProfile.SiteOrderKey, orderProfile.BuySell.ToString(), orderProfile.QuantityToWork.ToString(), LimitOrMarketPrice(orderProfile));
                /*// Update the GUI.
                txtOrderBook.Text += String.Format("Send {0} {1}|{2}@{3}{4}",
                    orderProfile.SiteOrderKey,
                    orderProfile.BuySell.ToString(),
                    orderProfile.QuantityToWork.ToString(),
                    orderProfile.OrderType == OrderType.Limit ? orderProfile.LimitPrice.ToString() : "Market Price",
                    System.Environment.NewLine);*/
            }
            catch (Exception err)
            {
                ErrorMessage(err.Message);
            }
        }
        #endregion

    } // end of CLASS

} // end of NAMESPACE

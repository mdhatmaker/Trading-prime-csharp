using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Tools.G;
using static Tools.GDate;
using ZeroSumAPI;
using TradingTechnologies.TTAPI;
using TradingTechnologies.TTAPI.Tradebook;
using TradingTechnologies.TTAPI.WinFormsHelpers;
using TradingTechnologies.TTAPI.CustomerDefaults;


namespace ZeroSumAPI
{
    public class TengineTT : TradingEngine, IDisposable
    {
        public static string Username = "PRIMEDTS2";
        public static string Password = "12345678";
        //public UniversalLoginTTAPI API { get { return m_TTAPI; } }
        public string DefaultCustomer { get { return m_lcd[0].Customer; } }

        // ****** ADD NEW PRODUCTS HERE!!! ******
        // Translate IQFeed symbol root into TT ProductKey
        static Dictionary<string, ProductKey> TTProductKeyTranslate = new Dictionary<string, ProductKey>()
        {
            { "@ES", new ProductKey("CME", ProductType.Future, "ES") },
            { "@VX", new ProductKey("CFE", ProductType.Future, "VX") },
            { "QHG", new ProductKey("CME", ProductType.Future, "HG") }
        };
        // ****** OR HERE IF NOT FUTURES!!! ******
        // Translate IQFeed symbol into TTInstrument struct
        static Dictionary<string, TTInstrument> TTInstrumentTranslate = new Dictionary<string, TTInstrument>()
        {
            { "M.CU3=LX", new TTInstrument(new ProductKey("LME", ProductType.Future, "CA"), "3M") }
        };
        // ****** AND ADD DIVISOR (decimal places) HERE FOR PROPER CONVERSION FROM INTEGER PRICE ******
        static Dictionary<string, int> TTPriceDecimalsTranslate = new Dictionary<string, int>()
        {
            { "@ES", 0 },       // zero decimal places in price
            { "@VX", 2 },       // two decimal places in price
            { "QHG", 0 },       // etc...
            { "M.CU3=LX", 2 }
        };

        private Thread m_workerThread;
        private Dictionary<uint, TTInstrument> _instruments = new Dictionary<uint, TTInstrument>();
        private OrderMap<string, TTOrder> _orders = new OrderMap<string, TTOrder>();

        #region Declare the TTAPI objects
        private UniversalLoginTTAPI m_TTAPI = null;
        private WorkerDispatcher m_disp = null;
        private bool m_disposed = false;
        private object m_lock = new object();
        private CustomerDefaultsSubscription m_cdSub;
        private List<InstrumentLookupSubscription> m_lreq = new List<InstrumentLookupSubscription>();
        private List<TimeAndSalesSubscription> m_lts = new List<TimeAndSalesSubscription>();
        private Dictionary<InstrumentKey, TradeSubscription> m_dtrd = new Dictionary<InstrumentKey, TradeSubscription>();
        private List<PriceSubscription> m_lprc = new List<PriceSubscription>();
        private List<FillsSubscription> m_lfil = new List<FillsSubscription>();
        private List<CustomerDefaultEntry> m_lcd = new List<CustomerDefaultEntry>();
        //private List<ContractDetails> m_lcd = new List<ContractDetails>();
        /*private XTraderModeTTAPI m_TTAPI = null;
        private InstrumentTradeSubscription m_instrumentTradeSubscription = null;*/
        #endregion


        public TengineTT()  //, TradingEngineCallbacks callbacks) : base(callbacks)
        {
            // TODO: Maybe we only create this TTAPI worker thread once (as a static, perhaps?)
            // Start TT API on a separate thread
            m_workerThread = new Thread(this.Start);
            m_workerThread.Name = "TengineTT TradingEngine Thread";
            m_workerThread.Start();
        }

        #region TradingEngine abstract overrides ---------------------------------------------------------------------------------------------------------------
        public override void Startup()
        {
        }

        public override void Shutdown()
        {
            //shutdownTTAPI();
        }

        // Expect 'symbol' as IQFeed Symbol ("@ESJ16", "@VXK16", "@VXQ17")
        public override uint CreateInstrument(uint iid, string symbol)
        {
            instruments[iid] = new ZInstrument(iid, symbol);
            _instruments[iid] = TranslateInstrument(symbol);                            // create new TTInstrument object
            return iid;
        }

        public override void Subscribe(uint iid)
        {
            //m_disp.BeginInvoke(SubscribeInstrument, _instruments[iid]);
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

            // Add this to the OrderMap which will allow us to convert to/from TTAPI orders
            var ttinstr = _instruments[iid];
            OrderProfile op = new OrderProfile(ttinstr.DefaultOrderFeed, ttinstr.Instrument, this.DefaultCustomer);
            var tto = new TTOrder(op);
            _orders.Add(tto.Key, tto, oid);

            return oid;
        }

        public override void SubmitOrder(uint oid)
        {
            ZOrder o = orders[oid];
            TTOrder tto = _orders[oid];
            //var order = _orders[oid];
            //GetBook(o).Insert(order);
            /*var ttinstr = _instruments[o.Iid];
            SendOrder(ttinstr, o);*/
            SendOrder(tto, o);
            o.SetState(ZOrderState.Working);        // TODO: set the state when we receive "order added" callback
        }

        public override void DeleteOrder(uint oid)
        {
            ZOrder o = orders[oid];
            TTOrder tto = _orders[oid];
            tto.Order.Delete();
            o.SetState(ZOrderState.Cancelled);      // TODO: set the state when we receive "order deleted" callback
        }

        public override void DeleteAllOrders()
        {
            foreach (ZOrder o in WorkingOrders)
            {
                DeleteOrder(o.Oid);
            }
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
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Helper Functions -------------------------------------------------------------------------------------------------------------------------------
        // Given an IQFeed Future Symbol (that ends in 'mYY' monthcode and 2-digit year)
        // Return (as out parameters) both the corresponding TT Symbol and the TT Month/Year (both as strings)
        // Return True if both Symbol and Month/Year were translated successfully; otherwise return False
        public static TTInstrument TranslateInstrument(string iqSymbol)
        {
            string mYY = GetmYY(iqSymbol);
            // symbol will either be the "root" (if it's a futures contract) OR the whole iqSymbol (if not)
            string symbol = iqSymbol;
            TTInstrument ttinstr;
            if (mYY != null)
            {
                symbol = iqSymbol.Substring(0, iqSymbol.Length - 3);
                ProductKey ttProductKey = TTProductKeyTranslate[symbol];
                string ttMonthYear = MonthYear(mYY);
                ttinstr = new TTInstrument(ttProductKey, ttMonthYear);
                ttinstr.PriceDecimals = TTPriceDecimalsTranslate[symbol];               // number of decimal places (n) for this instrument (divide integer price by 10^n)
                return ttinstr;
            }
            else
            {
                ttinstr = TTInstrumentTranslate[symbol];
                ttinstr.PriceDecimals = TTPriceDecimalsTranslate[symbol];               // number of decimal places (n) for this instrument (divide integer price by 10^n)
                return ttinstr;
            }
        }

        // Given a ZeroSumAPI order side
        // Return a BuySell order side
        public static BuySell TranslateSide(ZOrderSide side)
        {
            if (side == ZOrderSide.Buy)
                return BuySell.Buy;
            else if (side == ZOrderSide.Sell)
                return BuySell.Sell;
            else
                return BuySell.Unknown;
        }

        private string LimitOrMarketPrice(OrderProfile orderProfile)
        {
            return orderProfile.OrderType == OrderType.Limit ? orderProfile.LimitPrice.ToString() : "Market Price";
        }

        private string LimitOrMarketPrice(Order order)
        {
            return order.OrderType == OrderType.Limit ? order.LimitPrice.ToString() : "Market Price";
        }

        // This is NOT fast, because it is used infrequently (update to faster lookup if that changes in the future...)
        private uint GetIid(Instrument instr)
        {
            uint result = 0;
            foreach (uint iid in _instruments.Keys)
            {
                if (_instruments[iid].ProductKey == instr.Key.ProductKey && instr.Name.EndsWith(_instruments[iid].Contract))
                {
                    result = iid;
                    break;
                }
            }
            return result;
        }

        // Get the integer price from a ZOrder object and divide it by the proper power of 10 to get the converted decimal price (as a string)
        private string GetPriceString(ZOrder o)
        {
            return _instruments[o.Iid].ConvertPrice(o.Price).ToString();
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        #region TTAPI Startup and Shutdown ---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Create and start the Dispatcher
        /// </summary>
        public void Start()
        {
            cout("TengineTT::Start: Attaching dispatcher...");
            // Attach a WorkerDispatcher to the current thread
            m_disp = Dispatcher.AttachWorkerDispatcher();
            m_disp.BeginInvoke(new Action(Init));
            m_disp.Run();
        }

        /// <summary>
        /// Initialize TT API
        /// </summary>
        public void Init()
        {
            cout("TengineTT::Init: Initializing TTAPI...");
            // Use "Universal Login" Login Mode
            ApiInitializeHandler h = new ApiInitializeHandler(ttApiInitComplete);
            TTAPI.CreateUniversalLoginTTAPI(Dispatcher.Current, Username, Password, h);
        }

        /// <summary>
        /// Event notification for status of TT API initialization
        /// </summary>
        public void ttApiInitComplete(TTAPI api, ApiCreationException ex)
        {
            cout("TengineTT: TTAPI initialization complete");
            if (ex == null)
            {
                // Authenticate your credentials
                m_TTAPI = (UniversalLoginTTAPI)api;
                m_TTAPI.AuthenticationStatusUpdate += new EventHandler<AuthenticationStatusUpdateEventArgs>(apiInstance_AuthenticationStatusUpdate);    // for UniversalLoginTTAPI
                //m_TTAPI.ConnectionStatusUpdate += new EventHandler<ConnectionStatusUpdateEventArgs>(ttapiInstance_ConnectionStatusUpdate);              // for XTraderModeTTAPI
                m_TTAPI.Start();

                m_cdSub = new CustomerDefaultsSubscription(m_TTAPI.Session, Dispatcher.Current);
                m_cdSub.CustomerDefaultsChanged += new EventHandler(m_cd_CustomerDefaultsChanged);
                m_cdSub.Start();
            }
            else
            {
                cout("TengineTT: TTAPI Initialization Failed: {0}", ex.Message);
                Dispose();
            }
        }

        /// <summary>
        /// Event notification for status of authentication
        /// </summary>
        public void apiInstance_AuthenticationStatusUpdate(object sender, AuthenticationStatusUpdateEventArgs e)
        {
            if (e.Status.IsSuccess)
            {
                m_isStarted = true;
                cout("TengineTT: TT Login successful!");
            }
            else
            {
                cout("TengineTT: TT Login failed: {0}", e.Status.StatusMessage);
                Dispose();
            }
        }

        /// <summary>
        /// Shuts down the TT API
        /// </summary>
        public void Dispose()
        {
            lock (m_lock)
            {
                if (!m_disposed)
                {
                    // Dispose of all request objects
                    if (m_cdSub != null)
                    {
                        m_cdSub.CustomerDefaultsChanged -= m_cd_CustomerDefaultsChanged;
                        m_cdSub.Dispose();
                        m_cdSub = null;
                    }

                    // TODO: DISPOSE OF ALL TT OBJECTS HERE!
                    /*if (m_instrumentTradeSubscription != null)
                    {
                        m_instrumentTradeSubscription.OrderAdded -= m_instrumentTradeSubscription_OrderAdded;
                        m_instrumentTradeSubscription.OrderRejected -= m_instrumentTradeSubscription_OrderRejected;
                        m_instrumentTradeSubscription.Dispose();
                        m_instrumentTradeSubscription = null;
                    }

                    if (m_priceSubscription != null)
                    {
                        m_priceSubscription.FieldsUpdated -= m_priceSubscription_FieldsUpdated;
                        m_priceSubscription.Dispose();
                        m_priceSubscription = null;
                    }*/

                    // Begin shutdown the TT API
                    TTAPI.ShutdownCompleted += new EventHandler(TTAPI_ShutdownCompleted);
                    TTAPI.Shutdown();

                    m_disposed = true;
                }
            }
        }

        /// <summary>
        /// Event notification for completion of TT API shutdown
        /// </summary>
        public void TTAPI_ShutdownCompleted(object sender, EventArgs e)
        {
            // Shutdown the Dispatcher
            if (m_disp != null)
            {
                m_disp.BeginInvokeShutdown();
                m_disp = null;
            }

            m_isShutdown = true;

            // Dispose of any other objects / resources
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        #region TTAPI event callbacks --------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Event notification for instrument lookup
        /// </summary>
        public void req_Update(object sender, InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.Instrument != null && e.Error == null)
            {
                cout("Subscribed to Instrument: {0}", e.Instrument.Key);

                _instruments[GetIid(e.Instrument)].FoundInstrument(e.Instrument);

                // Market Depth subscription (or just Inside Market if you change the PriceSubscriptionSettings)
                var prcSub = new PriceSubscription(e.Instrument, Dispatcher.Current);
                //psSub.Settings = new PriceSubscriptionSettings(PriceSubscriptionType.InsideMarket);
                prcSub.Settings = new PriceSubscriptionSettings(PriceSubscriptionType.MarketDepth);
                prcSub.FieldsUpdated += new FieldsUpdatedEventHandler(m_prc_FieldsUpdated);
                m_lprc.Add(prcSub);
                prcSub.Start();

                // Time & Sales subscription
                var tsSub = new TimeAndSalesSubscription(e.Instrument, Dispatcher.Current);
                tsSub.Update += new EventHandler<TimeAndSalesEventArgs>(tsSub_Update);
                m_lts.Add(tsSub);
                tsSub.Start();

                // Fills subscription
                var filSub = new FillsSubscription(m_TTAPI.Session, Dispatcher.Current);
                filSub.FillAdded += new EventHandler<FillAddedEventArgs>(m_fil_FillAdded);
                filSub.FillAmended += new EventHandler<FillAmendedEventArgs>(m_fil_FillAmended);
                filSub.FillBookDownload += new EventHandler<FillBookDownloadEventArgs>(m_fil_FillBookDownload);
                filSub.FillDeleted += new EventHandler<FillDeletedEventArgs>(m_fil_FillDeleted);
                filSub.FillListEnd += new EventHandler<FillListEventArgs>(m_fil_FillListEnd);
                filSub.FillListStart += new EventHandler<FillListEventArgs>(m_fil_FillListStart);
                m_lfil.Add(filSub);
                filSub.Start();

                // Trade Subscription (to listen for order / fill events only for orders submitted through it)
                var trdSub = new InstrumentTradeSubscription(m_TTAPI.Session, Dispatcher.Current, e.Instrument, true, true, false, false);
                trdSub.OrderUpdated += new EventHandler<OrderUpdatedEventArgs>(m_trd_OrderUpdated);
                trdSub.OrderAdded += new EventHandler<OrderAddedEventArgs>(m_trd_OrderAdded);
                trdSub.OrderDeleted += new EventHandler<OrderDeletedEventArgs>(m_trd_OrderDeleted);
                trdSub.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_trd_OrderFilled);
                trdSub.OrderRejected += new EventHandler<OrderRejectedEventArgs>(m_trd_OrderRejected);
                m_dtrd[e.Instrument.Key] = trdSub;
                trdSub.Start();

            }
            else if (e.IsFinal)
            {
                // Instrument was not found and TT API has given up looking for it
                cout("Cannot find instrument, and giving up: {0}", e.Error.Message);
                Dispose();
            }
            else
            {
                cout("Cannot find instrument: {0}", e.Error.Message);
            }
        }

        /// <summary>
        /// Event notification for CustomerDefaultsChanged subscription
        /// </summary>
        void m_cd_CustomerDefaultsChanged(object sender, EventArgs e)
        {
            m_lcd.Clear();

            CustomerDefaultsSubscription cds = sender as CustomerDefaultsSubscription;
            foreach (CustomerDefaultEntry entry in cds.CustomerDefaults)
            {
                m_lcd.Add(entry);
            }
        }

        /// <summary>
        /// Event notification for trade updates
        /// </summary>
        public void tsSub_Update(object sender, TimeAndSalesEventArgs e)
        {
            // process the update
            if (e.Error == null)
            {
                foreach (TimeAndSalesData tsData in e.Data)
                {
                    Console.WriteLine("{0} : {1} --> LTP/LTQ : {2}/{3}",
                        Thread.CurrentThread.Name, e.Instrument.Name, tsData.TradePrice, tsData.TradeQuantity);
                }

                // Add strategy logic here
            }
        }

        /// <summary>
        /// Event notification for fill download beginning for a given gateway
        /// </summary>
        void m_fil_FillListStart(object sender, FillListEventArgs e)
        {
            Console.WriteLine("Begin adding fills from {0}", e.FeedConnectionKey.ToString());
        }

        /// <summary>
        /// Event notification for fill download completion for a given gateway
        /// </summary>
        void m_fil_FillListEnd(object sender, FillListEventArgs e)
        {
            Console.WriteLine("Finished adding fills from {0}", e.FeedConnectionKey.ToString());
        }

        /// <summary>
        /// Event notification for fill deletion
        /// </summary>
        void m_fil_FillDeleted(object sender, FillDeletedEventArgs e)
        {
            Console.WriteLine("Fill Deleted:");
            Console.WriteLine("    Fill: FillKey={0}, InstrKey={1}, Qty={2}, MatchPrice={3}", e.Fill.FillKey, e.Fill.InstrumentKey, e.Fill.Quantity, e.Fill.MatchPrice);
        }

        /// <summary>
        /// Event notification for fills being downloaded
        /// </summary>
        void m_fil_FillBookDownload(object sender, FillBookDownloadEventArgs e)
        {
            foreach (Fill f in e.Fills)
            {
                Console.WriteLine("Fill from download:");
                Console.WriteLine("    Fill: FillKey={0}, InstrKey={1}, Qty={2}, MatchPrice={3}", f.FillKey, f.InstrumentKey, f.Quantity, f.MatchPrice);
            }
        }

        /// <summary>
        /// Event notification for fill amendments
        /// </summary>
        void m_fil_FillAmended(object sender, FillAmendedEventArgs e)
        {
            Console.WriteLine("Fill Amended:");
            Console.WriteLine("    Old Fill: FillKey={0}, InstrKey={1}, Qty={2}, MatchPrice={3}", e.OldFill.FillKey, e.OldFill.InstrumentKey, e.OldFill.Quantity, e.OldFill.MatchPrice);
            Console.WriteLine("    New Fill: FillKey={0}, InstrKey={1}, Qty={2}, MatchPrice={3}", e.NewFill.FillKey, e.NewFill.InstrumentKey, e.NewFill.Quantity, e.NewFill.MatchPrice);
        }

        /// <summary>
        /// Event notification for a new fill
        /// </summary>
        void m_fil_FillAdded(object sender, FillAddedEventArgs e)
        {
            Console.WriteLine("Fill Added:");
            Console.WriteLine("    Fill: FillKey={0}, InstrKey={1}, Qty={2}, MatchPrice={3}", e.Fill.FillKey, e.Fill.InstrumentKey, e.Fill.Quantity, e.Fill.MatchPrice);
        }

        /// <summary>
        /// Event notification for price update
        /// </summary>
        void m_prc_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.UpdateType == UpdateType.Snapshot)
                {
                    // Received a market data snapshot

                    dout("Ask Depth Snapshot");
                    int askDepthLevels = e.Fields.GetLargestCurrentDepthLevel(FieldId.BestAskPrice);
                    for (int i = 0; i < askDepthLevels; i++)
                    {
                        dout("    Level={0} Qty={1} Price={2}", i, e.Fields.GetBestAskQuantityField(i).Value.ToInt(), e.Fields.GetBestAskPriceField(i).Value.ToString());
                    }

                    dout("Bid Depth Snapshot");
                    int bidDepthLevels = e.Fields.GetLargestCurrentDepthLevel(FieldId.BestBidPrice);
                    for (int i = 0; i < bidDepthLevels; i++)
                    {
                        dout("    Level={0} Qty={1} Price={2}", i, e.Fields.GetBestBidQuantityField(i).Value.ToInt(), e.Fields.GetBestBidPriceField(i).Value.ToString());
                    }
                }
                else
                {
                    // Only some fields have changed

                    dout("Depth Updates");
                    int depthLevels = e.Fields.GetLargestCurrentDepthLevel();
                    for (int i = 0; i < depthLevels; i++)
                    {
                        if (e.Fields.GetChangedFieldIds(i).Length > 0)
                        {
                            dout("Level={0}", i);
                            foreach (FieldId id in e.Fields.GetChangedFieldIds(i))
                            {
                                dout("    {0} : {1}", id.ToString(), e.Fields[id, i].FormattedValue);
                            }
                        }
                    }
                }
            }
            else
            {
                if (e.Error.IsRecoverableError == false)
                {
                    ErrorMessage("Unrecoverable price subscription error: {0}", e.Error.Message);
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Event notification for order rejected
        /// </summary>
        void m_trd_OrderRejected(object sender, OrderRejectedEventArgs e)
        {
            Console.WriteLine("Order was rejected: [{0}] '{1}'", e.Order.ToString(), e.Message);
        }

        /// <summary>
        /// Event notification for order filled
        /// </summary>
        void m_trd_OrderFilled(object sender, OrderFilledEventArgs e)
        {
            if (e.FillType == FillType.Full)
            {
                Console.WriteLine("Order was fully filled for {0} at {1}.", e.Fill.Quantity, e.Fill.MatchPrice);
            }
            else
            {
                Console.WriteLine("Order was partially filled for {0} at {1}.", e.Fill.Quantity, e.Fill.MatchPrice);
            }

            //e.Fill.InstrumentKey
            //Console.WriteLine("Average Buy Price = {0} : Net Position = {1} : P&L = {2}", m_ts.ProfitLossStatistics.BuyAveragePrice, m_ts.ProfitLossStatistics.NetPosition, m_ts.ProfitLoss.AsPrimaryCurrency);
        }

        /// <summary>
        /// Event notification for order deleted
        /// </summary>
        void m_trd_OrderDeleted(object sender, OrderDeletedEventArgs e)
        {
            Console.WriteLine("Order was deleted.");
        }

        /// <summary>
        /// Event notification for order added
        /// </summary>
        void m_trd_OrderAdded(object sender, OrderAddedEventArgs e)
        {
            TTOrder tto = _orders[e.Order.SiteOrderKey];
            tto.AddedOrder(e.Order);
            Console.WriteLine("Order was added with price of {0}.", e.Order.LimitPrice);
        }

        /// <summary>
        /// Event notification for order update
        /// </summary>
        void m_trd_OrderUpdated(object sender, OrderUpdatedEventArgs e)
        {
            Console.WriteLine("Order was updated with price of {0}.", e.NewOrder.LimitPrice);
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        public void SubscribeInstrument(TTInstrument ttinstr)
        {
            cout("TengineTT: Subscribing to Instrument: {0}", ttinstr);

            InstrumentLookupSubscription req = new InstrumentLookupSubscription(m_TTAPI.Session, m_disp, ttinstr.ProductKey, ttinstr.Contract);
            req.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(req_Update);
            m_lreq.Add(req);
            req.Start();
        }

        public void UnsubscribeInstrument(TTInstrument ttinstr)
        {
            cout("TengineTT: Unsubscribing from Instrument: {0}", ttinstr);
        }

        #region SendOrder
        /// <summary>
        /// This function sets up the OrderProfile and submits
        /// the order using the InstrumentTradeSubscription SendOrder method.
        /// </summary>
        /// <param name="buySell">The side of the market to place the order on.</param>
        private void SendOrder(TTOrder tto, ZOrder o)
        {
            ZOrderSide orderSide = o.Side;
            string qtyStr = o.Qty.ToString();
            string priceStr = GetPriceString(o);
            ZOrderType orderType = o.Type;
            string stopPriceStr = o.StopPrice.ToString();                 // TODO: WE NEED TO DEAL WITH STOP ORDERS!!!
            TTInstrument ttinstr = _instruments[o.Iid];

            cout("TengineTT::SendOrder: {0}", o.ToString());
            try
            {
                //OrderProfile op = new OrderProfile(ttinstr.DefaultOrderFeed, ttinstr.Instrument, this.DefaultCustomer);
                OrderProfile op = tto.Profile;

                op.BuySell = TranslateSide(orderSide);                                        // Set for Buy or Sell.
                op.QuantityToWork = Quantity.FromString(ttinstr.Instrument, qtyStr);       // Set the quantity.

                if (orderType == ZOrderType.Market)             // Market Order
                {
                    op.OrderType = OrderType.Market;

                }
                else if (orderType == ZOrderType.Limit)         // Limit Order
                {
                    // Set the limit order price.
                    op.LimitPrice = Price.FromString(ttinstr.Instrument, priceStr);
                }
                else if (orderType == ZOrderType.StopMarket)    // Stop Market Order
                {
                    op.OrderType = OrderType.Market;                                       // Set the order type to "Market" for a market order.                    
                    op.Modifiers = OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.                    
                    op.StopPrice = Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }
                else if (orderType == ZOrderType.StopLimit)     // Stop Limit Order
                {
                    op.OrderType = OrderType.Limit;                                        // Set the order type to "Limit" for a limit order.
                    op.Modifiers = OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.
                    op.LimitPrice = Price.FromString(ttinstr.Instrument, priceStr);        // Set the limit order price.
                    op.StopPrice = Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }

                //_orders[op.SiteOrderKey]
                m_dtrd[ttinstr.InstrumentKey].SendOrder(op);    // Send the order.
                
                cout("TengineTT::SendOrder: TT Order Send {0} {1}|{2}@{3}", op.SiteOrderKey, op.BuySell.ToString(), op.QuantityToWork.ToString(), LimitOrMarketPrice(op));
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


    } // end of CLASS TengineTT


} // end of NAMESPACE

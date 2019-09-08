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
    public class TengineTT4 : TradingEngine
    {        
        // ****** ADD NEW PRODUCTS HERE!!! ******
        // Translate IQFeed symbol root into TT ProductKey
        static Dictionary<string, ProductKey> TTProductKeyTranslate = new Dictionary<string, ProductKey>()
        {
            { "@ES", new ProductKey("CME", ProductType.Future, "ES") },
            { "@VX", new ProductKey("CME", ProductType.Future, "VX") }
        };
        // ****** OR HERE IF NOT FUTURES!!! ******
        // Translate IQFeed symbol into TTInstrument struct
        static Dictionary<string, TTInstrument> TTInstrumentTranslate = new Dictionary<string, TTInstrument>()
        {
            { "M.CU3=LX", new TTInstrument(new ProductKey("LME", ProductType.Future, "CA"), "3M") }
        };

        private Dictionary<uint, TTInstrument> _instruments = new Dictionary<uint, TTInstrument>();

        private frmPriceUpdate m_tt;

        public static string Username = "PRIMEDTS2";
        public static string Password = "12345678";

        public UniversalLoginTTAPI API { get { return m_ttapi; } }

        #region Declare the TTAPI objects
        private UniversalLoginTTAPI m_ttapi = null;
        private WorkerDispatcher m_disp = null;
        private bool m_disposed = false;
        private object m_lock = new object();
        private List<InstrumentLookupSubscription> m_lreq = new List<InstrumentLookupSubscription>();
        private List<TimeAndSalesSubscription> m_ltsSub = new List<TimeAndSalesSubscription>();
        private List<TradeSubscription> m_ltrd = new List<TradeSubscription>();
        private List<ContractDetails> m_lcd = new List<ContractDetails>();
        /*private XTraderModeTTAPI m_TTAPI = null;
        private CustomerDefaultsSubscription m_customerDefaultsSubscription = null;
        private InstrumentTradeSubscription m_instrumentTradeSubscription = null;
        private PriceSubscription m_priceSubscription = null;
        private List<CustomerDefaults.CustomerDefaultEntry> customers = new List<CustomerDefaultEntry>();
        private CustomerDefaults.CustomerDefaultEntry m_defaultCustomer;*/
        #endregion


        public TengineTT4(frmPriceUpdate ttForm)  //, TradingEngineCallbacks callbacks) : base(callbacks)
        {
            m_tt = ttForm;

            // Add translations from IQFeed symbols to definition fields of a TT Instrument
            //_translateSymbol.Add("@ESZ17", new TTInstrument("CME", ProductType.Future, "ES", "Dec17"));
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
            _instruments.Add(iid, TranslateInstrument(symbol));
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
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Helper Functions -------------------------------------------------------------------------------------------------------------------------------
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
                ProductKey ttProductKey = TTProductKeyTranslate[symbol];
                string ttMonthYear = MonthYear(mYY);
                return new TTInstrument(ttProductKey, ttMonthYear);
            }
            else
                return TTInstrumentTranslate[symbol];
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
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


        public void SubscribeInstrument(TTInstrument ttinstr)
        {
            cout("TengineTT: Subscribing to Instrument: {0}", ttinstr);

            m_tt.FindInstrument(ttinstr.ProductKey, ttinstr.Contract);
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
        private void SendOrder(TTInstrument ttinstr, ZOrder o)
        {
            var orderSide = o.Side;
            var qtyStr = o.Qty.ToString();
            var priceStr = o.Price.ToString();
            var orderType = o.Type;
            var stopPriceStr = o.StopPrice.ToString();                 // TODO: WE NEED TO DEAL WITH STOP ORDERS!!!

            cout("TeTTApi::SendOrder: {0}", o.ToString());
            try
            {
                CustomerDefaultEntry m_defaultCustomer = null;
                OrderProfile orderProfile = new OrderProfile(ttinstr.DefaultOrderFeed, ttinstr.Instrument, m_defaultCustomer.Customer);

                orderProfile.BuySell = TranslateSide(orderSide);                                        // Set for Buy or Sell.
                orderProfile.QuantityToWork = Quantity.FromString(ttinstr.Instrument, qtyStr);       // Set the quantity.

                if (orderType == ZOrderType.Market)         // Market Order
                {
                    orderProfile.OrderType = OrderType.Market;

                }
                else if (orderType == ZOrderType.Limit)     // Limit Order
                {
                    // Set the limit order price.
                    orderProfile.LimitPrice = Price.FromString(ttinstr.Instrument, priceStr);
                }
                else if (orderType == ZOrderType.StopMarket)     // Stop Market Order
                {
                    orderProfile.OrderType = OrderType.Market;                                       // Set the order type to "Market" for a market order.                    
                    orderProfile.Modifiers = OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.                    
                    orderProfile.StopPrice = Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }
                else if (orderType == ZOrderType.StopLimit)      // Stop Limit Order
                {
                    orderProfile.OrderType = OrderType.Limit;                                        // Set the order type to "Limit" for a limit order.
                    orderProfile.Modifiers = OrderModifiers.Stop;                                    // Set the order modifiers to "Stop" for a stop order.
                    orderProfile.LimitPrice = Price.FromString(ttinstr.Instrument, priceStr);        // Set the limit order price.
                    orderProfile.StopPrice = Price.FromString(ttinstr.Instrument, stopPriceStr);     // Set the stop price.
                }

                //m_instrumentTradeSubscription.SendOrder(orderProfile);  // Send the order.
                
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


    } // end of CLASS TengineTT


    /// <summary>
    /// struct for encapsulating contract details
    /// </summary>
    public struct ContractDetails
    {
        public MarketKey m_marketKey;
        public ProductType m_productType;
        public string m_product;
        public string m_contract;

        public ContractDetails(MarketKey mk, ProductType pt, string prod, string cont)
        {
            m_marketKey = mk;
            m_productType = pt;
            m_product = prod;
            m_contract = cont;
        }
    } // end of STRUCT ContractDetails

} // end of NAMESPACE

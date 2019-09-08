using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.G;
using OME = OrderMatchingEngine.OrderBook;
namespace ZeroSumAPI
{
    public class TengineTest : TradingEngine
    {
        private Dictionary<uint, OME.Instrument> _instruments = new Dictionary<uint, OME.Instrument>();
        private Dictionary<uint, OME.BuyOrders> _buyOrderBook = new Dictionary<uint, OME.BuyOrders>();
        private Dictionary<uint, OME.SellOrders> _sellOrderBook = new Dictionary<uint, OME.SellOrders>();
        private Dictionary<uint, OME.Order> _orders = new Dictionary<uint, OME.Order>();

        private Dictionary<uint, OME.Trades> _trades = new Dictionary<uint, OME.Trades>();
        //private Dictionary<uint, OME.Trades.InMemoryTradeProcessor> _tradeProcessor;

        public TengineTest()
        {
        }

        private OME.Orders GetBook(ZOrder o)
        {
            if (o.IsBuy)
                return _buyOrderBook[o.Iid];
            else
                return _sellOrderBook[o.Iid];
        }

        private OME.Order.BuyOrSell GetSide(ZOrder o)
        {
            return (o.Side == ZOrderSide.Buy) ? OME.Order.BuyOrSell.Buy : OME.Order.BuyOrSell.Sell;
        }

        #region TradingEngine abstract overrides
        public override void Startup()
        {
            cout("TestAPI: Startup");
            m_isShutdown = false;
        }

        public override void Shutdown()
        {
            cout("TestAPI: Shutdown");
            m_isShutdown = true;
        }

        public override void Subscribe(uint iid)
        {
            cout("TE Child: Subscribe");
        }

        public override void Unsubscribe(uint iid)
        {
            cout("TE Child: Unsubscribe");
        }

        public override uint CreateInstrument(uint iid, string symbol)
        {
            instruments[iid] = new ZInstrument(iid, symbol);
            _instruments[iid] = new OME.Instrument(symbol);

            var instrument = _instruments[iid];
            _buyOrderBook[iid] = new OME.BuyOrders(instrument);
            _sellOrderBook[iid] = new OME.SellOrders(instrument);
            _trades[iid] = new OME.Trades(instrument);
            //_tradeProcessor = _trades.TradeProcessingStrategy as Trades.InMemoryTradeProcessor;

            return iid;
        }

        public override uint CreateOrder(uint iid, ZOrderSide side, int price, uint qty, ZOrderType type=ZOrderType.Limit, ZOrderTimeInForce tif=ZOrderTimeInForce.GoodTilDate)
        {
            uint oid = GenerateOrderId();
            var o = new ZOrder(oid, iid, side, price, qty, type, tif);
            orders[oid] = o;

            _orders[o.Oid] = new OME.EquityOrder(_instruments[o.Iid], OME.Order.OrderTypes.GoodUntilCancelled, GetSide(o), o.Price, o.Qty);

            return oid;
        }

        public override void SubmitOrder(uint oid)
        {
            ZOrder o = orders[oid];
            var order = _orders[oid];
            GetBook(o).Insert(order);
            o.SetState(ZOrderState.Working);
        }

        public override void DeleteOrder(uint oid)
        {
            ZOrder o = orders[oid];
            var order = _orders[oid];
            GetBook(o).Remove(order);
            o.SetState(ZOrderState.Cancelled);
        }

        public override void DeleteAllOrders()
        {
            throw new NotImplementedException();
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

        public override void PrintBook()
        {
            cout("=================================================================================");
            foreach (uint iid in _buyOrderBook.Keys)
            {
                var bob = _buyOrderBook[iid];
                if (bob.Count() > 0)
                {
                    cout("BuyOrders [{0}]:", iid);
                    foreach (var order in bob)
                        Console.WriteLine(Str(order));
                }
                var sob = _sellOrderBook[iid];
                if (sob.Count() > 0)
                {
                    cout("SellOrders [{0}]:", iid);
                    foreach (var order in sob)
                        Console.WriteLine(Str(order));
                }

                /*if (OME.OrderProcessing.OrderProcessor.TryMatchOrder(  m_BuyOrder, m_SellOrders, m_Trades)) ;
                Trade trade = m_TradeProcessor.Trades[0];
                Assert.That(trade.Instrument, Is.EqualTo(m_Instrument));
                Assert.That(trade.Price, Is.EqualTo(m_SellOrder.Price));
                Assert.That(trade.Quantity, Is.EqualTo(buyQuantity));*/
                //cout("---------------------------------------------------------------------------------");
            }
            cout("=================================================================================");
        }
        #endregion


    } // end of CLASS

} // end of NAMESPACE

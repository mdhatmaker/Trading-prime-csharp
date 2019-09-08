using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZeroSumAPI
{
    public abstract class TradingEngine
    {
        protected TradingEngineCallbacks m_callbacks;

        protected Dictionary<uint, ZInstrument> instruments = new Dictionary<uint, ZInstrument>();
        protected Dictionary<uint, ZOrder> orders = new Dictionary<uint, ZOrder>();

        protected IEnumerable<ZOrder> WorkingOrders { get { return orders.Values.Where(x => x.State == ZOrderState.Working); } }

        protected bool m_isShutdown = false, m_shutdownInProcess = false, m_isStarted = false;

        private uint nextUniqueOrderId = 1001;

        public TradingEngine()
        {
        }

        public virtual bool IsStarted { get { return m_isStarted; } }
        public virtual bool IsShutdown { get { return m_isShutdown; } }

        public abstract void Startup();
        public abstract void Shutdown();

        public abstract void Subscribe(uint iid);
        public abstract void Unsubscribe(uint iid);

        public abstract uint CreateInstrument(uint iid, string symbol);

        public abstract uint CreateOrder(uint iid, ZOrderSide side, int price, uint qty, ZOrderType type=ZOrderType.Limit, ZOrderTimeInForce tif=ZOrderTimeInForce.GoodTilDate);

        public abstract void SubmitOrder(uint oid);

        public abstract void DeleteOrder(uint oid);
        public abstract void DeleteAllOrders();

        public abstract void ModifyOrder(uint oid, int price, uint qty);
        public abstract void ModifyOrder(uint oid, int price);
        public abstract void ModifyOrder(uint oid, uint qty);

        public abstract ZOrder GetOrder(uint oid);

        public virtual void PrintBook()
        {
        }

        public uint GenerateOrderId()
        {
            //Id = Interlocked.Increment(ref GlobalOrderId);
            //CreationTime = DateTime.Now;
            return nextUniqueOrderId++;
        }

        public void SetTradingEngineCallbacks(TradingEngineCallbacks callbacks)
        {
            m_callbacks = callbacks;
        }
    } // end of CLASS




    public delegate void FillCallback(uint intrumentId, uint orderId, int price, uint qty);
    public delegate void TradeCallback(uint instrumentId, int price, uint qty);
    public delegate void MarketUpdateCallback(uint instrumentId, ZPriceLevels bids, ZPriceLevels asks);
    public delegate void RejectCallback(uint instrumentId, uint orderId);

    public interface TradingEngineCallbacks
    {
        void Fill(uint iid, uint oid, int price, uint qty);
        void Trade(uint iid, int price, uint qty);
        void MarketUpdate(uint iid, ZPriceLevels bids, ZPriceLevels asks);
        void Reject(uint iid, uint oid);

    } // end of INTERFACE


} // end of NAMESPACE

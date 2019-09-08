using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSumAPI
{
    public enum ZOrderSide { Buy, Sell, Unknown };
    public enum ZOrderState { New, Submitted, Working, PartiallyFilled, Filled, Cancelled, Rejected };
    public enum ZOrderType { Limit, Market, StopLimit, StopMarket }; //, StopLoss };
    public enum ZOrderTimeInForce { GoodTilCancelled, GoodTilDate, ImmediateOrCancel };


    public class ZOrder
    {
        public uint Oid { get; private set; }
        public uint Iid { get; private set; }
        public ZOrderSide Side { get; private set; }
        public int Price { get; private set; }
        public uint Qty { get; private set; }
        public ZOrderType Type { get; set; }
        public ZOrderTimeInForce TimeInForce { get; set; }
        public ZOrderState State { get; private set; }
        public int? StopPrice { get; private set; }

        //private static Int64 GlobalOrderId;
        //public DateTime CreationTime { get; private set; }
        //public Int64 Id { get; private set; }

        public bool IsBuy { get { return Side == ZOrderSide.Buy; } }
        public bool IsSell { get { return Side == ZOrderSide.Sell; } }

        public ZOrder(uint oid, uint iid, ZOrderSide side, int price, uint qty, ZOrderType type, ZOrderTimeInForce timeInForce, int? stopPrice=null)
        {
            this.Oid = oid;
            this.Iid = iid;
            this.Side = side;
            this.Price = price;
            this.Qty = qty;
            this.Type = type;
            this.TimeInForce = timeInForce;
            this.State = ZOrderState.New;
            this.StopPrice = stopPrice;

            //Id = Interlocked.Increment(ref GlobalOrderId);
            //CreationTime = DateTime.Now;
        }

        public void SetState(ZOrderState state)
        {
            this.State = state;
        }

        public void SetPrice(int price)
        {
            this.Price = price;
        }

        public void SetQty(uint qty)
        {
            this.Qty = qty;
        }

        public override string ToString()
        {
            return string.Format("oid={0} iid={1} {2} {3}:{4}  {5} {6}  ({7})", Oid, Iid, Side, Price, Qty, Type, TimeInForce, State);
        }

    } // end of CLASS

} // end of NAMESPACE

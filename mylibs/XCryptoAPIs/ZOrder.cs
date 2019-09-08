using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CryptoAPIs
{
    #region ---------- ENUMS ------------------------------------------------------------------------------------------------
    public enum OrderSide
    {
        Buy = 1, Sell = 2
    }

    public enum OrderType
    {
        Limit = 1, Market = 2, StopLimit = 3, StopMarket = 4
    }

    public enum OrderTimeInForce
    {
        GTC = 1, Day = 2
    }

    public enum OrderStatus
    {
        Unknown = 0, Pending = 1, Open = 2, Active = 3, PartialFilled = 4, Filled = 5, Closed = 6, Canceled = 7, Rejected = 8, Expired = 9
    }
    #endregion --------------------------------------------------------------------------------------------------------------

    public delegate void OrdersUpdateHandler(object sender, OrdersUpdateArgs e);

    public class ZOrder : IDataRow
    {
        public CryptoExch Exchange;
        public string ExchangeSymbol;
        public OrderType OrderType;
        public OrderSide Side;
        public decimal Price;
        public decimal OrigQty;
        public decimal StopPrice;
        public OrderTimeInForce TimeInForce;
        public string ClientOrderId;
        public string[] OrderIds;
        public string OrderId { get { return OrderIds[0]; } }
        public decimal ExecutedQty;
        public OrderStatus Status;
        public string Time;

        public string Key { get => Exchange.ToString() + "_" + OrderId; set => throw new NotImplementedException(); }

        public static string[] Columns = { "Exchange", "Symbol", "OrderType", "Side", "Price", "OrigQty", "ExecutedQty", "Status", "Time", "btnCxl" };

        public string[] GetCells()
        {
            return new string[] { Exchange.ToString(), ExchangeSymbol, OrderType.ToString(), Side.ToString(), Price.ToString(), OrigQty.ToString(), ExecutedQty.ToString(), Status.ToString(), Time, "" };
        }

        public ZOrder(Exchange.Clients.BitFlyer.ChildOrder co)
        {
            Exchange = CryptoExch.BITFLYER;
            ExchangeSymbol = co.ProductCode;
            OrderType = (co.ChildOrderType == CryptoAPIs.Exchange.Clients.BitFlyer.ChildOrderType.Limit ? OrderType.Limit : OrderType.Market);
            Side = Translate.Side(co.Side);
            OrigQty = (decimal)co.Size;
            Price = (decimal)co.Price;
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            ClientOrderId = co.ChildOrderAcceptanceId;
            OrderIds = new string[] { co.ChildOrderId };
            ExecutedQty = (decimal)co.ExecutedSize;
            Status = Translate.Status(co.ChildOrderState);
            Time = co.ExpireDate;
        }

        public ZOrder(string pair, Exchange.Clients.Poloniex.TradeOrder to)
        {
            Exchange = CryptoExch.POLONIEX;
            ExchangeSymbol = pair;
            OrderType = OrderType.Limit;
            Side = Translate.Side(to.Type);
            OrigQty = to.AmountBase;                                // TODO: Or should this be AmountQuote?
            Price = to.PricePerCoin;
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            //ClientOrderId = or.id;
            OrderIds = new string[] { to.IdOrder.ToString() };
            //ExecutedQty = or.AmountFilled;
            //Status = Translate.Status(to.status, CryptoExch.BITSTAMP);
            //Time = to.datetime;
        }

        public ZOrder(Exchange.Clients.Bitstamp.OpenOrderResponse or)
        {
            Exchange = CryptoExch.BITSTAMP;
            ExchangeSymbol = "BTCUSD";
            OrderType = OrderType.Limit;
            Side = Translate.Side(or.type, CryptoExch.BITSTAMP);
            OrigQty = decimal.Parse(or.amount);
            Price = decimal.Parse(or.price);
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            //ClientOrderId = or.id;
            OrderIds = new string[] { or.id };
            //ExecutedQty = or.AmountFilled;
            Status = Translate.Status(or.status, CryptoExch.BITSTAMP);
            Time = or.datetime;
        }

        public ZOrder(Exchange.Clients.BitFinex.OrderStatusResponse or)
        {
            Exchange = CryptoExch.BITFINEX;
            ExchangeSymbol = or.symbol;
            OrderType = OrderType.Limit;
            Side = Translate.Side(or.type, CryptoExch.BITFINEX);
            OrigQty = decimal.Parse(or.original_amount);
            Price = decimal.Parse(or.price);
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            //ClientOrderId = or.id;
            OrderIds = new string[] { or.id };
            ExecutedQty = decimal.Parse(or.executed_amount);
            if (bool.Parse(or.is_live))
                Status = OrderStatus.Active;
            else if (bool.Parse(or.is_cancelled))
                Status = OrderStatus.Canceled;
            else
                Status = OrderStatus.Unknown;
            Time = or.timestamp;
        }

        public ZOrder(Exchange.Clients.ItBit.Order o)
        {
            Exchange = CryptoExch.ITBIT;
            ExchangeSymbol = o.Instrument.ToString();
            OrderType = OrderType.Limit;
            Side = (o.Side == CryptoAPIs.Exchange.Clients.ItBit.OrderSide.buy ? OrderSide.Buy : OrderSide.Sell);
            OrigQty = o.Amount;
            Price = o.Price;
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            ClientOrderId = o.ClientOrderIdentifier;
            OrderIds = new string[] { o.Id.ToString() };
            ExecutedQty = o.AmountFilled;
            Status = Translate.Status(o.Status);
            Time = o.CreatedTime.ToString();
        }

        public ZOrder(Exchange.Clients.Bittrex.OpenOrder oo)
        {
            Exchange = CryptoExch.BITTREX;
            //Pair = oo.
            if (oo.OrderType == "LIMIT_BUY")
            {
                Side = OrderSide.Buy;
                OrderType = OrderType.Limit;
            }
            else
            {
                Side = OrderSide.Sell;
                OrderType = OrderType.Limit;
            }
            OrigQty = oo.Quantity;
            Price = oo.Limit;
            //StopPrice = o.StopPrice;
            TimeInForce = OrderTimeInForce.GTC;
            //ClientOrderId = o.ClientOrderId;
            OrderIds = new string[] { oo.OrderUuid.ToString() };
            ExecutedQty = oo.Quantity - oo.QuantityRemaining;
            // TODO: Look at Closed and Closing to see if we can determine order status
            //Status = Translate.Status(oo.Status, CryptoExch.BITTREX);
            Time = oo.Opened.ToString();
        }

        public ZOrder(Exchange.Clients.GDAX.OrderResponse or)
        {
            Exchange = CryptoExch.GDAX;
            ExchangeSymbol = or.Product_id;
            OrderType = OrderType.Limit;
            Side = Translate.Side(or.Side, CryptoExch.GDAX);
            OrigQty = or.Size;
            Price = or.Price;
            //StopPrice = o.StopPrice;
            TimeInForce = Translate.TimeInForce(or.Time_in_force, CryptoExch.GDAX);
            //ClientOrderId = o.ClientOrderId;
            OrderIds = new string[] { or.Id.ToString() };
            ExecutedQty = or.Filled_size;
            Status = Translate.Status(or.Status, CryptoExch.GDAX);
            Time = or.Created_at.ToString();
        }

        public ZOrder(Exchange.Clients.GDAX.Order o)
        {
            Exchange = CryptoExch.GDAX;
            ExchangeSymbol = o.product_id;
            OrderType = OrderType.Limit;    //o.type == "limit"
            Side = Translate.Side(o.side, CryptoExch.GDAX);
            OrigQty = o.size;
            Price = o.price;
            //StopPrice = o.StopPrice;
            TimeInForce = Translate.TimeInForce(o.time_in_force, CryptoExch.GDAX);
            //ClientOrderId = o.ClientOrderId;
            //OrderIds = new string[] { o.OrderId.ToString() };
            //o.cancel_after
            //ExecutedQty = o.ExecutedQty;
            //Status = Translate.Status(o.Status, CryptoExch.GDAX);
            //Time = o.Time.ToString();     
        }

        public ZOrder(Exchange.Clients.Binance.Order o)
        {
            Exchange = CryptoExch.BINANCE;
            ExchangeSymbol = o.Symbol;
            OrderType = OrderType.Limit;
            Side = Translate.Side(o.Side, CryptoExch.BINANCE);
            OrigQty = o.OrigQty;
            Price = o.Price;
            StopPrice = o.StopPrice;
            TimeInForce = Translate.TimeInForce(o.TimeInForce, CryptoExch.BINANCE);
            ClientOrderId = o.ClientOrderId;
            OrderIds = new string[] { o.OrderId.ToString() };
            ExecutedQty = o.ExecutedQty;
            Status = Translate.Status(o.Status, CryptoExch.BINANCE);
            Time = o.Time.ToString();
        }

        /*public Order(Exchange.Clients.Kraken.KrakenOrder o)
        {
            Exchange = CryptoExch.KRAKEN;
            Symbol = o.Pair;
            OrderIds = o.Txid;
            Side = (o.Type == "buy" ? OrderSide.Buy : OrderSide.Sell);
            Price = o.Price.Value;
            OrigQty = o.Volume;
        }*/

        public ZOrder(string pair, Exchange.Clients.Kraken.OrderInfo oi)
        {
            Exchange = CryptoExch.KRAKEN;
            ExchangeSymbol = pair;
            OrderType = OrderType.Limit;
            //Side = 
            OrigQty = oi.Volume;
            Price = oi.LimitPrice.Value;
            StopPrice = oi.StopPrice.Value;
            //TimeInForce = 
            ClientOrderId = oi.UserRef.Value.ToString();
            OrderIds = new string[] { oi.RefId };
            ExecutedQty = oi.VolumeExecuted;
            Status = Translate.Status(oi.Status, CryptoExch.KRAKEN);
            Time = oi.OpenTm.ToString();
        }

        public string ToDisplay()
        {
            return string.Format("{0}:Order:{1}> {2} {3} {4} {5} {6} {7} [{8}] {9}", Exchange, ExchangeSymbol, OrderType, Side, Price, OrigQty, TimeInForce, Status, OrderId, Time);
        }

    } // end of class Order

    //=====================================================================================================================================

    public class ZOrderMap
    {
        public int ExchangeCount => m_orders.Keys.Count;

        private ConcurrentDictionary<string, ConcurrentBag<ZOrder>> m_orders = new ConcurrentDictionary<string, ConcurrentBag<ZOrder>>();

        public void Add(ZOrder zo)
        {
            if (!m_orders.ContainsKey(zo.ExchangeSymbol)) m_orders[zo.ExchangeSymbol] = new ConcurrentBag<ZOrder>();
            m_orders[zo.ExchangeSymbol].Add(zo);
        }

        public void Add(IEnumerable<ZOrder> zos)
        {
            foreach (var zo in zos)
            {
                Add(zo);
            }
        }

        public void Clear()
        {
            m_orders.Clear();
        }

        public List<ZOrder> Orders()
        {
            var result = new List<ZOrder>();

            foreach (var bag in m_orders.Values)
            {
                foreach (var zo in bag)
                {
                    result.Add(zo);
                }
            }

            return result;
        }

    } // end of class ZOrderMap

    //=====================================================================================================================================

    public class OrdersUpdateArgs : EventArgs
    {
        public CryptoExch Exch { get { return m_exch; } }
        public ZOrderMap Orders { get { return m_orders; } }

        private CryptoExch m_exch;
        private ZOrderMap m_orders;

        public OrdersUpdateArgs(CryptoExch exch, ZOrderMap orders)
        {
            m_exch = exch;
            m_orders = orders;
        }
    } // end of class OrdersUpdateArgs

} // end of namespace

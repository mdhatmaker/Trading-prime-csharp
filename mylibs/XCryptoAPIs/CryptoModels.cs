using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using static Tools.G;

namespace CryptoAPIs
{
    public delegate void BalancesUpdateHandler(object sender, BalanceUpdateArgs e);

    #region ---------- ENUMS ------------------------------------------------------------------------------------------------

    public enum BarInterval
    {
        Minutes1=1, Minutes5=5, Minutes15=15, Minutes30=30, Hours1=60, Hours2=120, Days1=1440
    }
    #endregion --------------------------------------------------------------------------------------------------------------


    public class OrderNew
    {
        public string Pair;
        public string OrderId { get { return OrderIds[0]; }}
        public string[] OrderIds;
        public ulong ClientId { get; private set; }
        public string TransactTime { get; private set; }
        public string ClientTime { get; private set; }
        public string Text { get; set; }

        private OrderNew()
        {
            ClientTime = DateTime.Now.ToString();
        }

        public OrderNew(string pair, Exchange.Clients.BitFlyer.PostResult pr)
        {
            Pair = pair;
            OrderIds = new string[] { pr.ChildOrderAcceptanceId };
            // TODO: See if either of these other IDs are useful
            //pr.ParentOrderAcceptanceId;
            //pr.MessageId;
        }

        public OrderNew(string pair, ulong orderId)
        {
            Pair = pair;
            OrderIds = new string[] { orderId.ToString() };
        }

        public OrderNew(string pair, Exchange.Clients.Bitstamp.BuySellResponse bsr) : base()
        {
            Pair = pair;
            TransactTime = bsr.datetime;
            Text = bsr.reason;
            OrderIds = new string[] { bsr.id.ToString() };
        }

        public OrderNew(string pair, Exchange.Clients.BitFinex.NewOrderResponse or) : base()
        {
            Pair = pair;
            TransactTime = or.timestamp;
            OrderIds = new string[] { or.order_id };
        }

        public OrderNew(string pair, Exchange.Clients.ItBit.Order o) : base()
        {
            Pair = pair;
            TransactTime = o.CreatedTime.ToString();
            OrderIds = new string[] { o.Id.ToString() };
        }

        public OrderNew(string pair, Exchange.Clients.Bittrex.OrderResult or) : base()
        {
            Pair = pair;
            OrderIds = (or == null ? null : new string[] { or.Uuid.ToString() });
        }

        public OrderNew(string pair, Exchange.Clients.GDAX.OrderResponse or) : base()
        {
            Pair = pair;
            TransactTime = or.Created_at.ToString();
            OrderIds = new string[] { or.Id.ToString() };
        }

        public OrderNew(Exchange.Clients.Poloniex.CurrencyPair pair, ulong orderId) : base()
        {
            Pair = pair.ToString();
            OrderIds = new string[] { orderId.ToString() };
        }

        public OrderNew(string pair, Exchange.Clients.Kraken.AddOrderResult aor) : base()
        {
            Pair = pair;
            OrderIds = aor.Txid;
            Text = string.Format("\n{0}\n{1}", aor.Descr.Order, aor.Descr.Close);
        }

        public OrderNew(string pair, Exchange.Clients.Binance.NewOrder no) : base()
        {
            Pair = pair;
            OrderIds = new string[] { no.ClientOrderId };
            ClientId = (ulong)no.OrderId;
            TransactTime = no.TransactTime.ToString();
        }
    } // end of class OrderNew

    public class OrderCxl
    {
        public string Pair;
        public int OrdersCancelledCount;
        public bool CancelSuccessful;

        public OrderCxl(string pair, string orderId)
        {
            Pair = pair;
        }

        public OrderCxl(string pair, bool success)
        {
            Pair = pair;
            CancelSuccessful = success;
        }

        public OrderCxl(string pair, Exchange.Clients.BitFinex.CancelOrderResponse or)
        {
            Pair = pair;
            //or.
        }

        public OrderCxl(string pair, Exchange.Clients.ItBit.Order o)
        {
            Pair = pair;
            // TODO: check o.Status to try and determine CancelSuccessful
        }

        public OrderCxl(string pair, Exchange.Clients.Bittrex.OrderResult or)
        {
            Pair = pair;
        }

        public OrderCxl(string pair, Exchange.Clients.GDAX.CancelOrderResponse cor)
        {
            Pair = pair;
            // TODO: How do I take the IEnumerable<OrderIds> that was returned and decipher OrdersCancelledCount and CancelSuccessful?
            //cor.OrderIds
        }

        public OrderCxl(string pair, Exchange.Clients.Kraken.CancelOrderResult cor)
        {
            Pair = pair;
            OrdersCancelledCount = cor.Count;
            CancelSuccessful = cor.Pending.Value;
            cout("Kraken:{0}> {1}", Pair, CancelSuccessful);
        }

        public OrderCxl(Exchange.Clients.Poloniex.CurrencyPair pair, bool cancelSuccessful)
        {
            Pair = pair.ToString();
            CancelSuccessful = cancelSuccessful;
            cout("Poloniex:{0}> {1}", Pair, CancelSuccessful);
        }

        public OrderCxl(Exchange.Clients.Binance.CanceledOrder co)
        {
            Pair = co.Symbol;
            cout("Binance:{0}> {1} {2} {3}", Pair, co.OrderId, co.OrigClientOrderId, co.ClientOrderId);
        }
    } // end of class CxlOrder

    public class ZOrderDetail
    {
        
    }


    public static class Translate
    {
        private static Dictionary<string, OrderStatus> m_statusKraken = new Dictionary<string, OrderStatus>() {
            {"pending", OrderStatus.Pending }, {"open", OrderStatus.Open }, {"closed", OrderStatus.Closed }, {"canceled", OrderStatus.Closed }, {"expired", OrderStatus.Expired }
        };
        private static Dictionary<string, OrderStatus> m_statusGDAX = new Dictionary<string, OrderStatus>() {
            {"open", OrderStatus.Open }, {"pending", OrderStatus.Pending }, { "active", OrderStatus.Active }, {"done", OrderStatus.Closed }, { "settled", OrderStatus.Closed }
        };
        private static Dictionary<string, OrderStatus> m_statusBinance = new Dictionary<string, OrderStatus>() {
            {"pending", OrderStatus.Pending }, {"cancelled", OrderStatus.Canceled }, {"NEW", OrderStatus.Open}
        };

        public static OrderStatus Status(string s, CryptoExch exch)
        {
            if (exch == CryptoExch.KRAKEN)
                return m_statusKraken[s];
            else if (exch == CryptoExch.GDAX)
                return m_statusGDAX[s];
            else if (exch == CryptoExch.BINANCE)
                return m_statusBinance[s];
            else
                return OrderStatus.Unknown;
        }

        public static OrderStatus Status(CryptoAPIs.Exchange.Clients.ItBit.OrderStatus status)
        {
            if (status == CryptoAPIs.Exchange.Clients.ItBit.OrderStatus.submitted)
                return OrderStatus.Pending;
            else if (status == CryptoAPIs.Exchange.Clients.ItBit.OrderStatus.open)
                return OrderStatus.Open;
            else if (status == CryptoAPIs.Exchange.Clients.ItBit.OrderStatus.filled)
                return OrderStatus.Filled;
            else if (status == CryptoAPIs.Exchange.Clients.ItBit.OrderStatus.cancelled)
                return OrderStatus.Canceled;
            else if (status == CryptoAPIs.Exchange.Clients.ItBit.OrderStatus.rejected)
                return OrderStatus.Rejected;
            else
                return OrderStatus.Unknown;
        }

        public static OrderStatus Status(CryptoAPIs.Exchange.Clients.BitFlyer.ChildOrderState state)
        {
            if (state == Exchange.Clients.BitFlyer.ChildOrderState.Active)
                return OrderStatus.Active;
            else if (state == Exchange.Clients.BitFlyer.ChildOrderState.Canceled)
                return OrderStatus.Canceled;
            else if (state == Exchange.Clients.BitFlyer.ChildOrderState.Completed)
                return OrderStatus.Closed;
            else if (state == Exchange.Clients.BitFlyer.ChildOrderState.Expired)
                return OrderStatus.Expired;
            else if (state == Exchange.Clients.BitFlyer.ChildOrderState.Rejected)
                return OrderStatus.Rejected;
            else
                return OrderStatus.Unknown;
        }

        public static OrderSide Side(string s, CryptoExch exch)
        {
            if (exch == CryptoExch.GDAX) return (s == "buy" ? OrderSide.Buy : OrderSide.Sell);
            else if (exch == CryptoExch.BINANCE) return (s == "BUY" ? OrderSide.Buy : OrderSide.Sell);
            else if (exch == CryptoExch.BITTREX) return (s == "BUY" ? OrderSide.Buy : OrderSide.Sell);
            else
                throw new ArgumentException(string.Format("Crypto::Translate::Side=> exchange {0} not supported", exch));
        }

        public static OrderSide Side(Exchange.Clients.Poloniex.OrderType ot)
        {
            return (ot == Exchange.Clients.Poloniex.OrderType.Buy ? OrderSide.Buy : OrderSide.Sell);
        }

        public static OrderSide Side(Exchange.Clients.BitFlyer.Side side)
        {
            return (side == Exchange.Clients.BitFlyer.Side.Buy ? OrderSide.Buy : OrderSide.Sell);
        }

        public static OrderSide Side(Exchange.Clients.Binance.OrderSide os)
        {
            return (os == Exchange.Clients.Binance.OrderSide.BUY ? OrderSide.Buy : OrderSide.Sell);
        }

        public static OrderTimeInForce TimeInForce(string s, CryptoExch exch)
        {
            if (exch == CryptoExch.GDAX) return (s == "GTC" ? OrderTimeInForce.GTC : OrderTimeInForce.Day);
            else if (exch == CryptoExch.BINANCE) return (s == "gtc" ? OrderTimeInForce.GTC : OrderTimeInForce.Day);
            else
                throw new ArgumentException(string.Format("Crypto::Translate::TimeInForce=> exchange {0} not supported", exch));
        }
    } // end of class Translate

    /*void BracketInsideMarket(string pair)
    {
        int dplaces = 1;
        decimal percent = 0.05M;
        var bid = Math.Round(ticker[xpair].BidPrice - percent * ticker[xpair].BidPrice, dplaces);    // bid - 5%
        var ask = Math.Round(ticker[xpair].AskPrice + percent * ticker[xpair].BidPrice, dplaces);    // ask + 5%
    }*/



} // end of namespace

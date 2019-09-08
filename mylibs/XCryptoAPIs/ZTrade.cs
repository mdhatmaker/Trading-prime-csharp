using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CryptoAPIs
{
    public delegate void TradeUpdateHandler(object sender, OrdersUpdateArgs e);

    public class ZTrade : IDataRow
    {
        public CryptoExch Exchange;
        public string Pair;
        public OrderSide Side;
        public decimal Price;
        public decimal Qty;
        public string OrderId;
        public string TradeId;
        public string Time;

        public Dictionary<string, string> Fields = new Dictionary<string, string>();    // Any exchange-specific fields will be stored here

        public string Key { get => Exchange.ToString() + "_" + OrderId; set => throw new NotImplementedException(); }

        public static string[] Columns = { "Exchange", "Symbol", "Side", "Price", "Qty", "Time" };

        public string[] GetCells()
        {
            return new string[] { Exchange.ToString(), Pair, Side.ToString(), Price.ToString(), Qty.ToString(), Time };
        }

        public ZTrade(string pair, Exchange.Clients.BitFlyer.PrivateExecution pe)
        {
            Exchange = CryptoExch.BITFLYER;
            Pair = pair;
            Side = Translate.Side(pe.Side);
            OrderId = pe.ChildOrderId;
            TradeId = pe.Id.ToString();
            Price = (decimal)pe.Price;
            Qty = (decimal)pe.Size;
            Time = pe.ExecDate.ToString();
            Fields["ChildOrderAcceptanceId"] = pe.ChildOrderAcceptanceId;
            Fields["Commission"] = pe.Commission.ToString();
        }

        public ZTrade(string pair, Exchange.Clients.Poloniex.TradeOrder to)
        {
            Exchange = CryptoExch.POLONIEX;
            Pair = pair;
            Side = Translate.Side(to.Type);
            OrderId = to.IdOrder.ToString();
            //TradeId = to.id;
            Price = to.PricePerCoin;
            Qty = to.AmountBase;                        // TODO: Or should this be AmountQuote?
            //Time = to.datetime;
            Fields["AmountQuote"] = to.AmountQuote.ToString();
        }

        public ZTrade(string pair, Exchange.Clients.Bitstamp.TransactionResponse tr)
        {
            Exchange = CryptoExch.BITSTAMP;
            Pair = pair;
            Side = Translate.Side(tr.type, CryptoExch.BITSTAMP);
            OrderId = tr.order_id;
            TradeId = tr.id;
            Price = decimal.Parse(tr.btc_usd);
            Qty = decimal.Parse(tr.btc);
            Time = tr.datetime;
            Fields["usd"] = tr.usd;
            Fields["fee"] = tr.fee;
            Fields["status"] = tr.status;
            Fields["reason"] = tr.reason;
        }

        public ZTrade(string pair, Exchange.Clients.ItBit.RecentTrade rt)
        {
            Exchange = CryptoExch.ITBIT;
            Pair = pair;
            //Side = Translate.Side(rt.OrderType, CryptoExch.ITBIT);
            //TradeId = rt.Id.ToString();
            Price = rt.Price;
            Qty = rt.Amount;
            Time = rt.Timestamp.ToString();
            Fields["MatchNumber"] = rt.MatchNumber.ToString();
        }

        public ZTrade(string pair, Exchange.Clients.Bittrex.HistoricTrade ht)
        {
            Exchange = CryptoExch.BITTREX;
            Pair = pair;
            Side = Translate.Side(ht.OrderType, CryptoExch.BITTREX);
            TradeId = ht.Id.ToString();
            Price = ht.Price;
            Qty = ht.Quantity;
            Time = ht.TimeStamp.ToString();
            Fields["FillType"] = ht.FillType;
            Fields["Total"] = ht.Total.ToString();
        }

        public ZTrade(string pair, Exchange.Clients.GDAX.FillResponse fr)
        {
            Exchange = CryptoExch.GDAX;
            Pair = pair;    // or fr.ProductId
            Side = Translate.Side(fr.Side, CryptoExch.GDAX);
            TradeId = fr.Trade_id.ToString();
            Price = fr.Price;
            Qty = fr.Size;
            Time = fr.Created_at.ToString();
            Fields["Fee"] = fr.Fee.ToString();
            // ....more fields here
        }

        public ZTrade(string pair, Exchange.Clients.Binance.Trade t)
        {
            Exchange = CryptoExch.BINANCE;
            Pair = pair;
            Side = t.IsBuyer ? OrderSide.Buy : OrderSide.Sell;
            TradeId = t.Id.ToString();
            Price = t.Price;
            Qty = t.Quantity;
            Time = t.Time.ToString();
            Fields["Commission"] = t.Commission.ToString();
            Fields["CommissionAsset"] = t.CommissionAsset;
            Fields["IsBestMatch"] = t.IsBestMatch.ToString();
            Fields["IsBuyer"] = t.IsBuyer.ToString();
            Fields["IsMaker"] = t.IsMaker.ToString();
        }

        public ZTrade(string pair, Exchange.Clients.Kraken.TradeInfo ti)
        {
            Exchange = CryptoExch.KRAKEN;
            Pair = ti.Pair;
            Side = Translate.Side(ti.Type, CryptoExch.KRAKEN);
            Price = ti.Price;
            Qty = ti.Vol;
            Time = ti.Time.ToString();
            OrderId = ti.OrderTxid;
            Fields["CCost"] = ti.CCost.Value.ToString();
            // ....more fields here
        }
    } // end of class Trade}

    //=====================================================================================================================================

    public class TradeUpdateArgs : EventArgs
    {
        public CryptoExch Exch { get { return m_exch; } }
        public ZTrade Trade { get { return m_trade; } }

        private CryptoExch m_exch;
        private ZTrade m_trade;

        public TradeUpdateArgs(CryptoExch exch, ZTrade trade)
        {
            m_exch = exch;
            m_trade = trade;
        }
    } // end of class TradeUpdateArgs

} // end of namespace
using System;

namespace CryptoAPIs.Exchange.Clients.Bitstamp
{
    public class BalanceResponse
    {
        public string usd_balance { get; set; }
        public string btc_balance { get; set; }
        public string eur_balance { get; set; }
        public string usd_reserved { get; set; }
        public string btc_reserved { get; set; }
        public string eur_reserved { get; set; }
        public string usd_available { get; set; }
        public string btc_available { get; set; }
        public string eur_available { get; set; }
        public string fee { get; set; }
    }

    public class BuySellResponse
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string type { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
    }

    public class OpenOrderResponse
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string type { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string reason { get; set; }

        public string type_human
        {
            get
            {
                if (type == "0") return "Buy";
                else if (type == "1") return "Sell";
                else return "Unknown";
            }
        }
    }

    public class OrderStatusResponse
    {
        public string status { get; set; }
        public Transaction[] transactions { get; set; }
    }

    public class Transaction
    {
        public string fee { get; set; }
        public string price { get; set; }
        public string datetime { get; set; }
        public string usd { get; set; }
        public string btc { get; set; }
        public string tid { get; set; }
        public string type { get; set; }
    }

    public class TickerResponse
    {
        public string last { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string vwap { get; set; }
        public string volume { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }

        public string avg
        {
            get
            {
                try
                {
                    return ((Convert.ToDouble(high.Replace(".", ","))
                        + Convert.ToDouble(low.Replace(".", ","))) / 2).ToString("f4").Replace(",", ".");
                }
                catch
                {
                    return "0.0000";
                }
            }
        }
    }

    public class TransactionResponse
    {
        public string datetime { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string usd { get; set; }
        public string btc { get; set; }
        public string btc_usd { get; set; }
        public string fee { get; set; }
        public string order_id { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
    }


} // end of namespace

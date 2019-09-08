using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CryptoAPIs.Exchange.Clients.HitBTC
{
    public class Address
    {
        /// <summary>
        /// BTC/LTC address to withdraw to
        /// </summary>
        public string address { get; set; }
    }

    public class Balance
    {
        /// <summary>
        /// Currency symbol, e.g. BTC
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// Funds amount
        /// </summary>
        public double balance { get; set; }
    }

    public class MultiCurrencyBalance
    {
        public List<Balance> balance { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public int statusCode { get; set; }
        public string body { get; set; }
        public override string ToString()
        {
            return $"{statusCode} - {message} - {body}";
        }
    }

    public class Orderbook
    {
        public List<KeyValuePair<string, string>> asks { get; set; }
        public List<KeyValuePair<string, string>> bids { get; set; }
    }

    public class Order
    {
        /// <summary>
        /// Order ID on the exchange
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// Order status
        /// new, partiallyFilled, filled, canceled, expired, rejected
        /// </summary>
        public string orderStatus { get; set; }

        /// <summary>
        /// UTC timestamp of the last change, in milliseconds
        /// </summary>
        public long lastTimestamp { get; set; }

        /// <summary>
        /// Order price
        /// </summary>
        public string orderPrice { get; set; }

        /// <summary>
        /// Order quantity, in lots
        /// </summary>
        public int orderQuantity { get; set; }

        /// <summary>
        /// Average price   
        /// </summary>
        public string avgPrice { get; set; }

        /// <summary>
        /// Remaining quantity, in lots
        /// </summary>
        public int quantityLeaves { get; set; }

        /// <summary>
        /// Type of an order
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        public string timeInForce { get; set; }

        /// <summary>
        /// Cumulative quantity
        /// </summary>
        public int cumQuantity { get; set; }

        /// <summary>
        /// Unique client-generated ID
        /// </summary>
        public string clientOrderId { get; set; }

        /// <summary>
        /// Currency symbol
        /// </summary>
        public string symbo { get; set; }

        /// <summary>
        /// Side of a trade
        /// </summary>
        public string side { get; set; }

        /// <summary>
        /// Last executed quantity, in lots
        /// </summary>
        public int execQuantity { get; set; }

    }

    public class Orders
    {
        public List<Order> orders { get; set; }
    }

    public class Symbol
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        ///Price step parameter
        /// </summary>
        public string step { get; set; }

        /// <summary>
        /// Lot size parameter
        /// </summary>
        public string lot { get; set; }

        /// <summary>
        /// Value of this symbol
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Second value of this symbol
        /// </summary>
        public string commodity { get; set; }

        /// <summary>
        /// Liquidity taker fee
        /// </summary>
        public string takeLiquidityRate { get; set; }

        /// <summary>
        /// Liquidity provider rebate
        /// </summary>
        public string provideLiquidityRate { get; set; }
    }

    public class Symbols
    {
        public List<Symbol> symbols { get; set; }
    }

    public class Ticker
    {
        /// <summary>
        /// Last price
        /// </summary>
        public string last { get; set; }

        /// <summary>
        /// Highest buy order
        /// </summary>
        public string bid { get; set; }

        /// <summary>
        /// Lowest sell order
        /// </summary>
        public string ask { get; set; }

        /// <summary>
        /// Highest trade price per last 24h + last incomplete minute
        /// </summary>
        public string high { get; set; }

        /// <summary>
        /// Lowest trade price per last 24h + last incomplete minute
        /// </summary>
        public string low { get; set; }

        /// <summary>
        /// Volume per last 24h + last incomplete minute
        /// </summary>
        public string volume { get; set; }

        /// <summary>
        /// Price in which instrument open
        /// </summary>
        public string open { get; set; }

        /// <summary>
        /// Volume in second currency per last 24h + last incomplete minute
        /// </summary>
        public string volume_quote { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        public long timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"last:{last}");
            sb.AppendLine($"bid:{bid}");
            sb.AppendLine($"ask:{ask}");
            sb.AppendLine($"high:{high}");
            sb.AppendLine($"low:{low}");
            sb.AppendLine($"volume:{volume}");
            sb.AppendLine($"open:{open}");
            sb.AppendLine($"volume_quote:{volume_quote}");
            sb.AppendLine($"timestamp:{timestamp}");

            return sb.ToString();
        }
    }

    public class Timestamp
    {
        /// <summary>
        /// time in UNIX timestamp format
        /// </summary>
        public long timestamp { get; set; }
    }

    public class Trade
    {
        /// <summary>
        /// Trade ID on the exchange
        /// </summary>
        public int tradeId { get; set; }

        /// <summary>
        /// Trade price
        /// </summary>
        public double execPrice { get; set; }

        /// <summary>
        /// Timestamp, in milliseconds
        /// </summary>
        public object timestamp { get; set; }

        /// <summary>
        /// Order ID on the exchange
        /// </summary>
        public string originalOrderId { get; set; }

        /// <summary>
        /// Fee for the trade, negative value means rebate
        /// </summary>
        public double fee { get; set; }

        /// <summary>
        /// Unique order ID generated by client. From 8 to 32 characters
        /// </summary>
        public string clientOrderId { get; set; }

        /// <summary>
        /// Currency symbol
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        /// Side of a trade
        /// </summary>
        public string side { get; set; }

        /// <summary>
        /// Trade size, in lots
        /// </summary>
        public int execQuantity { get; set; }
    }

    public class SpecifiedTrade
    {
        public object date { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public int tid { get; set; }
        public string side { get; set; }
    }

    public class Trades
    {
        public List<Trade> trades { get; set; }
    }

    public class SpecifiedTrades
    {
        public List<SpecifiedTrade> trades { get; set; }
    }

    public class TradingBalance
    {
        /// <summary>
        /// Currency symbol, e.g. BTC
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double cash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double reserved { get; set; }
    }

    public class TradingBalanceList
    {
        public List<TradingBalance> balance { get; set; }
    }

    public class Transaction
    {
        public string id { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public int created { get; set; }
        public int finished { get; set; }
        public double amount_from { get; set; }
        public string currency_code_from { get; set; }
        public double amount_to { get; set; }
        public string currency_code_to { get; set; }
        public object destination_data { get; set; }
        public int commission_percent { get; set; }
        public string bitcoin_address { get; set; }
        public string bitcoin_return_address { get; set; }
        public string external_data { get; set; }
    }

    public class TransactionObject
    {
        public Transaction transaction { get; set; }
    }

    public class TransactionList
    {
        public List<Transaction> transactions { get; set; }
    }

    public class PayoutTransaction
    {
        public string transaction { get; set; }
    }

    /// <summary>
    /// Converting JSON to Models 
    /// </summary>
    public class ApiResponse
    {
        public string content { get; set; }

        #region MarketData

        public static implicit operator Timestamp(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Timestamp>(response);
        }

        public static implicit operator Symbols(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Symbols>(response);
        }

        public static implicit operator Ticker(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Ticker>(response);
        }

        public static implicit operator Dictionary<string, Ticker>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasonArray(response);
        }

        public static implicit operator Orderbook(ApiResponse response)
        {
            if (response == null)
                return null;

            var orderbook = Utilities.ConverFromJason<OrderbookRaw>(response);

            List<KeyValuePair<string, string>> askList
                = orderbook.asks.Select(ask => new KeyValuePair<string, string>(ask[0], ask[1])).ToList();
            List<KeyValuePair<string, string>> bidList
                = orderbook.bids.Select(bid => new KeyValuePair<string, string>(bid[0], bid[1])).ToList();

            return new Orderbook { asks = askList, bids = bidList };
        }

        private class OrderbookRaw
        {
            public List<List<string>> asks { get; set; }
            public List<List<string>> bids { get; set; }
        }

        public static implicit operator SpecifiedTrades(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SpecifiedTrades>(response);
        }

        #endregion

        #region Paymant

        public static implicit operator MultiCurrencyBalance(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<MultiCurrencyBalance>(response);
        }

        #endregion

        #region Trading


        public static implicit operator TradingBalanceList(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<TradingBalanceList>(response);
        }

        public static implicit operator Orders(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Orders>(response);
        }

        public static implicit operator Transaction(ApiResponse response)
        {
            if (response != null)
            {
                if (response.content.Contains("message"))
                {
                    var error = Utilities.ConverFromJason<Error>(response);
                    throw new System.Exception(error.ToString());
                }
                return Utilities.ConverFromJason<TransactionObject>(response).transaction;
            }
            return null;
        }

        public static implicit operator TransactionList(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<TransactionList>(response);
        }

        public static implicit operator PayoutTransaction(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<PayoutTransaction>(response);
        }

        public static implicit operator Address(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Address>(response);
        }

        public static implicit operator Trades(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Trades>(response);
        }

        #endregion

    } // end of class ApiResponse

} // end of namespace

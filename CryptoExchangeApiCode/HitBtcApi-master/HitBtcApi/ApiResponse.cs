using System.Collections.Generic;
using System.Linq;
using HitBtcApi.Model;

namespace HitBtcApi
{
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

    }
}

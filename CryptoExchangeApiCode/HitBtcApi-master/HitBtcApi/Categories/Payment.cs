using System.Threading.Tasks;
using HitBtcApi.Model;
using RestSharp;

namespace HitBtcApi.Categories
{
    /// <summary>
    /// Payment RESTful API
    /// </summary>
    public class Payment
    {
        HitBtcApi _api;

        public Payment(HitBtcApi api)
        {
            _api = api;
        }

        /// <summary>
        /// returns multi-currency balance of the main account.
        /// /api/1/payment/balance
        /// </summary>
        /// <returns></returns>
        public async Task<MultiCurrencyBalance> GetMultiCurrencyBalance()
        {
            return await _api.Execute(new RestRequest("/api/1/payment/balance"));
        }

        /// <summary>
        /// returns payment transaction and its status.
        /// /api/1/payment/transactions/:id
        /// </summary>
        /// <param name="id">Transaction Id, Required</param>
        /// <returns></returns>
        public async Task<Transaction> GetTransactions(string id)
        {
            var request = new RestRequest("/api/1/payment/transactions/{id}");
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// returns a list of payment transactions and their statuses(array of transactions).
        /// /api/1/payment/transactions
        /// </summary>
        /// <param name="offset">Start index for the query, default = 0</param>
        /// <param name="limit">Maximum results for the query, Required</param>
        /// <param name="dir">Transactions are sorted ascending (ask) or descending (desc) (default)</param>
        /// <returns></returns>
        public async Task<TransactionList> GetTransactions(int limit = 1000, string dir = "desc", int offset = 0)
        {
            var request = new RestRequest("/api/1/payment/transactions");
            request.Parameters.Add(new Parameter { Name = "limit", Value = limit, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "dir", Value = dir, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "offset", Value = offset, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }

        /// <summary>
        /// withdraws money and creates an outgoing crypotocurrency transaction; 
        /// returns a transaction ID on the exchange or an error.
        /// /api/1/payment/payout
        /// </summary>
        /// <param name="amount">Funds amount to withdraw, Required</param>
        /// <param name="currency_code">Currency symbol, e.g.BTC, Required</param>
        /// <param name="address">BTC/LTC address to withdraw to, Required</param>
        /// <param name="id">payment id for cryptonote</param>
        /// <returns></returns>
        public async Task<Transaction> GetPyout(decimal amount, string currency_code, string address, string extra_id)
        {
            var request = new RestRequest("/api/1/payment/transactions", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "address", Value = address, Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(extra_id))
                request.Parameters.Add(new Parameter { Name = "extra_id", Value = extra_id, Type = ParameterType.GetOrPost });

            return await _api.Execute(request);
        }

        /// <summary>
        /// returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. 
        /// /api/1/payment/address/ (GET)
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<Address> GetAddress(string currency)
        {
            var request = new RestRequest("/api/1/payment/address/{currency}");
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. 
        /// /api/1/payment/address/ (GET)
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<Address> CreateAddress(string currency)
        {
            var request = new RestRequest("/api/1/payment/address/{currency}", Method.POST);
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// transfers funds between main and trading accounts; returns a transaction ID or an error.
        /// /api/1/payment/transfer_to_trading
        /// </summary>
        /// <param name="amount">Funds amount to transfer, Required</param>
        /// <param name="currency_code">Currency symbol, e.g. BTC, Required</param>
        /// <returns></returns>
        public async Task<Transaction> TransferToTrading(decimal amount, string currency_code)
        {
            var request = new RestRequest("/api/1/payment/transfer_to_trading", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }

        /// <summary>
        /// transfers funds between main and trading accounts; returns a transaction ID or an error.
        /// /api/1/payment/transfer_to_main
        /// </summary>
        /// <param name="amount">Funds amount to transfer, Required</param>
        /// <param name="currency_code">Currency symbol, e.g. BTC, Required</param>
        /// <returns></returns>
        public async Task<Transaction> TransferToMain(decimal amount, string currency_code)
        {
            var request = new RestRequest("/api/1/payment/transfer_to_main", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }
    }
}
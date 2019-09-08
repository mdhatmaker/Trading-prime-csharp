using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;

namespace CryptoAPIs.Exchange.Clients.BitFlyer
{
    // Created this BitflyerClient class to wrap both the PublicApi and PrivateApi
    public partial class BitflyerClient
    {
        public PublicApi PublicApi { get; private set; }
        public PrivateApi PrivateApi { get; private set; }

        public BitflyerClient(string apiKey, string apiSecret)
        {
            PublicApi = new PublicApi();
            PrivateApi = new PrivateApi(apiKey, apiSecret);
        } 
    } // end of class BitflyerClient



    public class PrivateApi
    {
        private static readonly MediaTypeHeaderValue MediaType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = BitFlyerConstants.BaseUri,
            Timeout = TimeSpan.FromSeconds(10)
        };

        private readonly string _apiKey;
        private readonly byte[] _apiSecret;

        public PrivateApi(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = Encoding.UTF8.GetBytes(apiSecret);
        }

        internal async Task<T> Get<T>(string path, Dictionary<string, object> query = null)
        {
            return await SendRequest<T>(HttpMethod.Get, path, query);
        }

        internal async Task<T> Post<T>(string path, object body)
        {
            return await SendRequest<T>(HttpMethod.Post, path, null, body);
        }

        internal async Task<T> Post<T>(string path, Dictionary<string, object> query, object body)
        {
            return await SendRequest<T>(HttpMethod.Post, path, query, body);
        }

        internal async Task Post(string path, object body)
        {
            await SendRequest(HttpMethod.Post, path, null, body);
        }

        internal async Task Post(string path, Dictionary<string, object> query, object body)
        {
            await SendRequest(HttpMethod.Post, path, query, body);
        }

        private async Task<T> SendRequest<T>(HttpMethod method, string path,
            Dictionary<string, object> query = null, object body = null)
        {
            var responseJson = await SendRequest(method, path, query, body);
            return JsonSerializer.Deserialize<T>(responseJson);
        }

        private async Task<string> SendRequest(HttpMethod method, string path,
            Dictionary<string, object> query = null, object body = null)
        {
            var queryString = string.Empty;
            if (query != null)
            {
                queryString = query.ToQueryString();
            }

            using (var message = new HttpRequestMessage(method, path + queryString))
            {
                byte[] bodyBytes = null;
                if (body != null)
                {
                    bodyBytes = JsonSerializer.Serialize(body);
                    message.Content = new ByteArrayContent(bodyBytes);
                    message.Content.Headers.ContentType = MediaType;
                }
                var timestamp = DateTimeOffset.UtcNow.ToUnixTime().ToString();
                var payload = bodyBytes == null ? Encoding.UTF8.GetBytes(timestamp + method + path + queryString) :
                    Encoding.UTF8.GetBytes(timestamp + method + path + queryString).Concat(bodyBytes).ToArray();
                var hash = SignWithHmacsha256(payload);
                message.Headers.Add("ACCESS-KEY", _apiKey);
                message.Headers.Add("ACCESS-TIMESTAMP", timestamp);
                message.Headers.Add("ACCESS-SIGN", hash);

                try
                {
                    var response = await HttpClient.SendAsync(message);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        Error error = null;
                        try
                        {
                            error = JsonSerializer.Deserialize<Error>(responseJson);
                        }
                        catch
                        {
                            // ignore
                        }

                        if (!string.IsNullOrEmpty(error?.ErrorMessage))
                        {
                            throw new BitFlyerApiException(path, error.ErrorMessage, error);
                        }
                        throw new BitFlyerApiException(path,
                            $"Error has occurred. Response StatusCode:{response.StatusCode} ReasonPhrase:{response.ReasonPhrase}.");
                    }

                    return responseJson;
                }
                catch (TaskCanceledException)
                {
                    throw new BitFlyerApiException(path, "Request Timeout");
                }
            }
        }

        private string SignWithHmacsha256(byte[] utf8Bytes)
        {
            using (var encoder = new HMACSHA256(_apiSecret))
            {
                return encoder.ComputeHash(utf8Bytes).ToHex();
            }
        }

        private const string AddressesApiPath = "/v1/me/getaddresses";

        public async Task<CryptoCurrencyAddress[]> GetAddresses()
        {
            return await Get<CryptoCurrencyAddress[]>(AddressesApiPath);
        }

        private const string BalanceApiPath = "/v1/me/getbalance";

        public async Task<Balance[]> GetBalance()
        {
            return await Get<Balance[]>(BalanceApiPath);
        }

        private const string BankAccountsApiPath = "/v1/me/getbankaccounts";

        public async Task<BankAccount[]> GetBankAccounts()
        {
            return await Get<BankAccount[]>(BankAccountsApiPath);
        }

        private const string SendChildOrderApiPath = "/v1/me/sendchildorder";
        private const string CancelChildOrderApiPath = "/v1/me/cancelchildorder";
        private const string CancelAllOrdersApiPath = "/v1/me/cancelallchildorders";
        private const string GetChildOrdersApiPath = "/v1/me/getchildorders";

        public async Task<PostResult> SendChildOrder(SendChildOrderParameter parameter)
        {
            return await Post<PostResult>(SendChildOrderApiPath, parameter);
        }

        public async Task CancelChildOrder(CancelChildOrderParameter parameter)
        {
            await Post(CancelChildOrderApiPath, parameter);
        }

        public async Task CancelAllOrders(CancelAllOrdersParameter parameter)
        {
            await Post(CancelAllOrdersApiPath, parameter);
        }

        public async Task<ChildOrder[]> GetChildOrders(string productCode,
            int? count = null, int? before = null, int? after = null, ChildOrderState? childOrderState = null)
        {
            var query = new Dictionary<string, object>
            {
                { "product_code", productCode }
            };

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }
            if (childOrderState != null)
            {
                query["child_order_state"] = childOrderState.GetEnumMemberValue();
            }

            return await Get<ChildOrder[]>(GetChildOrdersApiPath, query);
        }

        public async Task<ChildOrder[]> GetChildOrder(string productCode, long childOrderId)
        {
            var query = new Dictionary<string, object>
            {
                { "product_code", productCode },
                { "child_order_id", childOrderId }
            };

            return await Get<ChildOrder[]>(GetChildOrdersApiPath, query);
        }
    
        private const string CoinInApiPath = "/v1/me/getcoinins";

        public async Task<CoinIn[]> GetCoinIns(AddresseType type = AddresseType.Normal,
            int? count = null, int? before = null, int? after = null)
        {
            var query = new Dictionary<string, object>();

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }

            return await Get<CoinIn[]>(CoinInApiPath, query);
        }

        private const string CoinOutApiPath = "/v1/me/getcoinouts";

        public async Task<CoinOut> GetCoinOut(string messageId)
        {
            if (messageId == null)
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            var query = new Dictionary<string, object>
            {
                { "message_id", messageId }
            };

            return await Get<CoinOut>(CoinOutApiPath, query);
        }

        public async Task<CoinOut[]> GetCoinOuts(int? count = null, int? before = null, int? after = null)
        {
            var query = new Dictionary<string, object>();

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }

            return await Get<CoinOut[]>(CoinOutApiPath, query);
        }

        private const string CollateralApiPath = "/v1/me/getcollateral";

        public async Task<Collateral> GetCollateral()
        {
            return await Get<Collateral>(CollateralApiPath);
        }

        private const string GetCollateralHistoryApiPath = "/v1/me/getcollateralhistory";

        public async Task<CollateralHistory[]> GetCollateralHistory(int? count = null, int? before = null, int? after = null)
        {
            var query = new Dictionary<string, object>();

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }

            return await Get<CollateralHistory[]>(GetCollateralHistoryApiPath, query);
        }
    
        private const string DepositApiPath = "/v1/me/getdeposits";

        public async Task<Deposit[]> GetDeposits()
        {
            return await Get<Deposit[]>(DepositApiPath);
        }

        private const string SendParentOrderApiPath = "/v1/me/sendparentorder";
        private const string CancelParentOrderApiPath = "/v1/me/cancelparentorder";
        private const string GetParentOrderApiPath = "/v1/me/getparentorder";
        private const string GetParentOrdersApiPath = "/v1/me/getparentorders";

        public async Task<PostResult> SendParentOrder(SendParentOrderParameter parameter)
        {
            return await Post<PostResult>(SendParentOrderApiPath, parameter);
        }

        public async Task CancelParentOrder(CancelParentOrderParameter parameter)
        {
            await Post(CancelParentOrderApiPath, parameter);
        }

        public async Task<ParentOrderDetail> GetParentOrder(string productCode,
            string parentOrderId = null, string parentOrderAcceptanceId = null)
        {
            if (parentOrderId == null && parentOrderAcceptanceId == null)
            {
                throw new BitFlyerApiException(GetParentOrderApiPath,
                    "parentOrderId or parentOrderAcceptanceId is required.");
            }

            var query = new Dictionary<string, object>
            {
                { "product_code", productCode }
            };

            if (parentOrderId != null)
            {
                query["parent_order_id"] = parentOrderId;
            }
            if (parentOrderAcceptanceId != null)
            {
                query["parent_order_acceptance_id"] = parentOrderAcceptanceId;
            }

            return await Get<ParentOrderDetail>(GetParentOrderApiPath, query);
        }

        public async Task<ParentOrder[]> GetParentOrders(string productCode,
            int? count = null, int? before = null, int? after = null,
            ParentOrderState parentOrderState = ParentOrderState.Active)
        {
            var query = new Dictionary<string, object>
            {
                { "product_code", productCode },
                { "parent_order_state", parentOrderState.GetEnumMemberValue() }
            };

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }

            return await Get<ParentOrder[]>(GetParentOrdersApiPath, query);
        }
    
        private const string PermissionApiPath = "/v1/me/getpermissions";

        public async Task<string[]> GetPermissions()
        {
            return await Get<string[]>(PermissionApiPath);
        }

        private const string GetPositionsApiPath = "/v1/me/getpositions";

        public async Task<Position[]> GetPositions(string productCode)
        {
            return await Get<Position[]>(GetPositionsApiPath, new Dictionary<string, object>
            {
                { "product_code", productCode }
            });
        }

        private const string GetExecutionsApiPath = "/v1/me/getexecutions";

        public async Task<PrivateExecution[]> GetExecutions(string productCode,
            int? count = null, int? before = null, int? after = null,
            string childOrderId = null, string childOrderAcceptanceId = null)
        {
            var query = new Dictionary<string, object>
            {
                { "product_code", productCode }
            };

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }
            if (childOrderId != null)
            {
                query["child_order_id"] = childOrderId;
            }
            if (childOrderAcceptanceId != null)
            {
                query["child_order_acceptance_id"] = childOrderAcceptanceId;
            }

            return await Get<PrivateExecution[]>(GetExecutionsApiPath, query);
        }

        private const string SendCoinApiPath = "/v1/me/sendcoin";

        public async Task<PostResult> SendCoin(SendCoinParameter parameter)
        {
            return await Post<PostResult>(SendCoinApiPath, parameter);
        }

        private const string GetTradingCommissionApiPath = "/v1/me/gettradingcommission";

        public async Task<TradingCommission> GetTradingCommission(string productCode)
        {
            return await Get<TradingCommission>(GetTradingCommissionApiPath, new Dictionary<string, object>
            {
                { "product_code", productCode }
            });
        }

        private const string GetWithdrawalsApiPath = "/v1/me/getwithdrawals";

        public async Task<Withdrawal> GetWithdrawal(string messageId)
        {
            if (messageId == null)
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            var query = new Dictionary<string, object>
            {
                { "message_id", messageId }
            };

            return await Get<Withdrawal>(GetWithdrawalsApiPath, query);
        }

        public async Task<Withdrawal[]> GetWithdrawals(int? count = null, int? before = null, int? after = null)
        {
            var query = new Dictionary<string, object>();

            if (count != null)
            {
                query["count"] = count.Value;
            }
            if (before != null)
            {
                query["before"] = before.Value;
            }
            if (after != null)
            {
                query["after"] = after.Value;
            }

            return await Get<Withdrawal[]>(GetWithdrawalsApiPath, query);
        }

        private const string WithdrawApiPath = "/v1/me/withdraw";

        public async Task<PostResult> Withdraw(WithdrawParameter parameter)
        {
            return await Post<PostResult>(WithdrawApiPath, parameter);
        }

    } // end of class PrivateApi

} // end of namespace

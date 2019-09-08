using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace CryptoAPIs.Exchange.Clients.GDAX
{
    public class GdaxClient
    {
        public GdaxClient(
            IAuthenticator authenticator,
            bool sandBox = false)
        {
            var httpClient = new HttpClient();
            var clock = new Clock();
            var httpRequestMessageService = new HttpRequestMessageService(clock, sandBox);
            var queryBuilder = new QueryBuilder();

            AccountsService = new AccountsService(httpClient, httpRequestMessageService, authenticator);
            CoinbaseAccountsService = new CoinbaseAccountsService(httpClient, httpRequestMessageService, authenticator);
            OrdersService = new OrdersService(httpClient, httpRequestMessageService, authenticator);
            PaymentsService = new PaymentsService(httpClient, httpRequestMessageService, authenticator);
            WithdrawalsService = new WithdrawalsService(httpClient, httpRequestMessageService, authenticator);
            DepositsService = new DepositsService(httpClient, httpRequestMessageService, authenticator);
            ProductsService = new ProductsService(httpClient, httpRequestMessageService, authenticator, queryBuilder);
            CurrenciesService = new CurrenciesService(httpClient, httpRequestMessageService, authenticator);
            FillsService = new FillsService(httpClient, httpRequestMessageService, authenticator);
            FundingsService = new FundingsService(httpClient, httpRequestMessageService, authenticator, queryBuilder);
        }

        public AccountsService AccountsService { get; }

        public CoinbaseAccountsService CoinbaseAccountsService { get; }

        public OrdersService OrdersService { get; }

        public PaymentsService PaymentsService { get; }

        public WithdrawalsService WithdrawalsService { get; }

        public DepositsService DepositsService { get; }

        public ProductsService ProductsService { get; }

        public CurrenciesService CurrenciesService { get; }

        public FillsService FillsService { get; }

        public FundingsService FundingsService { get; }
    }

    public class AccountsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public AccountsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/accounts");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var accountList = JsonConvert.DeserializeObject<List<Account>>(contentBody);

            return accountList;
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/accounts/{id}");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var account = JsonConvert.DeserializeObject<Account>(contentBody);

            return account;
        }

        public async Task<IList<IList<AccountHistory>>> GetAccountHistoryAsync(string id, int limit = 100)
        {
            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<AccountHistory>(HttpMethod.Get, authenticator, $"/accounts/{id}/ledger?limit={limit}");

            return httpResponseMessage;
        }

        public async Task<IList<IList<AccountHold>>> GetAccountHoldsAsync(string id, int limit = 100)
        {
            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<AccountHold>(HttpMethod.Get, authenticator, $"/accounts/{id}/holds?limit={limit}");

            return httpResponseMessage;
        }
    } // end of class AccountsService

    public class CoinbaseAccountsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public CoinbaseAccountsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<CoinbaseAccount>> GetAllAccountsAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/coinbase-accounts");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var accounts = JsonConvert.DeserializeObject<List<CoinbaseAccount>>(contentBody);

            return accounts;
        }
    } // end of class CoinbaseAccountsService

    public class CurrenciesService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public CurrenciesService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/currencies");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var currencies = JsonConvert.DeserializeObject<List<Currency>>(contentBody);

            return currencies;
        }
    } // end of class CurrenciesService

    public class DepositsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public DepositsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<DepositResponse> DepositFundsAsync(string paymentMethodId, decimal amount, Currency currency)
        {
            var newDeposit = JsonConvert.SerializeObject(new Deposit
            {
                amount = amount,
                currency = currency.ToString().ToUpper(),
                payment_method_id = new Guid(paymentMethodId)
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/deposits/payment-method", newDeposit);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var depositResponse = JsonConvert.DeserializeObject<DepositResponse>(contentBody);

            return depositResponse;
        }

        public async Task<CoinbaseResponse> DepositCoinbaseFundsAsync(string coinbaseAccountId, decimal amount, Currency currency)
        {
            var newCoinbaseDeposit = JsonConvert.SerializeObject(new Coinbase
            {
                amount = amount,
                currency = currency.ToString().ToUpper(),
                coinbase_account_id = new Guid(coinbaseAccountId)
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/deposits/coinbase-account", newCoinbaseDeposit);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var depositResponse = JsonConvert.DeserializeObject<CoinbaseResponse>(contentBody);

            return depositResponse;
        }
    } // end of class DepositsService

    public class FillsService : AbstractService
    {
        private readonly IAuthenticator authenticator;

        public FillsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.authenticator = authenticator;
        }

        public async Task<IList<IList<FillResponse>>> GetAllFillsAsync(int limit = 100)
        {
            var fills = await SendHttpRequestMessagePagedAsync<FillResponse>(HttpMethod.Get, authenticator, $"/fills?limit={limit}");

            return fills;
        }

        public async Task<IList<IList<FillResponse>>> GetFillsByOrderIdAsync(string orderId, int limit = 100)
        {
            var fills = await SendHttpRequestMessagePagedAsync<FillResponse>(HttpMethod.Get, authenticator, $"/fills?limit={limit}&order_id={orderId}");

            return fills;
        }

        public async Task<IList<IList<FillResponse>>> GetFillsByProductIdAsync(ProductType productId, int limit = 100)
        {
            var fills = await SendHttpRequestMessagePagedAsync<FillResponse>(HttpMethod.Get, authenticator, $"/fills?limit={limit}&product_id={productId.ToDasherizedUpper()}");

            return fills;
        }
    } // end of class FillsService

    public class FundingsService : AbstractService
    {
        private readonly IAuthenticator authenticator;

        private readonly IQueryBuilder queryBuilder;

        public FundingsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator,
            IQueryBuilder queryBuilder)
                : base(httpClient, httpRequestMessageService)
        {
            this.authenticator = authenticator;
            this.queryBuilder = queryBuilder;
        }

        public async Task<IList<IList<Funding>>> GetAllFundingsAsync(int limit = 100, FundingStatus? status = null)
        {
            var queryString = queryBuilder.BuildQuery(
                new KeyValuePair<string, string>("limit", limit.ToString()),
                new KeyValuePair<string, string>("status", status?.ToString().ToLower()));

            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<Funding>(HttpMethod.Get, authenticator, "/funding" + queryString);

            return httpResponseMessage;
        }
    } // end of class FundingsService

    public class OrdersService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public OrdersService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)

        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<OrderResponse> PlaceMarketOrderAsync(
            OrderSide side,
            ProductType productPair,
            decimal size)
        {
            var newOrder = JsonConvert.SerializeObject(new Order
            {
                side = side.ToString().ToLower(),
                product_id = productPair.ToDasherizedUpper(),
                type = OrderType.Market.ToString().ToLower(),
                size = size
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/orders", newOrder);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(contentBody);

            return orderResponse;
        }

        public async Task<OrderResponse> PlaceLimitOrderAsync(
            OrderSide side,
            ProductType productPair,
            decimal size,
            decimal price,
            TimeInForce timeInForce = TimeInForce.Gtc,
            bool postOnly = true)
        {
            var newOrder = JsonConvert.SerializeObject(new Order
            {
                side = side.ToString().ToLower(),
                product_id = productPair.ToDasherizedUpper(),
                type = OrderType.Limit.ToString().ToLower(),
                price = price,
                size = size,
                time_in_force = timeInForce.ToString().ToUpper(),
                post_only = postOnly
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/orders", newOrder).ConfigureAwait(false);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(contentBody);

            return orderResponse;
        }

        public async Task<OrderResponse> PlaceLimitOrderAsync(
            OrderSide side,
            ProductType productPair,
            decimal size,
            decimal price,
            GoodTillTime cancelAfter,
            bool postOnly = true)
        {
            var newOrder = JsonConvert.SerializeObject(new Order
            {
                side = side.ToString().ToLower(),
                product_id = productPair.ToDasherizedUpper(),
                type = OrderType.Limit.ToString().ToLower(),
                price = price,
                size = size,
                cancel_after = cancelAfter.ToString().ToLower(),
                post_only = postOnly
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/orders", newOrder);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(contentBody);

            return orderResponse;
        }

        public async Task<OrderResponse> PlaceStopOrderAsync(
            OrderSide side,
            ProductType productPair,
            decimal size,
            decimal stopPrice)
        {
            var newOrder = JsonConvert.SerializeObject(new Order
            {
                side = side.ToString().ToLower(),
                product_id = productPair.ToDasherizedUpper(),
                type = OrderType.Stop.ToString().ToLower(),
                price = stopPrice,
                size = size
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/orders", newOrder);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(contentBody);

            return orderResponse;
        }

        public async Task<CancelOrderResponse> CancelAllOrdersAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Delete, authenticator, "/orders");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<IEnumerable<Guid>>(contentBody);

            return new CancelOrderResponse
            {
                OrderIds = orderResponse
            };
        }

        public async Task<CancelOrderResponse> CancelOrderByIdAsync(string id)
        {
            var httpRequestResponse = await SendHttpRequestMessageAsync(HttpMethod.Delete, authenticator, $"/orders/{id}");

            if (httpRequestResponse == null)
            {
                return new CancelOrderResponse
                {
                    OrderIds = Enumerable.Empty<Guid>()
                };
            }

            return new CancelOrderResponse
            {
                OrderIds = new List<Guid> { new Guid(id) }
            };
        }

        public async Task<IList<IList<OrderResponse>>> GetAllOrdersAsync(int limit = 100)
        {
            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<OrderResponse>(HttpMethod.Get, authenticator, $"/orders?limit={limit}");

            return httpResponseMessage;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(string id)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/orders/{id}");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(contentBody);

            return orderResponse;
        }
    } // end of class OrdersService

    public class PaymentsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public PaymentsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)

        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/payment-methods");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var paymentMethodsResponse = JsonConvert.DeserializeObject<IEnumerable<PaymentMethod>>(contentBody);

            return paymentMethodsResponse;
        }
    } // end of class PaymentsService

    public class ProductsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        private readonly IQueryBuilder queryBuilder;

        public ProductsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator,
            IQueryBuilder queryBuilder)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
            this.queryBuilder = queryBuilder;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/products");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var productsResponse = JsonConvert.DeserializeObject<IEnumerable<Product>>(contentBody);

            return productsResponse;
        }

        public async Task<ProductsOrderBookResponse> GetProductOrderBookAsync(ProductType productPair, ProductLevel productLevel = ProductLevel.One)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/products/{productPair.ToDasherizedUpper()}/book/?level={(int)productLevel}");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var productsOrderBookJsonResponse = JsonConvert.DeserializeObject<ProductsOrderBookJsonResponse>(contentBody);

            var productOrderBookResponse = ConvertProductOrderBookResponse(productsOrderBookJsonResponse, productLevel);

            return productOrderBookResponse;
        }

        public async Task<ProductTicker> GetProductTickerAsync(ProductType productPair)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/products/{productPair.ToDasherizedUpper()}/ticker");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var productTickerResponse = JsonConvert.DeserializeObject<ProductTicker>(contentBody);

            return productTickerResponse;
        }

        public async Task<ProductStats> GetProductStatsAsync(ProductType productPair)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/products/{productPair.ToDasherizedUpper()}/stats");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var productStatsResponse = JsonConvert.DeserializeObject<ProductStats>(contentBody);

            return productStatsResponse;
        }

        public async Task<IEnumerable<object[]>> GetHistoricRatesAsync(ProductType productPair, DateTime start, DateTime end, int granularity)
        {
            var isoStart = start.ToString("s");
            var isoEnd = end.ToString("s");

            var queryString = queryBuilder.BuildQuery(
                new KeyValuePair<string, string>("start", isoStart),
                new KeyValuePair<string, string>("end", isoEnd),
                new KeyValuePair<string, string>("granularity", granularity.ToString()));

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/products/{productPair.ToDasherizedUpper()}/candles" + queryString);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var productHistoryResponse = JsonConvert.DeserializeObject<IEnumerable<object[]>>(contentBody);

            return productHistoryResponse;
        }

        private ProductsOrderBookResponse ConvertProductOrderBookResponse(
            ProductsOrderBookJsonResponse productsOrderBookJsonResponse,
            ProductLevel productLevel)
        {
            var askList = productsOrderBookJsonResponse.Asks.Select(product => product.ToArray()).Select(askArray => new Ask(Convert.ToDecimal(askArray[0], CultureInfo.InvariantCulture), Convert.ToDecimal(askArray[1], CultureInfo.InvariantCulture))
            {
                OrderId = productLevel == ProductLevel.Three
                    ? new Guid(askArray[2])
                    : (Guid?)null,
                NumberOfOrders = productLevel == ProductLevel.Three
                    ? (decimal?)null
                    : Convert.ToDecimal(askArray[2], CultureInfo.InvariantCulture)
            }).ToArray();

            var bidList = productsOrderBookJsonResponse.Bids.Select(product => product.ToArray()).Select(bidArray => new Bid(Convert.ToDecimal(bidArray[0], CultureInfo.InvariantCulture), Convert.ToDecimal(bidArray[1], CultureInfo.InvariantCulture))
            {
                OrderId = productLevel == ProductLevel.Three
                    ? new Guid(bidArray[2])
                    : (Guid?)null,
                NumberOfOrders = productLevel == ProductLevel.Three
                    ? (decimal?)null
                    : Convert.ToDecimal(bidArray[2], CultureInfo.InvariantCulture)
            });

            var productOrderBookResponse = new ProductsOrderBookResponse(productsOrderBookJsonResponse.Sequence, bidList, askList);
            return productOrderBookResponse;
        }
    } // end of class ProductsService

    public class WithdrawalsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public WithdrawalsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<WithdrawalResponse> WithdrawFundsAsync(string paymentMethodId, decimal amount, Currency currency)
        {
            var newWithdrawal = JsonConvert.SerializeObject(new Withdrawal
            {
                amount = amount,
                currency = currency.ToString().ToUpper(),
                payment_method_id = new Guid(paymentMethodId)
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/withdrawals/payment-method", newWithdrawal);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var withdrawalResponse = JsonConvert.DeserializeObject<WithdrawalResponse>(contentBody);

            return withdrawalResponse;
        }

        public async Task<CoinbaseResponse> WithdrawToCoinbaseAsync(string coinbase_account_id, decimal amount, Currency currency)
        {
            var newCoinbaseWithdrawal = JsonConvert.SerializeObject(new Coinbase
            {
                amount = amount,
                currency = currency.ToString().ToUpper(),
                coinbase_account_id = new Guid(coinbase_account_id)
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/withdrawals/coinbase-account", newCoinbaseWithdrawal);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var coinbaseResponse = JsonConvert.DeserializeObject<CoinbaseResponse>(contentBody);

            return coinbaseResponse;
        }

        public async Task<CryptoResponse> WithdrawToCryptoAsync(string crypto_address, decimal amount, Currency currency)
        {
            var newCryptoWithdrawal = JsonConvert.SerializeObject(new Crypto
            {
                amount = amount,
                currency = currency.ToString().ToUpper(),
                crypto_address = new Guid(crypto_address)
            });

            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Post, authenticator, "/withdrawals/crypto", newCryptoWithdrawal);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var cryptoResponse = JsonConvert.DeserializeObject<CryptoResponse>(contentBody);

            return cryptoResponse;
        }
    } // end of class WithdrawalsService


    public abstract class AbstractService
    {
        private readonly IHttpRequestMessageService httpRequestMessageService;

        private readonly IHttpClient httpClient;

        protected AbstractService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService)
        {
            this.httpRequestMessageService = httpRequestMessageService;
            this.httpClient = httpClient;
        }

        protected async Task<HttpResponseMessage> SendHttpRequestMessageAsync(
            HttpMethod httpMethod,
            IAuthenticator authenticator,
            string uri,
            string content = null)
        {
            var httpRequestMessage = content == null
                ? httpRequestMessageService.CreateHttpRequestMessage(httpMethod, authenticator, uri)
                : httpRequestMessageService.CreateHttpRequestMessage(httpMethod, authenticator, uri, content);

            var httpResponseMessage = await httpClient.SendASync(httpRequestMessage).ConfigureAwait(false);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException(contentBody);
            }

            return httpResponseMessage;
        }

        protected async Task<IList<IList<T>>> SendHttpRequestMessagePagedAsync<T>(
            HttpMethod httpMethod,
            IAuthenticator authenticator,
            string uri,
            string content = null)
        {
            var pagedList = new List<IList<T>>();

            var httpRequestMessage = content == null
                ? httpRequestMessageService.CreateHttpRequestMessage(httpMethod, authenticator, uri)
                : httpRequestMessageService.CreateHttpRequestMessage(httpMethod, authenticator, uri, content);

            var httpResponseMessage = await httpClient.SendASync(httpRequestMessage).ConfigureAwait(false);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);

            var firstPage = JsonConvert.DeserializeObject<IList<T>>(contentBody);

            pagedList.Add(firstPage);

            if (!httpResponseMessage.Headers.TryGetValues("cb-after", out var firstPageAfterCursorId))
            {
                return pagedList;
            }

            var subsequentPages = await GetAllSubsequentPages<T>(authenticator, uri, firstPageAfterCursorId.First());

            pagedList.AddRange(subsequentPages);

            return pagedList;
        }

        private async Task<IList<IList<T>>> GetAllSubsequentPages<T>(
            IAuthenticator authenticator,
            string uri,
            string firstPageAfterCursorId)
        {
            var pagedList = new List<IList<T>>();
            var subsequentPageAfterHeaderId = firstPageAfterCursorId;

            while (true)
            {
                var subsequentHttpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, uri + $"&after={subsequentPageAfterHeaderId}").ConfigureAwait(false);

                if (!subsequentHttpResponseMessage.Headers.TryGetValues("cb-after", out var cursorHeaders))
                {
                    break;
                }

                subsequentPageAfterHeaderId = cursorHeaders.First();

                var subsequentContentBody = await httpClient.ReadAsStringAsync(subsequentHttpResponseMessage).ConfigureAwait(false);
                var page = JsonConvert.DeserializeObject<IList<T>>(subsequentContentBody);

                pagedList.Add(page);
            }

            return pagedList;
        }
    } // end of class AbstractService


} // end of namespace

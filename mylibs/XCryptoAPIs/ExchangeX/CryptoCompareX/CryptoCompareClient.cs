using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
//using CryptoCompareApi.News;

namespace CryptoAPIs.ExchangeX.CryptoCompareX
{
    /// <summary>
    /// Interface for cryptocompare api client.
    /// </summary>
    public interface ICryptoCompareClient : IDisposable
    {
        /// <summary>
        /// Gets the api client for coins related api endpoints.
        /// </summary>
        ICoinsClient Coins { get; }

        /// <summary>
        /// Gets the client for exchanges related api endpoints.
        /// </summary>
        IExchangesClient Exchanges { get; }

        /// <summary>
        /// Gets the api client for market history.
        /// </summary>
        IHistoryClient History { get; }

        /// <summary>
        /// Gets the api client for cryptocurrency prices.
        /// </summary>
        IPricesClient Prices { get; }

        /// <summary>
        /// Gets the api client for api calls rate limits.
        /// </summary>
        IRateLimitsClient RateLimits { get; }

        /// <summary>
        /// Gets the api client for subs endpoints.
        /// </summary>
        ISubsClient Subs { get; }

        /// <summary>
        /// Gets the api client for "tops" endpoints.
        /// </summary>
        ITopsClient Tops { get; }

        /// <summary>
        /// Gets the api client for news endpoints.
        /// </summary>
        INewsClient News { get; }

        /// <summary>
        /// Gets the api client for "social stats" endpoints.
        /// </summary>
        ISocialStatsClient SocialStats { get; }

    }

    /// <summary>
    /// CryptoCompare api client.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.ICryptoCompareClient"/>
    public class CryptoCompareClient : ICryptoCompareClient
    {
        private static readonly Lazy<CryptoCompareClient> Lazy =
            new Lazy<CryptoCompareClient>(() => new CryptoCompareClient());

        private readonly HttpClient _httpClient;

        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.CryptoCompareClient class.
        /// </summary>
        /// <param name="httpClientHandler">Custom HTTP client handler. Can be used to define proxy settigs</param>
        public CryptoCompareClient([NotNull] HttpClientHandler httpClientHandler)
        {
            Check.NotNull(httpClientHandler, nameof(httpClientHandler));
            this._httpClient = new HttpClient(httpClientHandler, true);
        }

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.CryptoCompareClient class.
        /// </summary>
        public CryptoCompareClient()
            : this(new HttpClientHandler())
        {
        }

        /// <summary>
        /// Gets a Singleton instance of CryptoCompare api client.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CryptoCompareClient Instance => Lazy.Value;

        /// <summary>
        /// Gets the client for coins related api endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Coins"/>
        public ICoinsClient Coins => new CoinsClient(this._httpClient);

        /// <summary>
        /// Gets the client for exchanges related api endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Exchanges"/>
        public IExchangesClient Exchanges => new ExchangesClient(this._httpClient);

        /// <summary>
        /// Gets the api client for market history.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.History"/>
        public IHistoryClient History => new HistoryClient(this._httpClient);

        /// <summary>
        /// Gets the api client for cryptocurrency prices.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Prices"/>
        public IPricesClient Prices => new PricesClient(this._httpClient);

        /// <summary>
        /// Gets or sets the client for api calls rate limits.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.RateLimits"/>
        public IRateLimitsClient RateLimits => new RateLimitsClient(this._httpClient);

        /// <summary>
        /// The subs.
        /// </summary>
        public ISubsClient Subs => new SubsClient(this._httpClient);

        /// <summary>
        /// Gets the api client for "tops" endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Tops"/>
        public ITopsClient Tops => new TopsClient(this._httpClient);


        /// <summary>
        /// Gets the api client for news endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.News"/>
        public INewsClient News => new NewsClient(this._httpClient);

        /// <summary>
        /// Gets the api client for "social" endpoints.
        /// </summary>
        /// <seealso cref="P:CryptoCompare.ICryptoCompareClient.Social"/>
        public ISocialStatsClient SocialStats => new SocialStatsClient(this._httpClient);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <seealso cref="M:System.IDisposable.Dispose()"/>
        public void Dispose() => this.Dispose(true);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to
        /// release only unmanaged resources.</param>
        internal virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._httpClient?.Dispose();
                }
                this._isDisposed = true;
            }
        }
    }

    /// <summary>
    /// A base API client.
    /// </summary>
    public abstract class BaseApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Clients.BaseApiClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        protected BaseApiClient([NotNull] HttpClient httpClient)
        {
            Check.NotNull(httpClient, nameof(httpClient));
            this._httpClient = httpClient;
        }

        /// <summary>
        /// Sends an api request asynchronously usin GET method.
        /// </summary>
        /// <typeparam name="TApiResponse">Type of the API response.</typeparam>
        /// <param name="resourceUri">The resource uri path.</param>
        /// <returns>
        /// The asynchronous result that yields the asynchronous.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.Clients.IApiClient.GetAsync{TApiResponse}(Uri)"/>
        public Task<TApiResponse> GetAsync<TApiResponse>(Uri resourceUri) =>
            this.SendRequestAsync<TApiResponse>(HttpMethod.Get, resourceUri);

        /// <summary>
        /// Sends an api request asynchronously.
        /// </summary>
        /// <exception cref="CryptoCompareException">Thrown when a CryptoCompare api error occurs.</exception>
        /// <typeparam name="TApiResponse">Type of the API response.</typeparam>
        /// <param name="httpMethod">The HttpMethod</param>
        /// <param name="resourceUri">The resource uri path</param>
        /// <returns>
        /// The asynchronous result that yields a TApiResponse.
        /// </returns>
        public async Task<TApiResponse> SendRequestAsync<TApiResponse>(HttpMethod httpMethod, [NotNull] Uri resourceUri)
        {
            Check.NotNull(resourceUri, nameof(resourceUri));

            var response = await this._httpClient.SendAsync(new HttpRequestMessage(httpMethod, resourceUri))
                               .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                var apiResponseObject = JsonConvert.DeserializeObject<TApiResponse>(jsonResponse);

                var baseApiResponse = apiResponseObject as BaseApiResponse;
                if (baseApiResponse != null && !baseApiResponse.IsSuccessfulResponse)
                {
                    throw new CryptoCompareException(baseApiResponse);
                }

                return apiResponseObject;
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                var apiErrorResponse = JsonConvert.DeserializeObject<BaseApiResponse>(jsonResponse);
                throw new CryptoCompareException(apiErrorResponse, jsonSerializationException);
            }
        }
    }

    /// <summary>
    /// The coins client. Gets general info for all the coins available on the website.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Clients.ICoinsClient"/>
    public class CoinsClient : BaseApiClient, ICoinsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Clients.CoinsClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public CoinsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Returns all the coins that CryptoCompare has added to the website.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.ICoinsClient.AllCoinsAsync()"/>
        public async Task<CoinListResponse> ListAsync()
        {
            return await this.GetAsync<CoinListResponse>(ApiUrls.AllCoins()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets data for a currency pair. It returns general block explorer information,
        /// aggregated data and individual data for each exchange available.
        /// </summary>
        /// <param name="fromSymbol">The symbol of the currency you want to get that for.</param>
        /// <param name="toSymbol">The symbol of the currency that data will be in.</param>
        /// <seealso cref="M:CryptoCompare.Clients.ICoinsClient.SnapshotAsync(string,string)"/>
        public async Task<CoinSnapshotResponse> SnapshotAsync([NotNull] string fromSymbol, [NotNull] string toSymbol)
        {
            Check.NotNull(toSymbol, nameof(toSymbol));
            Check.NotNull(fromSymbol, nameof(fromSymbol));
            return await this.GetAsync<CoinSnapshotResponse>(ApiUrls.CoinSnapshot(fromSymbol, toSymbol))
                       .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the general, subs (used to connect to the streamer and to figure out what exchanges we have data for and what are the exact coin pairs of the coin) 
        /// and the aggregated prices for all pairs available..
        /// </summary>
        /// <param name="id">The id of the coin you want data for.</param>
        /// <returns>
        /// The asynchronous result that yields a full CoinSnapshot.
        /// </returns>
        public async Task<CoinSnapshotFullResponse> SnapshotFullAsync(int id)
        {
            return await this.GetAsync<CoinSnapshotFullResponse>(ApiUrls.CoinSnapshotFull(id)).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// The exchanges api client.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Clients.BaseApiClient"/>
    /// <seealso cref="T:CryptoCompare.Clients.IExchangesClient"/>
    public class ExchangesClient : BaseApiClient, IExchangesClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Clients.ExchangesClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public ExchangesClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// all the exchanges that CryptoCompare has integrated with.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.ICoinsClient.AllExchangesAsync()"/>
        public async Task<ExchangeListResponse> ListAsync()
        {
            return await this.GetAsync<ExchangeListResponse>(ApiUrls.AllExchanges()).ConfigureAwait(false);
        }
    }

    public class HistoryClient : BaseApiClient, IHistoryClient
    {
        public HistoryClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the daily historical data.
        /// The values are based on 00:00 GMT time.It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">List of exchanges names.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        public async Task<HistoryResponse> DayAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null,
            string exchangeName = null,
            DateTimeOffset? toDate = null,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null)
        {
            return await this.History(
                       "day",
                       fromSymbol,
                       toSymbol,
                       limit,
                       exchangeName,
                       toDate,
                       allData,
                       aggregate,
                       tryConversion).ConfigureAwait(false);
        }

        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the hourly historical data.
        /// It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">List of exchanges names.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        public async Task<HistoryResponse> HourAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null,
            string exchangeName = null,
            DateTimeOffset? toDate = null,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null)
        {
            return await this.History(
                       "hour",
                       fromSymbol,
                       toSymbol,
                       limit,
                       exchangeName,
                       toDate,
                       allData,
                       aggregate,
                       tryConversion).ConfigureAwait(false);
        }

        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the each minute historical data. 
        /// This data is only stored for 7 days, if you need more,use the hourly or daily path. 
        /// It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">List of exchanges names.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        public async Task<HistoryResponse> MinuteAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null,
            string exchangeName = null,
            DateTimeOffset? toDate = null,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null)
        {
            return await this.History(
                       "minute",
                       fromSymbol,
                       toSymbol,
                       limit,
                       exchangeName,
                       toDate,
                       allData,
                       aggregate,
                       tryConversion).ConfigureAwait(false);
        }

        private async Task<HistoryResponse> History(
            [NotNull] string method,
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null,
            string exchangeName = null,
            DateTimeOffset? toDate = null,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null)
        {
            Check.NotNullOrWhiteSpace(method, nameof(method));
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));

            return await this.GetAsync<HistoryResponse>(
                       ApiUrls.History(
                           method,
                           fromSymbol,
                           toSymbol,
                           limit,
                           exchangeName,
                           toDate,
                           allData,
                           aggregate,
                           tryConversion)).ConfigureAwait(false);
        }
    }

    public interface IApiClient
    {
        /// <summary>
        /// Sends an api request asynchronously usin GET method.
        /// </summary>
        /// <param name="resourceUri">The resource uri path.</param>
        /// <returns>
        /// The asynchronous result that yields the asynchronous.
        /// </returns>
        Task<TApiResponse> GetAsync<TApiResponse>([NotNull] Uri resourceUri);

        /// <summary>
        /// Sends an api request asynchronously.
        /// </summary>
        /// <exception cref="CryptoCompareException">Thrown when a CryptoCompare api error occurs.</exception>
        /// <typeparam name="TApiResponse">Type of the API response.</typeparam>
        /// <param name="httpMethod">The HttpMethod</param>
        /// <param name="resourceUri">The resource uri path</param>
        /// <returns>
        /// The asynchronous result that yields a TApiResponse.
        /// </returns>
        Task<TApiResponse> SendRequestAsync<TApiResponse>(HttpMethod httpMethod, [NotNull] Uri resourceUri);
    }

    /// <summary>
    /// Coins api client. Gets general info for all the coins available on the website.
    /// </summary>
    public interface ICoinsClient : IApiClient
    {
        /// <summary>
        /// Returns all the coins that CryptoCompare has added to the website. 
        /// </summary>
        Task<CoinListResponse> ListAsync();

        /// <summary>
        /// Gets data for a currency pair. 
        /// It returns general block explorer information, aggregated data and individual data for each exchange available.
        /// </summary>
        /// <param name="fromSymbol">The symbol of the currency you want to get that for.</param>
        /// <param name="toSymbol">The symbol of the currency that data will be in.</param>
        Task<CoinSnapshotResponse> SnapshotAsync(string fromSymbol, string toSymbol);

        /// <summary>
        /// Get the general, subs (used to connect to the streamer and to figure out what exchanges we have data for and what are the exact coin pairs of the coin) 
        /// and the aggregated prices for all pairs available..
        /// </summary>
        /// <param name="id">The id of the coin you want data for.</param>
        /// <returns>
        /// The asynchronous result that yields a full CoinSnapshot.
        /// </returns>
        Task<CoinSnapshotFullResponse> SnapshotFullAsync(int id);
    }

    /// <summary>
    /// Interface for exchanges api client.
    /// </summary>
    public interface IExchangesClient : IApiClient
    {
        /// <summary>
        /// all the exchanges that CryptoCompare has integrated with..
        /// </summary>
        Task<ExchangeListResponse> ListAsync();
    }

    public interface IHistoryClient : IApiClient
    {
        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the daily historical data.
        /// The values are based on 00:00 GMT time.It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">Exchange name.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        Task<HistoryResponse> DayAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit,
            string exchangeName,
            DateTimeOffset? toDate,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null);

        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the hourly historical data.
        /// It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">Exchange name.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        Task<HistoryResponse> HourAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null,
            string exchangeName = null,
            DateTimeOffset? toDate = null,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null);

        /// <summary>
        /// Get open, high, low, close, volumefrom and volumeto from the each minute historical data. 
        /// This data is only stored for 7 days, if you need more,use the hourly or daily path. 
        /// It uses BTC conversion if data is not available because the coin is not trading in the specified currency.
        /// </summary>
        /// <param name="fromSymbol">from symbol. This cannot be null.</param>
        /// <param name="toSymbol">to symbol. This cannot be null.</param>
        /// <param name="limit">The limit number of returned results.</param>
        /// <param name="exchangeName">Exchange name.</param>
        /// <param name="toDate">to date.</param>
        /// <param name="allData">(Optional) retrieve all data.</param>
        /// <param name="aggregate">(Optional) aggregates result.</param>
        /// <param name="tryConversion">(Optional) tries conversion.</param>
        Task<HistoryResponse> MinuteAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit,
            string exchangeName,
            DateTimeOffset? toDate,
            bool? allData = null,
            int? aggregate = null,
            bool? tryConversion = null);
    }

    public interface INewsClient
    {
        /// <summary>
        /// Return all news providers.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NewsProvider>> NewsProviders();
        /// <summary>
        /// Get all news 
        /// </summary>
        /// <param name="lang">Language - EN,PT etc.</param>
        /// <param name="lTs">Timestamp</param>
        /// <param name="feeds">Feeds - for news</param>
        /// <param name="sign">if true cryptocompare will sign request</param>
        /// <returns></returns>
        Task<IEnumerable<NewsEntity>> News(string lang = null, long? lTs = null, string[] feeds = null,
            bool? sign = null);

    }

    public interface IPricesClient : IApiClient
    {
        /// <summary>
        /// Compute the current trading info (price, vol, open, high, low etc) of the requested pair as a volume weighted average based on the exchanges requested.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="markets">List of names of the exchanges.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        Task<PriceAverageResponse> AverageAsync(
            string fromSymbol,
            string toSymbol,
            IEnumerable<string> markets,
            bool? tryConversion = null);

        /// <summary>
        /// Get the price of any cryptocurrency in any other currency that you need at a given timestamp.
        /// The price comes from the daily info - so it would be the price at the end of the day GMT based on the requested TS.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be used for conversion.
        /// Tries to get direct trading pair data, if there is none or it is more than 10 days before the ts requested, it uses BTC conversion.
        /// If the oposite pair trades we invert it (eg.: BTC-XMR)The calculation types are: Close - a Close of the day close price,MidHighLow - the average between the 24 H high and low.VolFVolT - the total volume to / the total volume from
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="requestedDate">The requested date.</param>
        /// <param name="calculationType">(Optional) Type of the calculation.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <param name="markets">(Optional) Names of Exchanges default => CCCAGG.</param>
        Task<PriceHistoricalReponse> HistoricalAsync(
            string fromSymbol,
            IEnumerable<string> toSymbols,
            DateTimeOffset requestedDate,
            IEnumerable<string> markets = null,
            CalculationType? calculationType = null,
            bool? tryConversion = null);

        /// <summary>
        /// Same as single API path but with multiple from symbols.
        /// </summary>
        /// <param name="fromSymbols">from symbols.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <param name="exchangeName">(Optional) Exchange name defult => CCCAGG.</param>
        Task<PriceMultiResponse> MultiAsync(
            IEnumerable<string> fromSymbols,
            IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null);

        /// <summary>
        /// Get all the current trading info (price, vol, open, high, low etc) of any list of cryptocurrencies in any other currency that you need.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be used for conversion. 
        /// This API also returns Display values for all the fields.
        /// If the oposite pair trades we invert it (eg.: BTC-XMR).
        /// </summary>
        /// <param name="fromSymbols">from symbols.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <param name="exchangeName">(Optional) Exchange name default => CCCAGG.</param>
        Task<PriceMultiFullResponse> MultiFullAsync(
            IEnumerable<string> fromSymbols,
            IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null);

        /// <summary>
        /// Get the current price of any cryptocurrency in any other currency that you need.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be used for conversion.
        /// If the oposite pair trades we invert it (eg.: BTC-XMR).
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion">If set to false, it will try to get values without using any conversion at all (defaultVal:true)</param>
        /// <param name="exchangeName">Exchange name default => CCCAGG</param>
        Task<PriceSingleResponse> SingleAsync(
            string fromSymbol,
            IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null);
    }

    /// <summary>
    /// Interface of api client for cryptocompare api calls rate limits.
    /// </summary>
    public interface IRateLimitsClient : IApiClient
    {
        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the current hour..
        /// </summary>
        Task<RateLimitResponse> CurrentHourAsync();

        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the current minute.
        /// </summary>
        Task<RateLimitResponse> CurrentMinuteAsync();

        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the current second.
        /// </summary>
        Task<RateLimitResponse> CurrentSecondAsync();
    }

    public interface ISocialStatsClient
    {
        /// <summary>
        /// Get all the available social stats for a coin.
        /// </summary>
        /// <param name="id">coin id.</param>
        /// <returns>
        /// An asynchronous result that yields an object containing the social stats.
        /// </returns>
        Task<SocialStatsResponse> StatsAsync([NotNull] int id);
    }

    public interface ISubsClient : IApiClient
    {
        /// <summary>
        /// Get all the available streamer subscription channels for the requested pairs.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <returns>
        /// An asynchronous result that yields the list of subs.
        /// </returns>
        Task<SubListResponse> ListAsync(string fromSymbol, IEnumerable<string> toSymbols);
    }

    public interface ITopsClient : IApiClient
    {
        /// <summary>
        /// Get top exchanges by volume for a currency pair. 
        /// The number of exchanges you get is the minimum of the limit you set (default 5) and the total number of exchanges available.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopResponse.
        /// </returns>
        Task<TopResponse> ExchangesAsync(string fromSymbol, string toSymbol, int? limit = null);

        /// <summary>
        /// Get top pairs by volume for a currency (always uses our aggregated data). 
        /// The number of pairs you get is the minimum of the limit you set (default 5) and the total number of pairs available.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopResponse.
        /// </returns>
        Task<TopResponse> PairsAsync(string fromSymbol, int? limit = null);

        /// <summary>
        /// Get top coins by volume for the to currency. It returns volume24hto and total supply (where available). 
        /// The number of coins you get is the minimum of the limit you set (default 50) and the total number of coins available.
        /// </summary>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopVolumesResponse.
        /// </returns>
        Task<TopVolumesResponse> VolumesAsync(string toSymbol, int? limit = null);
    }

    public class NewsClient : BaseApiClient, INewsClient
    {
        public NewsClient([NotNull] HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IEnumerable<NewsProvider>> NewsProviders()
        {
            return await base.GetAsync<IEnumerable<NewsProvider>>(ApiUrls.NewsProviders()).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsEntity>> News(string lang = null, long? lTs = null, string[] feeds = null, bool? sign = null)
        {
            return await base.GetAsync<IEnumerable<NewsEntity>>(ApiUrls.News(lang, lTs, feeds, sign)).ConfigureAwait(false);
        }

    }

    public class PricesClient : BaseApiClient, IPricesClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.PricesClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public PricesClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Compute the current trading info (price, vol, open, high, low etc) of the requested pair as a volume weighted average based on the exchanges requested.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="markets">List of names of the exchanges.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <returns>
        /// An asynchronous result that yields the average.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.IPricesClient.AverageAsync(string,string,IEnumerable{string},bool?)"/>
        public async Task<PriceAverageResponse> AverageAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            [NotNull] IEnumerable<string> markets,
            bool? tryConversion = null)
        {
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            Check.NotEmpty(markets, nameof(markets));

            return await this.GetAsync<PriceAverageResponse>(
                       ApiUrls.PriceAverage(fromSymbol, toSymbol, markets, tryConversion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the price of any cryptocurrency in any other currency that you need at a given timestamp.
        /// The price comes from the daily info - so it would be the price at the end of the day GMT based on the requested TS.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be used for conversion.
        /// Tries to get direct trading pair data, if there is none or it is more than 10 days before the ts requested, it uses BTC conversion.
        /// If the oposite pair trades we invert it (eg.: BTC-XMR)The calculation types are: Close - a Close of the day close price,MidHighLow - the average between the 24 H high and low.VolFVolT - the total volume to / the total volume from.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="markets">(Optional) Exchange name default =&gt; CCCAGG.</param>
        /// <param name="requestedDate">The requested date.</param>
        /// <param name="calculationType">(Optional) Type of the calculation.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <seealso cref="M:CryptoCompare.Clients.IPricesClient.Historical(string,IEnumerable{string},DateTimeOffset,CalculationType?,bool?,string)"/>
        public async Task<PriceHistoricalReponse> HistoricalAsync(
            [NotNull] string fromSymbol,
            [NotNull] IEnumerable<string> toSymbols,
            DateTimeOffset requestedDate,
            IEnumerable<string> markets = null,
            CalculationType? calculationType = null,
            bool? tryConversion = null)
        {
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            Check.NotEmpty(toSymbols, nameof(toSymbols));

            return await this.GetAsync<PriceHistoricalReponse>(
                       ApiUrls.PriceHistorical(
                           fromSymbol,
                           toSymbols,
                           markets,
                           requestedDate,
                           calculationType,
                           tryConversion)).ConfigureAwait(false);
        }


        /// <summary>
        /// Same as single API path but with multiple from symbols.
        /// </summary>
        /// <param name="fromSymbols">from symbols.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <param name="exchangeName">(Optional) Exchange name defult =&gt; CCCAGG.</param>
        /// <seealso cref="M:CryptoCompare.Clients.IPricesClient.Multi(IEnumerable{string},IEnumerable{string},bool?,string)"/>
        public async Task<PriceMultiResponse> MultiAsync(
            [NotNull] IEnumerable<string> fromSymbols,
            [NotNull] IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null)
        {
            Check.NotEmpty(toSymbols, nameof(toSymbols));
            Check.NotEmpty(fromSymbols, nameof(fromSymbols));

            return await this.GetAsync<PriceMultiResponse>(
                       ApiUrls.PriceMulti(fromSymbols, toSymbols, tryConversion, exchangeName)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all the current trading info (price, vol, open, high, low etc) of any list of cryptocurrencies in any other currency that you need.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be used for conversion.
        /// This API also returns Display values for all the fields.
        /// If the oposite pair trades we invert it (eg.: BTC-XMR).
        /// </summary>
        /// <param name="fromSymbols">from symbols.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion">(Optional) If set to false, it will try to get values without
        /// using any conversion at all (defaultVal:true)</param>
        /// <param name="exchangeName">(Optional) Exchange name default =&gt; CCCAGG.</param>
        /// <returns>
        /// An asynchronous result that yields the multi full.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.IPricesClient.MultiFullAsync(IEnumerable{string},IEnumerable{string},bool?,string)"/>
        public async Task<PriceMultiFullResponse> MultiFullAsync(
            IEnumerable<string> fromSymbols,
            IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null)
        {
            Check.NotEmpty(toSymbols, nameof(toSymbols));
            Check.NotEmpty(fromSymbols, nameof(fromSymbols));

            return await this.GetAsync<PriceMultiFullResponse>(
                           ApiUrls.PriceMultiFull(fromSymbols, toSymbols, tryConversion, exchangeName))
                       .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the current price of any cryptocurrency in any other currency that you need.
        /// If the crypto does not trade directly into the toSymbol requested, BTC will be
        /// used for conversion. If the oposite pair trades we invert it (eg.: BTC-XMR).
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <param name="tryConversion"></param>
        /// <param name="exchangeName">Exchange name default = CCC</param>
        /// <seealso cref="M:CryptoCompare.Clients.IPricesClient.Single(string,IEnumerable{string})"/>
        public async Task<PriceSingleResponse> SingleAsync(
            [NotNull] string fromSymbol,
            [NotNull] IEnumerable<string> toSymbols,
            bool? tryConversion = null,
            string exchangeName = null)
        {
            Check.NotNull(fromSymbol, nameof(fromSymbol));
            Check.NotEmpty(toSymbols, nameof(toSymbols));

            return await this.GetAsync<PriceSingleResponse>(
                       ApiUrls.PriceSingle(fromSymbol, toSymbols, tryConversion, exchangeName)).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Api client for cryptocompare api calls rate limits.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Clients.BaseApiClient"/>
    /// <seealso cref="T:CryptoCompare.Clients.IRateLimitsClient"/>
    public class RateLimitsClient : BaseApiClient, IRateLimitsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Clients.RateLimitsClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public RateLimitsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the
        /// current hour.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.IRateLimitsClient.Hour()"/>
        public async Task<RateLimitResponse> CurrentHourAsync()
        {
            return await this.GetAsync<RateLimitResponse>(ApiUrls.RateLimitsByHour()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the
        /// current minute.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.IRateLimitsClient.Minute()"/>
        public async Task<RateLimitResponse> CurrentMinuteAsync()
        {
            return await this.GetAsync<RateLimitResponse>(ApiUrls.RateLimitsByMinute()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the rate limits left for you on the histo, price and news paths in the
        /// current second.
        /// </summary>
        /// <seealso cref="M:CryptoCompare.Clients.IRateLimitsClient.Second()"/>
        public async Task<RateLimitResponse> CurrentSecondAsync()
        {
            return await this.GetAsync<RateLimitResponse>(ApiUrls.RateLimitsBySecond()).ConfigureAwait(false);
        }
    }

    public class SocialStatsClient : BaseApiClient, ISocialStatsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.SocialClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public SocialStatsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Get all the available social stats for a coin.
        /// </summary>
        /// <param name="id">coin id.</param>
        /// <returns>
        /// An asynchronous result that yields an object containing the social stats.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.ISocialClient.StatsAsync(int id)"/>
        public async Task<SocialStatsResponse> StatsAsync([NotNull] int id)
        {
            Check.NotNull(id, nameof(id));
            return await this.GetAsync<SocialStatsResponse>(ApiUrls.SocialStats(id))
                       .ConfigureAwait(false);
        }
    }

    public class SubsClient : BaseApiClient, ISubsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.SubsClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public SubsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Get all the available streamer subscription channels for the requested pairs.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbols">to symbols.</param>
        /// <returns>
        /// An asynchronous result that yields the list of subs.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.ISubsClient.ListAsync(string,IEnumerable{string})"/>
        public async Task<SubListResponse> ListAsync(
            [NotNull] string fromSymbol,
            [NotNull] IEnumerable<string> toSymbols)
        {
            Check.NotEmpty(toSymbols, nameof(toSymbols));
            Check.NotNull(fromSymbol, nameof(fromSymbol));
            return await this.GetAsync<SubListResponse>(ApiUrls.SubsList(fromSymbol, toSymbols)).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Client for "tops" endpoints (Exchanges, volumes, pairs).
    /// </summary>
    /// <seealso cref="T:CryptoCompare.BaseApiClient"/>
    /// <seealso cref="T:CryptoCompare.ITopsClient"/>
    public class TopsClient : BaseApiClient, ITopsClient
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.TopsClient class.
        /// </summary>
        /// <param name="httpClient">The HTTP client. This cannot be null.</param>
        public TopsClient([NotNull] HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Get top exchanges by volume for a currency pair.
        /// The number of exchanges you get is the minimum of the limit you set (default 5) and the total number of exchanges available.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopResponse.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.ITopsClient.Exchanges(string,string,int?)"/>
        public async Task<TopResponse> ExchangesAsync(
            [NotNull] string fromSymbol,
            [NotNull] string toSymbol,
            int? limit = null)
        {
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            return await this.GetAsync<TopResponse>(ApiUrls.TopExchanges(fromSymbol, toSymbol, limit))
                       .ConfigureAwait(false);
        }

        /// <summary>
        /// Get top pairs by volume for a currency (always uses our aggregated data).
        /// The number of pairs you get is the minimum of the limit you set (default 5) and the total number of pairs available.
        /// </summary>
        /// <param name="fromSymbol">from symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopResponse.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.ITopsClient.Pairs(string,int?)"/>
        public async Task<TopResponse> PairsAsync([NotNull] string fromSymbol, int? limit = null)
        {
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            return await this.GetAsync<TopResponse>(ApiUrls.TopPairs(fromSymbol, limit)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get top coins by volume for the to currency. It returns volume24hto and total supply (where available).
        /// The number of coins you get is the minimum of the limit you set (default 50) and the total number of coins available.
        /// </summary>
        /// <param name="toSymbol">to symbol.</param>
        /// <param name="limit">(Optional) The limit.</param>
        /// <returns>
        /// An asynchronous result that yields a TopVolumesResponse.
        /// </returns>
        /// <seealso cref="M:CryptoCompare.ITopsClient.Volumes(string,int?)"/>
        public async Task<TopVolumesResponse> VolumesAsync([NotNull] string toSymbol, int? limit = null)
        {
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            return await this.GetAsync<TopVolumesResponse>(ApiUrls.TopVolumes(toSymbol, limit)).ConfigureAwait(false);
        }
    }


} // end of namespace

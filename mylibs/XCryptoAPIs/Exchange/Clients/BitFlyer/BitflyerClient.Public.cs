using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utf8Json;

namespace CryptoAPIs.Exchange.Clients.BitFlyer
{

    public class PublicApi
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = BitFlyerConstants.BaseUri,
            Timeout = TimeSpan.FromSeconds(5)
        };

        internal async Task<T> Get<T>(string path, Dictionary<string, object> query = null)
        {
            var queryString = string.Empty;
            if (query != null)
            {
                queryString = query.ToQueryString();
            }

            try
            {
                // TODO: see why I had to change this to use ".Result" in order to avoid the code hanging
                var response = HttpClient.GetAsync(path + queryString).Result;
                //var response = await HttpClient.GetAsync(path + queryString);
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Error error = null;
                    try
                    {
                        error = JsonSerializer.Deserialize<Error>(json);
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

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (TaskCanceledException)
            {
                throw new BitFlyerApiException(path, "Request Timeout");
            }
        }

        private const string BoardApiPath = "/v1/board";

        public async Task<Board> GetBoard(string productCode)
        {
            var query = new Dictionary<string, object>
        {
            { "product_code", productCode }
        };
            return await Get<Board>(BoardApiPath, query);
        }

        private const string BoardStateApiPath = "/v1/getboardstate";

        public async Task<BoardState> GetBoardState(string productCode)
        {
            var query = new Dictionary<string, object>
        {
            { "product_code", productCode }
        };
            return await Get<BoardState>(BoardStateApiPath, query);
        }

        private const string ChatApiPath = "/v1/getchats";

        public async Task<Chat[]> GetChat(DateTime? fromDate = null)
        {
            if (fromDate != null)
            {
                var query = new Dictionary<string, object>
            {
                { "from_date", fromDate.Value }
            };

                return await Get<Chat[]>(ChatApiPath, query);
            }

            return await Get<Chat[]>(ChatApiPath);
        }

        private const string ChatEuApiPath = "/v1/getchats/eu";

        public async Task<Chat[]> GetChatEu(DateTime? fromDate = null)
        {
            if (fromDate != null)
            {
                var query = new Dictionary<string, object>
            {
                { "from_date", fromDate.Value }
            };

                return await Get<Chat[]>(ChatEuApiPath, query);
            }

            return await Get<Chat[]>(ChatEuApiPath);
        }

        private const string ChatUsaApiPath = "/v1/getchats/usa";

        public async Task<Chat[]> GetChatUsa(DateTime? fromDate = null)
        {
            if (fromDate != null)
            {
                var query = new Dictionary<string, object>
            {
                { "from_date", fromDate.Value }
            };

                return await Get<Chat[]>(ChatUsaApiPath, query);
            }

            return await Get<Chat[]>(ChatUsaApiPath);
        }

        private const string HealthApiPath = "/v1/gethealth";

        public async Task<Health> GetHealth()
        {
            return await Get<Health>(HealthApiPath);
        }

        private const string MarketApiPath = "/v1/markets";

        public async Task<Market[]> GetMarkets()
        {
            return await Get<Market[]>(MarketApiPath);
        }

        private const string MarketEuApiPath = "/v1/markets/eu";

        public async Task<Market[]> GetMarketsEu()
        {
            return await Get<Market[]>(MarketEuApiPath);
        }

        private const string MarketUsaApiPath = "/v1/markets/usa";

        public async Task<Market[]> GetMarketsUsa()
        {
            return await Get<Market[]>(MarketUsaApiPath);
        }

        private const string ExecutionsApiPath = "/v1/executions";

        public async Task<PublicExecution[]> GetExecutions(string productCode,
            int? count = null, int? before = null, int? after = null)
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

            return await Get<PublicExecution[]>(ExecutionsApiPath, query);
        }

        private const string TickerApiPath = "/v1/ticker";

        public async Task<Ticker> GetTicker(string productCode)
        {
            var query = new Dictionary<string, object>
        {
            { "product_code", productCode }
        };
            return await Get<Ticker>(TickerApiPath, query);
        }

    } // end of class PublicApi

} // end of namespace

namespace GDAX.NET.Core {
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ExchangeClientBase {
        public const String API_ENDPOINT_URL = "https://api.gdax.com/";

        private const String ContentType = "application/json";

        private readonly CBAuthenticationContainer _authContainer;

        protected ExchangeClientBase( CBAuthenticationContainer authContainer ) { this._authContainer = authContainer; }

        protected async Task< ExchangeResponse > GetResponse( ExchangeRequestBase request ) {
            var relativeUrl = request.RequestUrl;
            var absoluteUri = new Uri( new Uri( API_ENDPOINT_URL ), relativeUrl );

            var timestamp = ( request.TimeStamp ).ToString( CultureInfo.InvariantCulture );
            var body = request.RequestBody;
            var method = request.Method;
            var url = absoluteUri.ToString();

            var passphrase = this._authContainer.Passphrase;
            var apiKey = this._authContainer.ApiKey;

            // Caution: Use the relative URL, *NOT* the absolute one.
            var signature = this._authContainer.ComputeSignature( timestamp, relativeUrl, method, body );

            using ( var httpClient = new HttpClient() ) {
                HttpResponseMessage response;

                httpClient.DefaultRequestHeaders.Add( "CB-ACCESS-KEY", apiKey );
                httpClient.DefaultRequestHeaders.Add( "CB-ACCESS-SIGN", signature );
                httpClient.DefaultRequestHeaders.Add( "CB-ACCESS-TIMESTAMP", timestamp );
                httpClient.DefaultRequestHeaders.Add( "CB-ACCESS-PASSPHRASE", passphrase );

                httpClient.DefaultRequestHeaders.Add( "User-Agent", "sefbkn.github.io" );

                switch ( method ) {
                    case "GET":
                        response = await httpClient.GetAsync( absoluteUri );
                        break;
                    case "POST":
                        var requestBody = new StringContent( body );
                        response = await httpClient.PostAsync( absoluteUri, requestBody );
                        break;
                    default:
                        throw new NotImplementedException( "The supplied HTTP method is not supported: " + method ?? "(null)" );
                }


                var contentBody = await response.Content.ReadAsStringAsync();
                var headers = response.Headers.AsEnumerable();
                var statusCode = response.StatusCode;
                var isSuccess = response.IsSuccessStatusCode;

                var genericExchangeResponse = new ExchangeResponse( statusCode, isSuccess, headers, contentBody );
                return genericExchangeResponse;
            }
        }
    }
}

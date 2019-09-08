namespace GDAX.NET.Endpoints.Fills {
    using Core;

    public class GetFillsRequest : ExchangePageableRequestBase {
        public GetFillsRequest() : base( "GET" ) {
            var urlFormat = "/fills";
            this.RequestUrl = urlFormat;
        }
    }
}

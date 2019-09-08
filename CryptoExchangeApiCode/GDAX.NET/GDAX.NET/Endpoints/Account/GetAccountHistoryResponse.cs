namespace GDAX.NET.Endpoints.Account {
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Newtonsoft.Json.Linq;

    public class GetAccountHistoryResponse : ExchangePageableResponseBase {
        public GetAccountHistoryResponse( ExchangeResponse response ) : base( response ) {
            var json = response.ContentBody;
            var jArray = JArray.Parse( json );

            this.AccountHistoryRecords = jArray.Select( AccountHistory.FromJToken ).ToList();
        }

        public List< AccountHistory > AccountHistoryRecords { get; private set; }
    }
}

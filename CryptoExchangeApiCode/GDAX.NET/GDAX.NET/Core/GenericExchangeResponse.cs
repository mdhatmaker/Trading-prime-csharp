namespace GDAX.NET.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class ExchangeResponse {
        public ExchangeResponse( HttpStatusCode statusCode, Boolean isSuccess, IEnumerable< KeyValuePair< String, IEnumerable< String > > > headers, String contentBody ) {
            this.Headers = headers.ToList();
            this.StatusCode = statusCode;
            this.ContentBody = contentBody;
            this.IsSuccessStatusCode = isSuccess;
        }

        public IEnumerable< KeyValuePair< String, IEnumerable< String > > > Headers { get; private set; }
        public String ContentBody { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public Boolean IsSuccessStatusCode { get; private set; }
    }
}

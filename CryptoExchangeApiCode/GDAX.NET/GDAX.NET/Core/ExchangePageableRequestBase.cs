namespace GDAX.NET.Core {
    using System;

    public abstract class ExchangePageableRequestBase : ExchangeRequestBase {
        public ExchangePageableRequestBase( String method, String cursor = null, Int32 recordCount = 100 ) : base( method ) { }

        public RequestPaginationType PaginationType { get; protected set; }
        public String Cursor { get; protected set; }
        public Int64 RecordCount { get; protected set; }
    }
}

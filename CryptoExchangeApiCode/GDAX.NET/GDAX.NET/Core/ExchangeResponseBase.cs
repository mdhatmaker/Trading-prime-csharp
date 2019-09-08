namespace GDAX.NET.Core {
    using System;

    public abstract class ExchangeResponseBase {
        private ExchangeResponseBase() { }

        protected ExchangeResponseBase( ExchangeResponse response ) { }
        public String BeforePaginationToken { get; set; }
        public String AfterPaginationToken { get; set; }
    }
}

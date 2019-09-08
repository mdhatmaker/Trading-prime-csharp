namespace GDAX.NET.Errors {
    using System;

    public class ExchangeErrorBase {
        public String Message { get; set; }
        public Int32 StatusCode { get; set; }
    }
}

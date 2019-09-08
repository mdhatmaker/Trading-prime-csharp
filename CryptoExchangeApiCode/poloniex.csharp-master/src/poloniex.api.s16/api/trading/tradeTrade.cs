using Newtonsoft.Json;
using System;
using XCT.BaseLib.Configuration;

namespace XCT.BaseLib.API.Poloniex.Trade
{
    public interface ITrade : ITradeOrder
    {
        DateTime Time { get; }
    }

    public class Trade : TradeOrder, ITrade
    {
        [JsonProperty("date")]
        private string TimeInternal
        {
            set { Time = value.ParseDateTime(); }
        }
        public DateTime Time { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class PriceUpdateIQ : PriceUpdate
    {
        public string MessageContents { get; private set; }
        public string MostRecentMarketConditions { get; private set; }

        public void FromUpdateMsg(string msg)
        {
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";
            var split = msg.Split(new char[] { ',' });
            // THE FOLLOWING ARE VALID FOR PROTOCOL 5.2
            int iresult;
            decimal dresult;
            this.Symbol = split[1];
            this.LastTradePrice = decimal.MinValue; //0.0M;
            if (decimal.TryParse(split[2], out dresult))
                this.LastTradePrice = dresult;
            this.LastTradeSize = 0;
            if (int.TryParse(split[3], out iresult))
                this.LastTradeSize = iresult;
            this.LastTradeTime = split[4];
            var lastTradeMarketCenter = split[5];
            var totalVolume = int.Parse(split[6]);
            this.Bid = decimal.MinValue;    //0.0M;
            if (decimal.TryParse(split[7], out dresult))
                this.Bid = dresult;
            this.BidSize = 0;
            if (int.TryParse(split[8], out iresult))
                this.BidSize = iresult;
            this.Ask = decimal.MinValue;    //0.0M;
            if (decimal.TryParse(split[9], out dresult))
                this.Ask = dresult;
            this.AskSize = 0;
            if (int.TryParse(split[10], out iresult))
                this.AskSize = iresult;
            this.OpenPrice = decimal.MinValue;
            this.HighPrice = decimal.MinValue;
            this.LowPrice = decimal.MinValue;
            this.ClosePrice = decimal.MinValue;
            decimal openPrice, highPrice, lowPrice, closePrice;
            if (decimal.TryParse(split[11], out openPrice)) OpenPrice = openPrice;
            if (decimal.TryParse(split[12], out highPrice)) HighPrice = highPrice;
            if (decimal.TryParse(split[13], out lowPrice)) LowPrice = lowPrice;
            if (decimal.TryParse(split[14], out closePrice)) ClosePrice = closePrice;
            this.MessageContents = split[15];
            this.MostRecentMarketConditions = split[16];
        }


    } // end of class
} // end of namespace

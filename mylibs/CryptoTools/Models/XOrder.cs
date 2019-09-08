using System;
using System.Collections.Generic;
using System.Text;
using CryptoTools;

namespace CryptoTools.Models
{
    public class XOrder
    {
        public DateTime TimeCreated { get; private set; }

        public decimal Amount { get; set; }
        public decimal AmountFilled { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Fees { get; set; }
        public string FeesCurrency { get; set; }
        public bool IsBuy { get; set; }
        public string Message { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderId { get; set; }
        public decimal Price { get; set; }
        public OrderResult Result { get; set; }
        public string Symbol { get; set; }

        //public IExchangeAPI API => m_api;
        //public ExchangeOrderResult EOR => m_eor;
        public string Exchange { get; private set; }


        //private IExchangeAPI m_api;
        //private ExchangeOrderResult m_eor;
        //private string m_exchange;
        private string m_strategyId;
        
        public XOrder(string exchange, string strategyId = "")
        {
            Exchange = exchange;
            m_strategyId = strategyId;
            TimeCreated = DateTime.Now;
        }
        

        public override string ToString()
        {
            string buySell = IsBuy ? "BUY " : "SELL";
            return string.Format("{0,-20} [oid:{1,9}] {2,-15} {3,-9} {4,-4} {5:0.00000000}  {6,13}    filled:{7}  avg_price:{8}  fees:{9} ({10}) '{11}'", OrderDate.ToLocalTime(), OrderId, Result.ToString(), Symbol, buySell, Price, Amount, AmountFilled, AveragePrice, Fees, FeesCurrency, Message);
        }



        
    } // end of class XOrder

} // end of namespace

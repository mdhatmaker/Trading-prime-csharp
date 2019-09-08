using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTechnologies.TTAPI;
using static Tools.G;

namespace ZeroSumAPI
{
    // Define the product (see TTAPI.ProductKey) AND a specific contract (ex: "Dec17")
    // (the actual Instrument will be updated when the FoundInstrument method is called)
    public class TTInstrument
    {
        public ProductKey ProductKey { get; private set; }
        public string Contract { get; private set; }
        public Instrument Instrument { get { return m_instrument; } }
        public InstrumentKey InstrumentKey { get { return m_instrument == null ? InstrumentKey.Empty : m_instrument.Key; } }
        public List<OrderFeed> OrderFeeds { get { return m_orderFeeds; } }
        public int PriceDecimals { get; set; }
        public OrderFeed DefaultOrderFeed { get; set; }

        private Instrument m_instrument;
        private List<OrderFeed> m_orderFeeds;

        public TTInstrument(ProductKey productKey, string contract)
        {
            this.ProductKey = productKey;
            this.Contract = contract;
            m_instrument = null;
            m_orderFeeds = null;
            this.DefaultOrderFeed = null;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} [{2}]", this.ProductKey, this.Contract, this.InstrumentKey);
        }

        public decimal ConvertPrice(int price)
        {
            return (decimal)price / (decimal)Math.Pow(10, PriceDecimals);
        }

        public void FoundInstrument(Instrument instrument)
        {
            //dout("FOUND: {0}", instrument.Name);
            m_instrument = instrument;
            m_orderFeeds = new List<OrderFeed>();
            foreach (OrderFeed orderFeed in instrument.GetValidOrderFeeds())
            {
                m_orderFeeds.Add(orderFeed);
            }
            if (m_orderFeeds.Count > 0)
                this.DefaultOrderFeed = m_orderFeeds[0];
        }
    } // end of class

} // end of namespace

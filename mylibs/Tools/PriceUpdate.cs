using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class PriceUpdate
    {
        public string Symbol { get; protected set; }
        public decimal LastTradePrice { get; protected set; }
        public int LastTradeSize { get; protected set; }
        public string LastTradeTime { get; protected set; }
        public decimal Bid { get; protected set; }
        public int BidSize { get; protected set; }
        public decimal Ask { get; protected set; }
        public int AskSize { get; protected set; }
        public int TotalVolume { get; protected set; }
        public decimal OpenPrice { get; protected set; }
        public decimal HighPrice { get; protected set; }
        public decimal LowPrice { get; protected set; }
        public decimal ClosePrice { get; protected set; }

        public decimal Mid { get { return (Bid + Ask) / 2; }}
        public decimal WeightedMid { get { return Bid + (Ask - Bid) * (AskSize / (BidSize+AskSize)); }}     // TODO: check this calculation

        public decimal PercentChange { get { return (decimal.MinValue==LastTradePrice || decimal.MinValue==ClosePrice) ? decimal.MinValue : Math.Round((LastTradePrice - ClosePrice) / ClosePrice * 100.0M, 2); } }

        //public static PriceUpdate Empty { get { return } }

        public PriceUpdate() { }

        // (starting after 'Q,') "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,"
        public PriceUpdate(string sUpdate)
        {
            FromString(sUpdate);
        }

        // Copy Constructor
        public PriceUpdate(PriceUpdate update)
        {
            Symbol = update.Symbol;
            LastTradePrice = update.LastTradePrice;
            LastTradeSize = update.LastTradeSize;
            LastTradeTime = update.LastTradeTime;
            Bid = update.Bid;
            BidSize = update.BidSize;
            Ask = update.Ask;
            AskSize = update.AskSize;
            TotalVolume = update.TotalVolume;
            OpenPrice = update.OpenPrice;
            HighPrice = update.HighPrice;
            LowPrice = update.LowPrice;
            ClosePrice = update.ClosePrice;
        }

        public void CopyTo(PriceUpdate update)
        {
            update.Symbol = Symbol;
            update.LastTradePrice = LastTradePrice;
            update.LastTradeSize = LastTradeSize;
            update.LastTradeTime = LastTradeTime;
            update.Bid = Bid;
            update.BidSize = BidSize;
            update.Ask = Ask;
            update.AskSize = AskSize;
            update.TotalVolume = TotalVolume;
            update.OpenPrice = OpenPrice;
            update.HighPrice = HighPrice;
            update.LowPrice = LowPrice;
            update.ClosePrice = ClosePrice;
        }

        public bool HasSymbolRoot(params string[] roots)
        {
            bool result = false;
            for (int i = 0; i < roots.Length; ++i)
            {
                if (Symbol.StartsWith(roots[i]))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // Convert PriceUpdate contents to a comma-delimited string
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", Symbol, LastTradePrice, LastTradeSize, LastTradeTime, Bid, BidSize, Ask, AskSize, TotalVolume, OpenPrice, HighPrice, LowPrice, ClosePrice);
        }

        // Convert from comma-delimited string to PriceUpdate object
        public void FromString(string sUpdate)
        {
            var split = sUpdate.Split(',');
            Symbol = split[0];
            LastTradePrice = decimal.Parse(split[1]);
            LastTradeSize = int.Parse(split[2]);
            LastTradeTime = split[3];
            Bid = decimal.Parse(split[4]);
            BidSize = int.Parse(split[5]);
            Ask = decimal.Parse(split[6]);
            AskSize = int.Parse(split[7]);
            TotalVolume = int.Parse(split[8]);
            OpenPrice = decimal.Parse(split[9]);
            HighPrice = decimal.Parse(split[10]);
            LowPrice = decimal.Parse(split[11]);
            ClosePrice = decimal.Parse(split[12]);
        }

        public string ToTickerString()
        {
            return string.Format("{0}  {1}  {2}:{3}-{4}:{5}", Symbol, LastTradePrice, BidSize, Bid, Ask, AskSize);
        }

    } // end of class PriceUpdate



    public class PriceUpdateMap
    {
        private Dictionary<string, PriceUpdate> m_map;
        private Dictionary<string, string> m_ids;

        public PriceUpdateMap()
        {
            m_map = new Dictionary<string, PriceUpdate>();
            m_ids = new Dictionary<string, string>();
        }

        // Store a new price update (use Symbol as key)
        public void Update(PriceUpdate update)
        {
            if (m_map.ContainsKey(update.Symbol))
            {
                update.CopyTo(m_map[update.Symbol]);
            }
            else
            {
                m_map[update.Symbol] = new PriceUpdate(update);
            }
        }

        // Associate a Symbol with an ID
        // An ID is a text identifier you can use to reference a specific symbol (ex: "VIX"->"@VXH18")
        public void SetId(string id, string symbol)
        {
            m_ids[id] = symbol;
        }

        // Get the Symbol associated with the given ID
        public string GetSymbol(string id)
        {
            string value = null;
            return m_ids.TryGetValue(id, out value) ? value : null;
        }

        // Given one or more strings (symbols
        public bool HasUpdatesFor(params string[] keys)
        {
            bool result = true;
            for (int i = 0; i < keys.Length; ++i)
            {
                if (m_map.ContainsKey(keys[i]) || (m_ids.ContainsKey(keys[i]) && m_map.ContainsKey(m_ids[keys[i]])))
                    continue;
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        // Return the stored PriceUpdate for a given Symbol OR Id
        public PriceUpdate this[string s]
        {
            get
            {
                if (m_map.ContainsKey(s))
                    return m_map[s];
                else if (m_ids.ContainsKey(s) && m_map.ContainsKey(m_ids[s]))
                    return m_map[m_ids[s]];
                else
                    return null;
            }
        }
    } // end of class PriceUpdateMap


} // end of namespace

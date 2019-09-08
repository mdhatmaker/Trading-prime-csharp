using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis
{
    public class TickerUpdateMap
    {
        private Dictionary<string, TickerUpdate> m_map;
        private Dictionary<string, string> m_ids;

        public TickerUpdateMap()
        {
            m_map = new Dictionary<string, TickerUpdate>();
            m_ids = new Dictionary<string, string>();
        }

        // Store a new price update (use Symbol as key)
        public void Update(TickerUpdate update)
        {
            if (m_map.ContainsKey(update.Symbol))
            {
                update.CopyTo(m_map[update.Symbol]);
            }
            else
            {
                m_map[update.Symbol] = new TickerUpdate(update);
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

        // Return the stored TickerUpdate for a given Symbol OR Id
        public TickerUpdate this[string s]
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

    } // end of class TickerUpdateMap
} // end of namespace

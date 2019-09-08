using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tools
{
    /// <summary>
    /// This class will manage the ApiKeys/ApiSecrets and any other security-sensitive information
    /// </summary>
    public class ApiSecurity
    {
        public string Filename { get; set; }
        public ApiKeyMap ApiKeys { get; private set; }

        // Given 'pathname' as a JSON file containing the ApiKey/ApiSecret credentials
        public ApiSecurity(string pathname)
        {
            Filename = pathname;
            ApiKeys = new ApiKeyMap();
            ReadCredentials();
        }

        // Read the ApiKey/ApiSecret credentials from the 'pathname' file
        public void ReadCredentials()
        {
            string json = GFile.ReadTextFile(Filename);
            ApiKeys.Clear();
            if (json == null) return;
            var res = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
            foreach (var exch in res.Keys)
            {
                var li = res[exch];
                ApiKeys.Add(exch, li[0], li[1]);   
            }
        }
    } // end of class ApiSecurity

    /// <summary>
    /// This class allows API credentials to be stored for each exchange ("BINANCE", "GDAX", "POLONIEX", ...)
    /// </summary>
    public class ApiKeyMap
    {
        Dictionary<string, ApiCredentials> m_apiKeys;

        public ApiKeyMap()
        {
            m_apiKeys = new Dictionary<string, ApiCredentials>();
        }

        // Implement version of Keys and Values properties (similar to Dictionary)
        public List<string> Keys { get { return m_apiKeys.Keys.ToList(); }}
        public List<ApiCredentials> Values { get { return m_apiKeys.Values.ToList(); }}

        public void Clear()
        {
            m_apiKeys.Clear();
        }

        public void Add(string exchange, string apikey, string apisecret)
        {
            m_apiKeys.Add(exchange.ToUpper(), new ApiCredentials(apikey, apisecret));
        }

        // Return the credentials for the given exchange ("BINANCE", "GDAX", "POLONIEX", ...)
        public ApiCredentials this[string exchange]
        {
            get
            {
                ApiCredentials creds = null;
                return m_apiKeys.TryGetValue(exchange.ToUpper(), out creds) ? creds : null;
            }
        }
    } // end of class ApiKeyMap

    /// <summary>
    /// This class stores an ApiKey/ApiSecret combination
    /// </summary>
    public class ApiCredentials
    {
        public string ApiKey { get { return m_apiKey ?? ""; } private set { m_apiKey = value; } }
        public string ApiSecret { get { return m_apiSecret ?? ""; } private set { m_apiSecret = value; } }

        private string m_apiKey, m_apiSecret;

        public ApiCredentials(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
        }
    } // end of class ApiCredentials


} // end of namespace

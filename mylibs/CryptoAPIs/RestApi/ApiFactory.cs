using System;
using System.Collections.Generic;
using System.Text;
using CryptoTools;
using CryptoTools.Cryptography;
using CryptoApis.Models;

namespace CryptoApis.RestApi
{
    public class ApiFactory
    {
        private Credentials m_credentials;
        private Dictionary<string, ICryptoRestApi> m_apis = new Dictionary<string, ICryptoRestApi>();

        public ApiFactory(string credentialsFile, string credentialsFilePassword)
        {
            //m_credentials = Credentials.LoadEncryptedJson(credentialsFile, credentialsFilePassword);
            m_credentials = Credentials.LoadEncryptedCsv(credentialsFile, credentialsFilePassword);
        }

        // where T like KrakenRestApi and exchange like "KRAKEN" and symbolId like "btcusd"
        public ICryptoRestApi Get(string exchange)
        {
            if (exchange == "KRAKEN")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new KrakenRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else if (exchange == "POLONIEX")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new PoloniexRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else if (exchange == "BITTREX")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new BittrexRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else if (exchange == "BINANCE")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new BinanceRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else if (exchange == "BITFINEX")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new BitfinexRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else if (exchange == "GDAX")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new GdaxRestApi(cred.Key, cred.Secret, "mickey+mouse");
                }
                return m_apis[exchange];
            }
            else if (exchange == "HITBTC")
            {
                if (!m_apis.ContainsKey(exchange) || m_apis[exchange] == null)
                {
                    var cred = m_credentials[exchange];
                    m_apis[exchange] = new CryptoApis.Exchange.HitBtc.HitBtcRestApi(cred.Key, cred.Secret);
                }
                return m_apis[exchange];
            }
            else
            {
                return null;
            }
        }


    } // end of class ApiFactory
} // end of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.ExchangeX.CryptoCompareX;

namespace CryptoAPIs.ExchangeX
{
    public partial class CryptoCompare : BaseApi, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://min-api.cryptocompare.com"; } }
        public override string Name { get { return "CRYPTOCOMPARE"; } }

        public static readonly string WebSocketUrl = "https://streamer.cryptocompare.com/";

        CryptoCompareClient m_api = CryptoCompareClient.Instance;

        // SINGLETON
        public static CryptoCompare Instance { get { return m_instance; } }
        private static readonly CryptoCompare m_instance = new CryptoCompare();

        private CryptoCompare() { }

        private List<string> m_symbolList = null;

        public List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>();
                    foreach (dynamic d in GetCoinList())
                    {
                        m_symbolList.Add(d);
                    }
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

       

        public void GetCoinSnapshots()
        {
            var coins = m_api.Coins.ListAsync().Result;
            string fsym = "BTC", tsym = "USD";
            var coinSnapshot = m_api.Coins.SnapshotAsync(fromSymbol: fsym, toSymbol: tsym);
            int coinId = 1;
            var coinSnapshotFull = m_api.Coins.SnapshotFullAsync(id: coinId);
        }




        public dynamic GetCoinList()
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/coinlist/");
            dynamic response = wrapper.Response;    // "Success"
            dynamic message = wrapper.Message;      // "Coin list successfully returned!"
            return wrapper.Data;
        }

        // {"BTC":0.009878,"USD":10.79,"EUR":10.37}
        public dynamic GetPrice(string fsym="ETH", string tsyms="BTC,USD,EUR", string exchange="CCCAGG")
        {
            dynamic wrapper = GetJsonObject("https://min-api.cryptocompare.com/data/price?fsym={0}&tsyms={1}&e={2}", fsym, tsyms, exchange);
            return wrapper;
        }

        // {"BTC":{"USD":1090.39,"EUR":1047.31},"ETH":{"USD":10.77,"EUR":10.35}}
        public dynamic GetPriceMulti(string fsyms = "ETH,DASH", string tsyms = "BTC,USD,EUR", string exchange = "CCCAGG")
        {
            dynamic wrapper = GetJsonObject("https://min-api.cryptocompare.com/data/pricemulti?fsyms={0}&tsyms={1}&e={2}", fsyms, tsyms, exchange);
            return wrapper;
        }

        public dynamic GetPriceMultiFull(string fsyms = "ETH,DASH", string tsyms = "BTC,USD,EUR", string exchange = "CCCAGG")
        {
            dynamic wrapper = GetJsonObject("https://min-api.cryptocompare.com/data/pricemultifull?fsyms={0}&tsyms={1}&e={2}", fsyms, tsyms, exchange);
            dynamic raw = wrapper.RAW;
            dynamic display = wrapper.DISPLAY;
            return raw;
        }

        // avgType: HourVWAP, MidHighLow, VolFVolT
        public dynamic GetGeneratedAvg(string fsym = "BTC", string tsym = "USD", string exchange = "CCCAGG", string avgType = "HourVWAP", int UTCHourDiff = 0)
        {
            dynamic wrapper = GetJsonObject("https://min-api.cryptocompare.com/data/generateAvg?fsym={0}&tsym={1}&e={2}&avgType={3}&UTCHourDiff={4}", fsym, tsym, exchange, avgType, UTCHourDiff);
            dynamic raw = wrapper.RAW;
            dynamic display = wrapper.DISPLAY;
            return raw;
        }

        // https://min-api.cryptocompare.com/data/pricehistorical?fsym=ETH&tsyms=BTC,USD,EUR&ts=1452680400&extraParams=your_app_name

        public dynamic GetCoinSnapshot(string fsym = "BTC", string tsym = "USD")
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/coinsnapshot/?fsym={0}&tsym={1}", fsym, tsym);
            dynamic response = wrapper.Response;    // "Success"
            dynamic message = wrapper.Message;      // "Coin snapshot successfully returned"
            dynamic type = wrapper.Type;            // 100
            return wrapper.Data;
        }

        // id: 1182 (BTC), 3808 (LTC), 7605 (ETH) .... id of the coin you want data for
        public dynamic GetCoinSnapshotFullById(int id = 1182)
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/coinsnapshotfullbyid/?id={0}", id);
            dynamic response = wrapper.Response;    // "Success"
            dynamic message = wrapper.Message;      // "Coin full snapshot successfully returned"
            return wrapper.Data;
        }

        // id: 1182 (BTC), 3808 (LTC), 7605 (ETH) .... id of the coin/exchange you want social data for
        public dynamic GetSocialStatsById(int id = 1182)
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/coinsnapshot/?id={0}", id);
            dynamic response = wrapper.Response;    // "Success"
            dynamic message = wrapper.Message;      // "Social data successfully returned"
            dynamic type = wrapper.Type;            // 100
            return wrapper.Data;
        }

        // https://min-api.cryptocompare.com/data/histominute?fsym=BTC&tsym=USD&limit=60&aggregate=3&e=CCCAGG
        // https://min-api.cryptocompare.com/data/histohour?fsym=BTC&tsym=USD&limit=60&aggregate=3&e=CCCAGG,
        // https://min-api.cryptocompare.com/data/histoday?fsym=BTC&tsym=USD&limit=60&aggregate=3&e=CCCAGG,

        public dynamic GetMiningContracts()
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/miningcontracts");
            //dynamic response = wrapper.Response;    // "Success"
            //dynamic message = wrapper.Message;      // "Mining contracts successfully returned"
            //dynamic type = wrapper.Type;            // 100
            //return wrapper.Data;
            return wrapper;
        }

        public dynamic GetMiningEquipment()
        {
            dynamic wrapper = GetJsonObject("https://www.cryptocompare.com/api/data/miningequipment");
            dynamic response = wrapper.Response;    // "Success"
            dynamic message = wrapper.Message;      // "Mining equipment data successfully returned"
            dynamic type = wrapper.Type;            // 100
            return wrapper.Data;
        }

        public dynamic GetTopPairs(string fsym = "BTC", int limit = 20)
        {
            dynamic wrapper = GetJsonObject("https://min-api.cryptocompare.com/data/top/pairs?fsym={0}&limit={1}", fsym, limit);
            dynamic response = wrapper.Response;    // "Success"
            return wrapper.Data;
        }



        public dynamic GetJsonObject(string url, params object[] p)
        {
            string json = GetJSON(string.Format(url, p));
            //dout(json);
            return JObject.Parse(json);
        }

    } // end of class CryptoCompare

} // end of namespace

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPIs.ExchangeX
{
    public class EtherChain
    {
        //public override string BaseUrl { get { return "https://etherchain.org/api"; } }
        //public override string ExchangeName { get { return "ETHERCHAIN"; } }

        // SINGLETON
        public static EtherChain Instance { get { return m_instance; } }
        private static EtherChain m_instance = new EtherChain();
        private EtherChain() { }

        string _url = "https://etherchain.org/api";

        // Get historical price data for Ethereum
        public Dictionary<DateTime, decimal> GetHistoricalPrices()
        {
            var result = new Dictionary<DateTime, decimal>();

            string json = QueryPublic("statistics/price");
            var res = JsonConvert.DeserializeObject<ResponseWrapper<List<HistoricalData>>>(json);
            foreach (var d in res.data)
            {
                result.Add(d.time, d.usd);
            }
            return result;
        }

        // Returns basic stats of the ethereum network
        public BasicStats GetBasicStats()
        {
            string json = QueryPublic("basic_stats");
            var res = JsonConvert.DeserializeObject<ResponseWrapper<BasicStats>>(json);
            return res.data;
        }

        // Gets the last :count (max is 100) blocks sorted by their number. To page between sets use the :offset parameter.
        public List<BlockXL> GetLastBlocks(int offset=0, int count=10)
        {
            string json = QueryPublic(string.Format("blocks/{0}/{1}", offset, count));
            var res = JsonConvert.DeserializeObject<ResponseWrapper<List<BlockXL>>>(json);
            return res.data;
        }

        // Get a block by its number or hash
        public BlockXL GetBlock(string id)
        {
            string json = QueryPublic(string.Format("block/{0}", id));
            var res = JsonConvert.DeserializeObject<ResponseWrapper<List<BlockXL>>>(json);
            return res.data[0];
        }

        // TODO: Complete these remaining EtherChain API calls:
        /*
        /api/block/:id/tx  ==>  Returns the transactions of a given block
        /api/tx/:id  ==>  Returns a transaction by it's hash
        /api/account/:id  ==>  Returns an account by it's address
        /api/account/:id/tx/:offset  ==>  Returns the transactions of an account.Use the offset parameter for paging.
        */


        //--------------------------------------------------------------------------------------------------------------

        public string QueryPublic(string method)    //, Dictionary<string, string> param = null)
        {
            string address = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", _url, method);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(address));
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";

            try
            {
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    Stream str = webResponse.GetResponseStream();
                    using (StreamReader sr = new StreamReader(str))
                        return sr.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                using (HttpWebResponse response = (HttpWebResponse)wex.Response)
                {
                    if (response == null)
                        throw;
                    Stream str = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(str))
                    {
                        if (response.StatusCode != HttpStatusCode.InternalServerError)
                            throw;
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        //==============================================================================================================

        public class ResponseWrapper<T> where T : new()
        {
            public int status { get; set; }
            public T data { get; set; }

            public ResponseWrapper() { data = new T(); }
        }

        public class HistoricalData
        {
            public DateTime time { get; set; }
            public decimal usd { get; set; }
        }

        /*public class Difficulty
        {
            public long number { get; set; }
            public string coinbase { get; set; }
            public DateTime time { get; set; }
            public decimal difficulty { get; set; }
            public long gasUsed { get; set; }
            public int uncle_count { get; set; }
            public int blockTime { get; set; }
            public string name { get; set; }
        }*/

        public class Block
        {
            public long number { get; set; }
            public string coinbase { get; set; }
            public DateTime time { get; set; }
            public decimal difficulty { get; set; }
            public long gasUsed { get; set; }
            public int uncle_count { get; set; }
            public int blockTime { get; set; }
            public long gasLimit { get; set; }
            public string name { get; set; }
        }

        public class BlockXL : Block
        {
            public string hash { get; set; }
            public string parentHash { get; set; }
            public string uncleHash { get; set; }
            public string root { get; set; }
            public string txHash { get; set; }
            public string extra { get; set; }
            public object mixDigest { get; set; }
            public string nonce { get; set; }
            public int tx_count { get; set; }
            public long size { get; set; }
            public decimal reward { get; set; }
            public decimal totalFee { get; set; }
        }

        public class TxCount
        {
            public decimal count { get; set; }
        }

        public class Tx
        {
            public string hash { get; set; }
            public string parentHash { get; set; }
            public long block_id { get; set; }
            public string sender { get; set; }
            public string recipient { get; set; }
            public decimal amount { get; set; }
            public DateTime time { get; set; }
            public string senderName { get; set; }
            public string recipientName { get; set; }
        }

        public class Stats
        {
            public decimal blockTime { get; set; }
            public decimal difficulty { get; set; }
            public decimal hashRate { get; set; }
            public decimal uncle_rate { get; set; }
        }

        public class BasicStats
        {
            public Block difficulty { get; set; }
            public TxCount txCount { get; set; }
            public Block blockCount { get; set; }
            public List<Block> blocks { get; set; }
            public List<Tx> txs { get; set; }
            public Dictionary<string, decimal> price { get; set; }
            public Stats stats { get; set; }

            public BasicStats()
            {
                blocks = new List<Block>();
                txs = new List<Tx>();
                price = new Dictionary<string, decimal>();
            }
        }

    } // end of class


} // end of namespace

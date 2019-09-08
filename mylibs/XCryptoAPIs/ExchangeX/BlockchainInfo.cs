using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Tools;
using static Tools.G;

namespace CryptoAPIs.ExchangeX
{
    public static class API
    {
        // https://blockchain.info/api                                  // Bitcoin Developer APIs
        // https://blockchain.info/api/charts_api                       // Blockchain Charts & Statistics API

        public static void Test()
        {
            var dict = BlockchainInfoTicker.GetDictionary();
            cout(dict);
            var obj = BlockchainInfoStats.GetObject();
            cout(obj.ToString());

            BlockchainInfoPools pools5 = BlockchainInfoPools.GetObject("5days");
            BlockchainInfoPools pools10 = BlockchainInfoPools.GetObject("10days");

        }


        //--------- BlockchainInfo STATIC METHODS ----------------------------------------------------------------------  

            // TODO: BlockchainInfo WebSocket
            /*            //var socket2 = StartWebSocket("wss://ws.blockchain.info/inv");

            // Debugging operations
            //SendWebSocketMessage(socket2, "{'op':'ping'}");           // echo
            //SendWebSocketMessage(socket2, "{'op':'ping_block'}");       // responds with the latest block
            //SendWebSocketMessage(socket2, "{'op':'ping_tx'}");          // responds with the latest transaction (if subscribed to any addresses it will respond with the latest transaction involving those addresses)

            // Subscribe to notifications for all new bitcoin transactions
            //SendWebSocketMessage(socket2, "{'op':'unconfirmed_sub'}");
            //SendWebSocketMessage(socket2, "{'op':'unconfirmed_unsub'}");

            // Receive new transactions for a specific bitcoin address
            //SendWebSocketMessage(socket2, "{'op':'addr_sub', 'addr':'$bitcoin_address'}");     
            //SendWebSocketMessage(socket2, "{'op':'addr_unsub', 'addr':'$bitcoin_address'}");

            // Receive notifications when a new block is found
            //SendWebSocketMessage(socket2, "{'op':'blocks_sub'}");
            //SendWebSocketMessage(socket2, "{'op':'blocks_unsub'}");*/

        public static BlockchainInfoStats GetStats() {
            return BlockchainInfoStats.GetObject();
        }

        public static BlockchainInfoPools GetPools(string timespan="5days") {
            return BlockchainInfoPools.GetObject(timespan);
        }

        public static BlockchainSingleBlock GetSingleBlock(string block_hash, bool return_hex_encoded_binary = false) {
            return BlockchainSingleBlock.GetObject(block_hash, return_hex_encoded_binary);
        }

        public static BlockchainSingleTransaction GetSingleTransaction(string tx_hash, bool return_hex_encoded_binary = false)
        {
            return BlockchainSingleTransaction.GetObject(tx_hash, return_hex_encoded_binary);
        }

        public static BlockchainChartData GetChartData(string chart_type)
        {
            return BlockchainChartData.GetObject(chart_type);
        }

        public static BlockchainBlockHeight GetBlockHeight(string block_height)
        {
            return BlockchainBlockHeight.GetObject(block_height);
        }

        public static BlockchainSingleAddress GetSingleAddress(string bitcoin_address, int n_limit = 50, int n_offset = 0)
        {
            return BlockchainSingleAddress.GetObject(bitcoin_address, n_limit, n_offset);
        }

        public static BlockchainMultiAddress GetMultiAddress(string address, int n_limit = 50, int n_offset = 0)
        {
            return BlockchainMultiAddress.GetObject(address, n_limit, n_offset);
        }

        public static BlockchainUnspentOutputs GetUnspentOutputs(string address, int n_limit = 50, int n_confirmations_limit = 0)
        {
            return BlockchainUnspentOutputs.GetObject(address, n_limit, n_confirmations_limit);
        }

        public static BlockchainBalance GetBalance(string address)
        {
            return BlockchainBalance.GetObject(address);
        }

        public static BlockchainLatestBlock GetLatestBlock()
        {
            return BlockchainLatestBlock.GetObject();
        }

        public static BlockchainUnconfirmedTransactions GetUnconfirmedTransactions()
        {
            return BlockchainUnconfirmedTransactions.GetObject();
        }

        public static BlockchainBlocks GetBlocks(string pool_name)
        {
            return BlockchainBlocks.GetObject(pool_name);
        }

        public static BlockchainBlocks GetBlocks(long time_in_milliseconds)
        {
            return BlockchainBlocks.GetObject(time_in_milliseconds);
        }
        //------------------------------------------------------------------------------------------------------------------

    }

    public class WebSocketsClient
    {
        private static object consoleLock = new object();
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 256;
        private const bool verbose = true;
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(30000);

        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            Connect("ws://ws.blockchain.info:8335/inv").Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static async Task Connect(string uri)
        {
            ClientWebSocket webSocket = null;

            try
            {
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
                Console.WriteLine();

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket closed.");
                    Console.ResetColor();
                }
            }
        }
        static UTF8Encoding encoder = new UTF8Encoding();

        private static async Task Send(ClientWebSocket webSocket)
        {

            //byte[] buffer = encoder.GetBytes("{\"op\":\"blocks_sub\"}"); //"{\"op\":\"unconfirmed_sub\"}");
            byte[] buffer = encoder.GetBytes("{\"op\":\"unconfirmed_sub\"}");
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            while (webSocket.State == WebSocketState.Open)
            {
                LogStatus(false, buffer, buffer.Length);
                await Task.Delay(delay);
            }
        }

        private static async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[receiveChunkSize];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    LogStatus(true, buffer, result.Count);
                }
            }
        }

        private static void LogStatus(bool receiving, byte[] buffer, int length)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = receiving ? ConsoleColor.Green : ConsoleColor.Gray;
                //Console.WriteLine("{0} ", receiving ? "Received" : "Sent");

                if (verbose)
                    Console.WriteLine(encoder.GetString(buffer));

                Console.ResetColor();
            }
        }
    } // end of class WebSocketsClient

    //==================================================================================================================

    public class BlockchainInfoTicker : IDataRow
    {
        static public string[] Columns = { "Symbol", "Last", "15m", "Bid", "Ask", "Currency" };

        public string Key { get { return m_symbol; } set {; } }

        [JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public float _15m { get; set; }
        public float last { get; set; }
        public float sell { get; set; }
        public float buy { get; set; }
        [JsonProperty(PropertyName = "symbol")]
        public string currency_symbol { get; set; }

        private string[] cellValues = new string[6];
        private string m_symbol;

        public override string ToString()
        {
            return string.Format("15m: {0}   last: {1}   buy: {2}   sell: {3}   currency: {4}", _15m, last, buy, sell, currency_symbol);
        }

        public string[] GetCells()
        {
            cellValues[0] = Key;
            cellValues[1] = last.ToString();
            cellValues[2] = _15m.ToString();
            cellValues[3] = sell.ToString();
            cellValues[4] = buy.ToString();
            cellValues[5] = currency_symbol.ToString();
            return cellValues;
        }

        public static Dictionary<string, BlockchainInfoTicker> GetDictionary()
        {
            string json = GetJSON("https://blockchain.info/ticker");
            //cout(json);
            //var dict = BlockchainInfoTicker.DeserializeDictionary(json);
            var dict = DeserializeJson<Dictionary<string, BlockchainInfoTicker>>(json);
            foreach (var k in dict.Keys)
            {
                dict[k].m_symbol = k;
            }
            return dict;
        }
    } // end of class BlockChainInfoTicker


    public class BlockchainInfoStats
    {
        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public float market_price_usd { get; set; }
        public float hash_rate { get; set; }
        public long total_fees_btc { get; set; }
        public long n_btc_mined { get; set; }
        public int n_tx { get; set; }
        public int n_blocks_mined { get; set; }
        public float minutes_between_blocks { get; set; }
        public long totalbc { get; set; }
        public int n_blocks_total { get; set; }
        public float estimated_transaction_volume_usd { get; set; }
        public long blocks_size { get; set; }
        public float miners_revenue_usd { get; set; }
        public int nextretarget { get; set; }
        public long difficulty { get; set; }
        public long estimated_btc_sent { get; set; }
        public int miners_revenue_btc { get; set; }
        public long total_btc_sent { get; set; }
        public float trade_volume_btc { get; set; }
        public float trade_volume_usd { get; set; }
        public long timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("BlockchainInfoStats: {0}", Str(this));
        }

        public static BlockchainInfoStats GetObject()
        {
            string json = GetJSON("https://api.blockchain.info/stats");
            return DeserializeJson<BlockchainInfoStats>(json);
        }
    } // end of class BlockChainInfoStats


    public class BlockchainInfoPools
    {
        public Dictionary<string, int> Pools { get; private set; }

        public BlockchainInfoPools(Dictionary<string, int> pools)
        {
            this.Pools = pools;
        }

        public override string ToString()
        {
            return string.Format("BlockChainInfoPools: {0}", Str(this));
        }

        // timespan: Duration over which the data is computed, maximum 10 days, default is 4 days.
        public static BlockchainInfoPools GetObject(string timespan="4days")
        {
            string json = GetJSON(string.Format("https://api.blockchain.info/pools?timespan={0}", timespan));
            return new BlockchainInfoPools(DeserializeJson<Dictionary<string, int>>(json));
        }
    } // end of class BlockChainInfoPools


    public class BlockchainSingleBlock
    {
        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public string hash { get; set; }
        public float ver { get; set; }                  // should this be int?
        public string prev_block { get; set; }
        public string mrkl_root { get; set; }
        public long time { get; set; }
        public long bits { get; set; }
        public long nonce { get; set; }
        public int n_tx { get; set; }
        public int size { get; set; }
        public int block_index { get; set; }
        public bool main_chain { get; set; }
        public int height { get; set; }
        public long received_time { get; set; }
        public string relayed_by { get; set; }
        public List<BlockchainSingleTransaction> tx { get; set; }

        public override string ToString()
        {
            return string.Format("BlockchainSingleBlock: {0}", Str(this));
        }

        public static BlockchainSingleBlock GetObject(string block_hash, bool return_hex_encoded_binary=false)
        {
            string return_heb = "";
            if (return_hex_encoded_binary == true) return_heb = "?format=hex";
            string json = GetJSON(string.Format("https://blockchain.info/rawblock/{0}{1}", block_hash, return_heb));
            return DeserializeJson<BlockchainSingleBlock>(json);
        }
    } // end of class BlockchainSingleBlock


    public class bciTxInput
    {
        public string hash { get; set; }
        public string value { get; set; }
        public string tx_index { get; set; }
        public string n { get; set; }
    }

    public class bciTxInputs
    {
        public bciTxInput prev_out { get; set; }
        public string script { get; set; }
    }

    public class bciTxOutput
    {
        public string value { get; set; }
        public string hash { get; set; }
        public string script { get; set; }
    }

    public class BlockchainSingleTransaction
    {
        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public string hash { get; set; }
        public float ver { get; set; }                  // should this be int?
        public int vin_sz { get; set; }
        public int vout_sz { get; set; }
        public string lock_time { get; set; }
        public int size { get; set; }
        public string relayed_by { get; set; }
        public int block_height { get; set; }
        public int tx_index { get; set; }
        public bciTxInputs inputs { get; set; }
        [JsonProperty(PropertyName = "out")]
        public List<bciTxOutput> _out {get; set;}

        public override string ToString()
        {
            return string.Format("BlockchainSingleTransaction: {0}", Str(this));
        }

        public static BlockchainSingleTransaction GetObject(string tx_hash, bool return_hex_encoded_binary = false)
        {
            string return_heb = "";
            if (return_hex_encoded_binary == true) return_heb = "?format=hex";
            string json = GetJSON(string.Format("https://blockchain.info/rawtx/{0}{1}", tx_hash, return_heb));
            return DeserializeJson<BlockchainSingleTransaction>(json);
        }
    } // end of class BlockchainSingleTransaction


    public class bciChartValue
    {
        public long x { get; set; }             // Unix timestamp
        public float y { get; set; }
    }

    public class BlockchainChartData
    {
        public List<bciChartValue> values { get; set; }

        public static BlockchainChartData GetObject(string chart_type)
        {
            string json = GetJSON(string.Format("https://blockchain.info/charts/{0}?format=json", chart_type));
            return DeserializeJson<BlockchainChartData>(json);
        }
    } // end of class BlockchainChartData


    public class BlockchainBlockHeight
    {
        public List<BlockchainSingleBlock> blocks { get; set; }

        public static BlockchainBlockHeight GetObject(string block_height)
        {
            string json = GetJSON(string.Format("https://blockchain.info/charts/{0}?format=json", block_height));
            return DeserializeJson<BlockchainBlockHeight>(json);
        }
    } // end of class BlockchainBlockHeight


    public class BlockchainSingleAddress
    {
        public string hash160 { get; set; }
        public string address { get; set; }
        public int n_tx { get; set; }
        public int n_unredeemed { get; set; }
        public long total_received { get; set; }
        public long total_sent { get; set; }
        public long final_balance { get; set; }
        public List<BlockchainSingleTransaction> txs { get; set; }

        public override string ToString()
        {
            return string.Format("BlockchainSingleAddress: {0}", Str(this));
        }

        // Address can be base58 or hash160
        // (optional) limit parameter to show n transactions (e.g. "&limit=50") (Default: 50, Max: 100)
        // (optional) offset parameter to skip the first n transactions (e.g. "&offset=100" --> Page 2 for limit 50)
        public static BlockchainSingleAddress GetObject(string bitcoin_address, int n_limit=50, int n_offset=0)
        {
            string json = GetJSON(string.Format("https://blockchain.info/rawaddr/{0}&limit={1}&offset={2}", bitcoin_address, n_limit, n_offset));
            return DeserializeJson<BlockchainSingleAddress>(json);
        }
    } // end of class BlockchainSingleAddress


    public class bciAddress
    {
        public string hash160 { get; set; }
        public string address { get; set; }
        public int n_tx { get; set; }
        public long total_received { get; set; }
        public long total_sent { get; set; }
        public long final_balance { get; set; }
    }

    public class BlockchainMultiAddress
    {
        public List<bciAddress> addresses { get; set; }
        public List<BlockchainSingleTransaction> txs { get; set; }      // latest 50 transactions

        // Address can be base58 or xpub
        // (optional) limit parameter to show n transactions (e.g. "&limit=50") (Default: 50, Max: 100)
        // (optional) offset parameter to skip the first n transactions (e.g. "&offset=100" --> Page 2 for limit 50)
        public static BlockchainMultiAddress GetObject(string address, int n_limit = 50, int n_offset = 0)
        {
            string json = GetJSON(string.Format("https://blockchain.info/rawaddr/{0}&limit={1}&offset={2}", address, n_limit, n_offset));
            return DeserializeJson<BlockchainMultiAddress>(json);
        }
    } // end of class BlockchainMultiAddress


    public class bciUnspentOutput
    {
        public string tx_age { get; set; }
        public string tx_hash { get; set; }
        public string tx_index { get; set; }
        public string tx_output_n { get; set; }
        public string script { get; set; }              // (Hex encoded)
        public string value { get; set; }

        // The tx hash is in reverse byte order.
        // What this means is that in order to get the html transaction hash from the JSON tx hash for the following transaction,
        // you need to decode the hex (using "https://paulschou.com/tools/xlate/" for example).
        // This will produce a binary output, which you need to reverse (the last 8bits/1byte move to the front,
        // second to last 8bits/1byte needs to be moved to second, etc.).
        // Then once the reversed bytes are decoded, you will get the html transaction hash. 
    }

    public class BlockchainUnspentOutputs
    {
        public List<bciUnspentOutput> unspent_outputs { get; set; }

        // Multiple addresses allowed, separated by "|"
        // Address can be base58 or xpub
        // (optional) limit parameter to show n transactions (e.g. "&limit=50") (Default: 250, Max: 1000)
        // (optional) confirmations parameter to limit the minimum confirmations (e.g. "&confirmations=6")
        public static BlockchainUnspentOutputs GetObject(string address, int n_limit = 50, int n_confirmations_limit = 0)
        {
            string json = GetJSON(string.Format("https://blockchain.info/unspent?active={0}&limit={1}&confirmations={2}", address, n_limit, n_confirmations_limit));
            return DeserializeJson<BlockchainUnspentOutputs>(json);
        }
    } // end of class BlockchainUnspentOutputs


    public class bciBalanceSummary
    {
        public long final_balance { get; set; }
        public int n_tx { get; set; }
        public long total_received { get; set; }
    }

    public class BlockchainBalance
    {
        public Dictionary<string, bciBalanceSummary> balance { get; private set; }         // balance summary by address

        public BlockchainBalance(Dictionary<string, bciBalanceSummary> balance)
        {
            this.balance = balance;
        }

        // Multiple addresses allowed, separated by "|"
        // Address can be base58 or xpub
        public static BlockchainBalance GetObject(string address)
        {
            string json = GetJSON(string.Format("https://blockchain.info/balance?active={0}", address));
            return new BlockchainBalance(DeserializeJson<Dictionary<string, bciBalanceSummary>>(json));
        }
    } // end of class BlockchainBalance


    public class BlockchainLatestBlock
    {
        public string hash { get; set; }
        public long time { get; set; }
        public int block_index { get; set; }
        public int height { get; set; }
        public List<int> txIndexes { get; set; }

        public static BlockchainLatestBlock GetObject()
        {
            string json = GetJSON("https://blockchain.info/latestblock");
            return DeserializeJson<BlockchainLatestBlock>(json);
        }
    } // end of class BlockchainLatestBlock

    public class BlockchainUnconfirmedTransactions
    {
        List<BlockchainSingleTransaction> txs { get; set; }

        public static BlockchainUnconfirmedTransactions GetObject()
        {
            string json = GetJSON("https://blockchain.info/unconfirmed-transactions?format=json");
            return DeserializeJson<BlockchainUnconfirmedTransactions>(json);
        }
    } // end of class BlockchainUnconfirmedTransactions


    public class block
    {
        public int height { get; set; }
        public string hash { get; set; }
        public long time { get; set; }
    }

    public class BlockchainBlocks
    {
        List<block> blocks { get; set; }

        // Blocks for one day
        public static BlockchainBlocks GetObject(long time_in_milliseconds)
        {
            string json = GetJSON(string.Format("https://blockchain.info/blocks/{0}?format=json", time_in_milliseconds));
            return DeserializeJson<BlockchainBlocks>(json);
        }

        // Blocks for a specific pool
        public static BlockchainBlocks GetObject(string pool_name)
        {
            string json = GetJSON(string.Format("https://blockchain.info/blocks/{0}?format=json", pool_name));
            return DeserializeJson<BlockchainBlocks>(json);
        }
    } // end of class BlockchainBlocks



    /**** Message On New Block *****************************************************
    {
        "op": "block",
        "x": {
            "txIndexes": [
                3187871,
                3187868
            ],
            "nTx": 0,
            "totalBTCSent": 0,
            "estimatedBTCSent": 0,
            "reward": 0,
            "size": 0,
            "blockIndex": 190460,
            "prevBlockIndex": 190457,
            "height": 170359,
            "hash": "00000000000006436073c07dfa188a8fa54fefadf571fd774863cda1b884b90f",
            "mrklRoot": "94e51495e0e8a0c3b78dac1220b2f35ceda8799b0a20cfa68601ed28126cfcc2",
            "version": 1,
            "time": 1331301261,
            "bits": 436942092,
            "nonce": 758889471
        }
    }
    ********************************************************************************/
        public class MessageNewBlockWrapper
    {
        public string op { get; set; }
        public MessageNewBlock x { get; set; }
    }

    public class MessageNewBlock
    {
        public List<long> txIndexes { get; set; }
        public int nTx { get; set; }
        public int totalBTCSent { get; set; }
        public int estimatedBTCSent { get; set; }
        public int reward { get; set; }
        public int size { get; set; }
        public long blockIndex { get; set; }
        public long prevBlockIndex { get; set; }
        public long height { get; set; }
        public string hash { get; set; }
        public string mrklRoot { get; set; }
        public int version { get; set; }
        public long time { get; set; }
        public long bits { get; set; }
        public long nonce { get; set; }
    }


    /**** Message On New Transaction ***********************************************
    {
        "op": "utx",
        "x": {
            "lock_time": 0,
            "ver": 1,
            "size": 192,
            "inputs": [
                {
                    "sequence": 4294967295,
                    "prev_out": {
                        "spent": true,
                        "tx_index": 99005468,
                        "type": 0,
                        "addr": "1BwGf3z7n2fHk6NoVJNkV32qwyAYsMhkWf",
                        "value": 65574000,
                        "n": 0,
                        "script": "76a91477f4c9ee75e449a74c21a4decfb50519cbc245b388ac"
                    },
                    "script": "483045022100e4ff962c292705f051c2c2fc519fa775a4d8955bce1a3e29884b2785277999ed02200b537ebd22a9f25fbbbcc9113c69c1389400703ef2017d80959ef0f1d685756c012102618e08e0c8fd4c5fe539184a30fe35a2f5fccf7ad62054cad29360d871f8187d"
                }
            ],
            "time": 1440086763,
            "tx_index": 99006637,
            "vin_sz": 1,
            "hash": "0857b9de1884eec314ecf67c040a2657b8e083e1f95e31d0b5ba3d328841fc7f",
            "vout_sz": 1,
            "relayed_by": "127.0.0.1",
            "out": [
                {
                    "spent": false,
                    "tx_index": 99006637,
                    "type": 0,
                    "addr": "1A828tTnkVFJfSvLCqF42ohZ51ksS3jJgX",
                    "value": 65564000,
                    "n": 0,
                    "script": "76a914640cfdf7b79d94d1c980133e3587bd6053f091f388ac"
                }
            ]
        }
    }
    ********************************************************************************/
    public class MessageNewTransactionWrapper
    {
        public string op { get; set; }
        public MessageNewTransactionWrapper x { get; set; }
    }

    public class MessageNewTransaction
    {
        public long lock_time { get; set; }
        public int ver { get; set; }
        public long size { get; set; }
        // etc, etc...
        // TODO: finish this class
    }

} // end of namespace

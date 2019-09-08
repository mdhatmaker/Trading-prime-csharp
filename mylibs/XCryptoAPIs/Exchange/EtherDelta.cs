using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange
{
    // https://github.com/etherdelta/etherdelta.github.io/blob/master/docs/API_OLD.md

    public class EtherDelta : BaseExchange
    {
        public override string BaseUrl { get { return "https://api.etherdelta.com"; } }     // or "https://cache.etherdelta.com"
        //public override string BaseUrl { get { return "https://cache.etherdelta.com"; } }
        public override string ExchangeName { get { return "ETHERDELTA"; } }
        public override CryptoExch Exch => CryptoExch.ETHERDELTA;

        // SINGLETON
        public static EtherDelta Instance { get { return m_instance; } }
        private static readonly EtherDelta m_instance = new EtherDelta();
        private EtherDelta() { }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();

        /*class EtherDeltaResponse<T>
        {
            public bool success { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }*/

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var tickers = GetTickerDetails();
                    m_symbolList = new List<string>();
                    foreach (var s in tickers.Keys)
                    {
                        m_symbolList.Add(s);
                    }
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        // where symbol like "ETH_Polymath" or "ETH_EOS"
        public override ZTicker GetTicker(string symbol)
        {
            var tickers = GetTickerDetails();
            return tickers[symbol];
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var tickers = GetTickerDetails();
            var dict = new Dictionary<string, ZTicker>();
            foreach (var s in tickers.Keys)
            {
                dict[s] = tickers[s] as ZTicker;
            }
            return dict;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var ticker = GetEtherDeltaTicker(symbol);
            int page = 0;
            // where tokenAddr like "0x8f3470a7388c05ee4e7af3d01d8c722b0ff52374"
            var request = GET<EtherDeltaOrderBook>(string.Format("{0}/orders/{1}/{2}", BaseUrl, ticker.tokenAddr, page));
            return request as ZCryptoOrderBook;
        }

        // where symbol like "ETH_Polymath" or "ETH_EOS"
        public EtherDeltaTicker GetEtherDeltaTicker(string symbol)
        {
            var tickers = GetTickerDetails();
            return tickers[symbol];
        }

        public EtherDeltaOrderBook GetOrders(string symbol, int page=0)
        {
            /*string json = GFile.ReadAll(Folders.misc_path("etherdelta_orders_ETH_EOS.json.txt"));
            return Deserialize<EtherDeltaOrderBook>(json);*/
            var ticker = GetEtherDeltaTicker(symbol);
            var request = GET<EtherDeltaOrderBook>(string.Format("{0}/orders/{1}/{2}", BaseUrl, ticker.tokenAddr, page));
            return request;
        }

        public List<EtherDeltaTrade> GetTrades(string symbol, int page=0)
        {
            var ticker = GetEtherDeltaTicker(symbol);
            var request = GET<List<EtherDeltaTrade>>(string.Format("{0}/trades/{1}/{2}", BaseUrl, ticker.tokenAddr, page));
            return request;
        }

        public EtherDeltaOrderBook GetUserOrders(string symbol, string user, int page=0)    // where user like "0xD0e0fECe8A16F36bC23E07f92f98B191624F331a"
        {
            var ticker = GetEtherDeltaTicker(symbol);
            var request = GET<EtherDeltaOrderBook>(string.Format("{0}/myOrders/{1}/{2}/{3}", BaseUrl, user, ticker.tokenAddr, page));
            return request;
        }

        public List<EtherDeltaTrade> GetUserTrades(string symbol, string user, int page=0)  // where user like "0xD0e0fECe8A16F36bC23E07f92f98B191624F331a"
        {
            var ticker = GetEtherDeltaTicker(symbol); 
            var request = GET<List<EtherDeltaTrade>>(string.Format("{0}/myTrades/{1}/{2}/{3}", BaseUrl, user, ticker.tokenAddr, page));
            return request;
        }

        public List<EtherDeltaFundEntry> GetUserFunds(string symbol, string user, int page=0)   // where user like "0xD0e0fECe8A16F36bC23E07f92f98B191624F331a"
        {
            var ticker = GetEtherDeltaTicker(symbol);
            var request = GET<List<EtherDeltaFundEntry>>(string.Format("{0}/funds/{1}/{2}/{3}", BaseUrl, user, ticker.tokenAddr, page));
            return request;
        }

        public Dictionary<string, EtherDeltaTicker> GetTickerDetails(bool forceUpdate=false)
        {
            //string json = @"{""ETH_Polymath"":{""tokenAddr"":""0x9992ec3cf6a55b00978cddf2b27bc6882d88d1ec"",""quoteVolume"":1593128.061,""baseVolume"":2062.115,""last"":0.001268256,""percentChange"":-0.0244,""bid"":0.001262,""ask"":0.00129},""ETH_ABS"":{""tokenAddr"":""0x8ce9411df545d6b51a9bc52a89e0f6d1b54a06dd"",""quoteVolume"":194476798498,""baseVolume"":1195.379,""last"":1e-8,""percentChange"":-0.4737,""bid"":7e-9,""ask"":1e-8}}";
            //var d = JsonConvert.DeserializeObject<Dictionary<string, EtherDeltaTicker>>(json);
            //cout(d["ETH_Polymath"].tokenAddr);
            Dictionary<string, EtherDeltaTicker> request;
            if (forceUpdate)
            {
                request = GET<Dictionary<string, EtherDeltaTicker>>(string.Format("{0}/returnTicker", BaseUrl));
            }
            else
            {
                string json = GFile.ReadTextFile(Folders.misc_path("etherdelta_tickers.json.txt"));
                request = DeserializeJSON<Dictionary<string, EtherDeltaTicker>>(json);
            }
            return request;
        }

        /*public List<EtherDeltaSymbolDetail> GetSymbolDetails()
        {
            var request = GET<EtherDeltaResponse<List<EtherDeltaSymbolDetail>>>("{0}/getmarkets", BaseUrl);
            return request.result;
        }

        public List<EtherDeltaCurrencyDetail> GetCurrencyDetails()
        {
            var request = GET<EtherDeltaResponse<List<EtherDeltaCurrencyDetail>>>("{0}/getcurrencies", BaseUrl);
            return request.result;
        }*/

        /*public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<OKExOrderBook>(string.Format("{0}/api/v1/depth.do?symbol={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
        }*/

        /*public List<List<string>> GetOrderBook(string symbol)
        {
            return new OrderBook(GetBitfinexOrderBook(symbol));
        }*/

        /*
         const unpacked = [
          contractAddr,
          tokenGet,
          amountGet,
          tokenGive,
          amountGive,
          expires,
          orderNonce,
        ];

        const condensed = pack(
          unpacked,
          [160, 160, 256, 160, 256, 256, 256]);
        const hash = `0x${sha256(new Buffer(condensed, 'hex'))}`;
        const sig = sign(hash, privateKey);

        const orderObject = {
          message: JSON.stringify({
            amountGet,
            amountGive,
            tokenGet,
            tokenGive,
            contractAddr,
            expires,
            nonce: orderNonce,
            user,
            v: sig.v,
            r: sig.r,
            s: sig.s,
          }),
        };

        request({
          url: 'https://api.etherdelta.com/message',
          method: 'POST',
          json: true,
          body: orderObject,
        }, (error, response, body) => {
          console.log(body);
        });
        */


    } // end of class EtherDelta

    //======================================================================================================================================

    /*public class EtherDeltaTickerX
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
    }*/

    public class EtherDeltaTicker : ZTicker
    {
        public string tokenAddr { get; set; }           // "0x8f3470a7388c05ee4e7af3d01d8c722b0ff52374"
        public decimal quoteVolume { get; set; }
        public decimal baseVolume { get; set; }
        public decimal last { get; set; }
        public decimal percentChange { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }

        public override decimal Bid { get { return bid; } }
        public override decimal Ask { get { return ask; } }
        public override decimal Last { get { return last; } }
        public override decimal High { get { return decimal.Parse("0M"); } }
        public override decimal Low { get { return decimal.Parse("0M"); } }
        public override decimal Volume { get { return quoteVolume; } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }
    } // end of class EtherDeltaTicker

    /*public class EtherDeltaCurrencyDetail
    {
        public string Currency { get; set; }            // "BTC"
        public string CurrencyLong { get; set; }        // "Bitcoin"
        public int MinConfirmation { get; set; }        // 2
        public decimal TxFee { get; set; }              // 0.00080000
        public bool IsActive { get; set; }              // true
        public string CoinType { get; set; }            // "BITCOIN"
        public bool MaintenanceMode { get; set; }       // false
    } // end of class EtherDeltaCurrencyDetail

    public class EtherDeltaSymbolDetail
    {
        public string MarketCurrency { get; set; }      // "SLR"
        public string BaseCurrency { get; set; }        // "ETH"
        public string MarketCurrencyLong { get; set; }  // "SolarCoin"
        public string BaseCurrencyLong { get; set; }    // "Ethereum"
        public decimal MinTradeSize { get; set; }       // "0.00001000"
        public string MarketName { get; set; }          // "SLR_ETH"
        public bool IsActive { get; set; }              // "true"
    } // end of class EtherDeltaSymbolDetail*/

    public class EtherDeltaFundEntry
    {
        public string txHash { get; set; }              // "0x23abe9a81116334bb62868826f0dc604cd0f9c05a604886c3a4735696f162882"
        public string date { get; set; }                // "2017-08-25T02:26:59.000Z"
        public string tokenAddr { get; set; }           // "0x0000000000000000000000000000000000000000"
        public string kind { get; set; }                // "Deposit"
        public string user { get; set; }                // "0x8492ee5ab447655e982f30be868dd8133ca8823e"
        public decimal amount { get; set; }             // "15.1"
        public decimal balance { get; set; }            // "15.171717267106331"
    }

    public class EtherDeltaTrade
    {
        public string txHash { get; set; }              // "0x9c1dd8d21bef7ea8bda90c9fecd89cdc5d2e6db473b4e406afea28b4ce03f337"
        public string date { get; set; }                // "2017-08-25T02:35:26.000Z"
        public decimal price { get; set; }              // "0.36199"
        public string side { get; set; }                // "buy"
        public decimal amount { get; set; }             // "3"
        public decimal amountBase { get; set; }         // "1.08597"
        public string buyer { get; set; }               // "0x8492ee5ab447655e982f30be868dd8133ca8823e"
        public string seller { get; set; }              // "0xd0e0fece8a16f36bc23e07f92f98b191624f331a"
    }

    // POST https://api.etherdelta.com/message
    // This allows you to post an order.
    // Body parameters: message, which should be JSON with the following parameters:
    public class EtherDeltaOrderMessage
    {
        public decimal amountGive { get; set; }         // the amount you want to give (in wei or the base unit of the token)
        public string tokenGive { get; set; }           // the token you want to give (use the zero address, 0x0000000000000000000000000000000000000000 for ETH)
        public decimal amountGet { get; set; }          // the amount you want to get (in wei or the base unit of the token)
        public string tokenGet { get; set; }            // the token you want to get (use the zero address, 0x0000000000000000000000000000000000000000 for ETH)
        public string contractAddr { get; set; }        // the EtherDelta smart contract address
        public long expires { get; set; }               // the block number when the order should expire
        public long nonce { get; set; }                 // a random number
        public string user { get; set; }                // the address of the user placing the order
        // v, r, s ==> the signature of sha256(contractAddr, tokenGet, amountGet, tokenGive, amountGive, expires, nonce) after being signed by the user
        public int v { get; set; }                      // ex: 28
        public string r { get; set; }                   // ex: "0xc8e7b6bcca7a84911b8e6455d0957e8e2c0b513362a6a376d624295c50cdbb90"
        public string s { get; set; }                   // ex: "0x0de9e45bf3e61cf00616ad97abf439a91cab7edf1f8ebac69e000d3c804a8c6f"

        // Restrictions:
        // All orders must involve a token and ETH.
        // Minimum size: 0.001 ETH.
        // Maximum orders per side (buy, sell) per address per token: 5.
        // Returns: an indication of success or failure (and reason why).
    }

    public class EtherDeltaOrderBookEntry
    {
        public string id { get; set; }                  // "2311a122e163dbaeb558d3d4a6f9d28b22ea6bbdba0b88d312e7c78a3837648f_buy"
        public string amount { get; set; }              // "8000000000000000000"
        public string price { get; set; }               // "0.291001"
        public string tokenGet { get; set; }            // "0x8f3470a7388c05ee4e7af3d01d8c722b0ff52374"
        public string amountGet { get; set; }           // "8000000000000000000"
        public string tokenGive { get; set; }           // "0x0000000000000000000000000000000000000000"
        public string amountGive { get; set; }          // "2328008000000000000"
        public string expires { get; set; }             // "4210573"
        public string nonce { get; set; }               // "2406242974"
        public int v { get; set; }                      // 28
        public string r { get; set; }                   // "0xc8e7b6bcca7a84911b8e6455d0957e8e2c0b513362a6a376d624295c50cdbb90"
        public string s { get; set; }                   // "0x0de9e45bf3e61cf00616ad97abf439a91cab7edf1f8ebac69e000d3c804a8c6f"
        public string user { get; set; }                // "0xD0e0fECe8A16F36bC23E07f92f98B191624F331a"
        public string updated { get; set; }             // "2017-08-25T02:37:08.779Z"
        public string availableVolume { get; set; }     // "8000000000000000000"
        public string ethAvailableVolume { get; set; }      // "8"
        public string availableVolumeBase { get; set; }     // "2328008000000000000"
        public string ethAvailableVolumeBase { get; set; }  // "2.328008"
        public string amountFilled { get; set; }            // null
    }

    public class EtherDeltaOrderBook : ZCryptoOrderBook
    {
        public List<EtherDeltaOrderBookEntry> buys { get; set; }
        public List<EtherDeltaOrderBookEntry> sells { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in buys)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b.price, b.amount));
            }
            sb.Append("ASKS\n");
            ix = 0;
            foreach (var a in sells)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a.price, a.amount));
            }
            return sb.ToString();
        }
    } // end of class EtherDeltaOrderBook


} // end of namespace

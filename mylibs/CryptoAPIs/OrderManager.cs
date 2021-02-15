using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using ExchangeSharp;
using System.Threading.Tasks;
using CryptoTools;
using CryptoTools.Models;
using CryptoExchange.Net.Authentication;
using Binance.Net;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.MarketStream;
using System.Threading;
using CryptoApis.WebsocketApi;
using Binance.Net.Objects.Spot;

namespace CryptoApis
{
    // Collection of orders with functionality for order management

    public class OrderManager
    {
        public XOrderList Orders => m_orders;
        public ConcurrentDictionary<string, BinanceStreamTick> BinanceTickers => m_binaSocket.BinanceTickers;

        public XOrder this[string orderId] => m_orders.GetOrder(orderId);

        private Credentials m_creds;
        private XOrderList m_orders;

        //private ConcurrentDictionary<string, BinanceStreamTick> m_binaTick;
        //private Binance.Net.BinanceSocketClient m_sbina;
        private BinanceClient m_bina;
        private BinanceWebsocket m_binaSocket;

        public OrderManager(Credentials creds)
        {
            m_creds = creds;
            m_orders = new XOrderList();

            // BINANCE Client (and Websocket Client)
            var cred = m_creds["BINANCE"];
            var opt = new BinanceClientOptions();
            opt.ApiCredentials = new ApiCredentials(cred.Key, cred.Secret);
            m_bina = new BinanceClient(opt);

            BinanceWebsocket.SetCredentials(cred.Key, cred.Secret);
            m_binaSocket = BinanceWebsocket.Instance;

            /*var sopt = new Binance.Net.BinanceSocketClientOptions();
            sopt.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(m_creds["BINANCE"].Key, m_creds["BINANCE"].Secret);
            m_sbina = new Binance.Net.BinanceSocketClient(sopt);
            m_binaTick = new ConcurrentDictionary<string, BinanceStreamTick>();
            GetBinanceTickers();
            GetBinanceAccountInfo();
            StartWebsockets();*/

            // BITTREX Client (and Websocket Client)
            // ...Bittrex initialization code goes here...

            // POLONIEX Client
            // ...Poloniex initialization code goes here...
        }

        public ExchangeOrderResult PlaceOrder(IExchangeAPI api, string symbol, CryptoTools.OrderSide side, decimal price, decimal amount, string strategyId = null)
        {
            var res = ApiHelper.PlaceOrder(api, symbol, side, price, amount);
            var xo = ApiHelper.CreateXOrder(api.Name, res, strategyId); //new XOrder(api, res, strategyId);
            m_orders.Add(xo);
            return res;
        }

        public async Task Cancel(ExchangeOrderResult eor)
        {
            await Cancel(eor.OrderId);
        }

        public async Task Cancel(string orderId)
        {
            var xo = m_orders.GetOrder(orderId);
            if (xo.Exchange == "BINANCE")
            {
                await m_bina.Spot.Order.CancelOrderAsync(xo.Symbol, long.Parse(xo.OrderId));
                //await xo.API.CancelOrderAsync(xo.OrderId, xo.Symbol);
            }
            else
            {
                Console.WriteLine("ERROR: Exchange '{0}' not found in OrderManager::Cancel", xo.Exchange);
            }            
        }

        public async Task CancelAll()
        {
            foreach (var xo in m_orders)
            {
                await Cancel(xo.OrderId);
                //await xo.API.CancelOrderAsync(xo.OrderId, xo.Symbol);
            }
        }

        /*public void Add(XOrder o)
        {
            m_orders.Add(o);
        }*/


 
      

    } // end of class OrderManager

} // end of namespace

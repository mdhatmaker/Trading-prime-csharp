using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
//using CryptoAPIs.ExchangeX.CryptoCompareX;

namespace CryptoAPIs.ExchangeX
{
    public partial class TradeBlock
    {
        protected ZWebSocket m_socket;

        public void WebSocketMessageHandler(MessageArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://clientapi.tradeblock.com/json/[ClientKey]", null);
        }

        public void SubscribeWebSocket(string[] args = null)
        {
            m_socket.SendMessage(@"""SubAdd"", { subs: [""0~Poloniex~BTC~USD""] }");
        }

        /*Subscribe to market data by emitting 'SubAdd' including a list of items you want to get updates on.
        var socket3 = StartWebSocket("wss://streamer.cryptocompare.com");
        SendWebSocketMessage(socket3, "'SubAdd', { subs: ['0~Poloniex~BTC~USD'] }");


        Subscription items have the format of '{SubscriptionId}~{ExchangeName}~{FromSymbol}~{ToSymbol}'

        Example:
        socket.emit('SubAdd', { subs: ['0~Poloniex~BTC~USD'] } );*/
        // SubscriptionId: 0 (TRADE), 2 (CURRENT), 5 (CURRENTAGG)

        /*Unsubscribe

        Unsubscribe by sending 'SubRemove' message with a list of items.*/

        /*Trade: Subscribe to trade level data.

        Example:
        ['0~Poloniex~BTC~USD']

        The response will have the following format:

        '{SubscriptionId}~{ExchangeName}~{CurrencySymbol}~{CurrencySymbol}~{Flag}~{TradeId}~{TimeStamp}~{Quantity}~{Price}~{Total}'*/
        // Flag: 1 (Buy), 2 (Sell), 4 (Unknown)

        /*Current: Get the latest quote updates for a currency pair on a specific exchange.

        Example:
        ['2~Poloniex~BTC~USD']

        The first response will have the following format:

        '{Type}~{ExchangeName}~{FromCurrency}~{ToCurrency}~{Flag}~{Price}~{LastUpdate}~{LastVolume}~{LastVolumeTo}~{LastTradeId}~{Volume24h}~{Volume24hTo}~{MaskInt}'

        After the first response, only updates will be sent. The MaskInt parameter maps the response to the properties. Use our utility functions to map the response, you can find the code here*/
        // Flag: 1 (PRICEUP), 2 (PRICEDOWN), 4 (PRICEUNCHANGED)

        /*CurrentAgg: Subscribe to aggregate quote updates. Aggregation is done over the last 24 hours.

        Example:
        ['5~CCCAGG~BTC~USD']

        The first response format will follow:

        '{SubscriptionId}~{ExchangeName}~{FromCurrency}~{ToCurrency}~{Flag}~{Price}~{LastUpdate}~{LastVolume}~{LastVolumeTo}~{LastTradeId}~{Volume24h}~{Volume24hTo}~{LastMarket}'

        After the first response, only updates will be sent. The MaskInt parameter maps the response to the properties. Use our utility functions to map the response, you can find the code here.*/

        // TODO: Flesh out the WebSocket code above

    } // end of class CryptoCompare

} // end of namespace

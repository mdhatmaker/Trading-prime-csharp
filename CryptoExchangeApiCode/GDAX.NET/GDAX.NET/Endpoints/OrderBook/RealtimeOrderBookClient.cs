namespace GDAX.NET.Endpoints.OrderBook {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Newtonsoft.Json.Linq;

    public class RealtimeOrderBookClient : ExchangeClientBase {
        private const String Product = "BTC-USD";
        private readonly Object _askLock = new Object();
        private readonly Object _bidLock = new Object();

        private readonly Object _spreadLock = new Object();

        public RealtimeOrderBookClient( CBAuthenticationContainer auth ) : base( auth ) {
            this._sells = new List<BidAskOrder>();
            this._buys = new List<BidAskOrder>();

            this.Sells = new List<BidAskOrder>();
            this.Buys = new List<BidAskOrder>();

            this.ResetStateWithFullOrderBook();
        }

        private List<BidAskOrder> _sells {
            get; set;
        }
        private List<BidAskOrder> _buys {
            get; set;
        }

        public List<BidAskOrder> Sells {
            get; set;
        }
        public List<BidAskOrder> Buys {
            get; set;
        }

        public Decimal Spread {
            get {
                lock ( this._spreadLock ) {
                    if ( !this.Buys.Any() || !this.Sells.Any() ) {
                        return 0;
                    }

                    var maxBuy = this.Buys.Select( x => x.Price ).Max();
                    var minSell = this.Sells.Select( x => x.Price ).Min();

                    return minSell - maxBuy;
                }
            }
        }

        public event EventHandler Updated;

        private async void ResetStateWithFullOrderBook() {
            var response = await this.GetProductOrderBook( Product, 3 );

            lock ( this._spreadLock ) {
                lock ( this._askLock ) {
                    lock ( this._bidLock ) {
                        this._buys = response.Buys.ToList();
                        this._sells = response.Sells.ToList();

                        this.Buys = this._buys.ToList();
                        this.Sells = this._sells.ToList();
                    }
                }
            }

            this.OnUpdated();

            Subscribe( Product, this.OnOrderBookEventReceived );
        }

        private void OnUpdated() {
            this.Updated?.Invoke( this, new EventArgs() );
        }

        public async Task<GetProductOrderBookResponse> GetProductOrderBook( String productId, Int32 level = 1 ) {
            var request = new GetProductOrderBookRequest( productId, level );
            var response = await this.GetResponse( request );
            var orderBookResponse = new GetProductOrderBookResponse( response );
            return orderBookResponse;
        }

        private void OnOrderBookEventReceived( RealtimeMessage message ) {
            if ( message is RealtimeReceived ) {
                var receivedMessage = message as RealtimeReceived;
                this.OnReceived( receivedMessage );
            }

            else if ( message is RealtimeOpen ) {
            }

            else if ( message is RealtimeDone ) {
                var doneMessage = message as RealtimeDone;
                this.OnDone( doneMessage );
            }

            else if ( message is RealtimeMatch ) {
            }

            else if ( message is RealtimeChange ) {
            }

            this.OnUpdated();
        }

        private void OnReceived( RealtimeReceived receivedMessage ) {
            var order = new BidAskOrder {
                Id = receivedMessage.OrderId,
                Price = receivedMessage.Price,
                Size = receivedMessage.Size
            };


            lock ( this._spreadLock ) {
                if ( receivedMessage.Side == "buy" ) {
                    lock ( this._bidLock ) {
                        this._buys.Add( order );
                        this.Buys = this._buys.ToList();
                    }
                }

                else if ( receivedMessage.Side == "sell" ) {
                    lock ( this._askLock ) {
                        this._sells.Add( order );
                        this.Sells = this._sells.ToList();
                    }
                }
            }
        }

        private void OnDone( RealtimeDone message ) {
            lock ( this._spreadLock ) {
                lock ( this._askLock ) {
                    lock ( this._bidLock ) {
                        this._buys.RemoveAll( b => b.Id == message.OrderId );
                        this._sells.RemoveAll( a => a.Id == message.OrderId );

                        this.Buys = this._buys.ToList();
                        this.Sells = this._sells.ToList();
                    }
                }
            }
        }

        private static async void Subscribe( String product, Action<RealtimeMessage> onMessageReceived ) {
            if ( String.IsNullOrWhiteSpace( product ) ) {
                throw new ArgumentNullException( nameof( product ) );
            }

            if ( onMessageReceived == null ) {
                throw new ArgumentNullException( nameof( onMessageReceived ), "Message received callback must not be null." );
            }

            var uri = new Uri( "wss://ws-feed.exchange.coinbase.com" );
            var webSocketClient = new ClientWebSocket();
            var cancellationToken = new CancellationToken();
            var requestString = String.Format( @"{{""type"": ""subscribe"",""product_id"": ""{0}""}}", product );
            var requestBytes = Encoding.UTF8.GetBytes( requestString );
            await webSocketClient.ConnectAsync( uri, cancellationToken );

            if ( webSocketClient.State == WebSocketState.Open ) {
                var subscribeRequest = new ArraySegment<Byte>( requestBytes );
                var sendCancellationToken = new CancellationToken();
                await webSocketClient.SendAsync( subscribeRequest, WebSocketMessageType.Text, true, sendCancellationToken );

                while ( webSocketClient.State == WebSocketState.Open ) {
                    var receiveCancellationToken = new CancellationToken();
                    var receiveBuffer = new ArraySegment<Byte>( new Byte[ 1024 * 1024 * 5 ] ); // 5MB buffer
                    var webSocketReceiveResult = await webSocketClient.ReceiveAsync( receiveBuffer, receiveCancellationToken );
                    if ( webSocketReceiveResult.Count == 0 ) {
                        continue;
                    }

                    var jsonResponse = Encoding.UTF8.GetString( receiveBuffer.Array, 0, webSocketReceiveResult.Count );
                    var jToken = JToken.Parse( jsonResponse );

                    var typeToken = jToken[ "type" ];
                    if ( typeToken == null ) {
                        continue;
                    }

                    var type = typeToken.Value<String>();
                    RealtimeMessage realtimeMessage = null;

                    switch ( type ) {
                        case "received":
                            realtimeMessage = new RealtimeReceived( jToken );
                            break;
                        case "open":
                            realtimeMessage = new RealtimeOpen( jToken );
                            break;
                        case "done":
                            realtimeMessage = new RealtimeDone( jToken );
                            break;
                        case "match":
                            realtimeMessage = new RealtimeMatch( jToken );
                            break;
                        case "change":
                            realtimeMessage = new RealtimeChange( jToken );
                            break;
                        default:
                            break;
                    }

                    if ( realtimeMessage == null ) {
                        continue;
                    }

                    onMessageReceived( realtimeMessage );
                }
            }
        }
    }
}

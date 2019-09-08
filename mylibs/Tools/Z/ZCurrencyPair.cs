using System;
using System.Linq;
using System.Collections.Generic;
using static Tools.G;

namespace Tools
{
    public class ZCurrencyPairMap
    {
        public int Count { get { return m_pairs.Count; } }

        private SortedDictionary<string, ZCurrencyPair> m_pairs = new SortedDictionary<string, ZCurrencyPair>();

        public List<string> Symbols { get { return m_pairs.Keys.ToList(); } }
        public List<ZCurrencyPair> Pairs { get { return m_pairs.Values.ToList(); } }

        public void Add(ZCurrencyPair pair)
        {
            if (pair == null) return;

            if (!m_pairs.ContainsKey(pair.Symbol))
                m_pairs.Add(pair.Symbol, pair);
        }

        public void Add(ZCurrencyPairMap zcpmap)
        {
            foreach (var zcp in zcpmap.Pairs)
            {
                Add(zcp);
            }
        }

        public void Clear()
        {
            m_pairs.Clear();
        }

        public ZCurrencyPair this[string symbol]
        {
            get
            {
                return m_pairs[symbol];
            }
        }

        public bool ContainsKey(string symbol)
        {
            return m_pairs.ContainsKey(symbol);
        }

        public bool ContainsValue(ZCurrencyPair currencyPair)
        {
            return m_pairs.ContainsValue(currencyPair);
        }

    } // end of class ZCurrencyPairMap



    public class ZCurrencyPair
    {
        //public string ExchangeSymbol { get; set; }
        //public CryptoExch Exchange { get; set; }
        public string Symbol { get; set; }                              // Universal OneChain symbol
        public string Left { get; private set; }                        // LEFT currency of pair (universal OneChain)
        public string Right { get; private set; }                       // RIGHT currency of pair (universal OneChain)
        public string ExchangeSpecificSymbol { get; private set; }      // Exchange-specific symbol
        public string ExchangeSpecificLeft { get; private set; }        // LEFT currency of pair (exchange-specific)
        public string ExchangeSpecificRight { get; private set; }       // RIGHT currency of pair (exchange-specific)

        public string BinanceSymbol { get { return Left + Right; }}
        public string BitfinexSymbol { get { return Left.ToLower() + Right.ToLower(); }}
        public string BitflyerSymbol { get { return Left + "_" + Right; }}
        public string BithumbSymbol { get { return Left + "/" + Right; }}
        public string BitsquareSymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}
        public string BitstampSymbol { get { return Left.ToLower() + Right.ToLower(); }}
        public string BittrexSymbol { get { return Right + "-" + Left; }}
        public string BleuTradeSymbol { get { return Left + "_" + Right; }}
        public string BlinkTradeSymbol { get { return Left + Right; }}
        public string BTCCSymbol { get { return Left + Right; }}
        public string CexSymbol { get { return Left + "/" + Right; }}
        public string ChangellySymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}
        public string GateIOSymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}
        public string GDAXSymbol { get { return Left + "-" + Right; }}
        public string GeminiSymbol { get { return Left.ToLower() + Right.ToLower(); }}
        public string HitBTCSymbol { get { return Left + Right; }}
        public string HuobiSymbol { get { return Left.ToLower() + Right.ToLower(); }}
        public string ItBitSymbol { get { return Left + Right; } }
        public string KrakenSymbol { get { return Left + Right; }}
        public string OkCoinSymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}
        public string PoloniexSymbol { get { return Right + "_" + Left; }}
        public string WexSymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}
        public string XCryptoSymbol { get { return Left.ToLower() + "_" + Right.ToLower(); }}


        private ZCurrencyPair()
        {
        }

        public ZCurrencyPair(string symbol, string exchangeSymbol, string exchangeLeft, string exchangeRight)
        {
            Symbol = symbol;
            int i = symbol.IndexOf("_");
            Left = symbol.Substring(0, i);
            Right = symbol.Substring(i + 1);
            ExchangeSpecificSymbol = exchangeSymbol;
            ExchangeSpecificLeft = exchangeLeft;
            ExchangeSpecificRight = exchangeRight;
        }

        /*public ZCurrencyPair(string symbol, CryptoExch exchange)
        {
            ExchangeSymbol = symbol;
            Exchange = exchange;
            var lr = GetLeftRight(symbol, exchange);
            Left = lr[0];
            Right = lr[1];
        }*/


        // Given a currency-pair symbol and an exchange
        // Return the ZCurrencyPair for that symbol/exchange combo
        public static ZCurrencyPair FromSymbol(string symbol, CryptoExch exchange)
        {
            ZCurrencyPair zcp;
            if (exchange == CryptoExch.KRAKEN)
                zcp = ParseKrakenSymbol(symbol);
            else if (exchange == CryptoExch.BITFLYER)
                zcp = ParseBitFlyerSymbol(symbol);
            else
            {
                var lr = GetLeftRight(symbol, exchange);
                if (lr == null)
                    return null;
                else
                {
                    zcp = new ZCurrencyPair();
                    zcp.Symbol = lr[0] + "_" + lr[1];
                    zcp.Left = lr[0];
                    zcp.Right = lr[1];
                    zcp.ExchangeSpecificSymbol = symbol;
                    zcp.ExchangeSpecificLeft = zcp.Left;
                    zcp.ExchangeSpecificRight = zcp.Right;
                }
            }

            //if (zcp != null) dout("ZCurrencyPair::FromSymbol=> {0}", zcp.ToString());

            return zcp;
        }

        // Given a symbol (ex: "BCCUSDT") and a CryptoExch exchange enum
        // Return a string[2] where string[0]=LeftCurrency and string[1]=RightCurrency
        private static string[] GetLeftRight(string symbol, CryptoExch exchange)
        {
            string[] result = new string[2];
            int i, l;
            switch (exchange)
            {
                case CryptoExch.BINANCE:
                case CryptoExch.BITFINEX:
                //case CryptoExch.BITMEX:
                case CryptoExch.BITSTAMP:
                case CryptoExch.HITBTC:
                case CryptoExch.GEMINI:
                case CryptoExch.ITBIT:
                    l = symbol.Length;
                    if (symbol.ToUpper().EndsWith("USDT"))
                    {
                        result[1] = "USDT";
                        result[0] = symbol.Substring(0, l - 4).ToUpper();
                    }
                    else
                    {
                        result[1] = symbol.Substring(l - 3).ToUpper();
                        result[0] = symbol.Substring(0, l - 3).ToUpper();
                    }
                    break;
                /*case CryptoExch.BITHUMB:
                    i = symbol.IndexOf("/");
                    result[0] = symbol.Substring(0, i).ToUpper();
                    result[1] = symbol.Substring(i + 1).ToUpper();
                    break;*/
                case CryptoExch.BITTREX:
                    i = symbol.IndexOf("-");
                    result[1] = symbol.Substring(0, i).ToUpper();
                    result[0] = symbol.Substring(i + 1).ToUpper();
                    break;
                /*case CryptoExch.BITSQUARE:
                    i = symbol.IndexOf("_");
                    result[0] = symbol.Substring(0, i).ToUpper();
                    result[1] = symbol.Substring(i + 1).ToUpper();
                    break;*/
                case CryptoExch.GDAX:
                    i = symbol.IndexOf("-");
                    result[0] = symbol.Substring(0, i).ToUpper();
                    result[1] = symbol.Substring(i + 1).ToUpper();
                    break;
                case CryptoExch.POLONIEX:
                    i = symbol.IndexOf("_");
                    result[1] = symbol.Substring(0, i).ToUpper();
                    result[0] = symbol.Substring(i + 1).ToUpper();
                    break;
                /*case CryptoExch.KRAKEN:
                    string universalSymbol = ParseKrakenSymbol(symbol, out var left, out var right);
                    break;*/
                default:
                    ErrorMessage("No valid translation for '{0}' on exchange {1}", symbol, exchange.ToString());
                    break;
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            var zcp = obj as ZCurrencyPair;
            if (zcp == null)
            {
                return false;
            }
            else
            {
                return (zcp.Left == Left && zcp.Right == Right);
            }
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() * 1000 + Right.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", Symbol, ExchangeSpecificSymbol, ExchangeSpecificLeft, ExchangeSpecificRight);
        }


        // Parse BITFLYER symbol (ignore symbols that don't match "XXX_XXX")
        // Return ZCurrencyPair
        public static ZCurrencyPair ParseBitFlyerSymbol(string symbol)
        {
            if (symbol.Length == 7 && symbol[3] == '_')
            {
                var zcp = new ZCurrencyPair();
                zcp = new ZCurrencyPair();
                zcp.ExchangeSpecificSymbol = symbol;
                zcp.Left = symbol.Substring(0, 3);
                zcp.Right = symbol.Substring(4);
                zcp.Symbol = zcp.Left + "_" + zcp.Right;
                return zcp;
            }
            else
                return null;
        }

        // Parse KRAKEN symbol
        // REturn ZCurrencyPair
        public static ZCurrencyPair ParseKrakenSymbol(string symbol)
        {
            if (symbol.EndsWith(".d")) return null;

            var zcp = new ZCurrencyPair();
            zcp.ExchangeSpecificSymbol = symbol;

            if (symbol.EndsWith("ZCAD") || symbol.EndsWith("ZEUR") || symbol.EndsWith("ZJPY") || symbol.EndsWith("ZUSD"))
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(0, symbol.Length - 4);
                zcp.ExchangeSpecificRight = symbol.Substring(symbol.Length - 4);
            }
            else if (symbol.EndsWith("CAD") || symbol.EndsWith("EUR") || symbol.EndsWith("JPY") || symbol.EndsWith("USD"))
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(0, symbol.Length - 3);
                zcp.ExchangeSpecificRight = symbol.Substring(symbol.Length - 3);
            }
            else if (symbol.EndsWith("XXBT"))
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(0, symbol.Length - 4);
                zcp.ExchangeSpecificRight = "BTC";
            }
            else if (symbol.EndsWith("XBT"))
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(0, symbol.Length - 3);
                zcp.ExchangeSpecificRight = "BTC";
            }
            else if (symbol.Length == 6)
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(0, 3);
                zcp.ExchangeSpecificRight = symbol.Substring(3);
            }
            else if (symbol.Length == 8 && symbol[0] == 'X' && symbol[4] == 'X')
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(1, 3);
                zcp.ExchangeSpecificRight = symbol.Substring(5, 3);
            }
            else if (symbol.Length == 8 && symbol[0] == 'X' && symbol[4] == 'Z')
            {
                zcp.ExchangeSpecificLeft = symbol.Substring(1, 3);
                zcp.ExchangeSpecificRight = symbol.Substring(5, 3);
            }
            else
            {
                ErrorMessage("No valid translation for '{0}' on exchange KRAKEN", symbol);
                return null;
            }

            zcp.Left = zcp.ExchangeSpecificLeft;
            zcp.Right = zcp.ExchangeSpecificRight;

            if (zcp.ExchangeSpecificLeft == "XXBT")
                zcp.Left = "BTC";
            else if (zcp.ExchangeSpecificLeft == "XXDG")
                zcp.Left = "DOGE";
            else if (zcp.ExchangeSpecificLeft == "XETC")
                zcp.Left = "ETC";
            else if (zcp.ExchangeSpecificLeft == "XICN")
                zcp.Left = "ICN";
            else if (zcp.ExchangeSpecificLeft == "XLTC")
                zcp.Left = "LTC";
            else if (zcp.ExchangeSpecificLeft == "XREP")
                zcp.Left = "REP";
            else if (zcp.ExchangeSpecificLeft == "XMLN")
                zcp.Left = "MLN";
            else if (zcp.ExchangeSpecificLeft == "XETH")
                zcp.Left = "ETH";
            else if (zcp.ExchangeSpecificLeft == "XXLM")
                zcp.Left = "XLM";
            else if (zcp.ExchangeSpecificLeft == "XXMR")
                zcp.Left = "XMR";
            else if (zcp.ExchangeSpecificLeft == "XXRP")
                zcp.Left = "XRP";
            else if (zcp.ExchangeSpecificLeft == "XZEC")
                zcp.Left = "ZEC";

            if (zcp.ExchangeSpecificRight == "ZCAD")
                zcp.Right = "CAD";
            else if (zcp.ExchangeSpecificRight == "ZEUR")
                zcp.Right = "EUR";
            else if (zcp.ExchangeSpecificRight == "ZJPY")
                zcp.Right = "JPY";
            else if (zcp.ExchangeSpecificRight == "ZUSD")
                zcp.Right = "USD";

            zcp.Symbol = zcp.Left + "_" + zcp.Right;

            return zcp;
        }


    } // end of class ZCurrencyPair

} // end of namespace

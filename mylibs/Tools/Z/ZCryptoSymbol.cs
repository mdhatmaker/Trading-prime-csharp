using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    //public enum CryptoExch
    //{
    //    BINANCE, BITFINEX, BITFLYER, BITHUMB, BITMEX, BITSQUARE, BITSTAMP, BITTREX, BLINKTRADE, BLEUTRADE, BTCC, /*BTER,*/
    //    CEX, CHANGELLY, /*COINIGY,*/ COINONE, GATEIO, GDAX, GEMINI, HITBTC, HUOBI, ITBIT, KORBIT, KRAKEN, KUCOIN, OKCOIN, OKEX,
    //    POLONIEX, VAULTORO, WEX, XCRYPTO
    //};

    public enum CryptoExch
    {
        BINANCE=1, BITFINEX, BITFLYER, BITSTAMP, BITTREX, CHANGELLY, GDAX, GEMINI, HITBTC, ITBIT, KRAKEN, POLONIEX, VAULTORO
    };

    public class ZCryptoSymbol
    {
        public static readonly string SYMBOLS_FILENAME = "crypto_symbols.DF.csv";
        public static readonly string SYMBOLS_FX_FILENAME = "crypto_symbols.fx.DF.csv";

        public static DataFrame dfReadCryptoSymbols()
        {
            DataFrame df = DataFrame.ReadDataFrame(Folders.system_path(SYMBOLS_FILENAME));
            return df;
        }

        public static DataFrame dfReadCryptoSymbolsFx()
        {
            DataFrame df = DataFrame.ReadDataFrame(Folders.system_path(SYMBOLS_FX_FILENAME), createIndex:true);
            return df;
        }

        /*public string Symbol { get; private set; }
        public CryptoExchange Exchange { get; private set; }
        public string Left { get; private set; }                                                // the "left" asset of the asset pair (in our standardized ZSymbol format)
        public string Right { get; private set; }                                               // the "right" asset of the asset pair (in our standardized ZSymbol format)
        public bool IsLeftCurrency { get { return Crypto.CurrencySymbols.ContainsKey(Left); } }        // is the "left" asset a fiat currency?
        public bool IsRightCurrency { get { return Crypto.CurrencySymbols.ContainsKey(Right); } }      // is the "right" asset a fiat currency?
        
        public ZCryptoSymbol(string symbol, CryptoExchange exchange)
        {
            this.Symbol = symbol;
            this.Exchange = exchange;
            string pair = GetAssetZSymbolPair(symbol, exchange);
            if (!string.IsNullOrWhiteSpace(pair))
            {
                var split = pair.Split('_');
                this.Left = split[0];
                this.Right = split[1];
            }
            else
            {
                this.Left = "";
                this.Right = "";                
            }
        }

        public string GetAssetZSymbolPair(string symbol, CryptoExchange exchange)
        {
            string result = null;

            if (exchange == CryptoExchange.Kraken)
            {
                result = Kraken.Instance.GetZSymbolPair(symbol);
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("Kraken:: '{0}'  '{1}' '{2}'  {3} {4}", Symbol, Left, Right, IsLeftCurrency, IsRightCurrency);
        }*/

    } // end of class ZCryptoSymbol

} // end of namespace

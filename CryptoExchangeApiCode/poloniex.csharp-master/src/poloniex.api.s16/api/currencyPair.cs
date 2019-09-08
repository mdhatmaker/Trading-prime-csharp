using System;
using System.Collections.Generic;
using System.Linq;

namespace XCT.BaseLib.API.Poloniex
{
    public class CurrencyPair
    {
        private const char SeparatorCharacter = '_';

        public string CurrencyName { get; private set; }
        public string CoinName { get; private set; }

        public CurrencyPair(string currency_name, string coin_name)
        {
            CurrencyName = currency_name;
            CoinName = coin_name;
        }

        public static CurrencyPair Parse(string currency_pair)
        {
            var _split = currency_pair.Split(SeparatorCharacter);
            return new CurrencyPair(_split[0], _split[1]);
        }

        public override string ToString()
        {
            return CurrencyName + SeparatorCharacter + CoinName;
        }

        public static bool operator ==(CurrencyPair a, CurrencyPair b)
        {
            if (ReferenceEquals(a, b)) return true;
            if ((object)a == null ^ (object)b == null) return false;

            return a.CurrencyName == b.CurrencyName && a.CoinName == b.CoinName;
        }

        public static bool operator !=(CurrencyPair a, CurrencyPair b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            var b = obj as CurrencyPair;
            return (object)b != null && Equals(b);
        }

        public bool Equals(CurrencyPair b)
        {
            return b.CurrencyName == CurrencyName && b.CoinName == CoinName;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
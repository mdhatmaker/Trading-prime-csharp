using System;

namespace Info.Blockchain.API.Models
{
    public class BitcoinValue : IEquatable<BitcoinValue>
    {
        private const int SATOSHIS_PER_BITCOIN = 100000000;
        private const int BITS_PER_BITCOIN = 1000000;
        private const int MILLIBITS_PER_BITCOIN = 1000;
        private readonly decimal _btc;

        public BitcoinValue(decimal btc)
        {
            _btc = btc;
        }

        public decimal MilliBits => _btc * MILLIBITS_PER_BITCOIN;

        public decimal Bits => _btc * BITS_PER_BITCOIN;

        public long Satoshis => (long)(_btc * SATOSHIS_PER_BITCOIN);

        public decimal GetBtc() => _btc;

        public static BitcoinValue Zero => new BitcoinValue(0);

        public static BitcoinValue FromSatoshis(long satoshis) => new BitcoinValue((decimal)satoshis / SATOSHIS_PER_BITCOIN);

        public static BitcoinValue FromBits(decimal bits) => new BitcoinValue(bits / BITS_PER_BITCOIN);

        public static BitcoinValue FromMilliBits(decimal mBtc) => new BitcoinValue(mBtc / MILLIBITS_PER_BITCOIN);

        public static BitcoinValue FromBtc(decimal btc) => new BitcoinValue(btc);

        public static BitcoinValue operator +(BitcoinValue x, BitcoinValue y)
        {
            decimal btc = x._btc + y._btc;
            return new BitcoinValue(btc);
        }

        public static BitcoinValue operator -(BitcoinValue x, BitcoinValue y)
        {
            decimal btc = x._btc - y._btc;
            return new BitcoinValue(btc);
        }

        public bool Equals(BitcoinValue other)
        {
            return _btc == other._btc;
        }

        public override bool Equals(object obj)
        {
            if (obj is BitcoinValue)
            {
                return this.Equals((BitcoinValue) obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _btc.GetHashCode();
        }

        public override string ToString() => _btc.ToString();

    }
}

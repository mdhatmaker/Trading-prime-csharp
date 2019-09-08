using Info.Blockchain.API.Models;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
    public class BitcoinValueTests
	{
		[Fact]
		public void BitcoinValue_ConvertSatoshis_ValidConversion()
		{
			const long satoshis = 123456789;
			BitcoinValue bitcoinValue = BitcoinValue.FromSatoshis(satoshis);
			Assert.Equal(bitcoinValue.Satoshis, satoshis);
			Assert.Equal(bitcoinValue.Bits, satoshis / 100m);
			Assert.Equal(bitcoinValue.MilliBits, satoshis / 100000m);
			Assert.Equal(bitcoinValue.GetBtc(), satoshis / 100000000m);
		}

		[Fact]
		public void BitcoinValue_ConverBits_ValidConversion()
		{
			const decimal bits = 1234567.89m;
			BitcoinValue bitcoinValue = BitcoinValue.FromBits(bits);
			Assert.Equal(bitcoinValue.Satoshis, bits * 100m);
			Assert.Equal(bitcoinValue.Bits, bits);
			Assert.Equal(bitcoinValue.MilliBits, bits / 1000m);
			Assert.Equal(bitcoinValue.GetBtc(), bits / 1000000m);
		}

		[Fact]
		public void BitcoinValue_ConvertMilliBits_ValidConversion()
		{
			const decimal milliBits = 1234.56789m;
			BitcoinValue bitcoinValue = BitcoinValue.FromMilliBits(milliBits);
			Assert.Equal(bitcoinValue.Satoshis, milliBits * 100000m);
			Assert.Equal(bitcoinValue.Bits, milliBits * 1000m);
			Assert.Equal(bitcoinValue.MilliBits, milliBits);
			Assert.Equal(bitcoinValue.GetBtc(), milliBits / 1000m);
		}

		[Fact]
		public void BitcoinValue_ConvertBtc_ValidConversion()
		{
			const decimal btc = 1.23456789m;
			BitcoinValue bitcoinValue = BitcoinValue.FromBtc(btc);
			Assert.Equal(bitcoinValue.Satoshis, btc * 100000000m);
			Assert.Equal(bitcoinValue.Bits, btc * 1000000m);
			Assert.Equal(bitcoinValue.MilliBits, btc * 1000m);
			Assert.Equal(bitcoinValue.GetBtc(), btc);

			bitcoinValue = new BitcoinValue(btc);
			Assert.Equal(bitcoinValue.Satoshis, btc * 100000000m);
			Assert.Equal(bitcoinValue.Bits, btc * 1000000m);
			Assert.Equal(bitcoinValue.MilliBits, btc * 1000m);
			Assert.Equal(bitcoinValue.GetBtc(), btc);
		}

		[Fact]
		public void BitcoinValue_Add_Valid()
		{
			const decimal btc1 = 1.23456789m;
			const decimal btc2 = 9.87654321m;
			BitcoinValue value1 = BitcoinValue.FromBtc(btc1);
			BitcoinValue value2 = BitcoinValue.FromBtc(btc2);
			BitcoinValue value3 = value1 + value2;
			Assert.Equal(value3.GetBtc(), btc1 + btc2);

			value3 = value2 + value1;
			Assert.Equal(value3.GetBtc(), btc1 + btc2);
		}

		[Fact]
		public void BitcoinValue_Subtract_Valid()
		{
			const decimal btc1 = 1.23456789m;
			const decimal btc2 = 9.87654321m;
			BitcoinValue value1 = BitcoinValue.FromBtc(btc1);
			BitcoinValue value2 = BitcoinValue.FromBtc(btc2);
			BitcoinValue value3 = value1 - value2;
			Assert.Equal(value3.GetBtc(), btc1 - btc2);

			value3 = value2 - value1;
			Assert.Equal(value3.GetBtc(), btc2 - btc1);
			Assert.NotEqual(value3.GetBtc(), btc1 - btc2);

			value3 = value1 - value1;
			Assert.Equal(value3.GetBtc(), 0);
		}

		[Fact]
		public void BitcoinValue_Zero_Valid()
		{
			Assert.Equal(BitcoinValue.Zero.GetBtc(), 0);
		}

		[Fact]
		public void BitcoinValue_ToString_Valid()
		{
			BitcoinValue value = BitcoinValue.FromBtc(1.34567m);
			Assert.Equal(value.ToString(), "1.34567");
		}
	}
}

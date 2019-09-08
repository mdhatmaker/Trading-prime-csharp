using System.Linq;
using Metaco.ItBit;
using NUnit.Framework;

namespace MetacoItBit.Tests
{
	[TestFixture]
	public class MarketTest
	{
		[Test]
		public async void CanGetTickers()
		{
			var itBit = new MarketClient();
			var ticker = await itBit.GetTickerAsync(TickerSymbol.XBTUSD);
			Assert.IsNotNull(ticker);
			Assert.AreEqual(TickerSymbol.XBTUSD, ticker.Pair);
		}

		[Test]
		public async void CanGetOrderBook()
		{
			var itBit = new MarketClient();
			var orderBook = await itBit.GetOrderBookAsync(TickerSymbol.XBTUSD);
			Assert.IsNotNull(orderBook);
			Assert.IsTrue(orderBook.Asks.Any());
			Assert.IsTrue(orderBook.Bids.Any());
		}

		[Test]
		public async void CanGetRecentTrades()
		{
			var itBit = new MarketClient();
			var trades = await itBit.GetRecentTradesAsync(TickerSymbol.XBTUSD);
			Assert.IsNotNull(trades);
			Assert.IsTrue(trades.Trades.Any());
		}
	}
}

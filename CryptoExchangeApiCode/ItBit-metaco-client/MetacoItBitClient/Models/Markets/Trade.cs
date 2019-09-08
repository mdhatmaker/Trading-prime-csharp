namespace Metaco.ItBit
{
	public class Trade
	{
		public Trade(decimal price, decimal amount)
		{
			Price = price;
			Amount = amount;
		}

		public decimal Price { get; set; }
		public decimal Amount { get; set; }
	}
}
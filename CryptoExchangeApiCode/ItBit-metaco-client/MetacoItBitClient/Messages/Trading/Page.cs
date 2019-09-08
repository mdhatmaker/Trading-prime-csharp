using System;

namespace Metaco.ItBit
{
	public class Page
	{
		public int Number { get; private set; }
		public int Size { get; private set; }

		public static Page Default = new Page(1, 50);

		public static Page Create(int number, int size)
		{
			if (number < 0 )
				throw new ArgumentOutOfRangeException("number", "page number must be greater or equal to 1");
			if(size < 1 || size > 50)
				throw new ArgumentOutOfRangeException("size", "page size must be beetwen 1 and 50");

			return new Page(number, size);
		}

		private Page(int number, int size)
		{
			Number = number;
			Size = size;
		}

		public string ToQueryString()
		{
			return "page={0}&perPage={1}".Uri(Number, Size);
		}
	}
}
namespace Metaco.ItBit
{
	static class StringExtensions
	{
		public static string Uri(this string str, params object[] args)
		{
			return string.Format(str, args);
		}
	}
}

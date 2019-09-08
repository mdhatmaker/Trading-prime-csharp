using System;

namespace XCT.BaseLib.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static OrderType FromString(string s)
        {
            switch (s)
            {
                case "sell":
                    return OrderType.Sell;
 
                case "buy":
                    return OrderType.Buy;

                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(OrderType v)
        {
            return Enum.GetName(typeof(OrderType), v).ToLowerInvariant();
        }
    }
}
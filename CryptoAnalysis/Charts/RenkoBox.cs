using System;
using System.Drawing;

namespace CryptoAnalysis.Charts
{
	// For RenkoBox with BoxDirection.Up, OHLC represents:
    // O = box bottom
    // H = C = box top
    // L = low of "tail"

	// For RenkoBox with BoxDirection.Down, OHLC represents:
    // O = box top
    // L = C = box bottom
    // H = high of "tail"

    public class RenkoBox
    {
		public Color BoxColor => IsUp ? Color.Green : Color.Red;
		public decimal Top => IsUp ? BoxInfo.Close : BoxInfo.Open;
		public decimal Bottom => IsUp ? BoxInfo.Open : BoxInfo.Close;
		public bool IsUp => Direction == BarDirection.Up;
		public bool IsDown => Direction == BarDirection.Down;

		public DateTime Timestamp { get; private set; }
		public BarDirection Direction { get; private set; }
		public OHLC BoxInfo { get; private set; }

        public RenkoBox(DateTime timestamp, BarDirection barDirection, OHLC ohlc)
        {
			Timestamp = timestamp;
			Direction = barDirection;
			BoxInfo = ohlc;
        }
    } // end of class RenkoBox

} // end of namespace

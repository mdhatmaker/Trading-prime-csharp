using System;

namespace CryptoTools.MathStat
{
    public interface IIndicator
    {
		bool IsPrimed { get; }
		void ReceiveTick(decimal value);
		void UpdateLastTick(decimal value);
		decimal Value { get; }
    }
}

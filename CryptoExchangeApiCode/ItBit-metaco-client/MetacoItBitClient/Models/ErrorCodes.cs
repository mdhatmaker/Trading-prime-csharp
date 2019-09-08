namespace Metaco.ItBit
{
	public enum ErrorCodes
	{
		InvalidSignature= 10002,
		Unathorized = 10003,
		ValidationErrors = 10005,
		InvalidWalletName = 80001,
		WalletNameAlreadyInUse = 80003,
		WalletNotFound = 80004,
		OrderNotFound = 80005,
		NoMatchingCurrency = 80008,
		InvalidDatetimeRange = 80011,
		TickerSymbolNotFound = 80012,
		WithdrawalAmoutOutOfRange = 80013,
		OrderDisplayGreaterThanAmount = 80020,
		OrderDisplayLessThanAmount = 80021,
		InsufficientWalletFunds = 81001,
		OrderCannotBeCancelled = 81002,
		WalletDailyDepositLimitReached = 85001,
		WalletDailyWithdrawalLimitReached = 85002,
		WalletMonthlyWithdrawalLimitReached = 85003,
		OrderTooManyMetadataEntries = 86001,
		OrderTooLongMetadataValue = 86002
	}
}

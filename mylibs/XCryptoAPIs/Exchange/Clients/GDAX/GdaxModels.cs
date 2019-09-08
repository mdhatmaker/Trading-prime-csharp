using System;
using System.Collections.Generic;

namespace CryptoAPIs.Exchange.Clients.GDAX
{
    public class Account
    {
        public Guid Id { get; set; }
		public string Currency { get; set; }
		public decimal Balance { get; set; }
		public decimal Hold { get; set; }
        public decimal Available { get; set; }
		public bool Margin_enabled { get; set; }
		public decimal Funded_amount { get; set; }
		public decimal Default_amount { get; set; }
	}
        
    public class AccountHistory
    {
        public string Id { get; set; }

        public DateTime Created_at { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public string Type { get; set; }

        public Details Details { get; set; }
    }

    public class Details
    {
        public string Order_id { get; set; }

        public string Trade_id { get; set; }

        public string Product_id { get; set; }
    }

    public class AccountHold
    {
        public string Id { get; set; }

        public string Account_id { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime Updated_at { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; }

        public string Ref { get; set; }
    }

    public class CoinbaseAccount
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }

        public bool Primary { get; set; }

        public bool Active { get; set; }

        public WireDepositInformation Wire_Deposit_Information { get; set; }

        public SepaDepositInformation Sepa_Deposit_Information { get; set; }
    }

    public class WireDepositInformation
    {
        public string Account_Number { get; set; }

        public string Routing_Number { get; set; }

        public string Bank_Name { get; set; }

        public string Bank_Address { get; set; }

        public BankCountry Bank_Country { get; set; }

        public string Account_Name { get; set; }

        public string Account_Address { get; set; }

        public string Reference { get; set; }
    }

    public class SepaDepositInformation
    {
        public string Iban { get; set; }

        public string Swift { get; set; }

        public string Bank_Name { get; set; }

        public string Bank_Address { get; set; }

        public string Bank_Country_Name { get; set; }

        public string Account_Name { get; set; }

        public string Account_Address { get; set; }

        public string Reference { get; set; }
    }

    public class BankCountry
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class Currency
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Min_size { get; set; }
    }

    public class DepositResponse
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Payout_at { get; set; }
    }

    public class Deposit
    {
        public decimal amount { get; set; }

        public string currency { get; set; }

        public Guid payment_method_id { get; set; }
    }

    public class FillResponse
    {
        public int Trade_id { get; set; }

        public string Product_id { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public Guid Order_id { get; set; }

        public DateTime Created_at { get; set; }

        public string Liquidity { get; set; }

        public decimal Fee { get; set; }

        public bool Settled { get; set; }

        public string Side { get; set; }
    }

    public class Fill
    {
        public Guid order_id { get; set; }

        public string product_id { get; set; }
    }

    public class Funding
    {
        public Guid Id { get; set; }

        public string Order_id { get; set; }

        public string Profile_id { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTime Created_at { get; set; }

        public string Currency { get; set; }

        public decimal Repaid_amount { get; set; }

        public decimal Default_amount { get; set; }

        public bool Repaid_default { get; set; }
    }

    public class CancelOrderResponse
    {
        public IEnumerable<Guid> OrderIds { get; set; }
    }

    public class OrderResponse
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public string Product_id { get; set; }

        public string Side { get; set; }

        public string Stp { get; set; }

        public string Type { get; set; }

        public string Time_in_force { get; set; }

        public bool Post_only { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime Done_at { get; set; }

        public string Done_reason { get; set; }

        public decimal Fill_fees { get; set; }

        public decimal Filled_size { get; set; }

        public decimal Executed_value { get; set; }

        public string Status { get; set; }

        public bool Settled { get; set; }

        public decimal Specified_funds { get; set; }
    }

    public class Order
    {
        public string side { get; set; }

        public decimal size { get; set; }

        public decimal price { get; set; }

        public string type { get; set; }

        public string product_id { get; set; }

        public string time_in_force { get; set; }

        public string cancel_after { get; set; }

        public bool post_only { get; set; }
    }

    public class PaymentMethod
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool Primary_buy { get; set; }

        public bool Primary_sell { get; set; }

        public bool Allow_buy { get; set; }

        public bool Allow_sell { get; set; }

        public bool Allow_deposit { get; set; }

        public bool Allow_withdraw { get; set; }

        public Limit Limits { get; set; }
    }

    public class Limit
    {
        public IEnumerable<BuyPower> Buy { get; set; }

        public IEnumerable<BuyPower> Instant_buy { get; set; }

        public IEnumerable<SellPower> Sell { get; set; }

        public IEnumerable<SellPower> Deposit { get; set; }
    }

    public class SellPower : Power
    {
    }

    public class BuyPower : Power
    {
    }

    public abstract class Power
    {
        public decimal Period_in_days { get; set; }

        public Total Total { get; set; }
    }

    public class Total
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }

    public class ProductsOrderBookJsonResponse
    {
        public decimal Sequence { get; set; }

        public IEnumerable<IEnumerable<string>> Bids { get; set; }

        public IEnumerable<IEnumerable<string>> Asks { get; set; }
    }

    public class ProductsOrderBookResponse
    {
        public ProductsOrderBookResponse(
            decimal sequence,
            IEnumerable<Bid> bids,
            IEnumerable<Ask> asks)
        {
            Sequence = sequence;
            Bids = bids;
            Asks = asks;
        }

        public decimal Sequence { get; }

        public IEnumerable<Bid> Bids { get; }

        public IEnumerable<Ask> Asks { get; }
    }

    public class Ask : Quote
    {
        public Ask(
            decimal price,
            decimal size)
                : base(price, size)
        {
        }
    }

    public class Bid : Quote
    {
        public Bid(
            decimal price,
            decimal size)
                : base(price, size)
        {
        }
    }

    public class Product
    {
        public string Id { get; set; }

        public string Base_currency { get; set; }

        public string Quote_currency { get; set; }

        public string Base_min_size { get; set; }

        public string Base_max_size { get; set; }

        public string Quote_increment { get; set; }
    }

    public class ProductStats
    {
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Volume { get; set; }
    }

    public class ProductTicker
    {
        public int Trade_id { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public decimal Bid { get; set; }

        public decimal Ask { get; set; }

        public decimal Volume { get; set; }

        public DateTime Time { get; set; }
    }

    public class Quote
    {
        public Quote(
            decimal price,
            decimal size)
        {
            Price = price;
            Size = size;
        }

        public decimal Price { get; }

        public decimal Size { get; }

        public decimal? NumberOfOrders { get; set; }

        public Guid? OrderId { get; set; }
    }

    public class CoinbaseResponse
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }

    public class CryptoResponse
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }

    public class WithdrawalResponse
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Payout_at { get; set; }
    }

    public class Coinbase
    {
        public decimal amount { get; set; }

        public string currency { get; set; }

        public Guid coinbase_account_id { get; set; }
    }

    public class Crypto
    {
        public decimal amount { get; set; }

        public string currency { get; set; }

        public Guid crypto_address { get; set; }
    }

    public class Withdrawal
    {
        public decimal amount { get; set; }

        public string currency { get; set; }

        public Guid payment_method_id { get; set; }
    }




} // end of namespace

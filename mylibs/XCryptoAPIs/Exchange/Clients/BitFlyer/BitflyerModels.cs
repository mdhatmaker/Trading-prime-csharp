using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Utf8Json;

namespace CryptoAPIs.Exchange.Clients.BitFlyer
{
    internal static class BitFlyerConstants
    {
        internal static readonly Uri BaseUri = new Uri("https://api.bitflyer.jp/");
        internal const string SubscribeKey = "sub-c-52a9ab50-291b-11e5-baaa-0619f8945a4f";
    }

    public enum CurrencyCode
    {
        [EnumMember(Value = "JPY")]
        Jpy,

        [EnumMember(Value = "BTC")]
        Btc,

        [EnumMember(Value = "ETH")]
        Eth,

        [EnumMember(Value = "ETC")]
        Etc,

        [EnumMember(Value = "LTC")]
        Ltc,

        [EnumMember(Value = "BCH")]
        Bch,

        [EnumMember(Value = "MONA")]
        Mona,

        Unknown
    }

    public enum Side
    {
        [EnumMember(Value = "")]
        Unknown,

        [EnumMember(Value = "BUY")]
        Buy,

        [EnumMember(Value = "SELL")]
        Sell,

        [EnumMember(Value = "BUYSELL")]
        BuySell
    }

    public enum BitflyerSystemHealth
    {
        [EnumMember(Value = "NORMAL")]
        Normal,

        [EnumMember(Value = "BUSY")]
        Busy,

        [EnumMember(Value = "VERY BUSY")]
        VeryBusy,

        [EnumMember(Value = "SUPER BUSY")]
        SuperBusy,

        [EnumMember(Value = "NO ORDER")]
        NoOrder,

        [EnumMember(Value = "STOP")]
        Stop
    }

    public enum BoardStates
    {
        [EnumMember(Value = "RUNNING")]
        Running,

        [EnumMember(Value = "CLOSED")]
        Closed,

        [EnumMember(Value = "STARTING")]
        Starting,

        [EnumMember(Value = "PREOPEN")]
        Preopen,

        [EnumMember(Value = "CIRCUIT BREAK")]
        CircuitBreak,

        [EnumMember(Value = "AWAITING SQ")]
        AWAITING_SQ,

        [EnumMember(Value = "MATURED")]
        MATURED,
    }

    public enum DepositStatus
    {
        [EnumMember(Value = "PENDING")]
        Pending,

        [EnumMember(Value = "COMPLETED")]
        Completed
    }

    public enum ChildOrderType
    {
        [EnumMember(Value = "LIMIT")]
        Limit,

        [EnumMember(Value = "MARKET")]
        Market
    }

    public enum TimeInForce
    {
        [EnumMember(Value = "GTC")]
        GoodTilCanceled,

        [EnumMember(Value = "IOC")]
        ImmediateOrCancel,

        [EnumMember(Value = "FOK")]
        FillOrKill
    }

    public enum OrderMethod
    {
        [EnumMember(Value = "SIMPLE")]
        Simple,

        [EnumMember(Value = "IFD")]
        IfDone,

        [EnumMember(Value = "OCO")]
        OneCancelsTheOther,

        [EnumMember(Value = "IFDOCO")]
        IfDoneOneCancelsTheOther
    }

    public enum ParentOrderType
    {
        [EnumMember(Value = "IFD")]
        IfDone,

        [EnumMember(Value = "OCO")]
        OneCancelsTheOther,

        [EnumMember(Value = "IFDOCO")]
        IfDoneOneCancelsTheOther,

        [EnumMember(Value = "STOP")]
        Stop,

        [EnumMember(Value = "STOP_LIMIT")]
        StopLimit,

        [EnumMember(Value = "TRAIL")]
        Trail,
    }

    public enum ConditionType
    {
        [EnumMember(Value = "LIMIT")]
        Limit,

        [EnumMember(Value = "MARKET")]
        Market,

        [EnumMember(Value = "STOP")]
        Stop,

        [EnumMember(Value = "STOP_LIMIT")]
        StopLimit,

        [EnumMember(Value = "TRAIL")]
        Trail
    }

    public enum AddresseType
    {
        [EnumMember(Value = "NORMAL")]
        Normal
    }

    public enum ChildOrderState
    {
        [EnumMember(Value = "ACTIVE")]
        Active,

        [EnumMember(Value = "COMPLETED")]
        Completed,

        [EnumMember(Value = "CANCELED")]
        Canceled,

        [EnumMember(Value = "EXPIRED")]
        Expired,

        [EnumMember(Value = "REJECTED")]
        Rejected
    }

    public enum ParentOrderState
    {
        [EnumMember(Value = "ACTIVE")]
        Active,

        [EnumMember(Value = "COMPLETED")]
        Completed,

        [EnumMember(Value = "CANCELED")]
        Canceled,

        [EnumMember(Value = "EXPIRED")]
        Expired,

        [EnumMember(Value = "REJECTED")]
        Rejected
    }

    public enum ProductAlias
    {
        [EnumMember(Value = "")]
        None,

        [EnumMember(Value = "BTCJPY_MAT1WK")]
        BtcJpyThisWeek,

        [EnumMember(Value = "BTCJPY_MAT2WK")]
        BtcJpyNextWeek
    }

    public enum CollateralReasonCode
    {
        [EnumMember(Value = "CLEARING_COLL")]
        Clearing,

        [EnumMember(Value = "EXCHANGE_COLL")]
        Exchange,

        [EnumMember(Value = "POST_COLL")]
        Post,

        [EnumMember(Value = "CANCEL_COLL")]
        Cancel
    }

    public class CancelAllOrdersParameter
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }
    }

    public class CancelChildOrderParameter
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "child_order_id")]
        public string ChildOrderId { get; set; }

        [DataMember(Name = "child_order_acceptance_id")]
        public string ChildOrderAcceptanceId { get; set; }
    }

    public class CancelParentOrderParameter
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "parent_order_id")]
        public string ParentOrderId { get; set; }

        [DataMember(Name = "parent_order_acceptance_id")]
        public string ParentOrderAcceptanceId { get; set; }
    }

    public class SendChildOrderParameter
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "child_order_type")]
        public ChildOrderType ChildOrderType { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "minute_to_expire")]
        public int MinuteToExpire { get; set; }

        [DataMember(Name = "time_in_force")]
        public TimeInForce TimeInForce { get; set; }
    }

    public class SendCoinParameter
    {
        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "additional_fee")]
        public double AdditionalFee { get; set; }

        [DataMember(Name = "code")]
        public int Code { get; set; }
    }

    public class SendParentOrderParameter
    {
        [DataMember(Name = "order_method")]
        public OrderMethod OrderMethod { get; set; }

        [DataMember(Name = "minute_to_expire")]
        public int MinuteToExpire { get; set; }

        [DataMember(Name = "time_in_force")]
        public TimeInForce TimeInForce { get; set; }

        [DataMember(Name = "parameters")]
        public ParentOrderDetailParameter[] Parameters { get; set; }
    }

    public class ParentOrderDetailParameter
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "condition_type")]
        public ConditionType ConditionType { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "trigger_price")]
        public double TriggerPrice { get; set; }

        [DataMember(Name = "offset")]
        public double Offset { get; set; }
    }

    public class WithdrawParameter
    {
        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "bank_account_id")]
        public long BankAccountId { get; set; }

        [DataMember(Name = "amount")]
        public long Amount { get; set; }

        [DataMember(Name = "code")]
        public int Code { get; set; }
    }

    public class Balance
    {
        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "available")]
        public double Available { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class BankAccount
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "is_verified")]
        public bool IsVerified { get; set; }

        [DataMember(Name = "bank_name")]
        public string BankName { get; set; }

        [DataMember(Name = "branch_name")]
        public string BranchName { get; set; }

        [DataMember(Name = "account_type")]
        public string AccountType { get; set; }

        [DataMember(Name = "account_number")]
        public string AccountNumber { get; set; }

        [DataMember(Name = "account_name")]
        public string AccountName { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Board
    {
        [DataMember(Name = "mid_price")]
        public double MiddlePrice { get; set; }

        [DataMember(Name = "asks")]
        public BoardOrder[] Asks { get; set; }

        [DataMember(Name = "bids")]
        public BoardOrder[] Bids { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class BoardOrder
    {
        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class BoardState
    {
        [DataMember(Name = "health")]
        public BitflyerSystemHealth Health { get; set; }

        [DataMember(Name = "state")]
        public BoardStates State { get; set; }

        [DataMember(Name = "data")]
        public BoardStateData Data { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class BoardStateData
    {
        [DataMember(Name = "special_quotation")]
        public double SpecialQuotation { get; set; }
    }

    public class Chat
    {
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class ChildOrder
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "child_order_id")]
        public string ChildOrderId { get; set; }

        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "child_order_type")]
        public ChildOrderType ChildOrderType { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "average_price")]
        public double AveragePrice { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "child_order_state")]
        public ChildOrderState ChildOrderState { get; set; }

        [DataMember(Name = "expire_date")]
        public string ExpireDate { get; set; }

        [DataMember(Name = "child_order_date")]
        public string ChildOrderDate { get; set; }

        [DataMember(Name = "child_order_acceptance_id")]
        public string ChildOrderAcceptanceId { get; set; }

        [DataMember(Name = "outstanding_size")]
        public double OutstandingSize { get; set; }

        [DataMember(Name = "cancel_size")]
        public double CancelSize { get; set; }

        [DataMember(Name = "executed_size")]
        public double ExecutedSize { get; set; }

        [DataMember(Name = "total_commission")]
        public double TotalCommission { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class CoinIn
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "order_id")]
        public string OrderId { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "tx_hash")]
        public string TransactionHash { get; set; }

        [DataMember(Name = "status")]
        public DepositStatus Status { get; set; }

        [DataMember(Name = "event_date")]
        public DateTime EventDate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class CoinOut
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "order_id")]
        public string OrderId { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "tx_hash")]
        public string TransactionHash { get; set; }

        [DataMember(Name = "additional_fee")]
        public double AdditionalFee { get; set; }

        [DataMember(Name = "status")]
        public DepositStatus Status { get; set; }

        [DataMember(Name = "event_date")]
        public DateTime EventDate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }

    }

    public class Collateral
    {
        [DataMember(Name = "collateral")]
        public double Amount { get; set; }

        [DataMember(Name = "open_position_pnl")]
        public double OpenPositionProfitOrLoss { get; set; }

        [DataMember(Name = "require_collateral")]
        public double RequireCollateral { get; set; }

        [DataMember(Name = "keep_rate")]
        public double KeepRate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class CollateralHistory
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "change")]
        public double Change { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "reason_code")]
        public CollateralReasonCode ResonCode { get; set; }

        [DataMember(Name = "date")]
        public DateTime date { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class CryptoCurrencyAddress
    {
        [DataMember(Name = "type")]
        public AddresseType Type { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Deposit
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "order_id")]
        public string OrderId { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "status")]
        public DepositStatus Status { get; set; }

        [DataMember(Name = "event_date")]
        public DateTime EventDate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }

    }

    public class Error
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "error_message")]
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Health
    {
        [DataMember(Name = "status")]
        public BitflyerSystemHealth Status { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Market
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "alias")]
        public ProductAlias ProductAlias { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class ParentOrder
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "parent_order_id")]
        public string ParentOrderId { get; set; }

        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "parent_order_type")]
        public ParentOrderType ParentOrderType { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "average_price")]
        public double AveragePrice { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "parent_order_state")]
        public ParentOrderState ParentOrderState { get; set; }

        [DataMember(Name = "expire_date")]
        public string ExpireDate { get; set; }

        [DataMember(Name = "parent_order_date")]
        public DateTime ParentOrderDate { get; set; }

        [DataMember(Name = "parent_order_acceptance_id")]
        public string ParentOrderAcceptanceId { get; set; }

        [DataMember(Name = "outstanding_size")]
        public double OutstandingSize { get; set; }

        [DataMember(Name = "cancel_size")]
        public double CancelSize { get; set; }

        [DataMember(Name = "executed_size")]
        public double ExecutedSize { get; set; }

        [DataMember(Name = "total_commission")]
        public double TotalCommission { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class ParentOrderDetail
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "parent_order_id")]
        public string ParentOrderId { get; set; }

        [DataMember(Name = "order_method")]
        public OrderMethod OrderMethod { get; set; }

        [DataMember(Name = "parent_order_acceptance_id")]
        public string ParentOrderAcceptanceId { get; set; }

        [DataMember(Name = "parameters")]
        public ParentOrderDetailParameter[] Parameters { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Position
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "commission")]
        public double Commission { get; set; }

        [DataMember(Name = "swap_point_accumulate")]
        public double SwapPointAccumulate { get; set; }

        [DataMember(Name = "require_collateral")]
        public double RequireCollateral { get; set; }

        [DataMember(Name = "open_date")]
        public DateTime OpenDate { get; set; }

        [DataMember(Name = "leverage")]
        public double Leverage { get; set; }

        [DataMember(Name = "pnl")]
        public double ProfitOrLoss { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class PostResult
    {
        [DataMember(Name = "message_id")]
        public string MessageId { get; set; }

        [DataMember(Name = "child_order_acceptance_id")]
        public string ChildOrderAcceptanceId { get; set; }

        [DataMember(Name = "parent_order_acceptance_id")]
        public string ParentOrderAcceptanceId { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class PrivateExecution
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "child_order_id")]
        public string ChildOrderId { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "commission")]
        public double Commission { get; set; }

        [DataMember(Name = "exec_date")]
        public DateTime ExecDate { get; set; }

        [DataMember(Name = "child_order_acceptance_id")]
        public string ChildOrderAcceptanceId { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class PublicExecution
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "side")]
        public Side Side { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "size")]
        public double Size { get; set; }

        [DataMember(Name = "exec_date")]
        public DateTime ExecDate { get; set; }

        [DataMember(Name = "buy_child_order_acceptance_id")]
        public string BuyChildOrderAcceptanceId { get; set; }

        [DataMember(Name = "sell_child_order_acceptance_id")]
        public string SellChildOrderAcceptanceId { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Ticker
    {
        [DataMember(Name = "product_code")]
        public string ProductCode { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "tick_id")]
        public long TickId { get; set; }

        [DataMember(Name = "best_bid")]
        public double BestBid { get; set; }

        [DataMember(Name = "best_ask")]
        public double BestAsk { get; set; }

        [DataMember(Name = "best_bid_size")]
        public double BestBidSize { get; set; }

        [DataMember(Name = "best_ask_size")]
        public double BestAskSize { get; set; }

        [DataMember(Name = "total_bid_depth")]
        public double TotalBidDepth { get; set; }

        [DataMember(Name = "total_ask_depth")]
        public double TotalAskDepth { get; set; }

        [DataMember(Name = "ltp")]
        public double LatestPrice { get; set; }

        [DataMember(Name = "volume")]
        public double Volume { get; set; }

        [DataMember(Name = "volume_by_product")]
        public double VolumeByProduct { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class TradingCommission
    {
        [DataMember(Name = "commission_rate")]
        public double CommissionRate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }

    public class Withdrawal
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "order_id")]
        public string OrderId { get; set; }

        [DataMember(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "status")]
        public DepositStatus Status { get; set; }

        [DataMember(Name = "event_date")]
        public DateTime EventDate { get; set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(JsonSerializer.Serialize(this));
        }
    }






    public class BitFlyerApiException : Exception
    {
        public string Path { get; }

        public Error ErrorResponse { get; }

        public BitFlyerApiException(string path, string message, Error errorResponse, Exception inner)
            : base(message, inner)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            Path = path;
            ErrorResponse = errorResponse;
        }

        public BitFlyerApiException(string path, string message) :
            this(path, message, null, null)
        {
        }

        public BitFlyerApiException(string path, string message, Error errorResponse) :
            this(path, message, errorResponse, null)
        {
        }

        public BitFlyerApiException(string path, string message, Exception inner) :
            this(path, message, null, inner)
        {
        }

        public override string ToString()
        {
            return $"The request to {Path} has thrown an exception: {base.ToString()}";
        }
    }





} // end of namespace

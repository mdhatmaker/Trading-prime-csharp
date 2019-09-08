using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using System.IO;

namespace CryptoAPIs.Exchange.Clients.Kraken
{
    public class ResponseBase
    {
        public List<string> Error;
    }

    public class GetServerTimeResult
    {
        public int UnixTime;
        public string Rfc1123;
    }

    public class GetServerTimeResponse : ResponseBase
    {
        public GetServerTimeResult Result;
    }

    public class AssetInfo
    {
        /// <summary>
        /// Alternate name.
        /// </summary>
        public string Altname;

        /// <summary>
        /// Asset class.
        /// </summary>
        public string Aclass;

        /// <summary>
        /// Scaling decimal places for record keeping.
        /// </summary>
        public int Decimals;

        /// <summary>
        /// Scaling decimal places for output display.
        /// </summary>
        [JsonProperty(PropertyName = "display_decimals ")]
        public int DisplayDecimals;
    }

    public class GetAssetInfoResponse : ResponseBase
    {
        public Dictionary<string, AssetInfo> Result;
    }

    public class AssetPair
    {
        /// <summary>
        /// Alternate pair name.
        /// </summary>
        public string Altname;

        /// <summary>
        /// Asset private class of base component.
        /// </summary>
        [JsonProperty(PropertyName = "aclass_base")]
        public string AclassBase;

        /// <summary>
        /// Asset id of base component
        /// </summary>
        public string Base;

        /// <summary>
        /// Asset class of quote component.
        /// </summary>
        [JsonProperty(PropertyName = "aclass_quote")]
        public string AclassQuote;

        /// <summary>
        /// Asset id of quote component.
        /// </summary>
        public string Quote;

        /// <summary>
        /// Volume lot size.
        /// </summary>
        public string Lot;

        /// <summary>
        /// Scaling decimal places for pair.
        /// </summary>
        [JsonProperty(PropertyName = "pair_decimals")]
        public int PairDecimals;

        /// <summary>
        /// Scaling decimal places for volume.
        /// </summary>
        [JsonProperty(PropertyName = "lot_decimals")]
        public int LotDecimals;

        /// <summary>
        /// Amount to multiply lot volume by to get currency volume.
        /// </summary>
        [JsonProperty(PropertyName = "lot_multiplier")]
        public int LotMultiplier;

        /// <summary>
        /// Array of leverage amounts available when buying.
        /// </summary>
        [JsonProperty(PropertyName = "leverage_buy")]
        public decimal[] LeverageBuy;

        /// <summary>
        /// Array of leverage amounts available when selling.
        /// </summary>
        [JsonProperty(PropertyName = "leverage_sell")]
        public decimal[] LeverageSell;

        /// <summary>
        /// Fee schedule array in [volume, percent fee].
        /// </summary>
        public decimal[][] Fees;

        /// <summary>
        /// Maker fee schedule array in [volume, percent fee] tuples(if on maker/taker).
        /// </summary>
        [JsonProperty(PropertyName = "fees_maker")]
        public decimal[][] FeesMaker;

        /// <summary>
        /// Volume discount currency
        /// </summary>
        [JsonProperty(PropertyName = "fee_volume_currency")]
        public string FeeVolumeCurrency;

        /// <summary>
        /// Margin call level.
        /// </summary>
        [JsonProperty(PropertyName = "margin_call")]
        public decimal MarginCall;

        /// <summary>
        /// Stop-out/liquidation margin level.
        /// </summary>
        [JsonProperty(PropertyName = "margin_stop")]
        public decimal MarginStop;
    }

    public class GetAssetPairsResponse : ResponseBase
    {
        public Dictionary<string, AssetPair> Result;
    }

    public class Ticker
    {
        /// <summary>
        /// Ask array(<price>, <whole lot volume>, <lot volume>).
        /// </summary>
        [JsonProperty(PropertyName = "a")]
        public decimal[] Ask;

        /// <summary>
        /// Bid array(<price>, <whole lot volume>, <lot volume>).
        /// </summary>
        [JsonProperty(PropertyName = "b")]
        public decimal[] Bid;

        /// <summary>
        /// Last trade closed array(<price>, <lot volume>).
        /// </summary>
        [JsonProperty(PropertyName = "c")]
        public decimal[] Closed;

        /// <summary>
        /// Volume array(<today>, <last 24 hours>).
        /// </summary>
        [JsonProperty(PropertyName = "v")]
        public decimal[] Volume;

        /// <summary>
        /// Volume weighted average price array(<today>, <last 24 hours>).
        /// </summary>
        [JsonProperty(PropertyName = "p")]
        public decimal[] VWAP;

        /// <summary>
        /// Number of trades array(<today>, <last 24 hours>).
        /// </summary>
        [JsonProperty(PropertyName = "t")]
        public int[] Trades;

        /// <summary>
        /// Low array(<today>, <last 24 hours>).
        /// </summary>
        [JsonProperty(PropertyName = "l")]
        public decimal[] Low;

        /// <summary>
        /// High array(<today>, <last 24 hours>).
        /// </summary>
        [JsonProperty(PropertyName = "h")]
        public decimal[] High;

        /// <summary>
        /// Today's opening price.
        /// </summary>
        [JsonProperty(PropertyName = "o")]
        public decimal Open;

        public decimal AskPrice { get { return Ask[0]; } }
        public decimal AskVolume { get { return Ask[2]; } }
        public decimal AskVolumeWholeLot { get { return Ask[1]; } }
        public decimal BidPrice { get { return Bid[0]; } }
        public decimal BidVolume { get { return Bid[2]; } }
        public decimal BidVolumeWholeLot { get { return Bid[1]; } }

        public override string ToString()
        {
            return string.Format("[Ticker] ({0}) {1}x{2}:{3}x{4} ({5})  open:{6} high(24h):{7} low(24h):{8} volume(24h):{9} vwap(24h):{10}", Bid[2], Bid[1], Bid[0], Ask[0], Ask[1], Ask[2], Open, High[1], Low[1], Volume[1], VWAP[1]);
        }
    }

    public class GetTickerResponse : ResponseBase
    {
        public Dictionary<string, Ticker> Result;
    }

    public class KrakenOHLC : ZCandlestick
    {
        public KrakenOHLC(int timestamp, decimal o, decimal h, decimal l, decimal c, decimal volume, decimal VWAP, int count)
            : base(o, h, l, c, timestamp)
        {
            this.v = volume;
            this.vw = VWAP;
            this.n = count;
        }

        public override string ToString()
        {
            return string.Format("[OHLC:{0}]  o:{1} h:{2} l:{3} c:{4} volume:{5} vwap:{6}  {7}", count, open, high, low, close, volume, vwap, time.ToDateTimeString());
        }
    } // end of class KrakenOHLC

    public class GetOHLCResult
    {
        public Dictionary<string, List<KrakenOHLC>> Pairs;

        /// <summary>
        /// Id to be used as since when polling for new, committed OHLC data.
        /// </summary>
        public long Last;

        public void WriteToFile(string pathname)
        {
            using (var writer = new StreamWriter(pathname))
            {
                writer.WriteLine("DateTime,Open,High,Low,Close,Volume,VWAP");

                foreach (var bar in Pairs.Values.First())
                {
                    decimal o = bar.open;
                    decimal h = bar.high;
                    decimal l = bar.low;
                    decimal c = bar.close;
                    DateTime time = bar.time;
                    decimal volume = bar.volume;
                    decimal vwap = bar.vwap;
                    int count = bar.count;
                    writer.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", time.ToDateTimeString(), o, h, l, c, volume, vwap));
                }
            }
        }
    }

    public class GetOHLCResponse : ResponseBase
    {
        public GetOHLCResult Result;
    }

    public class OrderBook
    {
        /// <summary>
        /// Ask side array of array entries(<price>, <volume>, <timestamp>)
        /// </summary>
        public decimal[][] Asks;

        /// <summary>
        /// Bid side array of array entries(<price>, <volume>, <timestamp>)
        /// </summary>
        public decimal[][] Bids;
    }

    public class GetOrderBookResponse : ResponseBase
    {
        public Dictionary<string, OrderBook> Result;
    }

    public class Trade
    {
        public decimal Price;
        public decimal Volume;
        public int Time;
        public string Side;
        public string Type;
        public string Misc;
    }

    public class GetRecentTradesResult
    {
        public Dictionary<string, List<Trade>> Trades;

        /// <summary>
        /// Id to be used as since when polling for new trade data.
        /// </summary>
        public long Last;
    }

    public class SpreadItem
    {
        public int Time;
        public decimal Bid;
        public decimal Ask;
    }

    public class GetRecentSpreadResult
    {
        public Dictionary<string, List<SpreadItem>> Spread;

        /// <summary>
        /// Id to be used as since when polling for new spread data
        /// </summary>
        public long Last;
    }

    public class GetBalanceResponse : ResponseBase
    {
        public Dictionary<string, decimal> Result;
    }

    public class TradeBalanceInfo
    {
        /// <summary>
        /// Equivalent balance(combined balance of all currencies).
        /// </summary>
        [JsonProperty(PropertyName = "eb")]
        public decimal EquivalentBalance;

        /// <summary>
        /// Trade balance(combined balance of all equity currencies).
        /// </summary>
        [JsonProperty(PropertyName = "tb")]
        public decimal TradeBalance;

        /// <summary>
        /// Margin amount of open positions.
        /// </summary>
        [JsonProperty(PropertyName = "m")]
        public decimal MarginAmount;

        /// <summary>
        /// Unrealized net profit/loss of open positions.
        /// </summary>
        [JsonProperty(PropertyName = "n")]
        public decimal UnrealizedProfitAndLoss;

        /// <summary>
        /// Cost basis of open positions.
        /// </summary>
        [JsonProperty(PropertyName = "c")]
        public decimal CostBasis;

        /// <summary>
        /// Current floating valuation of open positions.
        /// </summary>
        [JsonProperty(PropertyName = "v")]
        public decimal FloatingValutation;

        /// <summary>
        /// Equity = trade balance + unrealized net profit/loss.
        /// </summary>
        [JsonProperty(PropertyName = "e")]
        public decimal Equity;

        /// <summary>
        /// Free margin = equity - initial margin(maximum margin available to open new positions).
        /// </summary>
        [JsonProperty(PropertyName = "mf")]
        public decimal FreeMargin;

        /// <summary>
        /// Margin level = (equity / initial margin) * 100
        /// </summary>
        [JsonProperty(PropertyName = "ml")]
        public decimal MarginLevel;

        public override string ToString()
        {
            return string.Format("[TradeBalanceInfo] eqbal:{0} trdbal:{1} margin:{2} unrealizedPL:{3} costbasis:{4} floatingval:{5} equity:{6} freemargin:{7} marginlevel:{8}", EquivalentBalance, TradeBalance, MarginAmount, UnrealizedProfitAndLoss, CostBasis, FloatingValutation, Equity, FreeMargin, MarginLevel);
        }
    }

    public class GetTradeBalanceResponse : ResponseBase
    {
        public TradeBalanceInfo Result;
    }

    public class OrderDescription
    {
        /// <summary>
        /// Asset pair.
        /// </summary>
        public string Pair;

        /// <summary>
        /// Type of order (buy/sell).
        /// </summary>
        public string Type;

        /// <summary>
        /// Order type (See Add standard order).
        /// </summary>
        public string OrderType;

        /// <summary>
        /// Primary price.
        /// </summary>
        public decimal Price;

        /// <summary>
        /// Secondary price
        /// </summary>
        public decimal Price2;

        /// <summary>
        /// Amount of leverage
        /// </summary>
        public string Leverage;

        /// <summary>
        /// Order description.
        /// </summary>
        public string Order;

        /// <summary>
        /// Conditional close order description (if conditional close set).
        /// </summary>
        public string Close;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} ({4}) lev:{5}", Pair, Type, OrderType, Price, Price2, Leverage);
        }
    }

    public class OrderInfo
    {
        /// <summary>
        /// Referral order transaction id that created this order
        /// </summary>
        public string RefId;

        /// <summary>
        /// User reference id
        /// </summary>
        public int? UserRef;

        /// <summary>
        /// Status of order
        /// pending = order pending book entry
        /// open = open order
        /// closed = closed order
        /// canceled = order canceled
        /// expired = order expired
        /// </summary>
        public string Status;

        /// <summary>
        /// Unix timestamp of when order was placed
        /// </summary>
        public double OpenTm;

        /// <summary>
        /// Unix timestamp of order start time (or 0 if not set)
        /// </summary>
        public double StartTm;

        /// <summary>
        /// Unix timestamp of order end time (or 0 if not set)
        /// </summary>
        public double ExpireTm;

        /// <summary>
        /// Unix timestamp of when order was closed
        /// </summary>
        public double? CloseTm;

        /// <summary>
        /// Additional info on status (if any)
        /// </summary>
        public string Reason;

        /// <summary>
        /// Order description info
        /// </summary>
        public OrderDescription Descr;

        /// <summary>
        /// Volume of order (base currency unless viqc set in oflags)
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public decimal Volume;

        /// <summary>
        /// Volume executed (base currency unless viqc set in oflags)
        /// </summary>
        [JsonProperty(PropertyName = "vol_exec")]
        public decimal VolumeExecuted;

        /// <summary>
        /// Total cost (quote currency unless unless viqc set in oflags)
        /// </summary>
        public decimal Cost;

        /// <summary>
        /// Total fee (quote currency)
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Average price (quote currency unless viqc set in oflags)
        /// </summary>
        public decimal Price;

        /// <summary>
        /// Stop price (quote currency, for trailing stops)
        /// </summary>
        public decimal? StopPrice;

        /// <summary>
        /// Triggered limit price (quote currency, when limit based order type triggered)
        /// </summary>
        public decimal? LimitPrice;

        /// <summary>
        /// Comma delimited list of miscellaneous info
        /// stopped = triggered by stop price
        /// touched = triggered by touch price
        /// liquidated = liquidation
        /// partial = partial fill
        /// </summary>
        public string Misc;

        /// <summary>
        /// Comma delimited list of order flags
        /// viqc = volume in quote currency
        /// fcib = prefer fee in base currency (default if selling)
        /// fciq = prefer fee in quote currency (default if buying)
        /// nompp = no market price protection
        /// </summary>
        public string Oflags;

        /// <summary>
        /// Array of trade ids related to order (if trades info requested and data available)
        /// </summary>
        public List<string> Trades = new List<string>();

        public override string ToString()
        {
            return string.Format("[OrderInfo:{0}:{1}] {2} {3}x{4}/{5}x{6}  cost:{7} fee:{8} '{9}' '{10}'", RefId, UserRef, Status, VolumeExecuted, Price, Volume, LimitPrice, Cost, Fee, Reason, Descr);
        }

        public string ToShortString()
        {
            return string.Format("[{0}:{1}] {2} {3}x{4}/{5}x{6} '{7}'", RefId, UserRef, Status, VolumeExecuted, Price, Volume, LimitPrice, Reason);
        }

    }

    public class QueryOrdersResponse : ResponseBase
    {
        public Dictionary<string, OrderInfo> Result;
    }

    public class TradeInfo
    {
        /// <summary>
        /// Order responsible for execution of trade.
        /// </summary>
        public string OrderTxid;

        /// <summary>
        /// Asset pair.
        /// </summary>
        public string Pair;

        /// <summary>
        /// Unix timestamp of trade.
        /// </summary>
        public double Time;

        /// <summary>
        /// Type of order (buy/sell).
        /// </summary>
        public string Type;

        /// <summary>
        /// Order type.
        /// </summary>
        public string OrderType;

        /// <summary>
        /// Average price order was executed at (quote currency).
        /// </summary>
        public decimal Price;

        /// <summary>
        /// Total cost of order (quote currency).
        /// </summary>
        public decimal Cost;

        /// <summary>
        /// Total fee (quote currency).
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Volume (base currency).
        /// </summary>
        public decimal Vol;

        /// <summary>
        /// Initial margin (quote currency).
        /// </summary>
        public decimal Margin;

        /// <summary>
        /// Comma delimited list of miscellaneous info.
        /// closing = trade closes all or part of a position.
        /// </summary>
        public string Misc;

        /// <summary>
        /// Position status(open/closed).
        /// </summary>
        public string PosStatus;

        /// <summary>
        /// Average price of closed portion of position(quote currency).
        /// </summary>
        public decimal? CPrice;

        /// <summary>
        /// Total cost of closed portion of position(quote currency).
        /// </summary>
        public decimal? CCost;

        /// <summary>
        /// Total fee of closed portion of position(quote currency).
        /// </summary>
        public decimal? CFee;

        /// <summary>
        /// Total fee of closed portion of position(quote currency).
        /// </summary>
        public decimal? CVol;

        /// <summary>
        /// Total margin freed in closed portion of position(quote currency).
        /// </summary>
        public decimal? CMargin;

        /// <summary>
        /// Net profit/loss of closed portion of position(quote currency, quote currency scale).
        /// </summary>
        public decimal? Net;

        /// <summary>
        /// List of closing trades for position(if available).
        /// </summary>
        public string[] Trades;
    }

    public class GetTradesHistoryResult
    {
        public Dictionary<string, TradeInfo> Trades;
        public int Count;
    }

    public class GetTradesHistoryResponse : ResponseBase
    {
        public GetTradesHistoryResult Result;
    }

    public class QueryTradesResponse : ResponseBase
    {
        public Dictionary<string, TradeInfo> Result;
    }

    public class PositionInfo
    {
        /// <summary>
        /// Order responsible for execution of trade.
        /// </summary>
        public string OrderTxid;

        /// <summary>
        /// Asset pair.
        /// </summary>
        public string Pair;

        /// <summary>
        /// Unix timestamp of trade.
        /// </summary>
        public double Time;

        /// <summary>
        /// Type of order used to open position (buy/sell).
        /// </summary>
        public string Type;

        /// <summary>
        /// Order type used to open position.
        /// </summary>
        public string OrderType;

        /// <summary>
        /// Opening cost of position (quote currency unless viqc set in oflags).
        /// </summary>
        public decimal Cost;

        /// <summary>
        /// opening fee of position (quote currency).
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Position volume (base currency unless viqc set in oflags).
        /// </summary>
        public decimal Vol;

        /// <summary>
        /// Position volume closed (base currency unless viqc set in oflags).
        /// </summary>
        [JsonProperty(PropertyName = "vol_closed")]
        public decimal VolClosed;

        /// <summary>
        /// Initial margin (quote currency).
        /// </summary>
        public decimal Margin;

        /// <summary>
        /// Current value of remaining position (if docalcs requested.  quote currency).
        /// </summary>
        public decimal Value;

        /// <summary>
        /// Unrealized profit/loss of remaining position (if docalcs requested.  quote currency, quote currency scale).
        /// </summary>
        public decimal Net;

        /// <summary>
        /// Comma delimited list of miscellaneous info.
        /// </summary>
        public string Misc;

        /// <summary>
        /// Comma delimited list of order flags.
        /// </summary>
        public string OFlags;

        /// <summary>
        /// Volume in quote currency.
        /// </summary>
        public decimal Viqc;
    }

    public class GetOpenPositionsResponse : ResponseBase
    {
        public Dictionary<string, PositionInfo> Result;
    }

    public class LedgerInfo
    {
        /// <summary>
        /// Reference id.
        /// </summary>
        public string Refid;

        /// <summary>
        /// Unix timestamp of ledger.
        /// </summary>
        public double Time;

        /// <summary>
        /// Type of ledger entry.
        /// </summary>
        public string Type;

        /// <summary>
        /// Asset class.
        /// </summary>
        public string Aclass;

        /// <summary>
        /// Asset.
        /// </summary>
        public string Asset;

        /// <summary>
        /// Transaction amount.
        /// </summary>
        public decimal Amount;

        /// <summary>
        /// Transaction fee.
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Resulting balance.
        /// </summary>
        public decimal Balance;
    }

    public class GetLedgerResult
    {
        public Dictionary<string, LedgerInfo> Ledger;
        public int Count;
    }

    public class GetLedgerResponse : ResponseBase
    {
        public GetLedgerResult Result;
    }

    public class QueryLedgersResponse : ResponseBase
    {
        public Dictionary<string, LedgerInfo> Result;
    }

    public class FeeInfo
    {
        /// <summary>
        /// Current fee in percent.
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Minimum fee for pair (if not fixed fee).
        /// </summary>
        public decimal MinFee;

        /// <summary>
        /// Maximum fee for pair (if not fixed fee).
        /// </summary>
        public decimal MaxFee;

        /// <summary>
        /// Next tier's fee for pair (if not fixed fee.  nil if at lowest fee tier).
        /// </summary>
        public decimal NextFee;

        /// <summary>
        /// Volume level of next tier (if not fixed fee.  nil if at lowest fee tier).
        /// </summary>
        public decimal NextVolume;

        /// <summary>
        /// Volume level of current tier (if not fixed fee.  nil if at lowest fee tier).
        /// </summary>
        public decimal TierVolume;
    }

    public class GetTradeVolumeResult
    {
        /// <summary>
        /// Volume currency.
        /// </summary>
        public string Currency;

        /// <summary>
        /// Current discount volume.
        /// </summary>
        public decimal Volume;

        /// <summary>
        /// Fee tier info (if requested).
        /// </summary>
        public Dictionary<string, FeeInfo> Fees;

        /// <summary>
        /// Maker fee tier info (if requested) for any pairs on maker/taker schedule.
        /// </summary>
        [JsonProperty(PropertyName = "fees_maker")]
        public Dictionary<string, FeeInfo> FeesMaker;
    }

    public class GetTradeVolumeResponse : ResponseBase
    {
        public GetTradeVolumeResult Result;
    }

    public class KrakenOrder
    {
        // Required fields first

        /// <summary>
        /// Asset pair.
        /// </summary>
        public string Pair;

        /// <summary>
        /// Type of order (buy/sell).
        /// </summary>
        public string Type;

        /// <summary>
        /// Order type:
        /// market
        /// limit(price = limit price)
        /// stop-loss(price = stop loss price)
        /// take-profit(price = take profit price)
        /// stop-loss-profit(price = stop loss price, price2 = take profit price)
        /// stop-loss-profit-limit(price = stop loss price, price2 = take profit price)
        /// stop-loss-limit(price = stop loss trigger price, price2 = triggered limit price)
        /// take-profit-limit(price = take profit trigger price, price2 = triggered limit price)
        /// trailing-stop(price = trailing stop offset)
        /// trailing-stop-limit(price = trailing stop offset, price2 = triggered limit offset)
        /// stop-loss-and-limit(price = stop loss price, price2 = limit price)
        /// settle-position
        /// </summary>
        public string OrderType;

        /// <summary>
        /// Order volume in lots.
        /// </summary>
        public decimal Volume;

        // Optional fields

        /// <summary>
        /// Price (optional.  dependent upon ordertype).
        /// </summary>
        public decimal? Price;

        /// <summary>
        /// Secondary price (optional.  dependent upon ordertype).
        /// </summary>
        public decimal? Price2;

        /// <summary>
        /// Amount of leverage desired (optional.  default = none).
        /// </summary>
        public decimal? Leverage;

        /// <summary>
        /// Comma delimited list of order flags (optional):
        /// viqc = volume in quote currency(not available for leveraged orders)
        /// fcib = prefer fee in base currency
        /// fciq = prefer fee in quote currency
        /// nompp = no market price protection
        /// post = post only order(available when ordertype = limit)
        /// </summary>
        public string OFlags;

        /// <summary>
        /// scheduled start time (optional):
        /// 0 = now(default)
        /// +<n> = schedule start time<n> seconds from now
        /// <n> = unix timestamp of start time
        /// </summary>
        public int? StartTm;

        /// <summary>
        /// Expiration time (optional):
        /// 0 = no expiration(default)
        /// +<n> = expire<n> seconds from now
        /// <n> = unix timestamp of expiration time
        /// </summary>
        public int? ExpireTm;

        /// <summary>
        /// User reference id.  32-bit signed number.  (optional).
        /// </summary>
        public int? UserRef;

        /// <summary>
        /// Validate inputs only.  do not submit order (optional)
        /// </summary>
        public bool? Validate;

        /// <summary>
        /// Optional closing order to add to system when order gets filled:
        /// close[ordertype] = order type
        /// close[price] = price
        /// close[price2] = secondary price
        /// </summary>
        public Dictionary<string, string> Close;

        // The following fields are set in AddOrder when the order was added successfully

        /// <summary>
        /// Order description info.
        /// </summary>
        public AddOrderDescr Descr;

        /// <summary>
        /// Array of transaction ids for order (if order was added successfully).
        /// </summary>
        public string[] Txid;

        public override string ToString()
        {
            return string.Format("[KrakenOrder] {0} {1} {2} {3}x{4} price2:{5} lev:{6} [{7}]", Pair, Type, OrderType, Volume, Price, Price2, Leverage, OFlags);
        }
    }

    public class AddOrderDescr
    {
        /// <summary>
        /// Order description.
        /// </summary>
        public string Order;

        /// <summary>
        /// Conditional close order description (if conditional close set).
        /// </summary>
        public string Close;

        public override string ToString()
        {
            return string.Format("{0}", Order);
        }
    }

    public class AddOrderResult
    {
        /// <summary>
        /// Order description info.
        /// </summary>
        public AddOrderDescr Descr;

        /// <summary>
        /// Array of transaction ids for order (if order was added successfully).
        /// </summary>
        public string[] Txid;

        public override string ToString()
        {
            return string.Format("\n({0}) {1}", string.Join(",", Txid), Descr);
        }
    }

    public class AddOrderResponse : ResponseBase
    {
        public AddOrderResult Result;
    }

    public class CancelOrderResult
    {
        /// <summary>
        /// Number of orders canceled.
        /// </summary>
        public int Count;

        /// <summary>
        /// If set, order(s) is/are pending cancellation.
        /// </summary>
        public bool? Pending;

        public override string ToString()
        {
            return string.Format("[CancelOrderResult] pending:{0} count:{1}", Pending, Count);
        }
    }

    public class CancelOrderResponse : ResponseBase
    {
        public CancelOrderResult Result;
    }

    public class GetDepositMethodsResult
    {
        /// <summary>
        /// Name of deposit method.
        /// </summary>
        public string Method;

        /// <summary>
        /// Maximum net amount that can be deposited right now, or false if no limit
        /// </summary>
        public string Limit;

        /// <summary>
        /// Amount of fees that will be paid.
        /// </summary>
        public string Fee;

        /// <summary>
        /// Whether or not method has an address setup fee (optional).
        /// </summary>
        [JsonProperty(PropertyName = "address-setup-fee")]
        public bool? AddressSetupFee;
    }

    public class GetDepositMethodsResponse : ResponseBase
    {
        public GetDepositMethodsResult[] Result;
    }

    public class GetDepositAddressesResult
    {
    }

    public class GetDepositAddressesResponse : ResponseBase
    {
        public GetDepositAddressesResult Result;
    }

    public class GetDepositStatusResult
    {
        /// <summary>
        /// Name of the deposit method used.
        /// </summary>
        public string Method;

        /// <summary>
        /// Asset class.
        /// </summary>
        public string Aclass;

        /// <summary>
        /// Asset X-ISO4217-A3 code.
        /// </summary>
        public string Asset;

        /// <summary>
        /// Reference id.
        /// </summary>
        public string RefId;

        /// <summary>
        /// Method transaction id.
        /// </summary>
        public string Txid;

        /// <summary>
        /// Method transaction information.
        /// </summary>
        public string Info;

        /// <summary>
        /// Amount deposited.
        /// </summary>
        public decimal Amount;

        /// <summary>
        /// Fees paid.
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Unix timestamp when request was made.
        /// </summary>
        public int Time;

        /// <summary>
        /// status of deposit
        /// </summary>
        public string Status;

        // status-prop = additional status properties(if available)
        //    return = a return transaction initiated by Kraken
        //    onhold = deposit is on hold pending review
    }

    public class GetDepositStatusResponse : ResponseBase
    {
        public GetDepositStatusResult[] Result;
    }

    public class GetWithdrawInfoResult
    {
        /// <summary>
        /// Name of the withdrawal method that will be used
        /// </summary>
        public string Method;

        /// <summary>
        /// Maximum net amount that can be withdrawn right now.
        /// </summary>
        public decimal Limit;

        /// <summary>
        /// Amount of fees that will be paid.
        /// </summary>
        public decimal Fee;
    }

    public class GetWithdrawInfoResponse : ResponseBase
    {
        public GetWithdrawInfoResult Result;
    }

    public class WithdrawResult
    {
        public string RefId;
    }

    public class WithdrawResponse : ResponseBase
    {
        public WithdrawResult Result;
    }

    public class GetWithdrawStatusResult
    {
        /// <summary>
        /// Name of the withdrawal method used.
        /// </summary>
        public string Method;

        /// <summary>
        /// Asset class.
        /// </summary>
        public string Aclass;

        /// <summary>
        /// Asset X-ISO4217-A3 code.
        /// </summary>
        public string Asset;

        /// <summary>
        /// Reference id.
        /// </summary>
        public string RefId;

        /// <summary>
        /// Method transaction id.
        /// </summary>
        public string Txid;

        /// <summary>
        /// Method transaction information.
        /// </summary>
        public string Info;

        /// <summary>
        /// Amount withdrawn.
        /// </summary>
        public decimal Amount;

        /// <summary>
        /// Fees paid.
        /// </summary>
        public decimal Fee;

        /// <summary>
        /// Unix timestamp when request was made.
        /// </summary>
        public int Time;

        /// <summary>
        /// Status of withdrawal.
        /// </summary>
        public string Status;

        //status-prop = additional status properties(if available).
        //cancel-pending = cancelation requested.
        //canceled = canceled.
        //cancel-denied = cancelation requested but was denied.
        //return = a return transaction initiated by Kraken; it cannot be canceled.
        //onhold = withdrawal is on hold pending review.
    }

    public class GetWithdrawStatusResponse : ResponseBase
    {
        public GetWithdrawStatusResult Result;
    }

    public class WithdrawCancelResponse : ResponseBase
    {
        public bool Result;
    }


} // end of namespace

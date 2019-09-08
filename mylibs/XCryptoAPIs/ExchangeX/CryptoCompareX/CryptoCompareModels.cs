using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CryptoAPIs.ExchangeX.CryptoCompareX
{

    public class AggregatedData
    {
        [JsonProperty("FLAGS")]
        public string Flags { get; set; }
        [JsonProperty("FROMSYMBOL")]
        public string FromSymbol { get; set; }
        [JsonProperty("HIGH24HOUR")]
        public double? High24Hour { get; set; }
        [JsonProperty("LASTTRADEID")]
        public string LastTradeId { get; set; }
        [JsonConverter(typeof(UnixTimeConverter))]
        [JsonProperty("LASTUPDATE")]
        public DateTimeOffset LastUpdate { get; set; }
        [JsonProperty("LASTVOLUME")]
        public double? LastVolume { get; set; }
        [JsonProperty("LASTVOLUMETO")]
        public double? LastVolumeTo { get; set; }
        [JsonProperty("LOW24HOUR")]
        public double? Low24Hour { get; set; }
        [JsonProperty("MARKET")]
        public string Market { get; set; }
        [JsonProperty("OPEN24HOUR")]
        public double? Open24Hour { get; set; }
        [JsonProperty("PRICE")]
        public double? Price { get; set; }
        [JsonProperty("TOSYMBOL")]
        public string ToSymbol { get; set; }
        [JsonProperty("TYPE")]
        public string Type { get; set; }
        [JsonProperty("VOLUME24HOUR")]
        public double? Volume24Hour { get; set; }
        [JsonProperty("VOLUME24HOURTO")]
        public double? Volume24HourTo { get; set; }
    }

    /// <summary>
    /// A base API response.
    /// CryptoCompare don't use status code for errors. They use a status reported into "Response" property
    /// </summary>
    public class BaseApiResponse
    {
        /// <summary>
        /// Gets or sets the errors summary.
        /// </summary>
        public string ErrorsSummary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a successful response.
        /// </summary>
        /// <value>
        /// True if this is a successful response, false if not.
        /// </value>
        public bool IsSuccessfulResponse => !string.Equals(
                                                this.Status,
                                                Constants.ResponseErrorStatus,
                                                StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the full pathname of the resource called.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the response status.
        /// </summary>
        [JsonProperty("Response")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty("Message")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the type of the status.
        /// </summary>
        [JsonProperty("Type")]
        public int StatusType { get; set; }
    }

    public enum CalculationType
    {
        /// <summary>
        /// The day close price.
        /// </summary>
        Close,

        /// <summary>
        /// The average between the 24 H high and low.
        /// </summary>
        MidHighLow,

        /// <summary>
        /// The total volume to / the total volume from.
        /// </summary>
        VolFVolT
    }

    public class Calls
    {
        /// <summary>
        /// Calls to history apis.
        /// </summary>
        public int Histo { get; set; }

        /// <summary>
        /// Calls to news api.
        /// </summary>
        public int News { get; set; }

        /// <summary>
        /// Calls to price apis.
        /// </summary>
        public int Price { get; set; }
    }

    public class CandleData
    {
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset Time { get; set; }
        public decimal VolumeFrom { get; set; }
        public decimal VolumeTo { get; set; }
    }

    public class CoinAggregatedData : AggregatedData
    {
        [JsonProperty("HIGHDAY")]
        public double? HighDay { get; set; }
        [JsonProperty("LASTMARKET")]
        public string LastMarket { get; set; }
        [JsonProperty("LOWDAY")]
        public double? LowDay { get; set; }
        [JsonProperty("OPENDAY")]
        public double? OpenDay { get; set; }
        [JsonProperty("VOLUMEDAY")]
        public double? VolumeDay { get; set; }
        [JsonProperty("VOLUMEDAYTO")]
        public double? VolumeDayTo { get; set; }
    }

    public class CoinFullAggregatedData : CoinAggregatedData
    {
        [JsonProperty("CHANGE24HOUR")]
        public decimal? Change24Hour { get; set; }
        [JsonProperty("CHANGEDAY")]
        public decimal? ChangeDay { get; set; }
        [JsonProperty("CHANGEPCT24HOUR")]
        public decimal? ChangePCT24Hour { get; set; }
        [JsonProperty("CHANGEPCTDAY")]
        public decimal? ChangePCTDay { get; set; }
        [JsonProperty("MKTCAP")]
        public decimal? MarketCap { get; set; }
        [JsonProperty("TOTALVOLUME24H")]
        public decimal? TotalVolume24H { get; set; }
        [JsonProperty("TOTALVOLUME24HTO")]
        public decimal? TotalVolume24HTo { get; set; }
    }

    public class CoinFullAggregatedDataDisplay
    {
        [JsonProperty("CHANGE24HOUR")]
        public string Change24Hour { get; set; }

        [JsonProperty("CHANGEDAY")]
        public string ChangeDay { get; set; }

        [JsonProperty("CHANGEPCT24HOUR")]
        public string ChangePCT24Hour { get; set; }

        [JsonProperty("CHANGEPCTDAY")]
        public string ChangePCTDay { get; set; }

        [JsonProperty("FROMSYMBOL")]
        public string FromSymbol { get; set; }

        [JsonProperty("HIGH24HOUR")]
        public string High24Hour { get; set; }

        [JsonProperty("HIGHDAY")]
        public string HighDay { get; set; }

        [JsonProperty("LASTMARKET")]
        public string LastMarket { get; set; }

        [JsonProperty("LASTTRADEID")]
        public string LastTradeId { get; set; }

        [JsonProperty("LASTUPDATE")]
        public string LastUpdate { get; set; }

        [JsonProperty("LASTVOLUME")]
        public string LastVolume { get; set; }

        [JsonProperty("LASTVOLUMETO")]
        public string LastVolumeTo { get; set; }

        [JsonProperty("LOW24HOUR")]
        public string Low24Hour { get; set; }

        [JsonProperty("LOWDAY")]
        public string LowDay { get; set; }

        [JsonProperty("MARKET")]
        public string Market { get; set; }

        [JsonProperty("MKTCAP")]
        public string MarketCap { get; set; }

        [JsonProperty("OPEN24HOUR")]
        public string Open24Hour { get; set; }

        [JsonProperty("OPENDAY")]
        public string OpenDay { get; set; }

        [JsonProperty("PRICE")]
        public string Price { get; set; }

        [JsonProperty("SUPPLY")]
        public string Supply { get; set; }

        [JsonProperty("TOSYMBOL")]
        public string ToSymbol { get; set; }

        [JsonProperty("TOTALVOLUME24H")]
        public string TotalVolume24H { get; set; }

        [JsonProperty("TOTALVOLUME24HTO")]
        public string TotalVolume24HTo { get; set; }

        [JsonProperty("VOLUME24HOUR")]
        public string Volume24Hour { get; set; }

        [JsonProperty("VOLUME24HOURTO")]
        public string Volume24HourTo { get; set; }

        [JsonProperty("VOLUMEDAY")]
        public string VolumeDay { get; set; }

        [JsonProperty("VOLUMEDAYTO")]
        public string VolumeDayTo { get; set; }
    }

    public class CoinGeneralInfo
    {
        public string AffiliateUrl { get; set; }

        public string Algorithm { get; set; }

        public string BaseAngularUrl { get; set; }

        public long? BlockNumber { get; set; }

        public long? BlockReward { get; set; }

        public string BlockRewardReduction { get; set; }

        public long? BlockTime { get; set; }

        public string DangerTop { get; set; }

        public string Description { get; set; }

        public string DifficultyAdjustment { get; set; }

        public string DocumentType { get; set; }

        public string Features { get; set; }

        public string H1Text { get; set; }

        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string InfoTop { get; set; }

        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset? LastBlockExplorerUpdateTS { get; set; }

        public string Name { get; set; }

        public double? NetHashesPerSecond { get; set; }

        public double? PreviousTotalCoinsMined { get; set; }

        public string ProofType { get; set; }

        [JsonConverter(typeof(IsoDateTimeWithFormatConverter), "dd/MM/yyyy")]
        public DateTime? StartDate { get; set; }

        public string Symbol { get; set; }

        public string Technology { get; set; }

        public double? TotalCoinsMined { get; set; }

        public string TotalCoinSupply { get; set; }

        public string Twitter { get; set; }

        public string Url { get; set; }

        public string WarningTop { get; set; }

        public string Website { get; set; }
    }

    public class CoinInfo
    {
        /// <summary>
        /// Gets or sets the algorithm of the coin.
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the name of the coin.
        /// </summary>
        public string CoinName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the coins.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the number of fully premined coins.
        /// </summary>
        /// <value>
        /// The fully premined.
        /// </value>
        public string FullyPremined { get; set; }

        /// <summary>
        /// Gets or sets the internal id, this is used in other calls.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets he logo image of the coin.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the coin name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pre-mined value.
        /// </summary>
        public string PreMinedValue { get; set; }

        /// <summary>
        /// Gets or sets the proof type.
        /// </summary>
        public string ProofType { get; set; }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the  coin is sponsored.
        /// </summary>
        public bool Sponsored { get; set; }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the total number of freed coins.
        /// </summary>
        public string TotalCoinsFreeFloat { get; set; }

        /// <summary>
        /// Gets or sets the total number of supplied coins.
        /// </summary>
        public string TotalCoinSupply { get; set; }

        /// <summary>
        /// Gets or sets the url of the coin on cryptocompare.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// List of coins.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Responses.BaseApiResponse"/>
    public class CoinListResponse : BaseApiResponse
    {
        public string BaseImageUrl { get; set; }

        public string BaseLinkUrl { get; set; }

        /// <summary>
        /// Gets or sets the coins data.
        /// </summary>
        [JsonProperty("Data")]
        public IReadOnlyDictionary<string, CoinInfo> Coins { get; set; }
    }

    public class CoinSnapshotData
    {
        public CoinAggregatedData AggregatedData { get; set; }

        public string Algorithm { get; set; }

        public long? BlockNumber { get; set; }

        public double? BlockReward { get; set; }

        public IReadOnlyList<AggregatedData> Exchanges { get; set; }

        public double? NetHashesPerSecond { get; set; }

        public string ProofType { get; set; }

        public long TotalCoinsMined { get; set; }
    }

    public class CoinSnapshotFullData
    {
        public CoinGeneralInfo General { get; set; }

        public ICO ICO { get; set; }

        public SEO SEO { get; set; }

        public IReadOnlyList<string> StreamerDataRaw { get; set; }

        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub> Subs { get; set; }
    }

    public class CoinSnapshotFullResponse : BaseApiResponse
    {
        public CoinSnapshotFullData Data { get; set; }
    }

    public class CoinSnapshotResponse : BaseApiResponse
    {
        public CoinSnapshotData Data { get; set; }
    }

    public class ExchangeListResponse : ReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>>
    {
        public ExchangeListResponse(IDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>> dictionary)
            : base(dictionary)
        {
        }
    }

    public class HistoryResponse : BaseApiResponse
    {
        public IReadOnlyList<CandleData> Data { get; set; }
    }

    public class ICO
    {
        public string Blog { get; set; }

        public string BlogLink { get; set; }

        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset Date { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset EndDate { get; set; }

        public string Features { get; set; }

        public string FundingCap { get; set; }

        public string FundingTarget { get; set; }

        public string FundsRaisedList { get; set; }

        public string FundsRaisedUSD { get; set; }

        public string ICOTokenSupply { get; set; }

        public string Jurisdiction { get; set; }

        public string LegalAdvisers { get; set; }

        public string LegalForm { get; set; }

        public string PaymentMethod { get; set; }

        public string PublicPortfolioId { get; set; }

        public string PublicPortfolioUrl { get; set; }

        public string SecurityAuditCompany { get; set; }

        public string StartPrice { get; set; }

        public string StartPriceCurrency { get; set; }

        public string Status { get; set; }

        public string TokenPercentageForInvestors { get; set; }

        public string TokenReserveSplit { get; set; }

        public string TokenSupplyPostICO { get; set; }

        public string TokenType { get; set; }

        public string Website { get; set; }

        public string WebsiteLink { get; set; }

        public string WhitePaper { get; set; }

        public string WhitePaperLink { get; set; }
    }

    public class NewsEntity
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("published_on")]
        public long PublishDateUnix { get; set; }
        [JsonProperty("imageurl")]
        public string ImageUrl { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("tags")]
        public string TagsString { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("source_info")]
        public NewsProvider SourceInfo { get; set; }

    }

    public class NewsProvider
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("img")]
        public string ImageUrl { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class PriceAverageResponse
    {
        [JsonProperty("DISPLAY")]
        public CoinFullAggregatedDataDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public CoinFullAggregatedData Raw { get; set; }
    }

    public class PriceHistoricalReponse : ReadOnlyDictionary<string, IReadOnlyDictionary<string, decimal>>
    {
        public PriceHistoricalReponse(IDictionary<string, IReadOnlyDictionary<string, decimal>> dictionary)
            : base(dictionary)
        {
        }
    }

    public class PriceMultiFullDisplay : ReadOnlyDictionary<string, IReadOnlyDictionary<string, CoinFullAggregatedDataDisplay>>
    {
        public PriceMultiFullDisplay(
            IDictionary<string, IReadOnlyDictionary<string, CoinFullAggregatedDataDisplay>> dictionary)
            : base(dictionary)
        {
        }
    }

    public class PriceMultiFullRaw : ReadOnlyDictionary<string, IReadOnlyDictionary<string, CoinFullAggregatedData>>
    {
        public PriceMultiFullRaw(IDictionary<string, IReadOnlyDictionary<string, CoinFullAggregatedData>> dictionary)
            : base(dictionary)
        {
        }
    }

    public class PriceMultiFullResponse
    {
        [JsonProperty("DISPLAY")]
        public PriceMultiFullDisplay Display { get; set; }

        [JsonProperty("RAW")]
        public PriceMultiFullRaw Raw { get; set; }
    }

    public class PriceMultiResponse : ReadOnlyDictionary<string, IReadOnlyDictionary<string, decimal>>
    {
        public PriceMultiResponse(IDictionary<string, IReadOnlyDictionary<string, decimal>> dictionary)
            : base(dictionary)
        {
        }
    }

    public class PriceSingleResponse : ReadOnlyDictionary<string, decimal>
    {
        public PriceSingleResponse(IDictionary<string, decimal> dictionary)
            : base(dictionary)
        {
        }
    }

    /// <summary>
    /// A rate limit.
    /// </summary>
    /// <seealso cref="T:CryptoCompare.Responses.BaseApiResponse"/>
    public class RateLimitResponse : BaseApiResponse
    {
        /// <summary>
        /// Gets or sets the calls left.
        /// </summary>
        public Calls CallsLeft { get; set; }

        /// <summary>
        /// Gets or sets the calls made.
        /// </summary>
        public Calls CallsMade { get; set; }
    }

    public class SEO
    {
        public string BaseImageUrl { get; set; }

        public string BaseUrl { get; set; }

        public int OgImageHeight { get; set; }

        public string OgImageUrl { get; set; }

        public int OgImageWidth { get; set; }

        public string PageDescription { get; set; }

        public string PageTitle { get; set; }
    }

    public class SocialStatsResponse
    {
        [JsonProperty("Response")]
        public string Response { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Type")]
        public long Type { get; set; }

        [JsonProperty("Data")]
        public SocialStats SocialStats { get; set; }
    }

    public class SocialStats
    {
        [JsonProperty("General")]
        public General General { get; set; }

        [JsonProperty("CryptoCompare")]
        public CryptoCompare CryptoCompare { get; set; }

        [JsonProperty("Twitter")]
        public Twitter Twitter { get; set; }

        [JsonProperty("Reddit")]
        public Reddit Reddit { get; set; }

        [JsonProperty("Facebook")]
        public Facebook Facebook { get; set; }

        [JsonProperty("CodeRepository")]
        public CodeRepository CodeRepository { get; set; }
    }

    public class CryptoCompare
    {
        [JsonProperty("SimilarItems")]
        public SimilarItem[] SimilarItems { get; set; }

        [JsonProperty("CryptopianFollowers")]
        public CryptopianFollower[] CryptopianFollowers { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }

        [JsonProperty("Followers")]
        public long Followers { get; set; }

        [JsonProperty("Posts")]
        public string Posts { get; set; }

        [JsonProperty("Comments")]
        public string Comments { get; set; }

        [JsonProperty("PageViewsSplit")]
        public PageViewsSplit PageViewsSplit { get; set; }

        [JsonProperty("PageViews")]
        public long PageViews { get; set; }
    }

    public class SimilarItem
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("FollowingType")]
        public long FollowingType { get; set; }
    }

    public class PageViewsSplit
    {
        [JsonProperty("Overview")]
        public long Overview { get; set; }

        [JsonProperty("Markets")]
        public long Markets { get; set; }

        [JsonProperty("Analysis")]
        public long Analysis { get; set; }

        [JsonProperty("Charts")]
        public long Charts { get; set; }

        [JsonProperty("Trades")]
        public long Trades { get; set; }

        [JsonProperty("Orderbook")]
        public long Orderbook { get; set; }

        [JsonProperty("Forum")]
        public long Forum { get; set; }

        [JsonProperty("Influence")]
        public long Influence { get; set; }
    }

    public class CryptopianFollower
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }

    public class General
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("CoinName")]
        public string CoinName { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }
    }
    public class Twitter
    {
        [JsonProperty("following")]
        public string Following { get; set; }

        [JsonProperty("account_creation")]
        public string AccountCreation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lists")]
        public long Lists { get; set; }

        [JsonProperty("statuses")]
        public long Statuses { get; set; }

        [JsonProperty("favourites")]
        public string Favourites { get; set; }

        [JsonProperty("followers")]
        public long Followers { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }
    }

    public class Reddit
    {
        [JsonProperty("posts_per_hour")]
        public string PostsPerHour { get; set; }

        [JsonProperty("comments_per_hour")]
        public string CommentsPerHour { get; set; }

        [JsonProperty("posts_per_day")]
        public string PostsPerDay { get; set; }

        [JsonProperty("comments_per_day")]
        public long CommentsPerDay { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("active_users")]
        public long ActiveUsers { get; set; }

        [JsonProperty("community_creation")]
        public string CommunityCreation { get; set; }

        [JsonProperty("subscribers")]
        public long Subscribers { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }
    }

    public class Facebook
    {
        [JsonProperty("likes")]
        public long Likes { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("is_closed")]
        public string IsClosed { get; set; }

        [JsonProperty("talking_about")]
        public string TalkingAbout { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }
    }

    public class CodeRepository
    {
        [JsonProperty("List")]
        public Repository[] Repositories { get; set; }

        [JsonProperty("Points")]
        public long Points { get; set; }
    }

    public class Parent
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("InternalId")]
        public long InternalId { get; set; }
    }

    public class Repository
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("open_total_issues")]
        public string OpenTotalIssues { get; set; }

        [JsonProperty("parent")]
        public Parent Parent { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("closed_total_issues")]
        public string ClosedTotalIssues { get; set; }

        [JsonProperty("stars")]
        public long Stars { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }

        [JsonProperty("forks")]
        public long Forks { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("closed_issues")]
        public string ClosedIssues { get; set; }

        [JsonProperty("closed_pull_issues")]
        public string ClosedPullIssues { get; set; }

        [JsonProperty("fork")]
        public string Fork { get; set; }

        [JsonProperty("last_push")]
        public string LastPush { get; set; }

        [JsonProperty("source")]
        public Parent Source { get; set; }

        [JsonProperty("open_pull_issues")]
        public string OpenPullIssues { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("subscribers")]
        public long Subscribers { get; set; }

        [JsonProperty("open_issues")]
        public string OpenIssues { get; set; }
    }

    public struct Sub : IEquatable<Sub>
    {
        public bool Equals(Sub other) => string.Equals(this.Exchange, other.Exchange)
                                            && string.Equals(this.FromSymbol, other.FromSymbol)
                                            && this.SubId == other.SubId
                                            && string.Equals(this.ToSymbol, other.ToSymbol);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Sub sub && this.Equals(sub);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Exchange.GetHashCode();
                hashCode = (hashCode * 397) ^ this.FromSymbol.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)this.SubId;
                hashCode = (hashCode * 397) ^ this.ToSymbol.GetHashCode();
                return hashCode;
            }
        }

        public Sub([NotNull] string exchange, [NotNull] string fromSymbol, SubId subId, [NotNull] string toSymbol)
        {
            Check.NotNullOrWhiteSpace(exchange, nameof(exchange));
            Check.NotNullOrWhiteSpace(fromSymbol, nameof(fromSymbol));
            Check.NotNullOrWhiteSpace(toSymbol, nameof(toSymbol));
            this.Exchange = exchange;
            this.FromSymbol = fromSymbol;
            this.SubId = subId;
            this.ToSymbol = toSymbol;
        }

        public string Exchange { get; }

        public string FromSymbol { get; }

        public SubId SubId { get; }

        public string ToSymbol { get; }

        public override string ToString()
        {
            return $"{this.SubId:D}~{this.Exchange}~{this.FromSymbol}~{this.ToSymbol}";
        }
    }

    public enum SubId
    {
        /// <summary>
        /// Trade level data on a currency pair from a specific exchange.
        /// </summary>
        Trade = 0,

        /// <summary>
        /// Latest quote update of a currency pair from a specific exchange.
        /// </summary>
        Current = 2,

        /// <summary>
        /// Quote update aggregated over the last 24 hours of a currency pair from a specific exchange.
        /// </summary>
        CurrentAgg = 5
    }

    public class SubListResponse : ReadOnlyDictionary<string, SubList>
    {
        public SubListResponse(IDictionary<string, SubList> dictionary)
            : base(dictionary)
        {
        }
    }

    public class SubList
    {
        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub> Current { get; set; }

        [JsonConverter(typeof(StringToSubConverter))]
        public Sub CurrentAgg { get; set; }

        [JsonConverter(typeof(StringToSubConverter))]
        public IReadOnlyList<Sub> Trades { get; set; }
    }

    public class TopInfo
    {
        public string Exchange { get; set; }

        public string FromSymbol { get; set; }

        public string ToSymbol { get; set; }

        public decimal Volume24H { get; set; }

        public decimal Volume24HTo { get; set; }
    }

    public class TopResponse : BaseApiResponse
    {
        public IReadOnlyList<TopInfo> Data { get; set; }
    }

    public class TopVolumeInfo
    {
        public string Fullname { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Supply { get; set; }

        public string Symbol { get; set; }

        public decimal Volume24HourTo { get; set; }
    }

    public class TopVolumesResponse : BaseApiResponse
    {
        public IReadOnlyList<TopVolumeInfo> Data { get; set; }

        public string VolSymbol { get; set; }
    }













} // end of namespace

#include "bitorder.h"

#include "appmarketaccount.h"

namespace qyvlik {

class BitOrderPrivate : public QSharedData
{
public:
    BitOrderPrivate()
        : appMarketAccountId(-1)
        , market("")
        , currency("")
        , symbol("")
        , orderId("")
        , orderAmount(0.0)
        , orderPrice(0.0)
        , processAmount(0.0)
        , processPrice(0.0)
    {}

    BitOrderPrivate(const BitOrderPrivate& other)
        : QSharedData(other)
    {}

    qint64 appMarketAccountId;
    QString market;
    QString currency;                               // usd, cny, btc
    QString symbol;                                 // trade symbol, btc, ltc, eth, etc ...
    QString orderId;                                // in market
    qreal orderAmount;
    qreal orderPrice;
    qreal fee;
    qreal feeAmount;
    qreal processAmount;
    qreal processPrice;
    qreal processFeeAmount;
    BitOrder::OrderType orderType;
    qint64 cancelTime;
    qint64 finishedTime;
    BitOrder::OrderState state;
};

const QString BitOrder::ERROR_ORDER_ID = "-1";

BitOrder::BitOrder()
    : BaseEntity()
    , d_ptr(new BitOrderPrivate)
{
}

BitOrder::BitOrder(const BitOrder &other)
    : BaseEntity(other)
    , d_ptr(other.d_ptr)
{

}

BitOrder &BitOrder::operator =(const BitOrder &other) {
    d_ptr = other.d_ptr;
    return *this;
}

BitOrder::~BitOrder(){}

BitOrder::OrderType BitOrder::getOrderType() const
{
    return d_ptr->orderType;
}

void BitOrder::setOrderType(const BitOrder::OrderType &value)
{
    d_ptr->orderType = value;
}

qreal BitOrder::getProcessFeeAmount() const
{
    return d_ptr->processFeeAmount;
}

void BitOrder::setProcessFeeAmount(const qreal &value)
{
    d_ptr->processFeeAmount = value;
}

qint64 BitOrder::getCancelTime() const
{
    return d_ptr->cancelTime;
}

void BitOrder::setCancelTime(const qint64 &value)
{
    d_ptr->cancelTime = value;
}

qint64 BitOrder::getFinishedTime() const
{
    return d_ptr->finishedTime;
}

void BitOrder::setFinishedTime(const qint64 &value)
{
    d_ptr->finishedTime = value;
}

BitOrder::OrderState BitOrder::getState() const
{
    return d_ptr->state;
}

void BitOrder::setState(const BitOrder::OrderState &value)
{
    d_ptr->state = value;
}

qreal BitOrder::getFeeAmount() const
{
    return d_ptr->feeAmount;
}

void BitOrder::setFeeAmount(const qreal &value)
{
    d_ptr->feeAmount = value;
}

qreal BitOrder::getFee() const
{
    return d_ptr->fee;
}

void BitOrder::setFee(const qreal &value)
{
    d_ptr->fee = value;
}

qreal BitOrder::getProcessPrice() const
{
    return d_ptr->processPrice;
}

void BitOrder::setProcessPrice(const qreal &value)
{
    d_ptr->processPrice = value;
}

qreal BitOrder::getProcessAmount() const
{
    return d_ptr->processAmount;
}

void BitOrder::setProcessAmount(const qreal &value)
{
    d_ptr->processAmount = value;
}

qreal BitOrder::getOrderPrice() const
{
    return d_ptr->orderPrice;
}

void BitOrder::setOrderPrice(const qreal &value)
{
    d_ptr->orderPrice = value;
}

qreal BitOrder::getOrderAmount() const
{
    return d_ptr->orderAmount;
}

void BitOrder::setOrderAmount(const qreal &value)
{
    d_ptr->orderAmount = value;
}

QString BitOrder::getOrderId() const
{
    return d_ptr->orderId;
}

void BitOrder::setOrderId(const QString &value)
{
    d_ptr->orderId = value;
}

QString BitOrder::getSymbol() const
{
    return d_ptr->symbol;
}

void BitOrder::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

QString BitOrder::getCurrency() const
{
    return d_ptr->currency;
}

void BitOrder::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

QString BitOrder::getMarket() const
{
    return d_ptr->market;
}

void BitOrder::setMarket(const QString &value)
{
    d_ptr->market = value;
}

qint64 BitOrder::getAppMarketAccountId() const
{
    return d_ptr->appMarketAccountId;
}

void BitOrder::setAppMarketAccountId(const qint64 &value)
{
    d_ptr->appMarketAccountId = value;
}

///---------------TradeParam------------------------


class TradeParamPrivate : public QSharedData
{
public :
    TradeParamPrivate()
        : QSharedData()
    {}

    QString orderId;                        // orderId in market
    AppMarketAccount appMarketAccount;
    qreal amount;
    qreal price;
    QString currency;
    QString symbol;
    BitOrder::OrderType orderType;
    QString contractType;                   // not support
    BitOrder::OrderSide orderSide;
};

TradeParam::TradeParam()
    : d_ptr(new TradeParamPrivate)
{}

TradeParam::TradeParam(const TradeParam &other)
    : d_ptr(other.d_ptr)
{}

TradeParam &TradeParam::operator=(const TradeParam &other) {
    d_ptr = other.d_ptr;
    return *this;
}

TradeParam::~TradeParam()
{}

qreal TradeParam::getAmount() const
{
    return d_ptr->amount;
}

void TradeParam::setAmount(const qreal &value)
{
    d_ptr->amount = value;
}

qreal TradeParam::getPrice() const
{
    return d_ptr->price;
}

void TradeParam::setPrice(const qreal &value)
{
    d_ptr->price = value;
}

QString TradeParam::getSymbol() const
{
    return d_ptr->symbol;
}

void TradeParam::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

QString TradeParam::getCurrency() const
{
    return d_ptr->currency;
}

void TradeParam::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

BitOrder::OrderType TradeParam::getOrderType() const
{
    return d_ptr->orderType;
}

void TradeParam::setOrderType(const BitOrder::OrderType &value)
{
    d_ptr->orderType = value;
}

QString TradeParam::getContractType() const
{
    return d_ptr->contractType;
}

void TradeParam::setContractType(const QString &value)
{
    d_ptr->contractType = value;
}

BitOrder::OrderSide TradeParam::getOrderSide() const
{
    return d_ptr->orderSide;
}

void TradeParam::setOrderSide(const BitOrder::OrderSide &value)
{
    d_ptr->orderSide = value;
}

QString TradeParam::getOrderId() const
{
    return d_ptr->orderId;
}

void TradeParam::setOrderId(const QString &value)
{
    d_ptr->orderId = value;
}

AppMarketAccount TradeParam::getAppMarketAccount() const
{
    return d_ptr->appMarketAccount;
}

void TradeParam::setAppMarketAccount(const AppMarketAccount &value)
{
    d_ptr->appMarketAccount = value;
}

} // namespace qyvlik

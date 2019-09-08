#include "ticker.h"

namespace qyvlik {

class TickerPrivate : public QSharedData
{
public:
    TickerPrivate()
        : QSharedData()
        , market("")
        , symbol("")
        , currency("")
        , amount(0.0)
        , count(0)
        , open(0.0)
        , close(0.0)
        , low(0.0)
        , high(0.0)
        , vol(0.0)
        , bidPrice(0.0)
        , bidAmount(0.0)
        , askPrice(0.0)
        , askAmount(0.0)
    {}

    TickerPrivate(const TickerPrivate& other)
        : QSharedData(other)
        , market(other.market)
        , symbol(other.symbol)
        , currency(other.currency)
        , amount(other.amount)
        , count(other.count)
        , open(other.open)
        , close(other.close)
        , low(other.low)
        , high(other.high)
        , vol(other.vol)
        , bidPrice(other.bidPrice)
        , bidAmount(other.bidAmount)
        , askPrice(other.askPrice)
        , askAmount(other.askAmount)
    {}

    QString market;
    QString symbol;
    QString currency;
    qreal amount;
    qint32 count;
    qreal open;
    qreal close;
    qreal low;
    qreal high;
    qreal vol;
    qreal bidPrice;                         // price unit is currency
    qreal bidAmount;
    qreal askPrice;                         // price unit is currency
    qreal askAmount;
    qint64 timestamp;
};

Ticker::Ticker()
    : d_ptr(new TickerPrivate)
{

}

Ticker::Ticker(const Ticker &other)
    : d_ptr(other.d_ptr)
{}

Ticker &Ticker::operator=(const Ticker &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

Ticker::~Ticker()
{}

qreal Ticker::getAskAmount() const
{
    return d_ptr->askAmount;
}

void Ticker::setAskAmount(const qreal &value)
{
    d_ptr->askAmount = value;
}

qint64 Ticker::getTimestamp() const
{
    return d_ptr->timestamp;
}

void Ticker::setTimestamp(const qint64 &value)
{
    d_ptr->timestamp = value;
}

qreal Ticker::getAskPrice() const
{
    return d_ptr->askPrice;
}

void Ticker::setAskPrice(const qreal &value)
{
    d_ptr->askPrice = value;
}

qreal Ticker::getBidAmount() const
{
    return d_ptr->bidAmount;
}

void Ticker::setBidAmount(const qreal &value)
{
    d_ptr->bidAmount = value;
}

qreal Ticker::getBidPrice() const
{
    return d_ptr->bidPrice;
}

void Ticker::setBidPrice(const qreal &value)
{
    d_ptr->bidPrice = value;
}

qreal Ticker::getVol() const
{
    return d_ptr->vol;
}

void Ticker::setVol(const qreal &value)
{
    d_ptr->vol = value;
}

qreal Ticker::getHigh() const
{
    return d_ptr->high;
}

void Ticker::setHigh(const qreal &value)
{
    d_ptr->high = value;
}

qreal Ticker::getLow() const
{
    return d_ptr->low;
}

void Ticker::setLow(const qreal &value)
{
    d_ptr->low = value;
}

qreal Ticker::getClose() const
{
    return d_ptr->close;
}

void Ticker::setClose(const qreal &value)
{
    d_ptr->close = value;
}

qreal Ticker::getOpen() const
{
    return d_ptr->open;
}

void Ticker::setOpen(const qreal &value)
{
    d_ptr->open = value;
}

qint32 Ticker::getCount() const
{
    return d_ptr->count;
}

void Ticker::setCount(const qint32 &value)
{
    d_ptr->count = value;
}

qreal Ticker::getAmount() const
{
    return d_ptr->amount;
}

void Ticker::setAmount(const qreal &value)
{
    d_ptr->amount = value;
}

QString Ticker::getCurrency() const
{
    return d_ptr->currency;
}

void Ticker::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

QString Ticker::getSymbol() const
{
    return d_ptr->symbol;
}

void Ticker::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

QString Ticker::getMarket() const
{
    return d_ptr->market;
}

void Ticker::setMarket(const QString &value)
{
    d_ptr->market = value;
}

} // namespace qyvlik

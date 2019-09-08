#include "kline.h"

namespace qyvlik {

class KlinePrivate : public QSharedData
{
public:
    KlinePrivate()
        : QSharedData()
    {}
    KlinePrivate(const KlinePrivate& other)
        : QSharedData(other)
        , market(other.market)
        , symbol(other.symbol)
        , timestamp(other.timestamp)
        , open(other.open)
        , close(other.close)
        , high(other.high)
        , low(other.low)
        , vol(other.vol)
        , vwap(other.vwap)
    {}

    QString market;
    QString symbol;
    QString currency;
    qint64 timestamp;
    qreal amount;                                       // 成交量
    qint32 count;                                       // 成交笔数
    qreal open;
    qreal close;
    qreal high;
    qreal low;
    qreal vol;
    qreal vwap;
};

Kline::Kline()
    : d_ptr(new KlinePrivate)
{

}

Kline::Kline(const Kline &other)
    : d_ptr(other.d_ptr)
{

}

Kline &Kline::operator=(const Kline &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

Kline::~Kline()
{

}

QString Kline::getMarket() const
{
    return d_ptr->market;
}

void Kline::setMarket(const QString &value)
{
    d_ptr->market = value;
}

qint32 Kline::getCount() const
{
    return  d_ptr->count;
}

void Kline::setCount(const qint32 &value)
{
    d_ptr->count = value;
}

QString Kline::getCurrency() const
{
    return d_ptr->currency;
}

void Kline::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

qreal Kline::getAmount() const
{
    return  d_ptr->amount;
}

void Kline::setAmount(const qreal &value)
{
    d_ptr->amount = value;
}

QString Kline::getSymbol() const
{
    return d_ptr->symbol;
}

void Kline::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

qint64 Kline::getTimestamp() const
{
    return d_ptr->timestamp;
}

void Kline::setTimestamp(const qint64 &value)
{
    d_ptr->timestamp = value;
}

qreal Kline::getOpen() const
{
    return d_ptr->open;
}

void Kline::setOpen(const qreal &value)
{
    d_ptr->open = value;
}

qreal Kline::getClose() const
{
    return d_ptr->close;
}

void Kline::setClose(const qreal &value)
{
    d_ptr->close = value;
}

qreal Kline::getHigh() const
{
    return d_ptr->high;
}

void Kline::setHigh(const qreal &value)
{
    d_ptr->high = value;
}

qreal Kline::getLow() const
{
    return d_ptr->low;
}

void Kline::setLow(const qreal &value)
{
    d_ptr->low = value;
}

qreal Kline::getVol() const
{
    return d_ptr->vol;
}

void Kline::setVol(const qreal &value)
{
    d_ptr->vol = value;
}

qreal Kline::getVwap() const
{
    return d_ptr->vwap;
}

void Kline::setVwap(const qreal &value)
{
    d_ptr->vwap = value;
}

} // namespace qyvlik

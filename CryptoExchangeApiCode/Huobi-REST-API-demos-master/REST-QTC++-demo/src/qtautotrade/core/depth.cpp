#include "depth.h"

namespace qyvlik {

class DepthPrivate : public QSharedData
{
public:
    DepthPrivate()
        : QSharedData()
    {}

    DepthPrivate(const DepthPrivate& other)
        : QSharedData(other)
        , symbol(other.symbol)
        , market(other.market)
        , currency(other.currency)
        , timestamp(other.timestamp)
        , bids(other.bids)
        , asks(other.asks)
    {}

    QString symbol;
    QString market;
    QString currency;
    qint64 timestamp;
    DepthList bids;                                   // [price, amount]
    DepthList asks;                                   // [price, amount]
};


bool depthItem4Bid(const DepthItem &a1, const DepthItem &a2)
{
    return a1.first > a2.first;
}

bool depthItem4Ask(const DepthItem &a1, const DepthItem &a2)
{
    return a1.first < a2.first;
}

Depth::Depth()
    : d_ptr(new DepthPrivate)
{

}

Depth::Depth(const Depth &other)
    : d_ptr(other.d_ptr)
{}

Depth &Depth::operator=(const Depth &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

Depth::~Depth()
{}

DepthList Depth::getAsks() const
{
    return d_ptr->asks;
}

void Depth::setAsks(const DepthList &value)
{
    d_ptr->asks = value;
     std::sort(d_ptr->asks.begin(), d_ptr->asks.end(), depthItem4Ask);
}

QString Depth::getMarket() const
{
    return d_ptr->market;
}

void Depth::setMarket(const QString &value)
{
    d_ptr->market = value;
}

QString Depth::getCurrency() const
{
    return d_ptr->currency;
}

void Depth::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

bool Depth::isValidate() const
{
    return !this->getAsks().isEmpty() && !this->getBids().isEmpty();
}

QString Depth::getName() const
{
    return this->getMarket() + "-" + this->getCurrency() + "-" + this->getSymbol();
}

QString Depth::getSymbol() const
{
    return d_ptr->symbol;
}

void Depth::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

DepthList Depth::getBids() const
{
    return d_ptr->bids;
}

void Depth::setBids(const DepthList &value)
{
    d_ptr->bids = value;
     std::sort(d_ptr->bids.begin(), d_ptr->bids.end(), depthItem4Bid);
}

qint64 Depth::getTimestamp() const
{
    return d_ptr->timestamp;
}

void Depth::setTimestamp(const qint64 &value)
{
    d_ptr->timestamp = value;
}

} // namespace qyvlik

#include "marketparam.h"

namespace qyvlik {

class MarketParamPrivate : public QSharedData
{
public:
    MarketParamPrivate()
        : QSharedData()
        , symbol("")
        , period(MarketParam::PeriodType::OneMin)
        , depth(MarketParam::DepthType::Step0)
        , size(1)
    {}

    MarketParamPrivate(const MarketParamPrivate& other)
        : QSharedData(other)
        , symbol(other.symbol)
        , period(other.period)
        , depth(other.depth)
        , size(other.size)
    {}

    QString symbol;
    QString currency;
    MarketParam::PeriodType period;
    MarketParam::DepthType depth;
    qint32 size;
};

MarketParam::MarketParam()
    : d_ptr(new MarketParamPrivate)
{}

MarketParam::MarketParam(const MarketParam &other)
    : d_ptr(other.d_ptr)
{}

MarketParam &MarketParam::operator=(const MarketParam &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

MarketParam::~MarketParam()
{}

qint32 MarketParam::getSize() const
{
    return d_ptr->size;
}

void MarketParam::setSize(const qint32 &value)
{
    d_ptr->size = value;
}

QString MarketParam::getCurrency() const
{
    return d_ptr->currency;
}

void MarketParam::setCurrency(const QString &value)
{
    d_ptr->currency = value;
}

MarketParam::DepthType MarketParam::getDepth() const
{
    return d_ptr->depth;
}

void MarketParam::setDepth(const MarketParam::DepthType &value)
{
    d_ptr->depth = value;
}

MarketParam::PeriodType MarketParam::getPeriod() const
{
    return d_ptr->period;
}

void MarketParam::setPeriod(const MarketParam::PeriodType &value)
{
    d_ptr->period = value;
}

QString MarketParam::getSymbol() const
{
    return d_ptr->symbol;
}

void MarketParam::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

} // namespace qyvlik

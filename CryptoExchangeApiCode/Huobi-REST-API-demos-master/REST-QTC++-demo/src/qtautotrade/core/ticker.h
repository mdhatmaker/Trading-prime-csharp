#ifndef TICKER_H
#define TICKER_H

#include <QSharedData>
#include <QObject>

namespace qyvlik {

class TickerPrivate;
class Ticker
{
    Q_GADGET
public:
    Ticker();
    Ticker(const Ticker& other);
    Ticker& operator=(const Ticker& other);
    ~Ticker();

    QString getMarket() const;
    void setMarket(const QString &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getCurrency() const;
    void setCurrency(const QString &value);

    qreal getAmount() const;
    void setAmount(const qreal &value);

    qint32 getCount() const;
    void setCount(const qint32 &value);

    qreal getOpen() const;
    void setOpen(const qreal &value);

    qreal getClose() const;
    void setClose(const qreal &value);

    qreal getLow() const;
    void setLow(const qreal &value);

    qreal getHigh() const;
    void setHigh(const qreal &value);

    qreal getVol() const;
    void setVol(const qreal &value);

    qreal getBidPrice() const;
    void setBidPrice(const qreal &value);

    qreal getBidAmount() const;
    void setBidAmount(const qreal &value);

    qreal getAskPrice() const;
    void setAskPrice(const qreal &value);

    qreal getAskAmount() const;
    void setAskAmount(const qreal &value);

    qint64 getTimestamp() const;
    void setTimestamp(const qint64 &value);

private:
    QSharedDataPointer<TickerPrivate> d_ptr;
};

} // namespace qyvlik

#endif // TICKER_H

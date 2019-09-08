#ifndef KLINE_H
#define KLINE_H

#include <QObject>
#include <QSharedData>

namespace qyvlik {

class KlinePrivate;
class Kline
{
    Q_GADGET
public:
    Kline();
    Kline(const Kline& other);
    Kline& operator=(const Kline& other);
    ~Kline();

    qreal getVwap() const;
    void setVwap(const qreal &value);

    qreal getVol() const;
    void setVol(const qreal &value);

    qreal getLow() const;
    void setLow(const qreal &value);

    qreal getHigh() const;
    void setHigh(const qreal &value);

    qreal getClose() const;
    void setClose(const qreal &value);

    qreal getOpen() const;
    void setOpen(const qreal &value);

    qint64 getTimestamp() const;
    void setTimestamp(const qint64 &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getMarket() const;
    void setMarket(const QString &value);

    qreal getAmount() const;
    void setAmount(const qreal &value);

    qint32 getCount() const;
    void setCount(const qint32 &value);

    QString getCurrency() const;
    void setCurrency(const QString &value);

private:
    QSharedDataPointer<KlinePrivate> d_ptr;
};

} // namespace qyvlik

#endif // KLINE_H

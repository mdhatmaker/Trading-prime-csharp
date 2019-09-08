#ifndef DEPTH_H
#define DEPTH_H

#include <QSharedData>
#include <QObject>

namespace qyvlik {

typedef QPair<qreal, qreal> DepthItem;                      // [price, amount]
typedef QList<DepthItem> DepthList;

class DepthPrivate;
class Depth
{
    Q_GADGET
public:
    Depth();
    Depth(const Depth& other);
    Depth& operator=(const Depth& other);
    ~Depth();

    qint64 getTimestamp() const;
    void setTimestamp(const qint64 &value);

    DepthList getBids() const;
    void setBids(const DepthList &value);

    DepthList getAsks() const;
    void setAsks(const DepthList &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getMarket() const;
    void setMarket(const QString &value);

    QString getCurrency() const;
    void setCurrency(const QString &value);

    bool isValidate() const;

    QString getName() const;

private:
    QSharedDataPointer<DepthPrivate> d_ptr;
};

} // namespace qyvlik

#endif // DEPTH_H

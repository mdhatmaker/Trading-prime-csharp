#ifndef MARKETAPIFACTORY_H
#define MARKETAPIFACTORY_H

#include <QObject>
#include <QMap>

#include "abstractmarketapi.h"

namespace qyvlik {

class MarketApiFactoryPrivate;
class MarketApiFactory : public QObject
{
    Q_OBJECT
protected:
    explicit MarketApiFactory(QObject *parent = 0);
public:
    ~MarketApiFactory();
    static MarketApiFactory* singleton();

    AbstractMarketApi* get(const QString& marketName);

    QMap<QString, AbstractMarketApi*> getMarketApiMap() const;

    void put(const QString& marketName, AbstractMarketApi* marketApi);

    QNetworkAccessManager* getManager() const;

    void initMarketApiMap();

signals:

public slots:
private:
    friend class MarketApiFactoryPrivate;
    MarketApiFactoryPrivate* d;
};

} // namespace qyvlik

#endif // MARKETAPIFACTORY_H

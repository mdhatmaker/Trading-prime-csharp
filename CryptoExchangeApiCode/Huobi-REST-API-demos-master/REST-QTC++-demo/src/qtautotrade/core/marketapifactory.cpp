#include "marketapifactory.h"

#include <QCoreApplication>
#include <QReadWriteLock>
#include <QNetworkAccessManager>

#include "../market/huobi/huobproapi.h"

namespace qyvlik {

class MarketApiFactoryPrivate
{
public:
    QReadWriteLock lock;
    QMap<QString, AbstractMarketApi*> map;
    QNetworkAccessManager* manager = Q_NULLPTR;

    template<typename T>
    void createMarketApiAndAdd(MarketApiFactory* parent) {
        AbstractMarketApi* api = new T(parent);
        api->setNetworkAccessManager(parent->d->manager);
        QPair<QString, AbstractMarketApi*> pair(api->market(), api);
        parent->d->map.insert(pair.first, pair.second);
    }
};

MarketApiFactory::MarketApiFactory(QObject *parent)
    : QObject(parent)
    , d(new MarketApiFactoryPrivate)
{
    // qDebug() << Q_FUNC_INFO;

    d->manager = new QNetworkAccessManager(this);

    this->initMarketApiMap();
}

MarketApiFactory::~MarketApiFactory()
{
    delete d;
}

MarketApiFactory *MarketApiFactory::singleton() {
    static MarketApiFactory* factory = new MarketApiFactory(QCoreApplication::instance());
    return factory;
}

AbstractMarketApi *MarketApiFactory::get(const QString &marketName)
{
    QReadLocker locker(&d->lock);
    auto find = d->map.find(marketName);
    auto end = d->map.end();
    if (find != end) {
        return find.value();
    }
    return Q_NULLPTR;
}

QMap<QString, AbstractMarketApi *> MarketApiFactory::getMarketApiMap() const
{
    QReadLocker locker(&d->lock);
    return d->map;
}

void MarketApiFactory::put(const QString &marketName, AbstractMarketApi *marketApi)
{
    QWriteLocker locker(&d->lock);
    d->map.insert(marketName, marketApi);
}

QNetworkAccessManager *MarketApiFactory::getManager() const
{
    return d->manager;
}

void MarketApiFactory::initMarketApiMap()
{
    QWriteLocker locker(&d->lock);
    d->createMarketApiAndAdd<HuoBiProApi>(this);
}

} // namespace qyvlik

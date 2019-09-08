#ifndef ABSTRACTMARKETAPI_H
#define ABSTRACTMARKETAPI_H

#include <QObject>
#include <QUrl>
#include <QUrlQuery>
#include <QtPromise>
#include <QNetworkAccessManager>
#include <QNetworkRequest>
#include <QNetworkReply>

#include "asset.h"
#include "appmarketaccount.h"
#include "bitorder.h"
#include "marketparam.h"
#include "kline.h"
#include "depth.h"
#include "ticker.h"

namespace qyvlik {

class Asset;
class AppMarketAsset;
class AppMarketAccount;
class BitOrder;
class TradeParam;
class MarketParam;
class Kline;
class Ticker;
class Depth;

class AbstractMarketApiPrivate;
class AbstractMarketApi : public QObject
{
    Q_OBJECT
public:

    static QString cacheKeyName(AbstractMarketApi* api, const QString& currency, const QString& symbol);

    explicit AbstractMarketApi(QObject *parent = 0);

    ~AbstractMarketApi();

    virtual QString currency() const = 0;

    virtual QList<QString> supportSymbolList() const = 0;

    virtual QString market() const = 0;

    virtual QUrl host() const = 0;

    void setNetworkAccessManager(QNetworkAccessManager* manager);

    QNetworkAccessManager* getNetworkAccessManager() const;

    virtual QtPromise::QPromise<AppMarketAsset> assets(const AppMarketAccount& appMarketAccount) = 0;

    virtual QtPromise::QPromise<BitOrder> trade(const TradeParam& tradeParam) = 0;

    virtual QtPromise::QPromise<BitOrder> cancel(TradeParam& tradeParam) = 0;

    virtual QtPromise::QPromise<BitOrder> order(TradeParam& tradeParam) = 0;

    virtual QtPromise::QPromise<QList<Kline>> kline(const MarketParam& marketParam) = 0;

    virtual QtPromise::QPromise<Ticker> ticker(const MarketParam& marketParam) = 0;

    virtual QtPromise::QPromise<Depth> depth(const MarketParam& marketParam) = 0;

    virtual QByteArray sign(const AppMarketAccount& appMarketAccount, const QByteArray& info) = 0;

protected:
    QUrl getUrl(const QString &path);

signals:

public slots:

private:
    AbstractMarketApiPrivate* d_ptr;
};



} // namespace qyvlik

#endif // ABSTRACTMARKETAPI_H

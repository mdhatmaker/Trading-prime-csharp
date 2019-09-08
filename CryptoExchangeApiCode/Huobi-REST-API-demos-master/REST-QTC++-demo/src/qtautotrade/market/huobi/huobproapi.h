#ifndef HUOBIINNOVATIONCNYAPI_H
#define HUOBIINNOVATIONCNYAPI_H

#include <QUrlQuery>
#include <QNetworkAccessManager>
#include <QJsonValue>

#include "../../core/abstractmarketapi.h"

namespace qyvlik {

class HuoBiInnovationCnyApiResponsePrivate;
class HuoBiInnovationCnyApiResponse
{
    Q_GADGET
public:
    explicit HuoBiInnovationCnyApiResponse();

    HuoBiInnovationCnyApiResponse(const HuoBiInnovationCnyApiResponse& other);

    virtual ~HuoBiInnovationCnyApiResponse();

    QString getStatus() const;

    void setStatus(const QString &value);

    QJsonValue getData() const;

    void setData(const QJsonValue &value);

    QString getErrCode() const;

    void setErrCode(const QString &value);

    QString getErrMsg() const;

    void setErrMsg(const QString &value);

    QString getCh() const;
    void setCh(const QString &value);

    qint64 getTs() const;
    void setTs(const qint64 &value);

    QJsonValue getTick() const;
    void setTick(const QJsonValue &value);

private:
    QSharedDataPointer<HuoBiInnovationCnyApiResponsePrivate> d_ptr;
};

class HuoBiProApi : public AbstractMarketApi
{
    Q_OBJECT
public:
    HuoBiProApi(QObject *parent = 0);

    static QString orderTypeString(BitOrder::OrderType orderType);

    static BitOrder::OrderType toOrderType(const QString& orderType);

    static QString orderStateString(BitOrder::OrderState orderState);

    BitOrder::OrderState fromString2OrderState(const QString& orderState);

    static QString periodTypeString(MarketParam::PeriodType periodType);

    static QString depthTypeString(MarketParam::DepthType depthType);

    // AbstractMarketApi interface
public:
    QString currency() const override;

    QList<QString> supportSymbolList() const override;

    QString market() const override;

    QUrl host() const override;

    QtPromise::QPromise<HuoBiInnovationCnyApiResponse> accounts(const AppMarketAccount &appMarketAccount);

    QtPromise::QPromise<QString> accountId(const AppMarketAccount &appMarketAccount);

    QtPromise::QPromise<AppMarketAsset> assets(const AppMarketAccount &appMarketAccount) override;

    QtPromise::QPromise<BitOrder> cancel(TradeParam &tradeParam) override;

    QtPromise::QPromise<BitOrder> order(TradeParam &tradeParam) override;

    QtPromise::QPromise<QList<Kline>> kline(const MarketParam &marketParam) override;

    QtPromise::QPromise<Ticker> ticker(const MarketParam &marketParam) override;

    QtPromise::QPromise<Depth> depth(const MarketParam &marketParam) override;

    QByteArray sign(const AppMarketAccount& appMarketAccount, const QByteArray& info) override;

    QByteArray sign(const AppMarketAccount& appMarketAccount, QNetworkAccessManager::Operation op, const QUrl& url, const QUrlQuery& qurlQuery);

    QtPromise::QPromise<BitOrder> trade(const TradeParam &tradeParam) override;

protected:

    // 执行一个订单
    QtPromise::QPromise<BitOrder> place(const AppMarketAccount &appMarketAccount, const QString& orderId);

    QtPromise::QPromise<HuoBiInnovationCnyApiResponse> sendRequest(const AppMarketAccount& appMarketAccount, QNetworkAccessManager::Operation op, const QString& path, const QUrlQuery& postQuery, const QUrlQuery& signQuery);

    QtPromise::QPromise<HuoBiInnovationCnyApiResponse> sendRequestInternal(const QNetworkRequest& networkReuqest, const QByteArray& data,  QNetworkAccessManager::Operation op);

    AppMarketAsset convertAssetsFromData(const QJsonValue& data);

    BitOrder convertOrderFromData(const QJsonValue& data);

    QList<Kline> convertKlineListFromData(const QJsonValue& data, qint64 timestamp, const QString &symbol);

    Ticker convertTickerFromData(const QJsonValue& tick, const QString& symbol);

    Depth convertDepthFromData(const QJsonValue& tick, const QString& symbol);
};

} // namespace qyvlik

Q_DECLARE_METATYPE(qyvlik::HuoBiInnovationCnyApiResponse)


#endif // HUOBIINNOVATIONCNYAPI_H

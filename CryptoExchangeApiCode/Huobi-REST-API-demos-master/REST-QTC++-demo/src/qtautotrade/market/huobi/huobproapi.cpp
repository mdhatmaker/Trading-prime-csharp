#include "huobproapi.h"

#include <QMessageAuthenticationCode>
#include <QUrl>
#include <QList>
#include <QString>
#include <QNetworkReply>
#include <QDateTime>
#include <QJsonObject>
#include <QJsonArray>
#include <QJsonDocument>

#include "../../utils/httputils.h"

//! [huobi REST_authentication](https://github.com/huobiapi/API_Docs/wiki/REST_authentication)

using namespace QtPromise;

namespace qyvlik {

class HuoBiInnovationCnyApiResponsePrivate : public QSharedData
{
public:
    HuoBiInnovationCnyApiResponsePrivate()
    {}

    HuoBiInnovationCnyApiResponsePrivate(const HuoBiInnovationCnyApiResponsePrivate& other)
        : QSharedData(other)
        , status(other.status)
        , data(other.data)
    {}

    QString status;
    QJsonValue data;
    QString errCode;
    QString errMsg;
    QString ch;                             // channel
    qint64 ts;                              // timestamp
    QJsonValue tick;
};


HuoBiInnovationCnyApiResponse::HuoBiInnovationCnyApiResponse()
    : d_ptr(new HuoBiInnovationCnyApiResponsePrivate)
{}

HuoBiInnovationCnyApiResponse::HuoBiInnovationCnyApiResponse(const HuoBiInnovationCnyApiResponse &other):
    d_ptr(other.d_ptr)
{
}

HuoBiInnovationCnyApiResponse::~HuoBiInnovationCnyApiResponse()
{
}

QString HuoBiInnovationCnyApiResponse::getStatus() const
{
    return d_ptr->status;
}

void HuoBiInnovationCnyApiResponse::setStatus(const QString &value)
{
    d_ptr->status = value;
}

QJsonValue HuoBiInnovationCnyApiResponse::getData() const
{
    return d_ptr->data;
}

void HuoBiInnovationCnyApiResponse::setData(const QJsonValue &value)
{
    d_ptr->data = value;
}

QString HuoBiInnovationCnyApiResponse::getErrMsg() const
{
    return d_ptr->errMsg;
}

void HuoBiInnovationCnyApiResponse::setErrMsg(const QString &value)
{
    d_ptr->errMsg = value;
}

qint64 HuoBiInnovationCnyApiResponse::getTs() const
{
    return d_ptr->ts;
}

void HuoBiInnovationCnyApiResponse::setTs(const qint64 &value)
{
    d_ptr->ts = value;
}

QJsonValue HuoBiInnovationCnyApiResponse::getTick() const
{
    return d_ptr->tick;
}

void HuoBiInnovationCnyApiResponse::setTick(const QJsonValue &value)
{
    d_ptr->tick = value;
}

QString HuoBiInnovationCnyApiResponse::getCh() const
{
    return d_ptr->ch;
}

void HuoBiInnovationCnyApiResponse::setCh(const QString &value)
{
    d_ptr->ch = value;
}

QString HuoBiInnovationCnyApiResponse::getErrCode() const
{
    return d_ptr->errCode;
}

void HuoBiInnovationCnyApiResponse::setErrCode(const QString &value)
{
    d_ptr->errCode = value;
}


///----------HuoBiInnovationCnyApi-------

HuoBiProApi::HuoBiProApi(QObject *parent) :
    AbstractMarketApi(parent)
{

}

QByteArray HuoBiProApi::sign(const AppMarketAccount &appMarketAccount, QNetworkAccessManager::Operation op, const QUrl &url, const QUrlQuery &qurlQuery)
{
    QByteArray method;
    switch(op)
    {
    case QNetworkAccessManager::GetOperation:
        method = "GET";
        break;
    case QNetworkAccessManager::PostOperation:
        method = "POST";
        break;
    case QNetworkAccessManager::PutOperation:
        method = "PUT";
        break;
    default:
        break;
    }

    QString queryString = HttpUtils::sortUrlQuery2String(qurlQuery);

    QByteArray info;
    info.append(method).append("\n")
            .append(url.host().toLower().toUtf8()).append("\n")
            .append(url.path().toLower().toUtf8()).append("\n")
            .append(queryString.toUtf8());


    QByteArray signStr = sign(appMarketAccount, info);

    qDebug() << "sign url:" << url << ", info: " << info << ", signStr:" << signStr;

    return signStr;
}

QByteArray HuoBiProApi::sign(const AppMarketAccount &appMarketAccount, const QByteArray &info)
{
    QByteArray skey = appMarketAccount.getSecretKey();
    QByteArray sign = QMessageAuthenticationCode::hash(info, skey, QCryptographicHash::Sha256).toBase64();
    return sign;
}

QString HuoBiProApi::orderTypeString(BitOrder::OrderType orderType)
{
    switch(orderType)
    {
    case BitOrder::OrderType::BuyLimit:
        return "buy-limit";
    case BitOrder::OrderType::BuyMarket:
        return "buy-market";
    case BitOrder::OrderType::SellLimit:
        return "sell-limit";
    case BitOrder::OrderType::SellMarket:
        return "sell-market";
    default:
        return "";
    }
}

BitOrder::OrderType HuoBiProApi::toOrderType(const QString &orderType)
{
    QString orderTypeLower = orderType.toLower();
    if(orderTypeLower == "buy-limit") {
        return BitOrder::OrderType::BuyLimit;
    }
    if(orderTypeLower == "buy-market") {
        return BitOrder::OrderType::BuyMarket;
    }
    if(orderTypeLower == "sell-limit") {
        return BitOrder::OrderType::SellLimit;
    }
    if(orderTypeLower == "sell-market") {
        return BitOrder::OrderType::SellMarket;
    }

    return BitOrder::OrderType::BuyLimit;
}

QString HuoBiProApi::orderStateString(BitOrder::OrderState orderState)
{
    switch(orderState)
    {
    case BitOrder::OrderState::Canceled:
        return "canceled";
    case BitOrder::OrderState::Filled:
        return "filled";
    case BitOrder::OrderState::PartialCanceled:
        return "partial-canceled";
    case BitOrder::OrderState::PartialFilled:
        return "partial-filled";
    case BitOrder::OrderState::PreSubmitted:
        return "pre-submitted";
    case BitOrder::OrderState::Submitted:
        return "submitted";
    case BitOrder::OrderState::Submitting:
        return "submitting";
    default:
        return "";
    }
}

BitOrder::OrderState HuoBiProApi::fromString2OrderState(const QString &orderState)
{
    QString orderStateLower = orderState.toLower();

    if(orderStateLower == "canceled") {
        return BitOrder::OrderState::Canceled;
    }

    if(orderStateLower == "filled") {
        return BitOrder::OrderState::Filled;
    }

    if(orderStateLower == "partial-canceled") {
        return BitOrder::OrderState::PartialCanceled;
    }

    if(orderStateLower == "partial-filled") {
        return BitOrder::OrderState::PartialFilled;
    }

    if(orderStateLower == "pre-submitted") {
        return BitOrder::OrderState::PreSubmitted;
    }

    if(orderStateLower == "submitted") {
        return BitOrder::OrderState::Submitted;
    }

    if(orderStateLower == "submitting") {
        return BitOrder::OrderState::Submitting;
    }

    return BitOrder::OrderState::Canceled;
}

QString HuoBiProApi::periodTypeString(MarketParam::PeriodType periodType)
{
    switch(periodType)
    {
    case MarketParam::PeriodType::OneMin:
        return "1min";
    case MarketParam::PeriodType::ThreeMin:
        return "3min";
    case MarketParam::PeriodType::FiveMin:
        return "5min";
    case MarketParam::PeriodType::FifteenMin:
        return "15min";
    case MarketParam::PeriodType::ThirtyMin:
        return "30min";
    case MarketParam::PeriodType::OneHour:
        return "1hour";
    case MarketParam::PeriodType::TwoHour:
        return "2hour";
    case MarketParam::PeriodType::FourHour:
        return "4hour";
    case MarketParam::PeriodType::SixHour:
        return "6hour";
    case MarketParam::PeriodType::TwelveHour:
        return "12hour";
    case MarketParam::PeriodType::OneDay:
        return "1day";
    case MarketParam::PeriodType::ThreeDay:
        return "3day";
    case MarketParam::PeriodType::OneWeek:
        return "1week";
    case MarketParam::PeriodType::OneMon:
        return "1mon";
    case MarketParam::PeriodType::OneYear:
        return "1year";
    default:
        return "1min";
    }
}

QString HuoBiProApi::depthTypeString(MarketParam::DepthType depthType)
{
    switch(depthType)
    {
    case MarketParam::DepthType::Step0:
        return "step0";
    case MarketParam::DepthType::Step1:
        return "step1";
    case MarketParam::DepthType::Step2:
        return "step2";
    case MarketParam::DepthType::Step3:
        return "step3";
    case MarketParam::DepthType::Step4:
        return "step4";
    case MarketParam::DepthType::Step5:
        return "step5";
    default:
        return "step0";
    }
}

QString HuoBiProApi::currency() const
{
    return "cny";
}

QList<QString> HuoBiProApi::supportSymbolList() const
{
    static QList<QString> list {
        "eth", "etc", "btc", "bcc" // other symbol
    };
    return list;
}

QString HuoBiProApi::market() const
{
    static QString m = "HuoBiInnovationCny";
    return m;
}

QUrl HuoBiProApi::host() const
{
    static QUrl hostUrl("https://api.huobi.pro");
    return hostUrl;
}

QPromise<HuoBiInnovationCnyApiResponse> HuoBiProApi::accounts(const AppMarketAccount &appMarketAccount)
{
    static const QString path = "/v1/account/accounts";

    auto promise = this->sendRequest(appMarketAccount, QNetworkAccessManager::GetOperation, path,
                                     QUrlQuery(), QUrlQuery());
    return promise;
}

QtPromise::QPromise<QString> HuoBiProApi::accountId(const AppMarketAccount &appMarketAccount)
{
    if (!appMarketAccount.getAccountIdInMarket().isEmpty()) {
        return QPromise<QString>([appMarketAccount](
                                 const QPromiseResolve<QString>& resolve ,
                                 const QPromiseReject<QString>&){
            qDebug() << "accountId exists return:" << appMarketAccount.getAccountIdInMarket();
            resolve(appMarketAccount.getAccountIdInMarket());
        });
    }

    return this->accounts(appMarketAccount)
            .then([this](const HuoBiInnovationCnyApiResponse& res){
        qDebug() << "accountId status:" << res.getStatus();
        QJsonArray accountList = res.getData().toArray();

        if (accountList.isEmpty()) {
            return QString("-1");
        }

        QJsonObject account = accountList.at(0).toObject();

        QString id = QString::number(account.value("id").toInt());

        return id;
    });
}

QPromise<AppMarketAsset> HuoBiProApi::assets(const AppMarketAccount &appMarketAccount)
{
    static const QString pathTemp = "/v1/account/accounts/%1/balance";

    return this->accountId(appMarketAccount)
            .then([this, appMarketAccount](const QString& accountId){
        auto promise =  this->sendRequest(appMarketAccount,
                                          QNetworkAccessManager::GetOperation,
                                          pathTemp.arg(accountId),
                                          QUrlQuery(),
                                          QUrlQuery());
        return promise;
    }).then([this](const HuoBiInnovationCnyApiResponse& res){
        qDebug() << "status:" << res.getStatus();
        return convertAssetsFromData(res.getData());
    });
}

QtPromise::QPromise<BitOrder> HuoBiProApi::cancel(TradeParam &tradeParam)
{
    static const QString pathTemp = "/v1/order/orders/%1/submitcancel";
    const QString path = pathTemp.arg(tradeParam.getOrderId());

    auto promise = this->sendRequest(tradeParam.getAppMarketAccount(),
                                     QNetworkAccessManager::PostOperation,
                                     path,
                                     QUrlQuery(),
                                     QUrlQuery());
    return promise.then([](const HuoBiInnovationCnyApiResponse& response){
        BitOrder bitorder;
        if (response.getStatus() == "ok") {
            bitorder.setOrderId(response.getData().toString());            // string type
            qDebug() << "cancel success: " << bitorder.getOrderId();
        } else {
            bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
            qDebug() << "cancel fail : errCode : " << response.getErrCode()
                     << ", errMsg : " << response.getErrMsg();
        }
        return bitorder;
    });
}

QtPromise::QPromise<BitOrder> HuoBiProApi::order(TradeParam &tradeParam)
{
    static const QString pathTemp = "/v1/order/orders/%1";
    const QString path = pathTemp.arg(tradeParam.getOrderId());


    auto promise = this->sendRequest(tradeParam.getAppMarketAccount(),
                                     QNetworkAccessManager::GetOperation,
                                     path,
                                     QUrlQuery(),
                                     QUrlQuery());

    return promise.then([this, tradeParam](const HuoBiInnovationCnyApiResponse& response){
        BitOrder bitorder;
        if (response.getStatus() == "ok") {
            bitorder = convertOrderFromData(response.getData());
            bitorder.setOrderId(tradeParam.getOrderId());
        } else {
            bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
            qDebug() << "order fail : errCode : " << response.getErrCode()
                     << ", errMsg : " << response.getErrMsg();
        }
        return bitorder;
    });
}

QtPromise::QPromise<QList<Kline>> HuoBiProApi::kline(const MarketParam &marketParam)
{
    static const QString path = "/market/history/kline";
    QUrl url = getUrl(path);

    auto klineFail = [](){
        return QtPromise::QPromise<QList<Kline>>([](
                                                 const QPromiseResolve<QList<Kline>>& ,
                                                 const QPromiseReject<QList<Kline>>& reject){
            reject(QList<Kline>());
        });
    };

//    if (marketParam.getCurrency() != currency()) {
//        qDebug() << "kline fail : param currency not match";
//        return klineFail();
//    }

    if (!supportSymbolList().contains(marketParam.getSymbol())) {
        qDebug() << "kline fail : symbol currency not match";
        return klineFail();
    }

    QString period = periodTypeString(marketParam.getPeriod());
    QString symbol = marketParam.getSymbol() + this->currency();
    qint32 size = marketParam.getSize();

    QUrlQuery getQuery;
    getQuery.addQueryItem("period", period);
    getQuery.addQueryItem("symbol", symbol);
    getQuery.addQueryItem("size", QString::number(size));

    url.setQuery(getQuery);

    QNetworkRequest networkRequest = HttpUtils::browserRequest(url);

    auto promise = this->sendRequestInternal(networkRequest, "", QNetworkAccessManager::GetOperation);

    return promise.then([this, symbol](const HuoBiInnovationCnyApiResponse& response) {

        if (response.getStatus() != "ok") {
            qDebug() << "kline fail : "
                     << "error code: " << response.getErrCode()
                     << "error msg: " << response.getErrMsg();
            return QList<Kline>();
        }

        return convertKlineListFromData(response.getData(), response.getTs(), symbol);
    });
}

QtPromise::QPromise<Ticker> HuoBiProApi::ticker(const MarketParam &marketParam)
{
    static const QString path = "/market/detail/merged";
    QUrl url = getUrl(path);

    auto tickerFail = [](){
        return QtPromise::QPromise<Ticker>([](
                                           const QPromiseResolve<Ticker>& ,
                                           const QPromiseReject<Ticker>& reject){
            reject(Ticker());
        });
    };

    if (!supportSymbolList().contains(marketParam.getSymbol())) {
        qDebug() << "kline fail : symbol currency not match";
        return tickerFail();
    }

    QString symbol = marketParam.getSymbol() + this->currency();

    QUrlQuery getQuery;
    getQuery.addQueryItem("symbol", symbol);
    url.setQuery(getQuery);

    QNetworkRequest networkRequest = HttpUtils::browserRequest(url);

    auto promise = this->sendRequestInternal(networkRequest, "", QNetworkAccessManager::GetOperation);

    return promise.then([this, symbol](const HuoBiInnovationCnyApiResponse& response) {

        if (response.getStatus() != "ok") {
            qDebug() << "kline fail : "
                     << "error code: " << response.getErrCode()
                     << "error msg: " << response.getErrMsg();
            return Ticker();
        }

        return convertTickerFromData(response.getTick(), symbol);
    });
}

QtPromise::QPromise<Depth> HuoBiProApi::depth(const MarketParam &marketParam)
{
    static const QString path = "/market/depth";
    QUrl url = getUrl(path);

    auto tickerFail = [](){
        return QtPromise::QPromise<Depth>([](
                                          const QPromiseResolve<Depth>& ,
                                          const QPromiseReject<Depth>& reject){
            reject(Depth());
        });
    };

    QString symbol = marketParam.getSymbol() + this->currency();

    QString type = depthTypeString(marketParam.getDepth());

    QUrlQuery getQuery;
    getQuery.addQueryItem("symbol", symbol);
    getQuery.addQueryItem("type", type);
    url.setQuery(getQuery);

    QNetworkRequest networkRequest = HttpUtils::browserRequest(url);

    auto promise = this->sendRequestInternal(networkRequest, "", QNetworkAccessManager::GetOperation);

    return promise.then([this, symbol](const HuoBiInnovationCnyApiResponse& response) {

        if (response.getStatus() != "ok") {
            qDebug() << "kline fail : "
                     << "error code: " << response.getErrCode()
                     << "error msg: " << response.getErrMsg();
            return Depth();
        }

        return convertDepthFromData(response.getTick(), symbol);
    });

}

/**
 * amount :限价单表示下单数量，市价买单时表示买多少钱，市价卖单时表示卖多少币
 *
 */
QtPromise::QPromise<BitOrder> HuoBiProApi::trade(const TradeParam& tradeParam)
{
    static const QString path = "/v1/order/orders";

    auto tradeFail = [&](
            const QPromiseResolve<BitOrder>& ,
            const QPromiseReject<BitOrder>& reject) {
        BitOrder bitorder;
        bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
        reject(bitorder);
    };

    if (tradeParam.getAppMarketAccount().getAccountIdInMarket().isEmpty()) {
        qDebug() << "trade fail : account id in market is empty";
        return QPromise<BitOrder>(tradeFail);
    }

    if (tradeParam.getPrice() <= 0.0) {
        qDebug() << "trade fail : price is less than 0.0";
        return QPromise<BitOrder>(tradeFail);
    }

    if (tradeParam.getAmount() <= 0.0) {
        qDebug() << "trade fail : amount is less than 0.0";
        return QPromise<BitOrder>(tradeFail);
    }

    if (tradeParam.getSymbol().isEmpty()) {
        qDebug() << "trade fail : symbol is empty";
        return QPromise<BitOrder>(tradeFail);
    }

    if (tradeParam.getCurrency().isEmpty()) {
        qDebug() << "trade fail : currency is empty";
        return QPromise<BitOrder>(tradeFail);
    }

    if (tradeParam.getOrderSide() == BitOrder::OrderSide::Buy) {
        if (tradeParam.getOrderType() != BitOrder::OrderType::BuyLimit && tradeParam.getOrderType() != BitOrder::OrderType::BuyMarket) {
            qDebug() << "trade fail : order side not match";
            return QPromise<BitOrder>(tradeFail);
        }
    }

    if (tradeParam.getOrderSide() == BitOrder::OrderSide::Sell) {
        if (tradeParam.getOrderType() != BitOrder::OrderType::SellLimit && tradeParam.getOrderType() != BitOrder::OrderType::SellMarket) {
            qDebug() << "trade fail : order side not match";
            return QPromise<BitOrder>(tradeFail);
        }
    }

    if (!this->supportSymbolList().contains(tradeParam.getSymbol())) {
        qDebug() << "trade fail : not support symbol";
        return QPromise<BitOrder>(tradeFail);
    }

    if (this->currency() != tradeParam.getCurrency()) {
        qDebug() << "trade fail : not support currency";
        return QPromise<BitOrder>(tradeFail);
    }

    QUrlQuery postQuery;

    postQuery.addQueryItem("account-id", tradeParam.getAppMarketAccount().getAccountIdInMarket());
    postQuery.addQueryItem("amount", QString::number(tradeParam.getAmount()));
    postQuery.addQueryItem("price", QString::number(tradeParam.getPrice()));
    postQuery.addQueryItem("source", "api");
    postQuery.addQueryItem("symbol", tradeParam.getSymbol().toLower() + tradeParam.getCurrency().toLower());
    postQuery.addQueryItem("type", orderTypeString(tradeParam.getOrderType()));

    auto promise = this->sendRequest(tradeParam.getAppMarketAccount(),
                                     QNetworkAccessManager::PostOperation,
                                     path,
                                     postQuery,
                                     QUrlQuery());

    return promise.then([](const HuoBiInnovationCnyApiResponse& response){
        BitOrder bitorder;
        if (response.getStatus() == "ok") {
            bitorder.setOrderId(QString::number(response.getData().toDouble(-1)));         // long type
            qDebug() << "trade success: " << bitorder.getOrderId();
        } else {
            bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
            qDebug() << "trade fail : errCode : " << response.getErrCode()
                     << ", errMsg : " << response.getErrMsg();
        }
        return bitorder;
    }).then([this ,tradeParam ](const BitOrder& bitorder){
        auto promise2 = place(tradeParam.getAppMarketAccount(), bitorder.getOrderId());
        return promise2;
    });
}

QtPromise::QPromise<BitOrder> HuoBiProApi::place(const AppMarketAccount &appMarketAccount, const QString &orderId)
{
    static const QString pathTemp = "/v1/order/orders/%1/place";
    const QString path = pathTemp.arg(orderId);

    auto promise = this->sendRequest(appMarketAccount,
                                     QNetworkAccessManager::PostOperation,
                                     path,
                                     QUrlQuery(),
                                     QUrlQuery());

    return promise.then([](const HuoBiInnovationCnyApiResponse& response){
        BitOrder bitorder;
        if (response.getStatus() == "ok") {
            bitorder.setOrderId(response.getData().toString(BitOrder::ERROR_ORDER_ID));          // string type
            qDebug() << "place success: " << bitorder.getOrderId();
        } else {
            bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
            qDebug() << "place fail : errCode : " << response.getErrCode()
                     << ", errMsg : " << response.getErrMsg();
        }
        return bitorder;
    });
}

QPromise<HuoBiInnovationCnyApiResponse> HuoBiProApi::sendRequestInternal(const QNetworkRequest &networkReuqest, const QByteArray &data, QNetworkAccessManager::Operation op)
{
    auto callback = [networkReuqest](const HttpResponse& res){

        // qDebug() << "url:"<< networkReuqest.url() << ", response: " << data;
        const QByteArray& data = res.getResponse();
        QJsonDocument jsonDoc = QJsonDocument::fromJson(data);

        HuoBiInnovationCnyApiResponse response;

        if (jsonDoc.isNull()) {
            qDebug()  << "url:"<< networkReuqest.url() << ", jsonDoc is null";
            return response;
        }

        QJsonObject obj = jsonDoc.object();

        response.setData(obj.value("data"));
        response.setStatus(obj.value("status").toString());
        response.setCh(obj.value("ch").toString());
        response.setTs(obj.value("ts").toDouble(0));
        response.setTick(obj.value("tick"));
        response.setErrCode(obj.value("err-code").toString());
        response.setErrMsg(obj.value("err-msg").toString());

        return response;
    };

    if (QNetworkAccessManager::GetOperation == op) {

        auto promise = HttpUtils::get(this->getNetworkAccessManager(), networkReuqest);

        return promise.then(callback);
    } else {

        qDebug() << "sendRequestInternal : uri: " << networkReuqest.url().path() << ", post data:" << data;

        auto promise = HttpUtils::post(this->getNetworkAccessManager(), networkReuqest, data);

        return promise.then(callback);
    }
}

QtPromise::QPromise<HuoBiInnovationCnyApiResponse> HuoBiProApi::sendRequest(const AppMarketAccount& appMarketAccount, QNetworkAccessManager::Operation op, const QString &path, const QUrlQuery &postQuery, const QUrlQuery &signQuery) {
    QUrl url = getUrl(path);

    QUrlQuery wait4SignQuery(signQuery);

    QString timestamp = QDateTime::currentDateTime().toUTC().toString("yyyy-MM-ddThh:mm:ss");

    wait4SignQuery.addQueryItem("AccessKeyId", appMarketAccount.getAccessKey());
    wait4SignQuery.addQueryItem("SignatureMethod", "HmacSHA256");
    wait4SignQuery.addQueryItem("SignatureVersion", "2");
    wait4SignQuery.addQueryItem("Timestamp", timestamp);

    QString signStr = sign(appMarketAccount, op, url, wait4SignQuery);

    QString queryInPath = HttpUtils::sortUrlQuery2String(wait4SignQuery);
    queryInPath.append("&Signature=").append(QUrl::toPercentEncoding(signStr));

    url.setQuery(queryInPath);

    QNetworkRequest networkRequest = HttpUtils::browserRequest(url);

    QString postQueryStr;

    if(op == QNetworkAccessManager::PostOperation) {
        networkRequest.setRawHeader("Content-Type", "application/json");
        postQueryStr = HttpUtils::urlQuery2JsonString(postQuery);
    }

    return this->sendRequestInternal(networkRequest, postQueryStr.toUtf8(), op);
}

AppMarketAsset HuoBiProApi::convertAssetsFromData(const QJsonValue &data)
{
    AppMarketAsset appMarketAsset;

    if (data.isNull()) {
        qDebug() << "convertAssetsFromData fail : data is null";
        return appMarketAsset;
    }

    auto insertAsset = [](QMap<QString, Asset>& map, QJsonObject asset){
        static const QString TRADE = "trade";
        static const QString FROZEN = "frozen";
        QString currency = asset.value("currency").toString();
        QString type = asset.value("type").toString();              // trade: 交易余额，frozen: 冻结余额
        qreal balance = asset.value("balance").toDouble();

        auto find = map.find(currency);
        auto end = map.end();
        if (find != end) {

            Asset asset = find.value();
            if (type == TRADE) {
                asset.setAvailable(balance);
            } else if (type == FROZEN) {
                asset.setFrozen(balance);
            }
            map.insert(currency, asset);

        } else {
            Asset asset;
            if (type == TRADE) {
                asset.setAvailable(balance);
            } else if (type == FROZEN) {
                asset.setFrozen(balance);
            }
            map.insert(currency, asset);
        }
    };

    auto map2list = [](QMap<QString, Asset>& map) {
        QList<Asset> list;
        auto iter = map.begin();
        auto end = map.end();
        while(iter != end) {
            list << iter.value();
            iter++;
        }
        return list;
    };

    QMap<QString, Asset> assetBySymbol;

    QJsonObject obj = data.toObject();

    QJsonArray list = obj.value("list").toArray();

    auto iter = list.constBegin();
    auto end = list.constEnd();

    while(iter != end) {
        auto asset = (*iter).toObject();
        insertAsset(assetBySymbol, asset);
        iter++;
    }

    appMarketAsset.setAssetList(map2list(assetBySymbol));

    return appMarketAsset;
}

BitOrder HuoBiProApi::convertOrderFromData(const QJsonValue &data)
{
    BitOrder bitorder;

    QJsonObject obj = data.toObject();

    if (obj.isEmpty()) {
        bitorder.setOrderId(BitOrder::ERROR_ORDER_ID);
        return bitorder;
    }

    QString symbol = obj.value("symbol").toString().remove(currency());
    qreal amount = obj.value("amount").toDouble();
    qreal price = obj.value("price").toDouble();
    qint64 createdAt = obj.value("created-at").toDouble();
    BitOrder::OrderType type = toOrderType(obj.value("type").toString());
    qreal fieldAmount = obj.value("field-amount").toDouble();
    qreal fieldCashAmount = obj.value("field-cash-amount").toDouble();
    qreal fieldFees = obj.value("field-fees").toDouble();
    qint64 finishedAt = obj.value("filished-at").toDouble();
    BitOrder::OrderState state = fromString2OrderState(obj.value("state").toString());
    qint64 canceledAt = obj.value("caceled-at").toDouble();

    Q_UNUSED(fieldCashAmount)

    bitorder.setSymbol(symbol);
    bitorder.setOrderAmount(amount);
    bitorder.setOrderPrice(price);
    bitorder.setCreateTime(createdAt);
    bitorder.setOrderType(type);
    bitorder.setProcessAmount(fieldAmount);
    bitorder.setProcessFeeAmount(fieldFees);
    bitorder.setFinishedTime(finishedAt);
    bitorder.setCancelTime(canceledAt);
    bitorder.setState(state);

    return bitorder;
}

QList<Kline> HuoBiProApi::convertKlineListFromData(const QJsonValue &data, qint64 timestamp, const QString& symbol)
{
    QList<Kline> klineList;
    if (data.isNull()) {
        return klineList;
    }

    auto convertKlineFromData = [=](const QJsonObject& obj) {

        Kline kline;

        qreal amount = obj.value("amount").toDouble();
        qreal open = obj.value("open").toDouble();
        qreal high = obj.value("high").toDouble();
        qreal count = obj.value("count").toDouble();
        qreal low = obj.value("low").toDouble();
        qreal close = obj.value("close").toDouble();
        qreal vol = obj.value("vol").toDouble();

        kline.setAmount(amount);
        kline.setOpen(open);
        kline.setHigh(high);
        kline.setClose(close);
        kline.setCount(count);
        kline.setLow(low);
        kline.setVol(vol);
        kline.setTimestamp(timestamp);

        kline.setMarket(market());
        kline.setSymbol(symbol);
        kline.setCurrency(currency());

        return kline;
    };

    QJsonArray list = data.toArray();

    auto iter = list.begin();
    auto end = list.end();

    while(iter != end) {

        klineList.append(convertKlineFromData((*iter).toObject()));

        iter++;
    }

    return klineList;
}

Ticker HuoBiProApi::convertTickerFromData(const QJsonValue &tick, const QString &symbol)
{
    if (tick.isNull()) {
        return Ticker();
    }

    QJsonObject obj = tick.toObject();

    qint64 ts = obj.value("ts").toDouble();
    qreal close = obj.value("close").toDouble();
    qreal open = obj.value("open").toDouble();
    qreal high = obj.value("high").toDouble();
    qreal low = obj.value("low").toDouble();
    qreal amount = obj.value("amount").toDouble();
    qint32 count = obj.value("count").toInt();
    qreal vol = obj.value("vol").toDouble();

    QJsonArray ask = obj.value("ask").toArray();
    qreal askPrice = ask.at(0).toDouble();
    qreal askAmount = ask.at(1).toDouble();

    QJsonArray bid = obj.value("bid").toArray();
    qreal bidPrice = bid.at(0).toDouble();
    qreal bidAmount = bid.at(1).toDouble();


    Ticker ticker;

    ticker.setTimestamp(ts);
    ticker.setClose(close);
    ticker.setOpen(open);
    ticker.setHigh(high);
    ticker.setLow(low);
    ticker.setAmount(amount);
    ticker.setCount(count);
    ticker.setVol(vol);
    ticker.setAskPrice(askPrice);
    ticker.setAskAmount(askAmount);
    ticker.setBidPrice(bidPrice);
    ticker.setBidAmount(bidAmount);
    ticker.setSymbol(symbol);
    ticker.setMarket(market());

    return ticker;
}

Depth HuoBiProApi::convertDepthFromData(const QJsonValue &tick, const QString &symbol)
{
    if (tick.isNull()){
        return Depth();
    }

    auto covert = [](const QJsonArray& array){
        QList<QPair<qreal, qreal>> list;
        auto iter = array.begin();
        auto end = array.end();

        while(iter != end) {

            QJsonArray a = (*iter).toArray();

            qreal price = a.at(0).toDouble();
            qreal amount =a.at(0).toDouble();

            list << QPair<qreal, qreal>(price, amount);

            iter++;
        }

        return list;
    };

    QJsonObject obj = tick.toObject();

    qint64 ts = obj.value("ts").toDouble();
    QJsonArray bids = obj.value("bids").toArray();
    QJsonArray asks = obj.value("asks").toArray();

    Depth depth;

    depth.setCurrency(currency());
    depth.setSymbol(symbol);
    depth.setMarket(market());

    depth.setTimestamp(ts);
    depth.setBids(covert(bids));
    depth.setAsks(covert(asks));

    return depth;
}

} // namespace qyvlik

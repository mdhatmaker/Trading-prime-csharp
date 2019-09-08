#include <QString>
#include <QtTest>
#include <QUrlQuery>
#include <QCoreApplication>
#include <QNetworkAccessManager>

#include "market/huobi/huobproapi.h"

using namespace qyvlik;

class TestHuobiApi : public QObject
{
    Q_OBJECT

public:
    TestHuobiApi();

public Q_SLOTS:
    void test_sign();
    void test_accounts();
    void test_assets();
    void test_trade_buy();
    void test_cancel();
    void test_order();
    void test_kline();
    void test_ticker();
    void test_depth();
private:
    void cancel(const QString& orderId);

    QNetworkAccessManager* manager;
    HuoBiProApi* api;
    AppMarketAccount appMarketAccount;
};

TestHuobiApi::TestHuobiApi()
{
    manager = new QNetworkAccessManager(this);
    api = new HuoBiProApi(this);
    api->setNetworkAccessManager(manager);

    appMarketAccount.setAccessKey("YOUR_ACCESS_KEY");
    appMarketAccount.setSecretKey("YOUR_SECRET_KEY");
}

void TestHuobiApi::test_sign()
{
    QUrlQuery wait4SignQuery;

    wait4SignQuery.addQueryItem("AccessKeyId", appMarketAccount.getAccessKey());
    wait4SignQuery.addQueryItem("SignatureMethod", "HmacSHA256");
    wait4SignQuery.addQueryItem("SignatureVersion", "2");
    wait4SignQuery.addQueryItem("Timestamp", "2017-08-13T13:39:18");

    QUrl url("https://be.huobi.com/v1/account/accounts");

    QByteArray signStr = api->sign(appMarketAccount, QNetworkAccessManager::GetOperation, url, wait4SignQuery);

    qDebug() << "signStr: " << signStr;
}

void TestHuobiApi::test_accounts()
{
    auto promise = api->accounts(appMarketAccount);
    promise.then([](const HuoBiInnovationCnyApiResponse& res){
        qDebug() << "status:" << res.getStatus();
        auto data = res.getData();
        qDebug() << "type:" << data.type();
    });
}

void TestHuobiApi::test_trade_buy()
{
    api->accountId(appMarketAccount)
            .then([this](const QString& accountId){

        appMarketAccount.setAccountIdInMarket(accountId);

    }).then([this](){
        TradeParam tradeParam;
        tradeParam.setAmount(0.01);
        tradeParam.setPrice(10000);
        tradeParam.setCurrency("cny");
        tradeParam.setSymbol("eth");
        tradeParam.setOrderSide(BitOrder::OrderSide::Sell);
        tradeParam.setOrderType(BitOrder::OrderType::SellLimit);
        tradeParam.setAppMarketAccount(appMarketAccount);

        auto promise = api->trade(tradeParam);
        promise.then([](const BitOrder& bitOrder){
            qDebug() << "trade bitOrder :" << bitOrder.getOrderId();
        });
    });
}

void TestHuobiApi::test_cancel()
{
    //    cancel("22424148");
}

void TestHuobiApi::test_order()
{
    TradeParam tradeParam;
    tradeParam.setAppMarketAccount(appMarketAccount);
    tradeParam.setOrderId("22439562");
    auto promise = api->order(tradeParam);
    promise.then([=](const BitOrder& bitOrder){
        qDebug() << "order bitOrder : "
                 << bitOrder.getOrderId()
                 << bitOrder.getOrderAmount();
    });
}

void TestHuobiApi::test_kline()
{
    MarketParam marketParam;
    marketParam.setSymbol("eth");
    marketParam.setCurrency("usdt");

    auto promise = api->kline(marketParam);
    promise.then([](const QList<Kline>& klineList){
        qDebug() << "kline size: " << klineList.size();
    });

}

void TestHuobiApi::test_ticker()
{
    MarketParam marketParam;
    marketParam.setSymbol("eth");
    marketParam.setCurrency("usdt");

    auto promise = api->ticker(marketParam);
    promise.then([](const Ticker& ticker){
        qDebug() << "ticker close: " << ticker.getClose();
    });
}

void TestHuobiApi::test_depth()
{
    MarketParam marketParam;
    marketParam.setSymbol("eth");
    marketParam.setCurrency("usdt");

    auto promise = api->depth(marketParam);
    promise.then([](const Depth& depth){
        qDebug() << "depth asks: " << depth.getAsks();
    });
}

void TestHuobiApi::cancel(const QString &orderId)
{
    TradeParam tradeParam;
    tradeParam.setAppMarketAccount(appMarketAccount);
    tradeParam.setOrderId(orderId);
    auto promise = api->cancel(tradeParam);
    promise.then([=](const BitOrder& bitOrder){
        qDebug() << "cancel bitOrder : " << orderId << ", return id: " << bitOrder.getOrderId();
    });
}

void TestHuobiApi::test_assets()
{
    auto promise = api->assets(appMarketAccount);
    promise.then([](const AppMarketAsset& asset){
        qDebug() << "asset: " << asset.getAssetList().size();
    });
}

int main(int argc, char *argv[])
{
    QCoreApplication app(argc, argv);

    TestHuobiApi t;

    t.test_accounts();

    t.test_assets();

    t.test_trade_buy();

    t.test_order();

    t.test_kline();

    t.test_ticker();

    t.test_sign();

    t.test_depth();

    return app.exec();
}

//QTEST_MAIN(TestHuobiApi)

#include "tst_testhuobiapi.moc"

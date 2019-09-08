#include <QString>
#include <QtTest>
#include <QUrl>
#include <QUrlQuery>
#include <QNetworkAccessManager>
#include <QNetworkRequest>
#include <QNetworkReply>

#include "utils/httputils.h"

using namespace qyvlik;

class Test_httputilsTest : public QObject
{
    Q_OBJECT

public:
    Test_httputilsTest();

public Q_SLOTS:
    void testFetch();
    void testHttpUtilsGet();
    void testHttpUtilsSortUrQuery();

private:
    QNetworkAccessManager* manager;
};

Test_httputilsTest::Test_httputilsTest()
{
    manager = new QNetworkAccessManager(this);
}

void Test_httputilsTest::testHttpUtilsGet()
{
    QNetworkRequest req(QUrl("https://api.github.com/zen"));
    auto promise = HttpUtils::get(manager, req);
    promise.then([](const HttpResponse& response){
        const QByteArray& data = response.getResponse();
        qDebug() << "promise then: " << data;
    });
}

void Test_httputilsTest::testHttpUtilsSortUrQuery()
{
    QUrlQuery query;

    query.addQueryItem("AccessKeyId", "e2xxxxxx-99xxxxxx-84xxxxxx-7xxxx");
    query.addQueryItem("SignatureMethod", "HmacSHA256");
    query.addQueryItem("SignatureVersion", "2");
    query.addQueryItem("Timestamp", "2017-05-11T15:19:30");

    QString queryString = HttpUtils::sortUrlQuery2String(query);

    qDebug() << "queryString: " << queryString;
}

void Test_httputilsTest::testFetch()
{
    QNetworkRequest req(QUrl("https://www.51szzc.com/api/v1/ticker?symbol=btc_cny"));
    int count = 300;
    while(count-->0) {
        HttpUtils::get(manager, req, 1000)
                .then([count ](const HttpResponse& response){
            const QByteArray& data = response.getResponse();
//            qDebug() << "count: "<< count << data;
        }).fail([count](const HttpResponse& response){
            auto error = response.getError();
            qDebug() << "fail : count:" << count
                     << "error :" << error
                     << "status : " << response.getHttpStatus();
        });
    }

}

//int main(int argc, char *argv[])
//{
//    QCoreApplication app(argc, argv);

//    Test_httputilsTest t;

//    t.testHttpUtilsGet();

//    return app.exec();
//}

QTEST_MAIN(Test_httputilsTest)

#include "tst_test_httputilstest.moc"

#include "httputils.h"

#include "timerouthelper.h"

#include <QTimer>

using namespace QtPromise;

namespace qyvlik {

QtPromise::QPromise<HttpResponse> HttpUtils::get(QNetworkAccessManager *manager, const QNetworkRequest &request, const qint64 &timeout)
{
    Q_UNUSED(timeout);
    return QPromise<HttpResponse>([=](
                                  const QPromiseResolve<HttpResponse>& resolve,
                                  const QPromiseReject<HttpResponse>& reject) {
        QNetworkReply* reply = manager->get(request);
        QObject::connect(reply, &QNetworkReply::finished, [=]() {
            HttpResponse reponse = HttpResponse::createFromReply(reply);
            if (reply->error() == QNetworkReply::NoError) {
                resolve(reponse);
            } else {
                reject(reponse);
            }
            reply->deleteLater();
        });

        TimeoutHelper* helper = new TimeoutHelper(reply, reject);


        if (timeout > 0L) {
            QTimer::singleShot(timeout, helper, SLOT(onTimeout()));
        }
    });
}

QtPromise::QPromise<HttpResponse> HttpUtils::post(QNetworkAccessManager *manager, const QNetworkRequest &request, const QByteArray &data, const qint64 &timeout)
{
    Q_UNUSED(timeout);
    return QPromise<HttpResponse>([=](
                                  const QPromiseResolve<HttpResponse>& resolve,
                                  const QPromiseReject<HttpResponse>& reject) {
        QNetworkReply* reply = manager->post(request, data);
        QObject::connect(reply, &QNetworkReply::finished, [=]() {
            HttpResponse reponse = HttpResponse::createFromReply(reply);
            if (reply->error() == QNetworkReply::NoError) {
                resolve(reponse);
            } else {
                reject(reponse);
            }
            reply->deleteLater();
        });
    });
}

QNetworkRequest HttpUtils::browserRequest(const QUrl& url)
{
    QNetworkRequest request(url);
    request.setRawHeader("User-Agent" , "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
    request.setRawHeader("Content-Type", "application/x-www-form-urlencoded");
    return request;
}

QString HttpUtils::sortUrlQuery2String(const QUrlQuery &urlQuery)
{
    QString queryStr;

    if (urlQuery.isEmpty()) {
        return queryStr;
    }

    QList<QPair<QString, QString> > list = urlQuery.queryItems();

    QMap<QString, QString> paramsMap;

    Q_FOREACH(auto params, list) {
        paramsMap.insert(params.first, params.second);
    }

    auto iter = paramsMap.constBegin();
    auto end = paramsMap.constEnd();
    while (iter != end) {
        queryStr.append("&").append(iter.key()).append("=").append(QUrl::toPercentEncoding(iter.value()));
        ++iter;
    }
    queryStr.remove(0, 1);                  // remove first `&

    return queryStr;
}

QString HttpUtils::urlQuery2JsonString(const QUrlQuery &urlQuery)
{
    QString queryStr = "{}";

    if (urlQuery.isEmpty()) {
        return queryStr;
    }

    QJsonObject queryObj;

    QList<QPair<QString, QString> > list = urlQuery.queryItems();

    Q_FOREACH(auto params, list) {
        queryObj.insert(params.first, params.second);
    }

    QJsonDocument doc(queryObj);

    return doc.toJson(QJsonDocument::Compact);
}

QJsonValue HttpUtils::parse(const QByteArray &jsonString)
{
    QJsonDocument doc = QJsonDocument::fromJson(jsonString);

    if (doc.isArray()) {
        return doc.array();
    }

    if (doc.isObject()) {
        return doc.object();
    }

    return QJsonValue();
}


} // namespace qyvlik

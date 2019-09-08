#ifndef HTTPUTILS_H
#define HTTPUTILS_H

#include <QtPromise>
#include <QJsonObject>
#include <QJsonDocument>
#include <QJsonArray>
#include <QUrl>
#include <QNetworkReply>
#include <QNetworkRequest>
#include <QUrlQuery>

#include "httpresponse.h"

class QUrlQuery;
class QNetworkRequest;
class QNetworkAccessManager;

namespace qyvlik {

class HttpUtils
{
public:
    static QtPromise::QPromise<HttpResponse> get(QNetworkAccessManager* manager, const QNetworkRequest& request, const qint64& timeout = -1);

    static QtPromise::QPromise<HttpResponse> post(QNetworkAccessManager* manager, const QNetworkRequest& request, const QByteArray& data, const qint64& timeout = -1);

    static QNetworkRequest browserRequest(const QUrl& url);

    static QString sortUrlQuery2String(const QUrlQuery& urlQuery);

    static QString urlQuery2JsonString(const QUrlQuery& urlQuery);

    static QJsonValue parse(const QByteArray& jsonString);
};

} // namespace qyvlik

#endif // HTTPUTILS_H

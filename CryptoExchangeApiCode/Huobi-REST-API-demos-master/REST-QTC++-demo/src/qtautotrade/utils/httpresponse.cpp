#include "httpresponse.h"

namespace qyvlik {

class HttpResponsePrivate : public QSharedData
{
public:
    HttpResponsePrivate():
        QSharedData()
    {}

    HttpResponsePrivate(const HttpResponsePrivate& other)
        : QSharedData(other)
        , request(other.request)
        , httpStatus(other.httpStatus)
        , error(other.error)
        , rawHeaderParis(other.rawHeaderParis)
        , operation(other.operation)
        , response(other.response)
        , isTimeout(false)
    {}

    QNetworkRequest request;
    int httpStatus = 0;
    QNetworkReply::NetworkError error;
    QList<QNetworkReply::RawHeaderPair> rawHeaderParis;
    QNetworkAccessManager::Operation operation;
    QByteArray response;
    bool isTimeout = false;
};

HttpResponse::HttpResponse()
    : d(new HttpResponsePrivate)
{}

HttpResponse::HttpResponse(const HttpResponse &other)
    : d(other.d)
{}

HttpResponse &HttpResponse::operator=(const HttpResponse &other) {
    d = other.d;
    return *this;
}

HttpResponse::~HttpResponse()
{}

QByteArray HttpResponse::getResponse() const
{
    return d->response;
}

void HttpResponse::setResponse(const QByteArray &value)
{
    d->response = value;
}

int HttpResponse::getHttpStatus() const
{
    return d->httpStatus;
}

void HttpResponse::setHttpStatus(int value)
{
    d->httpStatus = value;
}

bool HttpResponse::getIsTimeout() const
{
    return d->isTimeout;
}

void HttpResponse::setIsTimeout(bool value)
{
    d->isTimeout = value;
}

HttpResponse HttpResponse::createFromReply(QNetworkReply *reply)
{
    HttpResponse response;
    response.setError(reply->error());
    response.setOperation(reply->operation());
    response.setRawHeaderParis(reply->rawHeaderPairs());
    response.setRequest(reply->request());

    if (reply->isReadable()) {
        response.setResponse(reply->readAll());
        response.setHttpStatus(reply->attribute(QNetworkRequest::HttpStatusCodeAttribute).toInt());
    } else if (reply->error() == QNetworkReply::OperationCanceledError) {
        response.setResponse("timeout and abort");
        response.setHttpStatus(-1);
    }

    return response;
}

QNetworkAccessManager::Operation HttpResponse::getOperation() const
{
    return d->operation;
}

void HttpResponse::setOperation(const QNetworkAccessManager::Operation &value)
{
    d->operation = value;
}

QList<QNetworkReply::RawHeaderPair> HttpResponse::getRawHeaderParis() const
{
    return d->rawHeaderParis;
}

void HttpResponse::setRawHeaderParis(const QList<QNetworkReply::RawHeaderPair> &value)
{
    d->rawHeaderParis = value;
}

QNetworkReply::NetworkError HttpResponse::getError() const
{
    return d->error;
}

void HttpResponse::setError(const QNetworkReply::NetworkError &value)
{
    d->error = value;
}

QNetworkRequest HttpResponse::getRequest() const
{
    return d->request;
}

void HttpResponse::setRequest(const QNetworkRequest &value)
{
    d->request = value;
}


} // namespace qyvlik

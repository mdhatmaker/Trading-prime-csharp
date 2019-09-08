#ifndef HTTPRESPONSE_H
#define HTTPRESPONSE_H


#include <QNetworkReply>
#include <QSharedDataPointer>

namespace qyvlik {

class HttpResponsePrivate;
class HttpResponse
{
public:
    HttpResponse();
    HttpResponse(const HttpResponse& other);
    HttpResponse& operator=(const HttpResponse& other);
    ~HttpResponse();

    static HttpResponse createFromReply(QNetworkReply* reply);

    QNetworkRequest getRequest() const;
    void setRequest(const QNetworkRequest &value);

    QNetworkReply::NetworkError getError() const;
    void setError(const QNetworkReply::NetworkError &value);

    QList<QNetworkReply::RawHeaderPair> getRawHeaderParis() const;
    void setRawHeaderParis(const QList<QNetworkReply::RawHeaderPair> &value);

    QNetworkAccessManager::Operation getOperation() const;
    void setOperation(const QNetworkAccessManager::Operation &value);

    QByteArray getResponse() const;
    void setResponse(const QByteArray &value);

    int getHttpStatus() const;
    void setHttpStatus(int value);

    bool getIsTimeout() const;
    void setIsTimeout(bool value);

private:
    QSharedDataPointer<HttpResponsePrivate> d;
};

} // namespace qyvlik

#endif // HTTPRESPONSE_H

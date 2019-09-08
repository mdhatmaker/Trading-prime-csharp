#ifndef TIMEROUTHELPER_H
#define TIMEROUTHELPER_H

#include <QtPromise>

#include <QObject>

#include "httpresponse.h"

class QNetworkReply;

namespace qyvlik {

class TimeoutHelper : public QObject
{
    Q_OBJECT
public:
    TimeoutHelper(QNetworkReply* parent, const QtPromise::QPromiseReject<HttpResponse>& reject);

public Q_SLOTS:
    void onTimeout();
private:
    QNetworkReply* mReply;
    QtPromise::QPromiseReject<HttpResponse> mReject;
};

} // namespace qyvlik

#endif // TIMEROUTHELPER_H

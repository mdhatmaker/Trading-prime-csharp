#include "timerouthelper.h"

#include <QNetworkReply>
#include <QDebug>

namespace qyvlik {

TimeoutHelper::TimeoutHelper(QNetworkReply *parent, const QtPromise::QPromiseReject<HttpResponse> &reject)
    : QObject(parent)
    , mReply(parent)
    , mReject(reject)
{}

void TimeoutHelper::onTimeout() {

    qDebug() << Q_FUNC_INFO << mReply;

    if (mReply->isFinished()) {
        return;
    }

    mReply->abort();

    HttpResponse response;

    response.setIsTimeout(true);
    response.setOperation(mReply->operation());
    response.setRequest(mReply->request());

    mReject(response);
    return;
}


} // namespace qyvlik

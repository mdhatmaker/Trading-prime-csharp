#include "abstractmarketapi.h"

#include <QNetworkAccessManager>

#include "bitorder.h"
#include "appmarketaccount.h"

namespace qyvlik {

using namespace QtPromise;

class AbstractMarketApiPrivate
{
public:
    QNetworkAccessManager* manager;
};

QString AbstractMarketApi::cacheKeyName(AbstractMarketApi *api, const QString &currency, const QString &symbol)
{
    return api->market() + "-" + currency + "-" + symbol;
}

AbstractMarketApi::AbstractMarketApi(QObject *parent)
    : QObject(parent)
    , d_ptr(new AbstractMarketApiPrivate())
{
}

AbstractMarketApi::~AbstractMarketApi()
{
    delete d_ptr;
}

QUrl AbstractMarketApi::getUrl(const QString &path)
{
    QUrl url(host());
    url.setPath(path);
    return url;
}

void AbstractMarketApi::setNetworkAccessManager(QNetworkAccessManager *manager)
{
    d_ptr->manager = manager;
}

QNetworkAccessManager *AbstractMarketApi::getNetworkAccessManager() const {
    return d_ptr->manager;
}


} // namespace qyvlik

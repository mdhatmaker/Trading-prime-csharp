#include "cacheutils.h"

#include <QCoreApplication>
#include <QReadWriteLock>
#include <QMap>

namespace qyvlik {

class CacheUtilsPrivate
{
public:
    QReadWriteLock lock;
    QMap<QString, QVariant> map;
};

CacheUtils::CacheUtils(QObject *parent)
    : QObject(parent)
    , d(new CacheUtilsPrivate)
{

}

CacheUtils::~CacheUtils()
{
    delete d;
}

CacheUtils *CacheUtils::singleton() {
    static CacheUtils* c = new CacheUtils(QCoreApplication::instance());
    return c;
}

QVariant CacheUtils::getVariant(const QString &key)
{
    QReadLocker locker(&d->lock);
    auto find = d->map.find(key);
    auto end = d->map.end();
    if (find != end) {
        return find.value();
    }
    return QVariant();
}

void CacheUtils::putVariant(const QString &key, const QVariant &value)
{
    QWriteLocker locker(&d->lock);
    d->map.insert(key, value);
}

} // namespace qyvlik

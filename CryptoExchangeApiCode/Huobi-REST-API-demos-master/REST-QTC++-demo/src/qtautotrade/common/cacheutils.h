#ifndef CACHEUTILS_H
#define CACHEUTILS_H

#include <QObject>
#include <QVariant>

namespace qyvlik {

class CacheUtilsPrivate;
class CacheUtils : public QObject
{
    Q_OBJECT
protected:
    explicit CacheUtils(QObject *parent = 0);
public:
    ~CacheUtils();

    static CacheUtils* singleton();

    QVariant getVariant(const QString& key);

    void putVariant(const QString& key, const QVariant& value);

    template<typename T>
    T get(const QString& key) {
        QVariant val = getVariant(key);
        if (val.canConvert<T>()) {
            return val.value<T>();
        }
        return T();
    }

    template<typename T>
    void put(const QString& key, const T& value) {
        QVariant val;
        val.setValue(value);
        this->putVariant(key, val);
    }

signals:

public slots:
private:
    CacheUtilsPrivate* d;
};

} // namespace qyvlik

#endif // CACHEUTILS_H

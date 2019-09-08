#include "appconfig.h"

#include <QCoreApplication>
#include <QDebug>

namespace qyvlik {

AppConfig *AppConfig::singleton()
{
    static AppConfig * appConfig = new AppConfig(QCoreApplication::instance());
    return appConfig;
}

AppConfig::AppConfig(QObject *parent)
    : QObject(parent)
{
    // qDebug() << Q_FUNC_INFO;

    setConfig("max.depth.len", "100");
}

void AppConfig::setConfig(const QString &key, const QString &value)
{
    QWriteLocker locker(&lock);
    configMap.insert(key, value);
}

QString AppConfig::getConfig(const QString &key)
{
  QReadLocker locker(&lock);
  auto find = configMap.find(key);
  auto end = configMap.end();
  if (find == end) {
      return QString();
  }
  return find.value();
}

qint64 AppConfig::getConfigLong(const QString &key, qint64 defaultValue)
{
    QString value = getConfig(key);
    bool ok = false;
    qint64 lval = value.toLong(&ok);
    if (ok) {
        return lval;
    }
    return defaultValue;;
}

} // namespace qyvlik

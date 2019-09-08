#ifndef APPCONFIG_H
#define APPCONFIG_H

#include <QObject>
#include <QMap>
#include <QReadWriteLock>

namespace qyvlik {

class AppConfig : public QObject
{
    Q_OBJECT
protected:
    explicit AppConfig(QObject *parent = 0);
public:
    static AppConfig* singleton();

    void setConfig(const QString& key, const QString& value);

    QString getConfig(const QString& key);

    qint64 getConfigLong(const QString& key, qint64 defaultValue = 0);

signals:

public slots:

private:
  QReadWriteLock lock;
  QMap<QString, QString> configMap;
};

} // namespace qyvlik

#endif // APPCONFIG_H

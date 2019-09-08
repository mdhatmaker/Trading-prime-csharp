#ifndef APPMARKETACCOUNT_H
#define APPMARKETACCOUNT_H

#include "baseentity.h"

namespace qyvlik {

class AppMarketAccountPrivate;
class AppMarketAccount : public BaseEntity
{
    Q_GADGET
    Q_PROPERTY(QString accountIdInMarket READ getAccountIdInMarket WRITE setAccountIdInMarket)
    Q_PROPERTY(QByteArray accessKey READ getAccessKey WRITE setAccessKey)
    Q_PROPERTY(QByteArray secretKey READ getSecretKey WRITE setSecretKey)
    Q_PROPERTY(QString email READ getEmail WRITE setEmail)
    Q_PROPERTY(QString market READ getMarket WRITE setMarket)
    Q_PROPERTY(bool enable READ getEnable WRITE setEnable)
    Q_PROPERTY(QString remarks READ getRemarks WRITE setRemarks)
public:
    AppMarketAccount();
    AppMarketAccount(const AppMarketAccount& other);
    AppMarketAccount& operator=(const AppMarketAccount& other);
    ~AppMarketAccount();

    QString getAccountIdInMarket() const;
    void setAccountIdInMarket(const QString& accountIdInMarket);

    QByteArray getAccessKey() const;
    void setAccessKey(const QByteArray &value);

    QByteArray getSecretKey() const;
    void setSecretKey(const QByteArray &value);

    QString getEmail() const;
    void setEmail(const QString &value);

    QString getMarket() const;
    void setMarket(const QString &value);

    bool getEnable() const;
    void setEnable(bool value);

    QString getRemarks() const;
    void setRemarks(const QString &value);

private:
    QSharedDataPointer<AppMarketAccountPrivate> d_ptr;
};

} // namespace qyvlik

Q_DECLARE_METATYPE(qyvlik::AppMarketAccount)

#endif // APPMARKETACCOUNT_H

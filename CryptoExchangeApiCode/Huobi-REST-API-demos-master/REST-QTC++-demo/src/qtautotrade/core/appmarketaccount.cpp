#include "appmarketaccount.h"

#include <QByteArray>

namespace qyvlik {
class AppMarketAccountPrivate : public QSharedData
{
public:
    AppMarketAccountPrivate()
        :accountIdInMarket("")
        , accessKey("")
        , secretKey("")
        , email("")
        , enable(true)
        , remarks("")
    {}

    AppMarketAccountPrivate(const AppMarketAccountPrivate& other)
        : QSharedData(other)
        , accountIdInMarket(other.accountIdInMarket)
        , accessKey(other.accessKey)
        , secretKey(other.secretKey)
        , email(other.email)
        , enable(other.enable)
        , remarks(other.remarks)
    {}

    QString accountIdInMarket;                      // account id in market;
    QByteArray accessKey;
    QByteArray secretKey;
    QString email;
    QString market;                                 // TODO MarketObject
    bool enable;
    QString remarks;
};

AppMarketAccount::AppMarketAccount()
    : BaseEntity()
    , d_ptr(new AppMarketAccountPrivate)
{

}

AppMarketAccount::AppMarketAccount(const AppMarketAccount &other)
    : BaseEntity(other)
    , d_ptr(other.d_ptr)
{

}

AppMarketAccount &AppMarketAccount::operator=(const AppMarketAccount &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

AppMarketAccount::~AppMarketAccount()
{
}

QString AppMarketAccount::getAccountIdInMarket() const
{
    return d_ptr->accountIdInMarket;
}

void AppMarketAccount::setAccountIdInMarket(const QString &accountIdInMarket)
{
    d_ptr->accountIdInMarket = accountIdInMarket;
}

QByteArray AppMarketAccount::getAccessKey() const
{
    return d_ptr->accessKey;
}

void AppMarketAccount::setAccessKey(const QByteArray &value)
{
    d_ptr->accessKey = value;
}

QByteArray AppMarketAccount::getSecretKey() const
{
    return d_ptr->secretKey;
}

void AppMarketAccount::setSecretKey(const QByteArray &value)
{
    d_ptr->secretKey = value;
}

QString AppMarketAccount::getEmail() const
{
    return d_ptr->email;
}

void AppMarketAccount::setEmail(const QString &value)
{
    d_ptr->email = value;
}

QString AppMarketAccount::getMarket() const
{
    return d_ptr->market;
}

void AppMarketAccount::setMarket(const QString &value)
{
    d_ptr->market = value;
}

bool AppMarketAccount::getEnable() const
{
    return d_ptr->enable;
}

void AppMarketAccount::setEnable(bool value)
{
    d_ptr->enable = value;
}

QString AppMarketAccount::getRemarks() const
{
    return d_ptr->remarks;
}

void AppMarketAccount::setRemarks(const QString &value)
{
    d_ptr->remarks = value;
}

} // namespace qyvlik

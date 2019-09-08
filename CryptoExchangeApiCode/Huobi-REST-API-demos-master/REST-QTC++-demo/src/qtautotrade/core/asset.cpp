#include "asset.h"

namespace qyvlik {

class AssetPrivate : public QSharedData
{
public:
    AssetPrivate()
        : appMarketAccountId(-1)
        , symbol("")
        , market("")
        , available(0.0)
        , frozen(0.0)
        , borrow(0.0)
    {}

    AssetPrivate(const AssetPrivate& other)
        : QSharedData(other)
        , appMarketAccountId(other.appMarketAccountId)
        , symbol(other.symbol)
        , market(other.market)
        , available(other.available)
        , frozen(other.frozen)
        , borrow(other.borrow)
    {}

    ~AssetPrivate()
    {}

    qint64 appMarketAccountId;
    QString symbol;
    QString market;
    qreal available;
    qreal frozen;
    qreal borrow;
};

Asset::Asset()
    : BaseEntity()
    , d_ptr(new AssetPrivate)
{
}

Asset::Asset(const Asset &other)
    : BaseEntity(other)
    , d_ptr(other.d_ptr)
{

}

Asset &Asset::operator=(const Asset &other)
{
    d_ptr = other.d_ptr;
    return *this;
}

Asset::~Asset()
{
}

qint64 Asset::getAppMarketAccountId() const
{
    return d_ptr->appMarketAccountId;
}

void Asset::setAppMarketAccountId(const qint64 &value)
{
    d_ptr->appMarketAccountId = value;
}

QString Asset::getSymbol() const
{
    return d_ptr->symbol;
}

void Asset::setSymbol(const QString &value)
{
    d_ptr->symbol = value;
}

QString Asset::getMarket() const
{
    return d_ptr->market;
}

void Asset::setMarket(const QString &value)
{
    d_ptr->market = value;
}

qreal Asset::getAvailable() const
{
    return d_ptr->available;
}

void Asset::setAvailable(const qreal &value)
{
    d_ptr->available = value;
}

qreal Asset::getFrozen() const
{
    return d_ptr->frozen;
}

void Asset::setFrozen(const qreal &value)
{
    d_ptr->frozen = value;
}

qreal Asset::getBorrow() const
{
    return d_ptr->borrow;
}

void Asset::setBorrow(const qreal &value)
{
    d_ptr->borrow = value;
}

///-----------------------------AppMarketAsset------------------------------

class AppMarketAssetPrivate : public QSharedData
{
public:
    AppMarketAssetPrivate()
        : QSharedData()
    {}

    AppMarketAssetPrivate(const AppMarketAssetPrivate& other)
        : QSharedData(other)
        , assetList(other.assetList)
    {}
    QList<Asset> assetList;
    //    qreal net2CNY;                      // 净资产
    //    qreal total2CNY;                    // 总资产
};

AppMarketAsset::AppMarketAsset()
    : d_ptr(new AppMarketAssetPrivate)
{}

AppMarketAsset::AppMarketAsset(const AppMarketAsset &other)
    : d_ptr(other.d_ptr)
{
    d_ptr = other.d_ptr;
}

AppMarketAsset& AppMarketAsset::operator=(const AppMarketAsset &other) {
    this->d_ptr = other.d_ptr;
    return *this;
}

AppMarketAsset::~AppMarketAsset()
{
}

QList<Asset> AppMarketAsset::getAssetList() const
{
    return d_ptr->assetList;
}

void AppMarketAsset::setAssetList(const QList<Asset> &value)
{
    d_ptr->assetList = value;
}

} // namespace qyvlik

#ifndef ASSET_H
#define ASSET_H

#include "baseentity.h"

namespace qyvlik {

class AssetPrivate;
class Asset : public BaseEntity
{
    Q_GADGET
    Q_PROPERTY(qint64 appMarketAccountId READ getAppMarketAccountId WRITE setAppMarketAccountId)
    Q_PROPERTY(QString symbol READ getSymbol WRITE setSymbol)
    Q_PROPERTY(QString market READ getMarket WRITE setMarket)
    Q_PROPERTY(qreal available READ getAvailable WRITE setAvailable)
    Q_PROPERTY(qreal frozen READ getFrozen WRITE setFrozen)
    Q_PROPERTY(qreal borrow READ getBorrow WRITE setBorrow)
public:
    Asset();
    Asset(const Asset& other);
    Asset& operator=(const Asset& other);
    ~Asset();

    qint64 getAppMarketAccountId() const;
    void setAppMarketAccountId(const qint64 &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getMarket() const;
    void setMarket(const QString &value);

    qreal getAvailable() const;
    void setAvailable(const qreal &value);

    qreal getFrozen() const;
    void setFrozen(const qreal &value);

    qreal getBorrow() const;
    void setBorrow(const qreal &value);

private:
    QSharedDataPointer<AssetPrivate> d_ptr;
};

class AppMarketAssetPrivate;
class AppMarketAsset
{
    Q_GADGET
    Q_PROPERTY(QList<Asset> assetList READ getAssetList WRITE setAssetList)
public:
    AppMarketAsset();
    AppMarketAsset(const AppMarketAsset& other);
    AppMarketAsset& operator=(const AppMarketAsset& other);
    ~AppMarketAsset();

    QList<Asset> getAssetList() const;

    void setAssetList(const QList<Asset> &value);

private:
    QSharedDataPointer<AppMarketAssetPrivate> d_ptr;
};

} // namespace qyvlik

Q_DECLARE_METATYPE(qyvlik::Asset)

Q_DECLARE_METATYPE(QList<qyvlik::Asset>)

Q_DECLARE_METATYPE(qyvlik::AppMarketAsset)

#endif // ASSET_H

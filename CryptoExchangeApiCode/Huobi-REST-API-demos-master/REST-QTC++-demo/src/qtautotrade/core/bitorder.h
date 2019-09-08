#ifndef BITORDER_H
#define BITORDER_H

#include "baseentity.h"

namespace qyvlik {

class AppMarketAccount;

class BitOrderPrivate;
class BitOrder : public BaseEntity
{
    Q_GADGET
public:
    enum class OrderType {
        BuyMarket,              // 市价买
        SellMarket,             // 市价卖
        BuyLimit,               // 限价买
        SellLimit,              // 限价卖
    };
    //    Q_ENUM(BitOrderType)

    enum class OrderSide {
        Buy,
        Sell
    };

    enum class OrderState {
        PreSubmitted,                                       // 准备提交
        Submitting,
        Submitted,                                          // 已提交
        PartialFilled,                                      // 部分成交
        PartialCanceled,                                    // 部分成交撤销
        Filled,                                             // 完全成交
        Canceled,                                           // 已撤销
    };

    static const QString ERROR_ORDER_ID;

public:
    BitOrder();
    BitOrder(const BitOrder& other);
    BitOrder& operator =(const BitOrder& other);
    ~BitOrder();

    qint64 getAppMarketAccountId() const;
    void setAppMarketAccountId(const qint64 &value);

    QString getMarket() const;
    void setMarket(const QString &value);

    QString getCurrency() const;
    void setCurrency(const QString &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getOrderId() const;
    void setOrderId(const QString &value);

    qreal getOrderAmount() const;
    void setOrderAmount(const qreal &value);

    qreal getOrderPrice() const;
    void setOrderPrice(const qreal &value);

    qreal getProcessAmount() const;
    void setProcessAmount(const qreal &value);

    qreal getProcessPrice() const;
    void setProcessPrice(const qreal &value);

    BitOrder::OrderType getOrderType() const;
    void setOrderType(const BitOrder::OrderType &value);

    qreal getFee() const;
    void setFee(const qreal &value);

    qreal getFeeAmount() const;
    void setFeeAmount(const qreal &value);

    qreal getProcessFeeAmount() const;
    void setProcessFeeAmount(const qreal &value);

    BitOrder::OrderState getState() const;
    void setState(const BitOrder::OrderState &value);

    qint64 getFinishedTime() const;
    void setFinishedTime(const qint64 &value);

    qint64 getCancelTime() const;
    void setCancelTime(const qint64 &value);

private:
    QSharedDataPointer<BitOrderPrivate> d_ptr;
};

class TradeParamPrivate;
class TradeParam
{
public:
    TradeParam();
    TradeParam(const TradeParam& other);
    TradeParam& operator=(const TradeParam& other);
    ~TradeParam();

    QString getOrderId() const;
    void setOrderId(const QString &value);

    AppMarketAccount getAppMarketAccount() const;
    void setAppMarketAccount(const AppMarketAccount &value);

    qreal getAmount() const;
    void setAmount(const qreal &value);

    qreal getPrice() const;
    void setPrice(const qreal &value);

    QString getSymbol() const;
    void setSymbol(const QString &value);

    QString getCurrency() const;
    void setCurrency(const QString& value);

    BitOrder::OrderType getOrderType() const;
    void setOrderType(const BitOrder::OrderType &value);

    QString getContractType() const;
    void setContractType(const QString &value);

    BitOrder::OrderSide getOrderSide() const;
    void setOrderSide(const BitOrder::OrderSide &value);

private:
    QSharedDataPointer<TradeParamPrivate> d_ptr;
};


} // namespace qyvlik

#endif // BITORDER_H

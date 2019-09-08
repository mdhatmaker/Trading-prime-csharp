#ifndef MARKETPARAM_H
#define MARKETPARAM_H

#include <QObject>
#include <QSharedData>


namespace qyvlik {
// 行情 API 参数
class MarketParamPrivate;
class MarketParam
{
    Q_GADGET
public:
    enum class PeriodType {
        OneMin,
        ThreeMin,
        FiveMin,
        FifteenMin,
        ThirtyMin,
        OneHour,
        TwoHour,
        FourHour,
        SixHour,
        TwelveHour,
        OneDay,
        ThreeDay,
        OneWeek,
        OneMon,
        OneYear,
    };

    enum class DepthType {
        Step0,                  //不合并深度
        Step1,
        Step2,
        Step3,
        Step4,
        Step5
    };

public:
    MarketParam();
    MarketParam(const MarketParam& other);
    MarketParam& operator=(const MarketParam& other);
    ~MarketParam();

    QString getSymbol() const;
    void setSymbol(const QString &value);

    MarketParam::PeriodType getPeriod() const;
    void setPeriod(const MarketParam::PeriodType &value);

    MarketParam::DepthType getDepth() const;
    void setDepth(const MarketParam::DepthType &value);

    qint32 getSize() const;
    void setSize(const qint32 &value);

    QString getCurrency() const;
    void setCurrency(const QString &value);

private:
    QSharedDataPointer<MarketParamPrivate> d_ptr;
};

} // namespace qyvlik

#endif // MARKETPARAM_H

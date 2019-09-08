#ifndef BASEENTITY_H
#define BASEENTITY_H

#include <QObject>

#include <QSharedDataPointer>

namespace qyvlik {

class BaseEntityPrivate;
class BaseEntity
{
    Q_GADGET
    Q_PROPERTY(qint64 id READ getId WRITE setId)
public:
    BaseEntity();

    BaseEntity(const BaseEntity& other);

    BaseEntity &operator=(const BaseEntity& other);

    virtual ~BaseEntity();

    qint64 getId() const;

    void setId(qint64 id);

    qint64 getCreateTime() const;

    void setCreateTime(qint64 createTime);

    qint64 getUpdateTime() const;

    void setUpdateTime(qint64 updateTime);

private:
    QSharedDataPointer<BaseEntityPrivate> d;
};

} // namespace qyvlik

Q_DECLARE_METATYPE(qyvlik::BaseEntity)

#endif // BASEENTITY_H

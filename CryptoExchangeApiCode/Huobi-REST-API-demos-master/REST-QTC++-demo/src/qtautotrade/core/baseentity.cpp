#include "baseentity.h"

#include <QDateTime>
#include <QDate>
#include <QSharedData>

namespace qyvlik {

class BaseEntityPrivate : public QSharedData
{
public:
    BaseEntityPrivate():
        id(-1),
        createTime(0),
        updateTime(0)
    {}

    BaseEntityPrivate(const BaseEntityPrivate& val)
         : QSharedData(val)
         , id(val.id)
         , createTime(val.createTime)
         , updateTime(val.updateTime)
    {}

    qint64 id;
    qint64 createTime;
    qint64 updateTime;
};

BaseEntity::BaseEntity():
    d(new BaseEntityPrivate)
{
}

BaseEntity::BaseEntity(const BaseEntity &other)
    : d(other.d)
{
}

BaseEntity &BaseEntity::operator=(const BaseEntity &other)
{
    d = other.d;
    return *this;
}

BaseEntity::~BaseEntity()
{
}

qint64 BaseEntity::getId() const {
    return d->id;
}

void BaseEntity::setId(qint64 id) {
    d->id = id;
}

qint64 BaseEntity::getCreateTime() const {
    return d->createTime;
}

void BaseEntity::setCreateTime(qint64 createTime) {
    d->createTime = createTime;
}

qint64 BaseEntity::getUpdateTime() const {
    return d->updateTime;
}

void BaseEntity::setUpdateTime(qint64 updateTime) {
    d->updateTime = updateTime;
}

} // namespace qyvlik
